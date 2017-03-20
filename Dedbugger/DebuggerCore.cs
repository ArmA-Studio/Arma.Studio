using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using ArmA.Studio.Debugger;
using System.Threading;
using asapJson;
using ArmA.Studio.Data.Configuration;

namespace Dedbugger
{
    public class DebuggerCore : IDebugger
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public enum ESendCommands
        {
            AddBreakpoint = 1,
            RemoveBreakpoint = 2,
            ContinueExecution = 3,
            TriggerMonitorDump = 4,
            SetEngineHookEnabled = 5,
            GetVariable = 6
        }
        public enum ERecvCommands
        {
            Halt = 1,
            ContinueExecution = 2,
            VariablesList = 3
        }
        public enum EStepType
        {
            Continue = 0,
            StepInto = 1,
            StepOver = 2,
            StepOut = 3,
        }
        public event EventHandler<OnHaltEventArgs> OnHalt;
        public event EventHandler<OnConnectionClosedEventArgs> OnConnectionClosed;
        public event EventHandler<OnErrorEventArgs> OnError;
        public event EventHandler<OnExceptionEventArgs> OnException;
        public event EventHandler<OnContinueEventArgs> OnContinue;

        private NamedPipeClientStream Pipe;
        private List<Breakpoint> Breakpoints;
        private JsonNode LastCallstack;

        public Thread PipeReadThread { get; private set; }
        public ConcurrentBag<asapJson.JsonNode> Messages;
        public string LastError { get; private set; }

        public DebuggerCore()
        {
            Messages = new ConcurrentBag<JsonNode>();
            this.Pipe = null;
        }

        public bool Attach()
        {
            if(this.Pipe != null && this.Pipe.IsConnected)
            {
                throw new InvalidOperationException();
            }
            if(this.Pipe != null)
            {
                this.Pipe.Dispose();
            }
            
            this.Pipe = new NamedPipeClientStream(".", @"ArmaDebugEnginePipeIface", PipeDirection.InOut, PipeOptions.Asynchronous, System.Security.Principal.TokenImpersonationLevel.None, System.IO.HandleInheritability.None);
            
            this.Breakpoints = new List<Breakpoint>();
            try
            {
                this.Pipe.Connect(1000);
                this.Pipe.ReadMode = PipeTransmissionMode.Message;
                this.PipeReadThread = new Thread(Thread_ReadPipeMessage);
                this.PipeReadThread.Start();
            }
            catch (TimeoutException ex)
            {
                this.LastError = ex.Message;
                this.Pipe = null;
                return false;
            }
            return true;
        }
        public void Detach()
        {
            if (this.Pipe == null)
                return;
            this.ClearBreakpoints();
            this.OnConnectionClosed?.Invoke(this, new OnConnectionClosedEventArgs());
            this.Pipe.Close();
            if (this.PipeReadThread.IsAlive)
            {
                this.PipeReadThread.Join(1000);
                if (this.PipeReadThread.IsAlive)
                    this.PipeReadThread.Abort();
            }
            this.Pipe.Dispose();
            this.Pipe = null;

        }
        public void Dispose()
        {
            this.Detach();
        }

        private void Thread_ReadPipeMessage()
        {
            try
            {
                var buffer = new byte[2048];
                while (this.Pipe.IsConnected)
                {
                    var builder = new StringBuilder();
                    do
                    {
                        var ammount = this.Pipe.Read(buffer, 0, buffer.Length);
                        for (int i = 0; i < ammount; i++)
                        {
                            builder.Append((char)buffer[i]);
                        }
                    } while (!this.Pipe.IsMessageComplete);
                    if (builder.Length > 0)
                    {
                        var node = new asapJson.JsonNode(builder.ToString(), true);
                        Logger.Log(NLog.LogLevel.Info, string.Format("RECV {0}", node.ToString()));
                        if (node.GetValue_Object().ContainsKey("exception"))
                        {
                            this.OnError?.Invoke(this, new OnErrorEventArgs() { Message = node.GetValue_Object()["exception"].GetValue_String() });
                        }
                        else
                        {
                            switch ((int)node.GetValue_Object()["command"].GetValue_Number())
                            {
                                case (int)ERecvCommands.Halt:
                                    {
                                        var callstack = this.LastCallstack = node.GetValue_Object()["callstack"];
                                        var instruction = node.GetValue_Object()["instruction"];
                                        var fileOffsetNode = instruction.GetValue_Object()["fileOffset"];
                                        var line = (int)fileOffsetNode.GetValue_Array()[0].GetValue_Number();
                                        var col = (int)fileOffsetNode.GetValue_Array()[2].GetValue_Number();
                                        this.OnHalt?.Invoke(this, new OnHaltEventArgs() { DocumentPath = instruction.GetValue_Object()["filename"].GetValue_String(), Col = col, Line = line });
                                    }
                                    break;
                                case (int)ERecvCommands.ContinueExecution:
                                    {
                                        this.OnContinue?.Invoke(this, new OnContinueEventArgs() { });
                                    }
                                    break;


                                default:
                                    this.Messages.Add(node);
                                    break;
                            }
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (ObjectDisposedException) { }
        }

        public asapJson.JsonNode ReadMessage(Func<asapJson.JsonNode, bool> cond)
        {
            while(true)
            {
                SpinWait.SpinUntil(() => this.Messages.Count > 0);
                var temp = new List<asapJson.JsonNode>();
                asapJson.JsonNode y = null;
                while (!this.Messages.IsEmpty) //Would be more efficient to use a ConcurrentSet
                {
                    this.Messages.TryTake(out y);
                    if (cond.Invoke(y))
                    {
                        break;
                    }
                    temp.Add(y);
                }
                foreach (var item in temp)
                {
                    this.Messages.Add(item); //Add back Items that didn't match
                }
                if (y != null)
                    return y;
            }
        }
        public void WriteMessage(asapJson.JsonNode node)
        {
            var str = node.ToString();
            Logger.Log(NLog.LogLevel.Info, string.Format("SEND {0}", str));
            var bytes = ASCIIEncoding.UTF8.GetBytes(str);
            this.Pipe.Write(bytes, 0, bytes.Length);
        }

        
        public void AddBreakpoint(Breakpoint b)
        {
            this.Breakpoints.Add(b);
            {
                var command = new asapJson.JsonNode(new Dictionary<string, asapJson.JsonNode>());
                command.GetValue_Object()["command"] = new asapJson.JsonNode((int)ESendCommands.AddBreakpoint);
                command.GetValue_Object()["data"] = b.Serialize();
                this.WriteMessage(command);
            }
        }

        public void RemoveBreakpoint(Breakpoint b)
        {
            this.Breakpoints.Remove(b);
            {
                var command = new asapJson.JsonNode(new Dictionary<string, asapJson.JsonNode>());
                command.GetValue_Object()["command"] = new asapJson.JsonNode((int)ESendCommands.RemoveBreakpoint);
                command.GetValue_Object()["data"] = b.Serialize();
                this.WriteMessage(command);
            }
        }

        public void UpdateBreakpoint(Breakpoint b)
        {
            {
                var command = new asapJson.JsonNode(new Dictionary<string, asapJson.JsonNode>());
                command.GetValue_Object()["command"] = new asapJson.JsonNode((int)ESendCommands.AddBreakpoint);
                command.GetValue_Object()["data"] = b.Serialize();
                this.WriteMessage(command);
            }
        }

        public void ClearBreakpoints()
        {
            var tmp = this.Breakpoints.ToList();
            foreach (var it in tmp)
            {
                this.RemoveBreakpoint(it);
            }
        }

        public IEnumerable<Variable> GetVariables(EVariableNamespace scope, params string[] names)
        {
            var command = new asapJson.JsonNode(new Dictionary<string, asapJson.JsonNode>());
            command.GetValue_Object()["command"] = new asapJson.JsonNode((int)ESendCommands.GetVariable);
            var data = command.GetValue_Object()["data"] = new asapJson.JsonNode(new Dictionary<string, asapJson.JsonNode>());
            data.GetValue_Object()["name"] = new asapJson.JsonNode(names.Select(name => new asapJson.JsonNode(name)));
            data.GetValue_Object()["scope"] = new asapJson.JsonNode((int)scope);
            this.WriteMessage(command);
            

            var response = this.ReadMessage((node) => (int)(node.GetValue_Object()["command"].GetValue_Number()) == (int)ERecvCommands.VariablesList);
            
            var variables = response.GetValue_Object()["data"];
            if (variables.GetValueType() == JsonNode.EJType.Array) //Might be null if no variables found
            foreach (var variable in variables.GetValue_Array())
            {
                var name = variable.GetValue_Object()["name"].GetValue_String();
                var type = variable.GetValue_Object()["type"].GetValue_String();
                var value = "";
                if (type == "void")
                {
                        yield return new Variable() { Name = name, Value = value, VariableType = Variable.ValueType.Parse(type) };
                }
                else if (type == "array")
                {
                    //ToDo: handle multi-dimensional arrays
                    var valueArray = variable.GetValue_Object()["value"];
                    value = "[";
                    if (valueArray.GetValueType() == JsonNode.EJType.Array) //Might be null if EMPTY
                    foreach (var val in valueArray.GetValue_Array())
                    {
                        if (val.GetValue_Object()["type"].GetValue_String() == "array") //Small workaround to allow 2D arrays
                        {
                            value += "[arrayPlaceholder]";
                        }
                        else
                        {
                             value += val.GetValue_Object()["value"].GetValue_String();
                        }
                        
                        value += ",";
                    }
                    value += "]";

                    var ns = (EVariableNamespace)variable.GetValue_Object()["ns"].GetValue_Number();//Namespace the variable comes from
                    yield return new Variable() { Name = name, Value = value, VariableType = Variable.ValueType.Parse(type), Namespace = ns };
                }
                else
                {
                    value = variable.GetValue_Object()["value"].GetValue_String();
                    var ns = (EVariableNamespace)variable.GetValue_Object()["ns"].GetValue_Number();//Namespace the variable comes from
                    yield return new Variable() { Name = name, Value = value, VariableType = Variable.ValueType.Parse(type), Namespace = ns };
                }
            }
        }

        public void SetVariable(Variable v)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CallstackItem> GetCallstack()
        {
            foreach (var node in this.LastCallstack.GetValue_Array())
            {
                if (!node.GetValue_Object().ContainsKey("lastInstruction")) //Not every callstackItem has instructions
                    continue;
                var line = (int)node.GetValue_Object()["lastInstruction"].GetValue_Object()["fileOffset"].GetValue_Array()[0].GetValue_Number();
                var col = (int)node.GetValue_Object()["lastInstruction"].GetValue_Object()["fileOffset"].GetValue_Array()[2].GetValue_Number();
                var sample = node.GetValue_Object()["contentSample"].GetValue_String();
                var file = node.GetValue_Object()["lastInstruction"].GetValue_Object()["filename"].GetValue_String();
                yield return new CallstackItem() { FileName = file, Column = col, ContentSample = sample, Line = line };
            }
        }

        public bool Perform(EOperation op)
        {
            switch (op)
            {
                case EOperation.Continue:
                    {
                        var node = new asapJson.JsonNode(new Dictionary<string, asapJson.JsonNode>());
                        node.GetValue_Object()["command"] = new asapJson.JsonNode((int)ESendCommands.ContinueExecution);
                        node.GetValue_Object()["data"] = new asapJson.JsonNode((int)EStepType.Continue);
                        this.WriteMessage(node);
                        return true;
                    }
                case EOperation.StepInto:
                    {
                        var node = new asapJson.JsonNode(new Dictionary<string, asapJson.JsonNode>());
                        node.GetValue_Object()["command"] = new asapJson.JsonNode((int)ESendCommands.ContinueExecution);
                        node.GetValue_Object()["data"] = new asapJson.JsonNode((int)EStepType.StepInto);
                        this.WriteMessage(node);
                        return true;
                    }
                case EOperation.StepOver:
                    {
                        var node = new asapJson.JsonNode(new Dictionary<string, asapJson.JsonNode>());
                        node.GetValue_Object()["command"] = new asapJson.JsonNode((int)ESendCommands.ContinueExecution);
                        node.GetValue_Object()["data"] = new asapJson.JsonNode((int)EStepType.StepOver);
                        this.WriteMessage(node);
                        return true;
                    }
                case EOperation.StepOut:
                    {
                        var node = new asapJson.JsonNode(new Dictionary<string, asapJson.JsonNode>());
                        node.GetValue_Object()["command"] = new asapJson.JsonNode((int)ESendCommands.ContinueExecution);
                        node.GetValue_Object()["data"] = new asapJson.JsonNode((int)EStepType.StepOut);
                        this.WriteMessage(node);
                        return true;
                    }
                default:
                    this.LastError = "Not Implemented";
                    return false;
            }
        }

        public string GetLastError()
        {
            var str = this.LastError;
            this.LastError = string.Empty;
            return str;
        }

        public IEnumerable<ConfigCategory> GetConfigurationOptions()
        {
            yield return new ConfigCategory("Dedbugger", @"/ArmA.Studio;component/Resources/Pictograms/Run/Run.ico");
        }
    }
}

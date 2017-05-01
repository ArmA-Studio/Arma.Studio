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
using ArmA.Studio.Plugin;
using ArmA.Studio.Data;

namespace Dedbugger
{
    public class DebuggerCore : IDebuggerPlugin
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public enum ESendCommands
        {
            GetVersionInfo = 1,
            AddBreakpointInfo = 2,
            RemoveBreakpointInfo = 3,
            ContinueExecution = 4,
            TriggerMonitorDump = 5,
            SetEngineHookEnabled = 6,
            GetVariable = 7
        }
        public enum ERecvCommands
        {
            VersionInfo = 1,
            HaltBreakpointInfo = 2,
            HaltStep = 3,
            HaltError = 4,
            HaltScriptAssert = 5,
            HaltScriptHalt = 6,
            HaltPlaceholder = 7,
            ContinueExecution = 8,
            VariablesList = 9
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
        private List<BreakpointInfo> BreakpointInfos;
        private asapJson.JsonNode LastCallstack;
        private const int MinimalDebuggerBuild = 23;

        public Thread PipeReadThread { get; private set; }
        public ConcurrentBag<asapJson.JsonNode> Messages;
        public string LastError { get; private set; }


        public string Name => "Debugger Bridge";

        public string Description => "Plugin to connect to the ArmA Debugger.";

        private bool IsHalted;

        public DebuggerCore()
        {
            Messages = new ConcurrentBag<JsonNode>();
            this.Pipe = null;
            this.IsHalted = false;
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
            
            this.BreakpointInfos = new List<BreakpointInfo>();
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

            var command = new asapJson.JsonNode(new Dictionary<string, asapJson.JsonNode>());
            command.GetValue_Object()["command"] = new asapJson.JsonNode((int)ESendCommands.GetVersionInfo);
            this.WriteMessage(command);
            var response = this.ReadMessage((node) => (int)(node.GetValue_Object()["command"].GetValue_Number()) == (int)ERecvCommands.VersionInfo);

            if (response.GetValue_Object()["gameType"].GetValueType() == JsonNode.EJType.String) //Might be null if init failed or attached too soon
            { 
                var gameType = response.GetValue_Object()["gameType"].GetValue_String();
            }
            if (response.GetValue_Object()["gameVersion"].GetValueType() == JsonNode.EJType.String) //Might be null if init failed or attached too soon
            {
                var gameVersion = response.GetValue_Object()["gameVersion"].GetValue_String();
            }
            var arch = response.GetValue_Object()["arch"].GetValue_String();
            var build = response.GetValue_Object()["build"].GetValue_Number();
            var version = response.GetValue_Object()["version"].GetValue_String();
            if (build < MinimalDebuggerBuild)
            {
                this.LastError = "Unsupported Debugger build: " + build +" \nMinimal build required: "+MinimalDebuggerBuild;
                Detach();
                return false;
            }
            return true;
        }
        public void Detach()
        {
            if (this.Pipe == null)
                return;
            if (this.IsHalted)
            {
                this.Perform(EOperation.Continue);
            }
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
                            switch ((ERecvCommands)node.GetValue_Object()["command"].GetValue_Number())
                            {
                                case ERecvCommands.HaltBreakpointInfo:
                                case ERecvCommands.HaltStep:
                                    {
                                        var callstack = this.LastCallstack = node.GetValue_Object()["callstack"];
                                        var instruction = node.GetValue_Object()["instruction"];
                                        var fileOffsetNode = instruction.GetValue_Object()["fileOffset"];
                                        var line = (int)fileOffsetNode.GetValue_Array()[0].GetValue_Number();
                                        var col = (int)fileOffsetNode.GetValue_Array()[2].GetValue_Number();
                                        this.IsHalted = true;
                                        this.OnHalt?.Invoke(this, new OnHaltEventArgs(instruction.GetValue_Object()["filename"].GetValue_String(), null, line, col));
                                    }
                                    break;
                                case ERecvCommands.HaltError:
                                    {
                                        var callstack = this.LastCallstack = node.GetValue_Object()["callstack"];
                                        var error = node.GetValue_Object()["error"];
                                        var fileOffsetNode = error.GetValue_Object()["fileOffset"];
                                        var errorMessage = error.GetValue_Object()["message"];//ToDo: display to user
                                        var fileContent = error.GetValue_Object()["content"];//File content in case we don't have that file
                                        var line = (int)fileOffsetNode.GetValue_Array()[0].GetValue_Number();
                                        var col = (int)fileOffsetNode.GetValue_Array()[2].GetValue_Number();
                                        this.IsHalted = true;
                                        this.OnHalt?.Invoke(this, new OnHaltEventArgs(error.GetValue_Object()["filename"].GetValue_String(), fileContent.GetValue_String(), line, col));
                                    }
                                    break;
                                case ERecvCommands.HaltScriptAssert:
                                case ERecvCommands.HaltScriptHalt:
                                    {
                                        var callstack = this.LastCallstack = node.GetValue_Object()["callstack"];
                                        var halt = node.GetValue_Object()["halt"];
                                        var fileOffsetNode = halt.GetValue_Object()["fileOffset"];
                                        var fileContent = halt.GetValue_Object()["content"];//File content in case we don't have that file
                                        var line = (int)fileOffsetNode.GetValue_Array()[0].GetValue_Number();
                                        var col = (int)fileOffsetNode.GetValue_Array()[2].GetValue_Number();
                                        this.IsHalted = true;
                                        this.OnHalt?.Invoke(this, new OnHaltEventArgs(halt.GetValue_Object()["filename"].GetValue_String(), null, line, col));
                                    }
                                    break;
                                case ERecvCommands.ContinueExecution:
                                    {
                                        this.IsHalted = false;
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
            catch (Exception ex) { Virtual.ShowOperationFailedMessageBox(ex); }
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

        
        public void AddBreakpoint(BreakpointInfo b)
        {
            this.BreakpointInfos.Add(b);
            {
                var command = new asapJson.JsonNode(new Dictionary<string, asapJson.JsonNode>());
                command.GetValue_Object()["command"] = new asapJson.JsonNode((int)ESendCommands.AddBreakpointInfo);
                command.GetValue_Object()["data"] = b.Serialize();
                this.WriteMessage(command);
            }
        }

        public void RemoveBreakpoint(BreakpointInfo b)
        {
            this.BreakpointInfos.Remove(b);
            {
                var command = new asapJson.JsonNode(new Dictionary<string, asapJson.JsonNode>());
                command.GetValue_Object()["command"] = new asapJson.JsonNode((int)ESendCommands.RemoveBreakpointInfo);
                command.GetValue_Object()["data"] = b.Serialize();
                this.WriteMessage(command);
            }
        }

        public void UpdateBreakpoint(BreakpointInfo b)
        {
            {
                var command = new asapJson.JsonNode(new Dictionary<string, asapJson.JsonNode>());
                command.GetValue_Object()["command"] = new asapJson.JsonNode((int)ESendCommands.AddBreakpointInfo);
                command.GetValue_Object()["data"] = b.Serialize();
                this.WriteMessage(command);
            }
        }

        public void ClearBreakpoints()
        {
            var tmp = this.BreakpointInfos.ToList();
            foreach (var it in tmp)
            {
                this.RemoveBreakpoint(it);
            }
        }

        private static string VariableArrayToString(JsonNode jsonNode)
        {
            var value = "[";
            if (jsonNode.GetValueType() == JsonNode.EJType.Array) //Might be null if EMPTY
                foreach (var val in jsonNode.GetValue_Array())
                {
                    if (val.GetValue_Object()["type"].GetValue_String() == "array") //Small workaround to allow 2D arrays
                    {
                        value += VariableArrayToString(val.GetValue_Object()["value"]);
                    }
                    else
                    {
                        value += val.GetValue_Object()["value"].GetValue_String();
                    }
                    value += ",";
                }
            value = value.TrimEnd(',');
            value += "]";
            return value;
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
                switch (type)
                {
                    case "void":
                        yield return new Variable() { Name = name, Value = value, VariableType = Variable.ValueType.ANY };
                        break;
                    case "array":
                        {
                            value = VariableArrayToString(variable.GetValue_Object()["value"]);
                            var ns = (EVariableNamespace)variable.GetValue_Object()["ns"].GetValue_Number();//Namespace the variable comes from
                            yield return new Variable() { Name = name, Value = value, VariableType = Variable.ValueType.ARRAY, Namespace = ns };
                        }
                        break;
                    default:
                        {
                            value = variable.GetValue_Object()["value"].GetValue_String();
                            var ns = (EVariableNamespace)variable.GetValue_Object()["ns"].GetValue_Number();//Namespace the variable comes from
                            yield return new Variable() { Name = name, Value = value, VariableType = Variable.ValueType.Parse(type), Namespace = ns };
                        }
                        break;
                }
            }
        }

        public void SetVariable(Variable v)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CallstackItem> GetCallstack()
        {
            if (this.LastCallstack == null || this.LastCallstack.GetValueType() != JsonNode.EJType.Array)
                yield break;
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

        public string GetDocumentContent(string armapath)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using ArmA.Studio.Debugger;
using System.Threading;
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
            AddBreakpoint,
            DelBreakpoin,
            BPContinue,
            MonitorDump,
            SetHookEnable,
            GetVariable,
            GetCurrentCode,
            GetAllScriptCommands
        }
        public enum ERecvCommands
        {
            VersionInfo = 1,
            Halt_breakpoint,
            Halt_step,
            Halt_error,
            Halt_scriptAssert,
            Halt_scriptHalt,
            Halt_placeholder,
            ContinueExecution,
            VariableReturn
        }
        [Flags]
        public enum EVariableScope
        { //This is a bitflag
            Invalid = 0,
            Callstack = 1,
            Local = 2,
            MissionNamespace = 4,
            UiNamespace = 8,
            ProfileNamespace = 16,
            ParsingNamespace = 32,
            All = Invalid | Callstack | Local | MissionNamespace | UiNamespace | ProfileNamespace | ParsingNamespace
        }
        public event EventHandler<OnHaltEventArgs> OnHalt;
        public event EventHandler<OnConnectionClosedEventArgs> OnConnectionClosed;
        public event EventHandler<OnErrorEventArgs> OnError;
        public event EventHandler<OnExceptionEventArgs> OnException;
        public event EventHandler<OnContinueEventArgs> OnContinue;

        private NamedPipeClientStream Pipe;
        private List<BreakpointInfo> BreakpointInfos;
        private dynamic LastCallstack;
        private const int MinimalDebuggerBuild = 23;

        public Thread PipeReadThread { get; private set; }
        public ConcurrentBag<dynamic> Messages;
        public string LastError { get; private set; }


        public string Name => "Debugger Bridge";

        public string Description => "Plugin to connect to the ArmaDebugEngine.";

        private bool IsHalted;

        public DebuggerCore()
        {
            this.Messages = new ConcurrentBag<dynamic>();
            this.Pipe = null;
            this.IsHalted = false;
        }

        public bool Attach()
        {
            if (this.Pipe != null && this.Pipe.IsConnected)
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
                this.PipeReadThread = new Thread(this.Thread_ReadPipeMessage);
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
            if (this.IsHalted)
            {
                this.Perform(EOperation.Continue);
            }
            this.OnConnectionClosed?.Invoke(this, new OnConnectionClosedEventArgs());
            this.ClearBreakpoints();
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
                        builder.Append(Encoding.Default.GetString(buffer, 0, ammount));
                    }
                    while (!this.Pipe.IsMessageComplete);
                    if (builder.Length > 0)
                    {
                        dynamic token = Newtonsoft.Json.Linq.JToken.Parse(builder.ToString());
                        ERecvCommands command = token.command;
                        switch(command)
                        {
                            case ERecvCommands.VersionInfo:
                                break;
                            case ERecvCommands.ContinueExecution:
                                this.IsHalted = false;
                                this.OnContinue?.Invoke(this, new OnContinueEventArgs() { });
                                break;
                            case ERecvCommands.Halt_breakpoint:
                            case ERecvCommands.Halt_step:
                            case ERecvCommands.Halt_error:
                            case ERecvCommands.Halt_scriptAssert:
                            case ERecvCommands.Halt_scriptHalt:
                                {
                                    this.LastCallstack = token.callstack;
                                    string filename = "";
                                    int line = 0;
                                    try
                                    {
                                        filename = token.instruction.filename;
                                        line = token.instruction.fileOffsetNode[0];
                                    }
                                    catch { }
                                    this.IsHalted = true;
                                    this.OnHalt?.Invoke(this, new OnHaltEventArgs(filename, token.callstack.contentSample, line, 0));
                                }
                                break;
                            case ERecvCommands.Halt_placeholder:
                                break;
                            case ERecvCommands.VariableReturn:
                                this.Messages.Add(token);
                                break;
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex) { Virtual.ShowOperationFailedMessageBox(ex); }
        }

        public void AddBreakpoint(BreakpointInfo b)
        {
            this.BreakpointInfos.Add(b);
            var node = new Newtonsoft.Json.Linq.JObject
            {
                { "command", (int)ESendCommands.AddBreakpoint },
                { "data", b.AsJToken() }
            };
            this.WriteMessage(node);
        }

        public void RemoveBreakpoint(BreakpointInfo b)
        {
            this.BreakpointInfos.Remove(b);
            var node = new Newtonsoft.Json.Linq.JObject
            {
                { "command", (int)ESendCommands.DelBreakpoin },
                { "data", b.AsJToken() }
            };
            this.WriteMessage(node);
        }

        public void UpdateBreakpoint(BreakpointInfo b)
        {
            throw new NotSupportedException();
        }

        public void ClearBreakpoints()
        {
            var tmp = this.BreakpointInfos.ToList();
            foreach (var it in tmp)
            {
                this.RemoveBreakpoint(it);
            }
        }

        private static string VariableArrayToString(dynamic value)
        {
            var builder = new StringBuilder("[");
            if (value is Newtonsoft.Json.Linq.JArray)//Might be null if EMPTY
            {
                bool comma = false;
                foreach (var val in value)
                {
                    if(comma)
                    {
                        builder.Append(",");
                    }
                    else
                    {
                        comma = true;
                    }
                    if(val.type == "array")
                    {
                        builder.Append(VariableArrayToString(val));
                    }
                    else
                    {
                        builder.Append(val.value);
                    }
                }
            }
            builder.Append("]");
            return builder.ToString();
        }

        public IEnumerable<Variable> GetVariables(EVariableNamespace scope, params string[] names)
        {
            var node = new Newtonsoft.Json.Linq.JObject
            {
                { "command", (int)ESendCommands.GetVariable },
                { "name", new Newtonsoft.Json.Linq.JArray(names.Select((name) => new Newtonsoft.Json.Linq.JValue(name))) },
                { "scope", 0 }
            };
            switch (scope)
            {
                case EVariableNamespace.All:
                    node["scope"] = (int)EVariableScope.All;
                    break;
                case EVariableNamespace.Callstack:
                    node["scope"] = (int)EVariableScope.Callstack;
                    break;
                case EVariableNamespace.LocalEvaluator:
                    node["scope"] = (int)EVariableScope.Local;
                    break;
                case EVariableNamespace.MissionNamespace:
                    node["scope"] = (int)EVariableScope.MissionNamespace;
                    break;
                case EVariableNamespace.ParsingNamespace:
                    node["scope"] = (int)EVariableScope.ParsingNamespace;
                    break;
                case EVariableNamespace.ProfileNamespace:
                    node["scope"] = (int)EVariableScope.ProfileNamespace;
                    break;
                case EVariableNamespace.UiNamespace:
                    node["scope"] = (int)EVariableScope.UiNamespace;
                    break;
            }
            this.WriteMessage(node);
            var response = this.ReadMessage((n) => n.command == (int)ERecvCommands.VariableReturn);

            foreach (var variable in response.data)
            {
                string name = variable.name;
                string type = variable.type;
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
            throw new NotSupportedException();
        }

        public IEnumerable<CallstackItem> GetCallstack()
        {
            if (this.LastCallstack == null)
            {
                yield break;
            }

            foreach (var node in this.LastCallstack.callstack.compiled)
            {
                string filename = "";
                string sample = "";
                int line = 0;
                try
                {
                    filename = node.filename;
                    line = node.fileOffset[0];
                    sample = (string)node.name + (string)node.type;
                }
                catch { }
                yield return new CallstackItem() { FileName = filename, Line = line, ContentSample = sample };
            }
        }

        public dynamic ReadMessage(Func<dynamic, bool> cond)
        {
            while (true)
            {
                SpinWait.SpinUntil(() => this.Messages.Count > 0);
                var temp = new List<dynamic>();
                dynamic y = null;
                while (!this.Messages.IsEmpty) // Would be more efficient to use a ConcurrentSet
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
                    this.Messages.Add(item); // Add back Items that didn't match
                }
                if (y != null)
                {
                    return y;
                }
            }
        }
        public void WriteMessage(Newtonsoft.Json.Linq.JToken node)
        {
            var str = node.ToString();
            Logger.Log(NLog.LogLevel.Info, String.Format("SEND {0}", str));
            var bytes = Encoding.UTF8.GetBytes(str);
            try
            {
                this.Pipe.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.Detach();
            }
        }

        public bool Perform(EOperation op)
        {
            switch (op)
            {
                case EOperation.Continue:
                case EOperation.StepInto:
                case EOperation.StepOver:
                case EOperation.StepOut:
                    {
                        var node = new Newtonsoft.Json.Linq.JObject
                        {
                            { "command", (int)ESendCommands.BPContinue },
                            { "data", op == EOperation.Continue ? 0 : op == EOperation.StepInto ? 1 : op == EOperation.StepOver ? 2 : 3 }
                        };
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
            this.LastError = String.Empty;
            return str;
        }

        public string GetDocumentContent(string armapath)
        {
            throw new NotSupportedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using ArmA.Studio.Debugger;

namespace Dedbugger
{
    public class DebuggerCore : IDebugger
    {
        public event EventHandler<OnHaltEventArgs> OnHalt;
        public event EventHandler<OnConnectionLostEventArgs> OnConnectionClosed;
        public event EventHandler<OnErrorEventArgs> OnError;
        public event EventHandler<OnExceptionEventArgs> OnException;

        private NamedPipeClientStream Pipe;

        public DebuggerCore()
        {
            this.Pipe = null;
        }

        public bool Attach()
        {
            if(this.Pipe != null || this.Pipe.IsConnected)
            {
                throw new InvalidOperationException();
            }
            if(this.Pipe != null)
            {
                this.Pipe.Dispose();
            }

            this.Pipe = new NamedPipeClientStream("\\\\.\\pipe\\ArmaDebugEnginePipeIface");
            try
            {
                this.Pipe.Connect(1000);
            }
            catch(TimeoutException ex)
            {
                this.Pipe = null;
                return false;
            }
            return true;
        }
        public void Detach()
        {
            if (this.Pipe == null)
                return;
            this.OnConnectionClosed?.Invoke(this, new OnConnectionLostEventArgs());
            this.Pipe.Close();
            this.Pipe.Dispose();
            this.Pipe = null;
        }
        public void Dispose()
        {
            this.Detach();
        }

        public void ClearBreakpoints()
        {
            throw new NotImplementedException();
        }


        public Callstack GetCallstack()
        {
            throw new NotImplementedException();
        }

        public Variable GetVariableByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Variable> GetVariables()
        {
            throw new NotImplementedException();
        }

        public void SetBreakpoint(Breakpoint b, bool flag)
        {
            throw new NotImplementedException();
        }

        public Variable SetVariable(Variable v)
        {
            throw new NotImplementedException();
        }

        public void UpdateBreakpoint(Breakpoint b)
        {
            throw new NotImplementedException();
        }
    }
}

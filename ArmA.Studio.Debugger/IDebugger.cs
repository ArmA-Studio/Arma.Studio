using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Debugger
{
    public interface IDebugger :  IDisposable
    {
        event EventHandler<OnHaltEventArgs> OnHalt;
        event EventHandler<OnExceptionEventArgs> OnException;
        event EventHandler<OnErrorEventArgs> OnError;
        event EventHandler<OnConnectionClosedEventArgs> OnConnectionClosed;
        event EventHandler<OnContinueEventArgs> OnContinue;

        void AddBreakpoint(Breakpoint b);
        void RemoveBreakpoint(Breakpoint b);
        void UpdateBreakpoint(Breakpoint b);
        void ClearBreakpoints();

        bool Attach();
        void Detach();

        Variable GetVariableByName(string name, string scope = "missionnamespace");
        Variable SetVariable(Variable v);
        IEnumerable<Variable> GetVariables();

        Callstack GetCallstack();
        bool Perform(EOperation stepInto);
        string GetLastError();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data;
using ArmA.Studio.Data.Configuration;
using ArmA.Studio.Debugger;

namespace ArmA.Studio.Plugin
{
    public interface IDebuggerPlugin : IPlugin, IDisposable
    {
        event EventHandler<OnHaltEventArgs> OnHalt;
        event EventHandler<OnExceptionEventArgs> OnException;
        event EventHandler<OnErrorEventArgs> OnError;
        event EventHandler<OnConnectionClosedEventArgs> OnConnectionClosed;
        event EventHandler<OnContinueEventArgs> OnContinue;

        void AddBreakpoint(BreakpointInfo b);
        void RemoveBreakpoint(BreakpointInfo b);
        void UpdateBreakpoint(BreakpointInfo b);
        void ClearBreakpoints();

        bool Attach();
        void Detach();

        IEnumerable<Variable> GetVariables(EVariableNamespace scope = EVariableNamespace.All, params string[] names);
        void SetVariable(Variable v);

        IEnumerable<CallstackItem> GetCallstack();
        bool Perform(EOperation op);
        string GetLastError();
        string GetDocumentContent(string armapath);
    }
}

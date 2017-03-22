using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Debugger;

namespace ArmA.Studio.Plugin
{
    public static class Extensions
    {
        #region IDebuggerPlugin
        public static async Task AddBreakpointAsync(this IDebuggerPlugin dbgr, Breakpoint b)
        {
            await Task.Run(() => dbgr.AddBreakpoint(b));
        }
        public static async Task RemoveBreakpointAsync(this IDebuggerPlugin dbgr, Breakpoint b)
        {
            await Task.Run(() => dbgr.RemoveBreakpoint(b));
        }
        public static async Task UpdateBreakpointAsync(this IDebuggerPlugin dbgr, Breakpoint b)
        {
            await Task.Run(() => dbgr.UpdateBreakpoint(b));
        }
        public static async Task ClearBreakpointsAsync(this IDebuggerPlugin dbgr)
        {
            await Task.Run(() => dbgr.ClearBreakpoints());
        }

        public static async Task<bool> AttachAsync(this IDebuggerPlugin dbgr)
        {
            return await Task.Run(() => dbgr.Attach());
        }
        public static async Task DetachAsync(this IDebuggerPlugin dbgr)
        {
            await Task.Run(() => dbgr.Detach());
        }

        public static async Task<IEnumerable<Variable>> GetVariablesAsync(this IDebuggerPlugin dbgr, EVariableNamespace scope = EVariableNamespace.All, params string[] names)
        {
            return await Task.Run(() => dbgr.GetVariables(scope, names));
        }
        public static async Task SetVariableAsync(this IDebuggerPlugin dbgr, Variable v)
        {
            await Task.Run(() => dbgr.SetVariable(v));
        }

        public static async Task<IEnumerable<CallstackItem>> GetCallstackAsync(this IDebuggerPlugin dbgr)
        {
            return await Task.Run(() => dbgr.GetCallstack());
        }

        public static async Task<bool> PerformAsync(this IDebuggerPlugin dbgr, EOperation op)
        {
            return await Task.Run(() => dbgr.Perform(op));
        }
        #endregion
    }
}
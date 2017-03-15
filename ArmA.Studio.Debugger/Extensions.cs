using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Debugger
{
    public static class Extensions
    {
        #region IDebugger
        public static async Task AddBreakpointAsync(this IDebugger dbgr, Breakpoint b)
        {
            await Task.Run(() => dbgr.AddBreakpoint(b));
        }
        public static async Task RemoveBreakpointAsync(this IDebugger dbgr, Breakpoint b)
        {
            await Task.Run(() => dbgr.RemoveBreakpoint(b));
        }
        public static async Task UpdateBreakpointAsync(this IDebugger dbgr, Breakpoint b)
        {
            await Task.Run(() => dbgr.UpdateBreakpoint(b));
        }
        public static async Task ClearBreakpointsAsync(this IDebugger dbgr)
        {
            await Task.Run(() => dbgr.ClearBreakpoints());
        }

        public static async Task<bool> AttachAsync(this IDebugger dbgr)
        {
            return await Task.Run(() => dbgr.Attach());
        }
        public static async Task DetachAsync(this IDebugger dbgr)
        {
            await Task.Run(() => dbgr.Detach());
        }

        public static async Task<IEnumerable<Variable>> GetVariablesAsync(this IDebugger dbgr, EVariableNamespace scope = EVariableNamespace.All, params string[] names)
        {
            return await Task.Run(() => dbgr.GetVariables(scope, names));
        }
        public static async Task SetVariableAsync(this IDebugger dbgr, Variable v)
        {
            await Task.Run(() => dbgr.SetVariable(v));
        }

        public static async Task<IEnumerable<CallstackItem>> GetCallstackAsync(this IDebugger dbgr)
        {
            return await Task.Run(() => dbgr.GetCallstack());
        }

        public static async Task<bool> PerformAsync(this IDebugger dbgr, EOperation op)
        {
            return await Task.Run(() => dbgr.Perform(op));
        }
        #endregion
    }
}
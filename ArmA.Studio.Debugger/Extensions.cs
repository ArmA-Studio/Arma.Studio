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
        public static async Task SetBreakpointAsync(this IDebugger dbgr, Breakpoint b, bool flag)
        {
            await Task.Run(() => dbgr.SetBreakpoint(b, flag));
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
        public static async Task<Variable> GetVariableByNameAsync(this IDebugger dbgr, string name)
        {
            return await Task.Run(() => dbgr.GetVariableByName(name));
        }
        public static async Task<Variable> SetVariableAsync(this IDebugger dbgr, Variable v)
        {
            return await Task.Run(() => dbgr.SetVariable(v));
        }
        public static async Task<IEnumerable<Variable>> GetVariablesAsync(this IDebugger dbgr)
        {
            return await Task.Run(() => dbgr.GetVariables());
        }
        public static async Task<Callstack> GetCallstackAsync(this IDebugger dbgr)
        {
            return await Task.Run(() => dbgr.GetCallstack());
        }
        #endregion
    }
}
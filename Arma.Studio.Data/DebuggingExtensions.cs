using Arma.Studio.Data.Debugging;
using Arma.Studio.Data.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data
{
    public static class DebuggingExtensions
    {
        public static IEnumerable<IBreakpoint> GetBreakpoints(this IBreakpointManager breakpointManager, File file, Func<IBreakpoint, bool> condition)
        { return breakpointManager.GetBreakpoints(file.FullPath, condition); }

        public static IBreakpoint CreateBreakpoint(this IBreakpointManager breakpointManager, File file)
        { return breakpointManager.AddBreakpoint(file.FullPath); }

        public static void RemoveBreakpoint(this IBreakpointManager breakpointManager, File file, IBreakpoint bp)
        { breakpointManager.RemoveBreakpoint(file.FullPath, bp); }

        public static void RemoveBreakpoints(this IBreakpointManager breakpointManager, File file, IEnumerable<IBreakpoint> bps)
        { breakpointManager.RemoveBreakpoints(file.FullPath, bps); }
        public static void RemoveBreakpoints(this IBreakpointManager breakpointManager, string file, IEnumerable<IBreakpoint> bps)
        {
            foreach (var it in bps)
            {
                breakpointManager.RemoveBreakpoint(file, it);
            }
        }
    }
}

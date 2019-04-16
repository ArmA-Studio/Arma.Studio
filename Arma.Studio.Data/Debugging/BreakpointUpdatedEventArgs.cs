

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Debugging
{
    public class BreakpointUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// A copy of the breakpoint brefore the actual change.
        /// </summary>
        public IBreakpoint BreakpointOld { get; }
        /// <summary>
        /// The actual instance that got changed.
        /// </summary>
        public IBreakpoint BreakpointNew { get; }
        public BreakpointUpdatedEventArgs(IBreakpoint breakpointOld, IBreakpoint breakpointNew)
        {
            this.BreakpointOld = breakpointOld;
            this.BreakpointNew = breakpointNew;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Debugging
{
    public class BreakpointEventArgs : EventArgs
    {
        public IBreakpoint Breakpoint { get; }
        public BreakpointEventArgs(IBreakpoint breakpoint)
        {
            this.Breakpoint = breakpoint;
        }
    }
}

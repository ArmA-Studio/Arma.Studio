using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Data.UI
{
    public interface IBreakpointManager
    {
        event EventHandler<BreakpointEventArgs> BreakpointAdded;
        event EventHandler<BreakpointEventArgs> BreakpointRemoved;

        IEnumerable<IBreakpoint> Breakpoints { get; }

        IBreakpoint AddBreakpoint(string file);
        void RemoveBreakpoint(string file, IBreakpoint bp);
        IEnumerable<IBreakpoint> GetBreakpoints(string file, Func<IBreakpoint, bool> condition);

    }
}

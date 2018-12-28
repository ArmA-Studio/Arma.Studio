using Arma.Studio.Data.Debugging;
using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Debugging
{
    public class BreakpointManager : IBreakpointManager
    {
        public event EventHandler<BreakpointEventArgs> BreakpointAdded;
        public event EventHandler<BreakpointEventArgs> BreakpointRemoved;

        protected readonly Dictionary<string, List<IBreakpoint>> Inner;

        public IEnumerable<IBreakpoint> Breakpoints => this.Inner.Values.SelectMany((it) => it);


        public IBreakpoint AddBreakpoint(string file)
        {
            if (this.Inner.TryGetValue(file, out var val))
            {
                var bp = new Breakpoint(file);
                val.Add(bp);
                this.BreakpointAdded?.Invoke(this, new BreakpointEventArgs(bp));
                return bp;
            }
            else
            {
                var list = new List<IBreakpoint>();
                this.Inner.Add(file, list);
                var bp = new Breakpoint(file);
                val.Add(bp);
                this.BreakpointAdded?.Invoke(this, new BreakpointEventArgs(bp));
                return bp;
            }
        }

        public IEnumerable<IBreakpoint> GetBreakpoints(string file, Func<IBreakpoint, bool> condition)
        {
            if (this.Inner.TryGetValue(file, out var val))
            {
                foreach (var it in val)
                {
                    if (condition(it))
                    {
                        yield return it;
                    }
                }
            }
        }

        public void RemoveBreakpoint(string file, IBreakpoint bp)
        {
            if (this.Inner.TryGetValue(file, out var val))
            {
                val.Remove(bp);
            }
        }
    }
}

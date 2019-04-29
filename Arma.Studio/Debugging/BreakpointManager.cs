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
        public event EventHandler<BreakpointUpdatedEventArgs> BreakpointUpdated;
        public event EventHandler<BreakpointEventArgs> BreakpointRemoved;

        protected readonly Dictionary<string, List<IBreakpoint>> Inner;

        public IEnumerable<IBreakpoint> Breakpoints => this.Inner.Values.SelectMany((it) => it);

        public BreakpointManager()
        {
            this.Inner = new Dictionary<string, List<IBreakpoint>>();
        }
        private void Breakpoint_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is Breakpoint bp)
            {
                this.BreakpointUpdated?.Invoke(this, new BreakpointUpdatedEventArgs(bp.PreChangeCopy, bp));
            }
        }


        public IBreakpoint AddBreakpoint(string file)
        {
            if (String.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(file);
            }
            if (this.Inner.TryGetValue(file, out var val))
            {
                var bp = new Breakpoint(file);
                val.Add(bp);
                this.BreakpointAdded?.Invoke(this, new BreakpointEventArgs(bp));
                bp.PropertyChanged += this.Breakpoint_PropertyChanged;
                return bp;
            }
            else
            {
                var list = new List<IBreakpoint>();
                this.Inner.Add(file, list);
                var bp = new Breakpoint(file);
                list.Add(bp);
                this.BreakpointAdded?.Invoke(this, new BreakpointEventArgs(bp));
                bp.PropertyChanged += this.Breakpoint_PropertyChanged;
                return bp;
            }
        }

        public IEnumerable<IBreakpoint> GetBreakpoints(string file, Func<IBreakpoint, bool> condition)
        {
            if (String.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(file);
            }
            var res = new List<IBreakpoint>();
            if (this.Inner.TryGetValue(file, out var val))
            {
                foreach (var it in val)
                {
                    if (condition(it))
                    {
                        res.Add(it);
                    }
                }
            }
            return res;
        }

        public void RemoveBreakpoint(string file, IBreakpoint bp)
        {
            if (this.Inner.TryGetValue(file, out var val) && val.Remove(bp))
            {
                bp.PropertyChanged -= this.Breakpoint_PropertyChanged;
                this.BreakpointRemoved?.Invoke(this, new BreakpointEventArgs(bp));
            }
        }
    }
}

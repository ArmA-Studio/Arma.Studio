using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Data.UI
{
    public class DockableInfo
    {
        public string Name { get; }
        public string IconSource { get; }
        public Func<DockableBase> Create { get; }

        public DockableInfo(string name, string iconSource, Func<DockableBase> func)
        {
            this.Name = name;
            this.IconSource = iconSource;
            this.Create = func;
        }
    }
}

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
        public Func<DockableBase> CreateFunc { get; }
        public Type Type { get; }

        private DockableInfo(string name, string iconSource, Func<DockableBase> func, Type type)
        {
            this.Name = name;
            this.IconSource = iconSource;
            this.CreateFunc = func;
            this.Type = type;
        }

        public static DockableInfo Create<T>(string name, string iconSource, Func<T> func) where T : DockableBase => new DockableInfo(name, iconSource, func, typeof(T));
    }
}

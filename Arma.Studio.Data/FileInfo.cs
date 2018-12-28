using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data
{
    public class FileInfo
    {
        public string Name { get; }
        public string IconSource { get; }
        public Func<DockableBase> CreateFunc { get; }
        public Type Type { get; }

        private FileInfo(string name, string iconSource, Func<DockableBase> func, Type type)
        {
            this.Name = name;
            this.IconSource = iconSource;
            this.CreateFunc = func;
            this.Type = type;
        }

        public static FileInfo Create<T>(string name, string iconSource, Func<T> func) where T : DockableBase, IApp => new FileInfo(name, iconSource, func, typeof(T));
    }
}

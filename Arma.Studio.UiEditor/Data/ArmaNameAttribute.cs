using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.UiEditor.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArmaNameAttribute : Attribute
    {
        public ArmaNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}

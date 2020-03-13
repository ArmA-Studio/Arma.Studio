using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.UiEditor.Data
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ControlTypeAttribute : Attribute
    {
        public ControlTypeAttribute(Type t)
        {
            this.TargetType = t;
        }
        public string IconPath { get; set; }
        public Type TargetType { get; }
    }
}

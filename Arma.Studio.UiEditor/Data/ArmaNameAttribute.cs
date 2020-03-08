using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.UiEditor.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArmaNameAttribute : PropertyAttribute
    {
        public ArmaNameAttribute(string title) : base(title)
        {
        }
    }
}

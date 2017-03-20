using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ArmA.Studio.Data.Configuration;

namespace ArmA.Studio.Dialogs.PropertiesDialogUtil
{
    public class ColorConfigEntry : ConfigEntry
    {
        private MethodInfo PropertySetter;

        public ColorConfigEntry(string root, string option, object value, MethodInfo setMethod) : base(root, option, value, EEditTemplate.Color)
        {
            this.PropertySetter = setMethod;
        }

        public override void OnValueChanged(object oldValue, object newValue)
        {
            this.PropertySetter.Invoke(null, new object[] { newValue });
        }
    }
}

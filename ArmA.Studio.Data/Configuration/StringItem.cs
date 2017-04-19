using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArmA.Studio.Data.Configuration
{
    public class StringItem : PropertyItem
    {
        private readonly static DataTemplate StringItemDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(StringItem).Assembly, "ArmA.Studio.Data.Configuration.StringItem.xaml");
        private readonly Func<string, bool> IsValidFunction;

        public StringItem(string name, string path, object propertyOwner) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
        public StringItem(string name, string path, object propertyOwner, Func<string, bool> isValid) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, isValid) { }
        public StringItem(string name, PropertyInfo property, object propertyOwner) : this(name, string.Empty, property, propertyOwner, (v) => true) { }
        public StringItem(string name, PropertyInfo property, object propertyOwner, Func<string, bool> isValid) : this(name, string.Empty, property, propertyOwner, isValid) { }
        public StringItem(string name, string icon, string path, object propertyOwner) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
        public StringItem(string name, string icon, string path, object propertyOwner, Func<string, bool> isValid) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner,
             isValid) { }
        public StringItem(string name, string icon, PropertyInfo property, object propertyOwner) : this(name, icon, property, propertyOwner, (v) => true) { }
        public StringItem(string name, string icon, PropertyInfo property, object propertyOwner, Func<string, bool> isValid) : base(StringItemDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
            this.IsValidFunction = isValid;
        }

        public override bool IsValidValue(object value)
        {
            return value is string ? this.IsValidFunction(value as string) : false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArmA.Studio.Data.Configuration
{
    public class BoolItem : PropertyItem
    {
        private readonly static DataTemplate ThisDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(BoolItem).Assembly, "ArmA.Studio.Data.Configuration.BoolItem.xaml");

        public BoolItem(string name, string path, object propertyOwner) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner) { }
        public BoolItem(string name, PropertyInfo property, object propertyOwner) : this(name, string.Empty, property, propertyOwner) { }
        public BoolItem(string name, string icon, string path, object propertyOwner) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner) { }
        public BoolItem(string name, string icon, PropertyInfo property, object propertyOwner) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArmA.Studio.Data.Configuration
{
    public class ComboBoxItem<T> : PropertyItem
    {
        private readonly static DataTemplate ThisDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(ComboBoxItem<T>).Assembly, "ArmA.Studio.Data.Configuration.ComboBoxItem.xaml");
        public IEnumerable<KeyValuePair<string, T>> KeyValueCollection { get; private set; }

        public ComboBoxItem(IEnumerable<KeyValuePair<string, T>> values, string name, string path, object propertyOwner) : this(values, name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner ) { }
        public ComboBoxItem(IEnumerable<KeyValuePair<string, T>> values, string name, string icon, string path, object propertyOwner) : this(values, name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner ) { }
        public ComboBoxItem(IEnumerable<KeyValuePair<string, T>> values, string name, PropertyInfo property, object propertyOwner) : this(values, name, string.Empty, property, propertyOwner ) { }
        public ComboBoxItem(IEnumerable<KeyValuePair<string, T>> values, string name, string icon, PropertyInfo property, object propertyOwner) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
            this.KeyValueCollection = values;
        }
    }
}

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

        public Func<object, object> ValueChangeCallback { get; set; } = null;

        public ComboBoxItem(IEnumerable<KeyValuePair<string, T>> values, string name, string path, object propertyOwner) : this(values, name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner ) { }
        public ComboBoxItem(IEnumerable<KeyValuePair<string, T>> values, string name, string icon, string path, object propertyOwner) : this(values, name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner ) { }
        public ComboBoxItem(IEnumerable<KeyValuePair<string, T>> values, string name, PropertyInfo property, object propertyOwner) : this(values, name, string.Empty, property, propertyOwner ) { }
        public ComboBoxItem(IEnumerable<KeyValuePair<string, T>> values, string name, string icon, PropertyInfo property, object propertyOwner) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
            this.KeyValueCollection = values;
        }

        /// <summary>
        /// Callback to provide new value for value change.
        /// Will be callen after value was validated.
        /// Determines the final value.
        /// </summary>
        /// <param name="oldValue">The old value that was used.</param>
        /// <param name="newValue">The new value that was passed.</param>
        /// <returns>The new value that will be set.</returns>
        public override object OnValueChange( object oldValue, object newValue )
        {
            var value = base.OnValueChange( oldValue, newValue );
            if ( ValueChangeCallback != null )
            {
                value = ValueChangeCallback( value );
            }
            return value;
        }
    }
}

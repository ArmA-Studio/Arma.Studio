using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArmA.Studio.Data.Configuration
{
    public abstract class PropertyItem : Item
    {
        protected readonly PropertyInfo Property;
        protected readonly object Owner;

        public PropertyItem(DataTemplate template, PropertyInfo property, object propertyOwner) : this(template, property, propertyOwner, property.GetValue(propertyOwner)) { }
        public PropertyItem(DataTemplate template, PropertyInfo property, object propertyOwner, object value) : base(template, value)
        {
            this.Property = property;
            this.Owner = propertyOwner;
        }

        public override bool IsValidValue(object value)
        {
            return true;
        }

        public override object OnValueChange(object oldValue, object newValue)
        {
            if (!this.CanConvert(newValue))
            {
                return oldValue;
            }
            var val = this.DoConversion(newValue);
            this.Property.SetValue(this.Owner, val);
            return this.Property.GetValue(this.Owner);
        }

        protected virtual object DoConversion(object value) => value;
        protected virtual bool CanConvert(object value) => true;
    }
}

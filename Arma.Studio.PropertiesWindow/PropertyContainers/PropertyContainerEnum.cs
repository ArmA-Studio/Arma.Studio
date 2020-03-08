using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.PropertiesWindow.PropertyContainers
{
    public class PropertyContainerEnum : PropertyContainerBase
    {
        public class EnumValue
        {
            public object Data { get; }
            public string Label { get; }
            public EnumValue(object data, string label)
            {
                this.Data = data;
                this.Label = label;
            }
        }
        public IEnumerable<EnumValue> EnumValues { get; }
        public PropertyContainerEnum(string title, string tooltip, object data, string propertyName, IEnumerable<EnumValue> enumValues, Func<object, object> getFunc, Action<object, object> setFunc) :
            base(title, tooltip, data, propertyName, getFunc, setFunc)
        {
            this.EnumValues = enumValues.ToArray();
        }

        /// <summary>
        /// Creates a new <see cref="PropertyContainerEnum"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerEnum Create(object data, PropertyInfo propertyInfo)
        {
            if (!propertyInfo.PropertyType.IsEnum)
            {
                throw new ArgumentException("Property is no enum.", nameof(propertyInfo));
            }
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }

            var enumValues = Enum.GetValues(propertyInfo.PropertyType);
            return new PropertyContainerEnum(
                attribute.Title,
                attribute.Description,
                data,
                propertyInfo.Name,
                enumValues.Cast<object>().Select((it) => new EnumValue(it, Studio.Data.UI.Converters.EnumNameConverter.Instance.Convert(propertyInfo.PropertyType, it))),
                (obj) => propertyInfo.GetValue(obj, null),
                (obj, val) => propertyInfo.SetValue(obj, val, null));
        }
    }
}

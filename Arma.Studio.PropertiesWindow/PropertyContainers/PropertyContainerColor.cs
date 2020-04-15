using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.PropertiesWindow.PropertyContainers
{
    public class PropertyContainerColor : PropertyContainerBase
    {
        public PropertyContainerColor(string title, string tooltip, object data, string propertyName, Func<object, Color> getFunc, Action<object, Color> setFunc) :
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (Color)val))
        { }

        /// <summary>
        /// Creates a new <see cref="PropertyContainerColor"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerColor Create(object data, PropertyInfo propertyInfo)
        {
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            var container = new PropertyContainerColor(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (Color)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
            container.Stepsize = attribute.Stepsize;
            container.MinValue = attribute.MinValue;
            container.MaxValue = attribute.MaxValue;
            return container;
        }
    }
}

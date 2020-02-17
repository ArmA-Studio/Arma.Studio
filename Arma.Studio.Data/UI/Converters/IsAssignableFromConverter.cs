using System;
using System.Globalization;
using System.Windows.Data;

namespace Arma.Studio.Data.UI.Converters
{
    /// <summary>
    /// Compares the <see cref="Type"/> passed in parameter with the <see cref="Type"/> of passed value using <see cref="Type.IsAssignableFrom(Type)"/>.
    /// <see cref="IsAssignableFromConverter.ConvertBack(Object, Type, Object, CultureInfo)"/> will always return <see cref="System.Windows.DependencyProperty.UnsetValue"/>.
    /// </summary>
    public class IsAssignableFromConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type type;
            if (parameter is string s)
            {
                type = Type.GetType(s, false);

            }
            else if (parameter is Type)
            {
                type = (Type)parameter;
            }
            else
            {
                return value;
            }
            if (type == null)
            {
                return value;
            }
            return value == null ? true : type.IsAssignableFrom(value.GetType());
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Windows.DependencyProperty.UnsetValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Arma.Studio.Data.UI.Converters
{
    /// <summary>
    /// Inverts the incoming <see cref="Boolean"/> value.
    /// </summary>
    public class InvertConverter : IValueConverter
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
            if (value is bool flag)
            {
                return !flag;
            }
            else if (value is short s)
            {
                return -s;
            }
            else if (value is int i)
            {
                return -i;
            }
            else if (value is long l)
            {
                return -l;
            }
            else if (value is float f)
            {
                return -f;
            }
            else if (value is double d)
            {
                return -d;
            }
            else if (value is decimal dec)
            {
                return -dec;
            }
            else
            {
                return value;
            }
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
            value = System.Convert.ChangeType(value, targetType);
            if (value is bool flag)
            {
                return !flag;
            }
            else if (value is short s)
            {
                return -s;
            }
            else if (value is int i)
            {
                return -i;
            }
            else if (value is long l)
            {
                return -l;
            }
            else if (value is float f)
            {
                return -f;
            }
            else if (value is double d)
            {
                return -d;
            }
            else if (value is decimal dec)
            {
                return -dec;
            }
            else
            {
                return value;
            }
        }
    }
}

using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Arma.Studio.Data.UI.Converters
{
    public class IsNullConverter : IValueConverter
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
            bool success = true;
            if (parameter is string para && (para.Equals("true", StringComparison.InvariantCultureIgnoreCase) || para.Equals("false", StringComparison.InvariantCultureIgnoreCase)))
            {
                success = System.Convert.ToBoolean(para);
            }

            if (value is string s)
            {
                return String.IsNullOrWhiteSpace(s) ? success : !success;
            }
            else
            {
                return value == null ? success : !success;
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
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}

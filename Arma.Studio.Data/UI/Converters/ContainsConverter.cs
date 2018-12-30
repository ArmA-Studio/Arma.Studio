using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Arma.Studio.Data.UI.Converters
{
    public class ContainsConverter : IValueConverter
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
            if (value is IEnumerable enumerable)
            {
                var tc = new System.ComponentModel.TypeConverter();
                Type t = null;
                object para = null;
                foreach (var it in enumerable)
                {
                    if (t == null)
                    {
                        t = it.GetType();
                        para = tc.ConvertTo(parameter, t);
                    }
                    if (it?.Equals(para) ?? false)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return value == parameter;
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

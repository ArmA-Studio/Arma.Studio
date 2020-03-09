using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Data;

namespace Arma.Studio.Data.UI.Converters
{
    /// <summary>
    /// Converts the passed <see cref="Enum"/> value to the corresponding <see cref="String"/> representing its name.
    /// Requires the parameter to be the <see cref="Type"/> of the enum to convert.
    /// Can convert backwards.
    /// </summary>
    public class EnumSourceConverter : IValueConverter
    {
        public static EnumSourceConverter Instance { get => _Instance; set => _Instance = value; }
        private static EnumSourceConverter _Instance;

        static EnumSourceConverter()
        {
            _Instance = new EnumSourceConverter();
        }
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
            if (value is Type t && t.IsEnum)
            {
                return Enum.GetValues(t);
            }
            return null;
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
            throw new NotImplementedException();
        }
    }
}

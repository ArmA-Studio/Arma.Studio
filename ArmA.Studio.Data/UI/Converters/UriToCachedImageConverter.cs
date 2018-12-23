using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ArmA.Studio.Data.UI.Converters
{
    public class UriToCachedImageConverter : IValueConverter
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
            if (value is BitmapImage)
            {
                return value;
            }
            else if (value is Uri)
            {
                value = ((Uri)value).ToString();
            }
            if (parameter is Uri)
            {
                parameter = ((Uri)parameter).ToString();
            }

            if (!String.IsNullOrWhiteSpace(value as string))
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(value as string);
                bi.CacheOption = BitmapCacheOption.OnDemand;
                bi.EndInit();
                bi.Freeze();
                return bi;
            }
            if (!String.IsNullOrWhiteSpace(parameter as string))
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(parameter as string);
                bi.CacheOption = BitmapCacheOption.OnDemand;
                bi.EndInit();
                bi.Freeze();
                return bi;
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
            return System.Windows.DependencyProperty.UnsetValue;
        }
    }
}

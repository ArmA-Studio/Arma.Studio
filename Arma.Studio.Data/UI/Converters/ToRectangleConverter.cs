using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Arma.Studio.Data.UI.Converters
{
    public class ToRectangleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)System.Convert.ChangeType(value, typeof(double));
            return new Rect(0, 0, d, d);
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            if (value is Rect rect)
            {
                return System.Convert.ChangeType(rect.X, targetTypes);
            }
            return value;
        }
    }
}

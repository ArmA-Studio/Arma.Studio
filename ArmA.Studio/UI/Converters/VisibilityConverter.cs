using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ArmA.Studio.UI.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visArr = new Visibility[] { Visibility.Hidden, Visibility.Visible };
            if (parameter != null && parameter is string)
            {
                var split = (parameter as string).Split('|', ',', ';');
                if (split.Length > 0)
                {
                    if (!Enum.TryParse(split[0], out visArr[1]))
                    {
                        throw new ArgumentException("provided value at pos 0 is no valid child of Visibility enum", nameof(parameter));
                    }
                }
                if (split.Length > 1)
                {
                    if (!Enum.TryParse(split[1], out visArr[0]))
                    {
                        throw new ArgumentException("provided value at pos 1 is no valid child of Visibility enum", nameof(parameter));
                    }
                }
                if (split.Length > 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(parameter), "Maximum ammount of visibility parameters for conversion is 2");
                }
            }
            if (value is bool)
            {
                return visArr[(bool)value ? 1 : 0];
            }
            else
            {
                throw new ArgumentException("provided value is not of type bool", nameof(value));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vis = Visibility.Visible;
            if (parameter != null && parameter is string)
            {
                var split = (parameter as string).Split('|', ',', ';');
                if (split.Length > 0)
                {
                    if (!Enum.TryParse(split[0], out vis))
                    {
                        throw new ArgumentException("provided value at pos 0 is no valid child of Visibility enum", nameof(parameter));
                    }
                }
            }

            if (value is Visibility)
            {
                return vis == (Visibility)value;
            }
            else
            {
                throw new ArgumentException("provided value is not of type Visibility", nameof(value));
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ArmA.Studio.UI.Converters
{
    public class SolutionExplorerItemVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string)
            {
                bool flag;
                bool.TryParse(parameter as string, out flag);
                parameter = flag;
            }
            if (value is bool)
            {
                if (parameter is bool && (bool)parameter)
                {
                    return (bool)value ? Visibility.Collapsed : Visibility.Visible;
                }
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
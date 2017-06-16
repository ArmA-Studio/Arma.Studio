using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ArmA.Studio.UI.Converters
{
    /// <summary>
    /// Provides the ability to check if provided value is null.
    /// If no parameter is provided, <see cref="Convert(object, Type, object, CultureInfo)"/> will return true if value is null and false if it is not.
    /// If parameter is of type bool and true, <see cref="Convert(object, Type, object, CultureInfo)"/> will return false if value is null and true if it is not.
    /// </summary>
    public class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag;
            if (parameter is string)
            {
                bool.TryParse(parameter as string, out flag);
            }
            else if (parameter is bool)
            {
                flag = (bool)parameter;
            }
            else
            {
                flag = false;
            }
            return flag ? value != null : value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
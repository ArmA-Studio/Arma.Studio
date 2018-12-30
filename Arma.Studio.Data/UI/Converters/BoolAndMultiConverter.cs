using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Arma.Studio.Data.UI.Converters
{
    public class BoolAndMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = false;
            bool flag;
            foreach(var it in values)
            {
                if ((it is bool))
                    ret = ret && (bool)it;
            }
            if (parameter != null && bool.TryParse(parameter as string, out flag) && flag)
            {
                return !ret;
            }
            else
            {
                return ret;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

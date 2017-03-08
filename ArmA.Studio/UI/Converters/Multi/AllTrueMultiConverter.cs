using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ArmA.Studio.UI.Converters.Multi
{
    public class AllTrueMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = true;
            if(parameter != null)
            {
                ret = bool.Parse(parameter as string);
            }
            foreach(var it in values)
            {
                if (!(it is bool) || !((bool)it))
                    return !ret;
            }
            return ret;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

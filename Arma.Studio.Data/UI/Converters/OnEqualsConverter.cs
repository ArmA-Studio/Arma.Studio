using System;
using System.Globalization;
using System.Windows.Data;

namespace Arma.Studio.Data.UI.Converters
{
    public class OnEqualsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string pstring))
            {
                return value.GetType().IsEquivalentTo(parameter.GetType()) ? value.Equals(parameter) : value;
            }
            string[] parr = pstring.Split('|');
            var valueType = value.GetType();
            if (value.Equals(valueType.IsEnum ? Enum.Parse(valueType, parr[0]) : System.Convert.ChangeType(parr[0], value.GetType())))
            {
                return System.Convert.ChangeType(parr[1], targetType);
            }

            return parr.Length == 3 ? System.Convert.ChangeType(parr[2], targetType) : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

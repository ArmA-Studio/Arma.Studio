using Arma.Studio.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Arma.Studio.UiEditor.UI.Converters
{
    public class ControlTextImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                str = str.TrimStart('\\', '/', ' ', '\t');
                var pbo = (Application.Current as IApp).MainWindow.FileManagement.FirstOrDefault((it) => str.StartsWith(it.FullPath, StringComparison.InvariantCultureIgnoreCase) || str.StartsWith(it.Prefix, StringComparison.InvariantCultureIgnoreCase));
                if (pbo is null)
                {
                    return null;
                }
                if (str.StartsWith(pbo.Prefix))
                {
                    return System.IO.Path.Combine(pbo.FullPath, str.Substring(pbo.Prefix.Length).TrimStart('\\', '/', ' ', '\t'));
                }
                else
                {
                    return str;
                }
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

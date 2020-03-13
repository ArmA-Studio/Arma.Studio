using Arma.Studio.UiEditor.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Arma.Studio.UiEditor.UI.Converters
{
    public class SizeExToFontSizeMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3 || !(values[0] is double fontSize) || !(values[1] is InterfaceSize interfaceSize) || !(values[2] is CanvasManager canvasManager))
            {
                return -1;
            }
            var res = interfaceSize.SizeExToFontSize(fontSize, canvasManager.Height);
            return res <= 0 ? 1 : res;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

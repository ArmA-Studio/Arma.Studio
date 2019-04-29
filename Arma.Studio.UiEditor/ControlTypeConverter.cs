using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Arma.Studio.UiEditor
{
    public class ControlTypeConverter : IValueConverter
    {
        public static DependencyProperty TypeProperty = DependencyProperty.RegisterAttached("Type", typeof(EControlType), typeof(ControlTypeConverter));
        public static void SetType(DependencyObject target, EControlType value) => target.SetValue(TypeProperty, value);
        public static EControlType GetType(DependencyObject target) => (EControlType)target.GetValue(TypeProperty);
        public static bool TryGetType(DependencyObject target, out EControlType type)
        {
            var val = target.GetValue(TypeProperty);
            if (val == null)
            {
                type = default(EControlType);
                return false;
            }
            else
            {
                type = (EControlType)val;
                return true;
            }
        }
        public DataTemplate Default { get; set; }
        public List<ControlTypeContainer> Templates { get; set; }

        public ControlTypeConverter()
        {
            this.Templates = new List<ControlTypeContainer>();
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EControlType type)
            {
                var first = this.Templates.FirstOrDefault((it) => it.Type == type);
                return first == null ? this.Default : first.Template;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

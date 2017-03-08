using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Documents;
using Utility;

namespace ArmA.Studio.UI.Attached
{
    public class SelectFileNameOnFocusAttached
    {
        public enum EInputType
        {
            NA,
            Numeric
        }

        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(SelectFileNameOnFocusAttached), new FrameworkPropertyMetadata(false, InputTypePropertyChanged));

        public static object GetAttach(TextBox control)
        {
            return control.GetValue(AttachProperty);
        }

        public static void SetAttach(TextBox control, bool value)
        {
            control.SetValue(AttachProperty, value);
        }
        private static void InputTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tb = d as TextBox;
            if (tb == null)
                return;
            if ((bool)e.NewValue)
            {
                tb.GotFocus += TextBox_GotFocus;
            }
            else
            {
                tb.GotFocus -= TextBox_GotFocus;
            }
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null)
                return;
            tb.Select(0, System.IO.Path.GetFileNameWithoutExtension(tb.Text).Length);
        }
    }
}

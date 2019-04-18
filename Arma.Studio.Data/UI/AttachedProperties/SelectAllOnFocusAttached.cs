using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arma.Studio.Data.UI.AttachedProperties
{
    public class SelectAllOnFocusAttached
    {
        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
                typeof(bool),
                typeof(SelectAllOnFocusAttached),
                new FrameworkPropertyMetadata(false, InputTypePropertyChanged));

        public static object GetAttach(TextBox control) => control.GetValue(AttachProperty);

        public static void SetAttach(TextBox control, bool value) => control.SetValue(AttachProperty, value);
        private static void InputTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tb)
            {
                if ((bool)e.NewValue)
                {
                    tb.GotFocus += TextBox_GotFocus;
                }
                else
                {
                    tb.GotFocus -= TextBox_GotFocus;
                }
            }
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.SelectAll();
            }
        }
    }
}

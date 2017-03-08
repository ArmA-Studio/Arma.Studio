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
    public class DialogResultAttached
    {
        public enum EInputType
        {
            NA,
            Numeric
        }

        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(DialogResultAttached), new FrameworkPropertyMetadata(null, DialogResultPropertyChanged));

        public static object GetDialogResult(Window control)
        {
            return control.GetValue(DialogResultProperty);
        }

        public static void SetDialogResult(Window control, object value)
        {
            control.SetValue(DialogResultProperty, value);
        }
        private static void DialogResultPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wnd = d as Window;
            if (wnd == null)
                return;
            wnd.DialogResult = e.NewValue as bool?;
        }
    }
}

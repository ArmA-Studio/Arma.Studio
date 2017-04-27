using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace ArmA.Studio.UI.Attached
{
    public class KeyboardFocusManager
    {
        public static DependencyProperty FocusedElementProperty =
            DependencyProperty.RegisterAttached("FocusedElement",
            typeof(Control),
            typeof(KeyboardFocusManager),
            new UIPropertyMetadata(FocusedElementPropertyChanged));

        public static void SetFocusedElement(DependencyObject target, Control control)
        {
            target.SetValue(FocusedElementProperty, control);
        }

        public static Control GetFocusedElement(DependencyObject target)
        {
            return target.GetValue(FocusedElementProperty) as Control;
        }

        private static void FocusedElementPropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if ((e.NewValue != null) && (e.OldValue == null))
            {
                target.Dispatcher.BeginInvoke((Action)delegate
                {
                    Keyboard.Focus(GetFocusedElement(target) as Control);
                }, DispatcherPriority.Render);
            }
        }
    }
}

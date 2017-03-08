using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Utility;

namespace ArmA.Studio.UI.Attached
{
    public class LooseFocusOnKeyAttached
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.RegisterAttached("Key", typeof(Key), typeof(LooseFocusOnKeyAttached), new FrameworkPropertyMetadata(default(Key), InputTypePropertyChanged));

        public static Key GetKey(TextBox control)
        {
            return (Key)control.GetValue(KeyProperty);
        }

        public static void SetKey(TextBox control, Key value)
        {
            control.SetValue(KeyProperty, value);
        }
        private static void InputTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tb = d as TextBox;
            if (tb == null)
                return;
            if (((Key)e.NewValue != Key.None) && ((Key)e.OldValue == Key.None))
            {
                tb.KeyDown += TextBox_KeyDown;
            }
            else if (((Key)e.NewValue == Key.None) && ((Key)e.OldValue != Key.None))
            {
                tb.KeyDown -= TextBox_KeyDown;
            }
        }

        private static void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null)
                return;
            if (e.Key == GetKey(tb))
            {
                Keyboard.ClearFocus();
            }
        }
    }
}

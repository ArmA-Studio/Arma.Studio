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
    public class TextboxInputTypeAttached
    {
        public enum EInputType
        {
            NA,
            Numeric
        }

        public static readonly DependencyProperty InputTypeProperty = DependencyProperty.RegisterAttached("InputType", typeof(EInputType), typeof(TextboxInputTypeAttached), new FrameworkPropertyMetadata(null, InputTypePropertyChanged));

        public static object GetInputType(TextBox control)
        {
            return control.GetValue(InputTypeProperty);
        }

        public static void SetInputType(TextBox control, object value)
        {
            control.SetValue(InputTypeProperty, value);
        }
        private static void InputTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tb = d as TextBox;
            if (tb == null)
                return;
            if (e.OldValue != null && e.OldValue is EInputType)
            {
                switch ((EInputType)e.OldValue)
                {
                    case EInputType.Numeric:
                        tb.PreviewTextInput -= TextBox_PreviewTextInput_NumericOnly;
                        break;
                }
            }
            if(e.NewValue != null && e.NewValue is EInputType)
            {
                switch ((EInputType)e.NewValue)
                {
                    case EInputType.Numeric:
                        tb.PreviewTextInput += TextBox_PreviewTextInput_NumericOnly;
                        break;
                }
            }
        }

        private static void TextBox_PreviewTextInput_NumericOnly(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (e.Text.Contains('\r'))
            {
                System.Windows.Input.Keyboard.ClearFocus();
            }
            else
            {
                var currentSelection = tb.SelectionStart;
                string text = tb.Text;
                if (tb.SelectionLength > 0)
                {
                    text = text.Remove(tb.SelectionStart, tb.SelectionLength);
                }
                text = text.Insert(tb.SelectionStart, e.Text);
                currentSelection += e.Text.Length;
                if (text.IsNumeric())
                {
                    tb.Text = text;
                    tb.SelectionStart = currentSelection;
                }
            }
            e.Handled = true;
        }
    }
}

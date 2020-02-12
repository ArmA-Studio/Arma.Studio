using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Arma.Studio.OutputWindow
{
    public class AvalonEditAttachedProperties
    {
        public static readonly DependencyProperty AutoscrollProperty = DependencyProperty.RegisterAttached(
            "Autoscroll",
            typeof(bool),
            typeof(AvalonEditAttachedProperties),
            new FrameworkPropertyMetadata(default(bool), InputTypePropertyChanged));

        public static bool GetAutoscroll(ICSharpCode.AvalonEdit.TextEditor control)
        {
            return (bool)control.GetValue(AutoscrollProperty);
        }

        public static void SetAutoscroll(ICSharpCode.AvalonEdit.TextEditor control, bool value)
        {
            control.SetValue(AutoscrollProperty, value);
        }
        private static void InputTypePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is ICSharpCode.AvalonEdit.TextEditor textEditor)
            {
                if ((bool)e.NewValue && !(bool)e.OldValue)
                {
                    textEditor.TextChanged += TextEditor_TextChanged;
                }
                else if ((bool)e.OldValue && !(bool)e.NewValue)
                {
                    textEditor.TextChanged -= TextEditor_TextChanged;
                }
            }
        }

        private static void TextEditor_TextChanged(object sender, EventArgs e)
        {
            if (sender is ICSharpCode.AvalonEdit.TextEditor textEditor)
            {
                textEditor.ScrollToEnd();
            }
        }
    }
}

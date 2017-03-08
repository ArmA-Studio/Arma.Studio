using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArmA.Studio.UI.Attached.Eventing
{
    public class PreviewKeyDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewKeyDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewKeyDown),
                                                new UIPropertyMetadata(null));

        public static DependencyProperty PreviewKeyDownHandledProperty =
            DependencyProperty.RegisterAttached("PreviewKeyDownHandled",
                                                typeof(bool),
                                                typeof(PreviewKeyDown),
                                                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static void SetCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(CommandProperty, value);
        }

        public static void SetCommandParameter(DependencyObject target, object value)
        {
            target.SetValue(CommandParameterProperty, value);
        }
        public static object GetCommandParameter(DependencyObject target)
        {
            return target.GetValue(CommandParameterProperty);
        }
        public static void SetPreviewKeyDownHandled(DependencyObject target, bool value)
        {
            target.SetValue(PreviewKeyDownHandledProperty, value);
        }
        public static bool GetPreviewKeyDownHandled(DependencyObject target)
        {
            return (bool)target.GetValue(PreviewKeyDownHandledProperty);
        }

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("KeyDown");
            var method = typeof(PreviewKeyDown).GetMethod("OnPreviewKeyDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var control = sender as FrameworkElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
            e.Handled = GetPreviewKeyDownHandled(control);
            SetPreviewKeyDownHandled(control, false);
        }
    }

}

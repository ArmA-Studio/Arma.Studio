using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ArmA.Studio.Data.UI.AttachedProperties.Eventing
{
    public class Checked
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Checked),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Checked),
                                                new UIPropertyMetadata(null));

        public static void SetCommand(ToggleButton target, ICommand value)
        {
            target.SetValue(CommandProperty, value);
            
        }

        public static void SetCommandParameter(ToggleButton target, object value)
        {
            target.SetValue(CommandParameterProperty, value);
        }
        public static object GetCommandParameter(ToggleButton target)
        {
            return target.GetValue(CommandParameterProperty);
        }

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Checked");
            var method = typeof(Checked).GetMethod("OnChecked");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnChecked(object sender, EventArgs e)
        {
            var control = sender as DependencyObject;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }

}

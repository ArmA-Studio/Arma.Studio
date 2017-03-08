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
    public class ContextMenuOpening
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(ContextMenuOpening),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ContextMenuOpening),
                                                new UIPropertyMetadata(null));

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

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("ContextMenuOpening");
            var method = typeof(ContextMenuOpening).GetMethod("OnContextMenuOpening");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var control = sender as FrameworkElement;
            if (control.ContextMenu != null)
            {
                var command = (ICommand)control.GetValue(CommandProperty);
                var commandParameter = control.GetValue(CommandParameterProperty);
                command.Execute(commandParameter);
                control.ContextMenu.DataContext = control.DataContext;
                control.ContextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

    }
}
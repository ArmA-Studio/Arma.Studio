using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock;

namespace ArmA.Studio.UI.AvalonDock
{
    public class ActiveContentChangedAttached
    {
        public enum EInputType
        {
            NA,
            Numeric
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ActiveContentChangedAttached), new FrameworkPropertyMetadata(null, CommandPropertyPropertyChanged));

        public static ICommand GetCommand(DockingManager control)
        {
            return control.GetValue(CommandProperty) as ICommand;
        }

        public static void SetCommand(DockingManager control, ICommand value)
        {
            control.SetValue(CommandProperty, value);
        }
        private static void CommandPropertyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dm = d as DockingManager;
            if (dm == null)
            {
                return;
            }

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                dm.ActiveContentChanged += Dm_ActiveContentChanged;
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                dm.ActiveContentChanged -= Dm_ActiveContentChanged;
            }
        }

        private static void Dm_ActiveContentChanged(object sender, EventArgs e)
        {
            var dm = sender as DockingManager;
            if (dm == null)
            {
                return;
            }

            var cmd = GetCommand(dm);
            cmd.Execute(dm);
        }
    }
}

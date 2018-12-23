using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ArmA.Studio.UI.AvalonDock
{
    public class AvalonDockStyleSelector : StyleSelector
    {
        public Style Style { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            return this.Style;
        }
    }
}

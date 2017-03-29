using System.Windows;
using System.Windows.Controls;
using ArmA.Studio.Data.UI;

namespace ArmA.Studio.UI.DataTemplates
{
    public class AvalonDockStyleSelector : StyleSelector
    {
        public Style DocumentBaseStyle { get; set; }
        public Style PanelBaseStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is DocumentBase)
                return this.DocumentBaseStyle;
            if (item is PanelBase)
                return this.PanelBaseStyle;

            return base.SelectStyle(item, container);
        }
    }
}

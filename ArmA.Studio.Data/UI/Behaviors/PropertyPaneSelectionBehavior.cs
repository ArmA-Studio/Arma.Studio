using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Controls.Primitives;

namespace ArmA.Studio.Data.UI.Behaviors
{
    public class PropertyPaneSelectionBehavior : Behavior<ItemsControl>
    {
        protected override void OnAttached()
        {
            if(this.AssociatedObject is Selector)
            {
                (this.AssociatedObject as Selector).SelectionChanged += Selector_SelectionChanged;
            }
            else if(this.AssociatedObject is TreeView)
            {
                (this.AssociatedObject as TreeView).SelectedItemChanged += TreeView_SelectedItemChanged;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected override void OnDetaching()
        {
            if (this.AssociatedObject is Selector)
            {
                (this.AssociatedObject as Selector).SelectionChanged -= Selector_SelectionChanged;
            }
            else if (this.AssociatedObject is TreeView)
            {
                (this.AssociatedObject as TreeView).SelectedItemChanged -= TreeView_SelectedItemChanged;
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        private void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PropertyPaneSelectionStatic.Instance.Provider = (sender as Selector).SelectedItem as IPropertyPaneProvider;
        }
        private void TreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            PropertyPaneSelectionStatic.Instance.Provider = (sender as TreeView).SelectedItem as IPropertyPaneProvider;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Controls.Primitives;
using Utility;

namespace ArmA.Studio.UI.Attached
{
    public class ChangePropertiesViewCurrentAttached : Behavior<Selector>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
        }


        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Workspace.CurrentWorkspace == null)
                return;
            Workspace.CurrentWorkspace.CurrentSelectedProperty = (sender as Selector).SelectedItem as ViewModel.IPropertyDatatemplateProvider;
        }
    }
}

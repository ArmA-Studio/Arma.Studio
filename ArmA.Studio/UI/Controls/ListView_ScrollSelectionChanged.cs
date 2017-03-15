using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArmA.Studio.UI.Controls
{
    public class ListView_ScrollSelectionChanged : ListView
    {
        public ListView_ScrollSelectionChanged() : base()
        {
            this.SelectionChanged += ListBox_ScrollSelectionChanged_SelectionChanged;
        }

        private void ListBox_ScrollSelectionChanged_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ScrollIntoView(this.SelectedItem);
        }
    }
}

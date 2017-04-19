using System.Windows;
using System.Windows.Input;
using ArmA.Studio.Data.UI;
using ArmA.Studio.Data.UI.Commands;

namespace ArmA.Studio.DataContext
{
    public class CallStackPane : PanelBase
    {
        public override string Title { get { return Properties.Localization.PanelDisplayName_CallStack; } }
        public override string Icon { get { return @"Resources\Pictograms\CallStackWindow\CallStackWindow_16x.png"; } }

        public ICommand CmdEntryOnDoubleClick { get { return new RelayCommand((p) => { MessageBox.Show("To be implemented"); }); } }

        

        public CallStackPane()
        {
        }
    }
}
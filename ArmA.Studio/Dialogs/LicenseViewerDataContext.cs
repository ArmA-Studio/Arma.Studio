using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;

namespace ArmA.Studio.Dialogs
{
    public class LicenseViewerDataContext : IDialogContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ObservableCollection<License> Licenses { get; set; }
        public ICommand CmdOKButtonPressed { get; private set; }
        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }


        public string WindowHeader { get { return Properties.Localization.LicenseViewer_Header; } }
        public string OKButtonText { get { return Properties.Localization.OK; } }
        public bool OKButtonEnabled { get { return true; } }

        private bool? _DialogResult;
        public LicenseViewerDataContext()
        {
            this.CmdOKButtonPressed = new UI.Commands.RelayCommand(Cmd_OKButtonPressed);
            var arr = App.Current.TryFindResource("Licenses") as Array;
            this.Licenses = new ObservableCollection<License>(arr.Cast<License>());
        }
        public void Cmd_OKButtonPressed(object param)
        {
            this.DialogResult = true;
        }
    }
}

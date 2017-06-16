using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ArmA.Studio.Data.UI.Commands;
using Utility;

namespace ArmA.Studio.Dialogs
{
    public class AboutDialogDataContext : IDialogContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ObservableCollection<Data.LicenseInfo> Licenses { get; set; }
        public ICommand CmdOKButtonPressed => new RelayCommand((p) => this.DialogResult = true);
        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;

        public string ToolVersion => App.CurrentVersion.ToString();


        public string WindowHeader => Properties.Localization.AboutDialog_Header;
        public string OKButtonText => Properties.Localization.OK;
        public bool OKButtonEnabled => true;
    }
}

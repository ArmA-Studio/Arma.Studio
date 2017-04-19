using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ArmA.Studio.Data.UI.Commands;
using Utility;

namespace ArmA.Studio.Dialogs
{
    public class LicenseViewerDataContext : IDialogContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ObservableCollection<Data.LicenseInfo> Licenses { get; set; }
        public ICommand CmdOKButtonPressed { get; private set; }
        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;


        public string WindowHeader { get { return Properties.Localization.LicenseViewer_Header; } }
        public string OKButtonText { get { return Properties.Localization.OK; } }
        public bool OKButtonEnabled { get { return true; } }

        public LicenseViewerDataContext()
        {
            this.CmdOKButtonPressed = new RelayCommand(Cmd_OKButtonPressed);
            var arr = App.Current.TryFindResource("Licenses") as Array;
            this.Licenses = new ObservableCollection<Data.LicenseInfo>(arr.Cast<Data.LicenseInfo>().Concat(App.GetPlugins<Plugin.ILicenseProviderPlugin>().SelectMany((p) => p.LicenseInfos)));
        }
        public void Cmd_OKButtonPressed(object param)
        {
            this.DialogResult = true;
        }
    }
}

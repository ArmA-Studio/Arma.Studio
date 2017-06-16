using System.ComponentModel;
using System.Windows.Input;
using ArmA.Studio.Data.UI.Commands;

namespace ArmA.Studio.Dialogs
{
    public class ReportDialogDataContext : IDialogContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ICommand CmdOKButtonPressed => new RelayCommand((p) => { this.DialogResult = true; });

        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;

        public string ReportText { get { return this._ReportText; } set { this._ReportText = value; this.OKButtonEnabled = !string.IsNullOrWhiteSpace(value); this.RaisePropertyChanged(); } }
        private string _ReportText;

        public string WindowHeader => Properties.Localization.ReportDialog_Header;

        public string OKButtonText => Properties.Localization.OK;

        public bool OKButtonEnabled { get { return this._OKButtonEnabled; } set { if (this._OKButtonEnabled == value) return; this._OKButtonEnabled = value; this.RaisePropertyChanged(); } }
        private bool _OKButtonEnabled;

        public ReportDialogDataContext()
        {
            this._OKButtonEnabled = false;
        }
    }
}
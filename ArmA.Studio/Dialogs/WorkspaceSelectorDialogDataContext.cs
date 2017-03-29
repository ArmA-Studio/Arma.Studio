using System.ComponentModel;
using System.Windows.Input;
using ArmA.Studio.Data.UI.Commands;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ArmA.Studio.Dialogs
{
    public class WorkspaceSelectorDialogDataContext : IDialogContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public string CurrentPath { get { return this._CurrentPath; } set { this._CurrentPath = value; this.OKButtonEnabled = !string.IsNullOrWhiteSpace(value); this.RaisePropertyChanged(); } }
        private string _CurrentPath;

        public ICommand CmdBrowse => new RelayCommand(Cmd_Browse);
        public ICommand CmdOKButtonPressed => new RelayCommand((p) => this.DialogResult = true);

        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;

        public string WindowHeader { get { return Properties.Localization.WorkspaceSelectorDialog_Header; } }

        public string OKButtonText { get { return Properties.Localization.OK; } }

        public bool OKButtonEnabled { get { return this._OKButtonEnabled; } set { this._OKButtonEnabled = value; this.RaisePropertyChanged(); } }
        private bool _OKButtonEnabled;

        public WorkspaceSelectorDialogDataContext()
        {
            this.CurrentPath = string.Empty;
        }
        public void Cmd_Browse(object param)
        {
            var cofd = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                Multiselect = false,
                Title = Properties.Localization.WorkspaceSelectorDialog_Description
            };
            if (!string.IsNullOrWhiteSpace(this.CurrentPath))
            {
                cofd.InitialDirectory = CurrentPath;
            }
            var dlgResult = cofd.ShowDialog();
            if(dlgResult == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
            {
                this.CurrentPath = cofd.FileName;
            }
        }
    }
}

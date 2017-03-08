using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ArmA.Studio.Dialogs
{
    public class PropertiesDialogDataContext : IDialogContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ICommand CmdOKButtonPressed { get; private set; }

        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;

        public string WindowHeader { get { return Properties.Localization.LicenseViewer_Header; } }

        public string OKButtonText { get { return Properties.Localization.OK; } }

        public bool OKButtonEnabled { get { return true; } }

        public bool RestartRequired { get; internal set; }

        public PropertiesDialogDataContext()
        {
            this.CmdOKButtonPressed = new UI.Commands.RelayCommand(Cmd_OKButtonPressed);
            throw new NotImplementedException();
            //ToDo: Implement Properties (automated, requires class Attribute for section, property attribute for "header" and optional conversion)
        }
        public void Cmd_OKButtonPressed(object param)
        {
            this.DialogResult = true;
        }
    }
}
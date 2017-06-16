using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ArmA.Studio.Data.UI.Commands;

namespace ArmA.Studio.Dialogs
{
    public class ConfigEntrySelectorDialogDataContext : IDialogContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ObservableCollection<object> ThisCollection { get { return this._ThisCollection; } set { this._ThisCollection = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<object> _ThisCollection;

        public object SelectedValue { get { return this._SelectedValue; } set { this._SelectedValue = value; this.OKButtonEnabled = true; this.RaisePropertyChanged(); } }
        private object _SelectedValue;

        public ICommand CmdOKButtonPressed { get; private set; }

        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;

        public string WindowHeader => Properties.Localization.ConfigEntrySelectorDialog_Header;

        public string OKButtonText => Properties.Localization.Choose;

        public bool OKButtonEnabled { get { return this._OKButtonEnabled; } set { this._OKButtonEnabled = value; this.RaisePropertyChanged(); } }
        private bool _OKButtonEnabled;

        public ConfigEntrySelectorDialogDataContext()
        {
            this.CmdOKButtonPressed = new RelayCommand(this.Cmd_OKButtonPressed);
            this._ThisCollection = new ObservableCollection<object>();
            this._OKButtonEnabled = false;
        }
        public void Cmd_OKButtonPressed(object param)
        {
            this.DialogResult = true;
        }
    }
}

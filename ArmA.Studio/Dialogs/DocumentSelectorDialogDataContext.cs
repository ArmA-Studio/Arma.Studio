using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ArmA.Studio.Data;
using ArmA.Studio.Data.UI.Commands;

namespace ArmA.Studio.Dialogs
{
    public class DocumentSelectorDialogDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ICommand CmdOKButtonPressed { get { return new RelayCommand((p) => this.DialogResult = true); } }
        public ICommand CmdPreviewKeyDown { get { return new RelayCommand((p) => { if(this.OKButtonEnabled && Keyboard.IsKeyDown(Key.Enter)) this.DialogResult = true; }); } }

        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;

        public string SelectedName { get { return this._SelectedName; } set { this._SelectedName = value; this.UpdateOkButtonEnabled(); this.RaisePropertyChanged(); } }
        private string _SelectedName;

        public ObservableCollection<Data.UI.DocumentBase.DocumentDescribor> FileTypeCollection { get; private set; }

        public object SelectedItem { get { return this._SelectedItem; } set { this._SelectedItem = value; this.UpdateOkButtonEnabled(); this.RaisePropertyChanged(); } }
        private object _SelectedItem;

        public string WindowHeader => Properties.Localization.CreateNewFileDialog_Header;

        public string OKButtonText => Properties.Localization.OK;

        public bool OKButtonEnabled { get { return this._OKButtonEnabled; } set { this._OKButtonEnabled = value; this.RaisePropertyChanged(); } }
        private bool _OKButtonEnabled;

        public DocumentSelectorDialogDataContext()
        {
            this.FileTypeCollection = new ObservableCollection<Data.UI.DocumentBase.DocumentDescribor>(App.GetPlugins<Plugin.IDocumentProviderPlugin>().SelectMany((it) => it.Documents));
            this.SelectedItem = this.FileTypeCollection.FirstOrDefault();
            this.UpdateOkButtonEnabled();
        }

        private void UpdateOkButtonEnabled()
        {
            this.OKButtonEnabled = this.SelectedItem != null;
        }

    }
}

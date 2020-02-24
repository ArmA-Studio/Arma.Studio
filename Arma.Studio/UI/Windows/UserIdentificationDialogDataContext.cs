using Arma.Studio.Data.UI;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Arma.Studio.UI.Windows
{
    public class UserIdentificationDialogDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
        }

        public bool OptOut
        {
            get => this._OptOut;
            set
            {
                if (value)
                {
                    var msgboxres = MessageBox.Show(
                        Properties.Language.UserIdentificationDialog_OptOut_Confirmation_Body,
                        Properties.Language.UserIdentificationDialog_OptOut_Confirmation_Caption,
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);
                    if (msgboxres == MessageBoxResult.Yes)
                    {
                        this._OptOut = value;
                        this.RaisePropertyChanged();
                        this.RaisePropertyChanged(nameof(this.OkButton));
                    }
                }
                else
                {
                    this._OptOut = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.OkButton));
                }
            }
        }
        private bool _OptOut;
        public string UserIdentifier
        {
            get => this._UserIdentifier;
            set
            {
                this._UserIdentifier = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.OkButton));
            }
        }
        private string _UserIdentifier;

        public ICommand OkButtonCommand => new RelayCommand(() =>
        {
            Configuration.Instance.UserIdentifier = this.UserIdentifier;
            Configuration.Instance.OptOutOfReportingAndUpdates = this.OptOut;
            Configuration.Instance.UserIdentificationDialogWasDisplayed = true;
            Configuration.Save(App.ConfigPath);
        });

        public bool OkButton => this.OptOut || !String.IsNullOrWhiteSpace(this.UserIdentifier);




        public UserIdentificationDialogDataContext()
        {
            this._OptOut = Configuration.Instance.OptOutOfReportingAndUpdates;
            if (String.IsNullOrWhiteSpace(Configuration.Instance.UserIdentifier))
            {
                var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
                var userName = windowsIdentity.Name.LastIndexOf('\\') != -1 ?
                               windowsIdentity.Name.Substring(windowsIdentity.Name.LastIndexOf('\\') + 1) :
                               windowsIdentity.Name;
                this._UserIdentifier = userName;
            }
            else
            {
                this._UserIdentifier = Configuration.Instance.UserIdentifier;
            }
        }
    }
}

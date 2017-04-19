using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using ArmA.Studio.Data.UI.Commands;

namespace ArmA.Studio.Dialogs
{
    public class DownloadToolUpdateDialogDataContext : INotifyPropertyChanged, IDialogContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ICommand CmdOKButtonPressed { get { return this._CmdOKButtonPressed; } set { this._CmdOKButtonPressed = value; this.RaisePropertyChanged(); } }
        private ICommand _CmdOKButtonPressed;
        public ICommand CmdInitialized { get { return new RelayCommandAsync((p) => this.Window_Initialized()); } }

        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;

        public string Title { get { return string.Format(Properties.Localization.DownloadingX, DownloadInfo.name); } }
        public string WindowHeader { get { return string.Format(Properties.Localization.DownloadingX, DownloadInfo.version); } }
        public string OKButtonText { get { return Properties.Localization.InstallUpdate; } }

        public bool OKButtonEnabled { get { return this._OKButtonEnabled; } set { this._OKButtonEnabled = value; this.RaisePropertyChanged(); } }
        private bool _OKButtonEnabled;

        public double ProgressValue { get { return this._ProgressValue; } set { this._ProgressValue = value; this.RaisePropertyChanged(); } }
        private double _ProgressValue;

        public long FileSize { get { return this._FileSize; } set { if (this._FileSize != default(long)) return; this._FileSize = value / 1024; this.RaisePropertyChanged(); } }
        private long _FileSize;

        public long CurrentProgress { get { return this._CurrentProgress; } set { this._CurrentProgress = value / 1024; this.RaisePropertyChanged(); } }
        private long _CurrentProgress;

        private readonly UpdateHelper.DownloadInfo DownloadInfo;

        public DownloadToolUpdateDialogDataContext(UpdateHelper.DownloadInfo info)
        {
            this.DownloadInfo = info;
            this._OKButtonEnabled = false;
        }

        public async Task Window_Initialized()
        {
            var file = await UpdateHelper.DownloadFile(DownloadInfo, new Progress<Tuple<long, long>>((t) =>
            {
                this.CurrentProgress = t.Item1;
                this.FileSize = t.Item2;
                this.ProgressValue = (((double)t.Item1) / t.Item2);
            }));
            this.CmdOKButtonPressed = new RelayCommand((p) =>
            {
                var process = new Process();
                process.StartInfo.FileName = file;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                this.DialogResult = true;
            });
            this.OKButtonEnabled = true;
        }
    }
}

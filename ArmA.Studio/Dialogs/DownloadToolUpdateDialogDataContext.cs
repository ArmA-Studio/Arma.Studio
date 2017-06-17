using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using ArmA.Studio.Data.UI.Commands;

namespace ArmA.Studio.Dialogs
{
    public class DownloadToolUpdateDialogDataContext : IDialogContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ICommand CmdOKButtonPressed { get { return this._CmdOkButtonPressed; } set { this._CmdOkButtonPressed = value; this.RaisePropertyChanged(); } }
        private ICommand _CmdOkButtonPressed;
        public ICommand CmdInitialized { get { return new RelayCommandAsync((p) => this.Window_Initialized()); } }

        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;

        public string Title => string.Format(Properties.Localization.DownloadingX, this.DownloadInfo.name);
        public string WindowHeader => string.Format(Properties.Localization.DownloadingX, this.DownloadInfo.version);
        public string OKButtonText => Properties.Localization.InstallUpdate;

        public bool OKButtonEnabled { get { return this._OkButtonEnabled; } set { this._OkButtonEnabled = value; this.RaisePropertyChanged(); } }
        private bool _OkButtonEnabled;

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
            this._OkButtonEnabled = false;
        }

        public async Task Window_Initialized()
        {
            var file = await UpdateHelper.DownloadFileAsync(this.DownloadInfo, new Progress<Tuple<long, long>>((t) =>
            {
                this.CurrentProgress = t.Item1;
                this.FileSize = t.Item2;
                this.ProgressValue = (((double)t.Item1) / t.Item2);
            }));
            this.CmdOKButtonPressed = new RelayCommand((p) =>
            {
                if (ConfigHost.App.UseInDevBuild)
                {
                    var dir = string.Concat(file, "DIR");
                    System.IO.Compression.ZipFile.ExtractToDirectory(file, dir);
                    Process.Start("cmd.exe", $"/c echo Update Shell Script & echo Please wait until the tool is closed & pause & xcopy /s \"{dir}\" \"{App.ExecutablePath}\" /Y");
                    this.DialogResult = true;
                }
                else
                {
                    var process = new Process
                    {
                        StartInfo =
                        {
                            FileName = file,
                            UseShellExecute = false
                        }
                    };
                    process.Start();
                    this.DialogResult = true;
                }
            });
            this.OKButtonEnabled = true;
        }
    }
}

using Arma.Studio.Data.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Arma.Studio.UI.Windows
{
    public class UpdateDialogDataContext : INotifyPropertyChanged, Data.UI.AttachedProperties.IOnInitialized
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
        }

        #region Command: CmdOKButtonPressed
        public ICommand CmdOKButtonPressed
        {
            get => this._CmdOkButtonPressed; set
            {
                this._CmdOkButtonPressed = value;
                this.RaisePropertyChanged();
            }
        }
        private ICommand _CmdOkButtonPressed;
        #endregion

        public string Title => String.Format(Properties.Language.UpdateDialog_Downloading_0name, this.DownloadInfo.name);
        public string Header => String.Format(Properties.Language.UpdateDialog_Updating_0name, Properties.Language.ArmaStudio);

        public bool OKButtonEnabled
        {
            get => this._OkButtonEnabled;
            set
            {
                this._OkButtonEnabled = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _OkButtonEnabled;
        private DateTime LastUpdate = DateTime.MinValue;
        private long LastProgress;
        #region Property: DisplayText (System.String)
        public string DisplayText
        {
            get => this._DisplayText;
            set
            {
                if (this._DisplayText == value)
                {
                    return;
                }
                this._DisplayText = value;
                this.RaisePropertyChanged();
            }
        }
        private string _DisplayText;
        #endregion
        #region Property: ProgressValue (System.String)
        public double ProgressValue
        {
            get => this._ProgressValue;
            set
            {
                if (this._ProgressValue == value)
                {
                    return;
                }
                this._ProgressValue = value;
                this.RaisePropertyChanged();
            }
        }
        private double _ProgressValue;
        #endregion
        #region Property: FileSize (System.Int64)
        public long FileSize
        {
            get => this._FileSize;
            set
            {
                if (this._FileSize == value)
                {
                    return;
                }
                this._FileSize = value / 1024;
                this.RaisePropertyChanged();
            }
        }
        private long _FileSize;
        #endregion
        #region Property: Speed (System.Int64)
        public long Speed
        {
            get => this._Speed;
            set
            {
                if (this._Speed == value)
                {
                    return;
                }
                this._Speed = value;
                this.RaisePropertyChanged();
            }
        }
        private long _Speed;
        #endregion
        #region Property: CurrentProgress (System.Int64)
        public long CurrentProgress
        {
            get => this._CurrentProgress;
            set
            {
                if (this._CurrentProgress == value)
                {
                    return;
                }
                this._CurrentProgress = value / 1024;
                this.RaisePropertyChanged();
            }
        }
        private long _CurrentProgress;
        #endregion

        private readonly UpdateHelper.DownloadInfo DownloadInfo;

        public UpdateDialogDataContext(UpdateHelper.DownloadInfo info)
        {
            this.DownloadInfo = info;
            this._OkButtonEnabled = false;
        }


        private static void RunBatchScript(string targetDir)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c echo Update Shell Script & echo Please wait until the tool is closed & pause & xcopy /s \"{targetDir}\" \"{App.ExecutablePath}\" /Y & echo Done! You can close this window now. & pause",
                Verb = "runas"
            };
            var p = new Process
            {
                StartInfo = psi
            };
            p.Start();
        }
        public void OnInitialized(FrameworkElement sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                this.LastUpdate = DateTime.Now;
                int count = int.MaxValue;
                string file = await UpdateHelper.DownloadFileAsync(this.DownloadInfo, ((progress, filesize) =>
                {
                    if (count++ > 10)
                    {
                        var now = DateTime.Now;
                        this.Speed = (long)(((progress - this.LastProgress) / (now - this.LastUpdate).TotalSeconds) / 1024);
                        this.LastProgress = progress;
                        this.LastUpdate = now;
                        this.FileSize = filesize;
                        this.ProgressValue = (((double)progress) / filesize);
                        count = 0;
                    }
                    this.CurrentProgress = progress;
                }));
                this.ProgressValue = 1;
                this.CmdOKButtonPressed = new RelayCommandAsync(async (p) =>
                {
                    string dir = String.Concat(file, "DIR");
                    if (Directory.Exists(dir))
                    {
                        App.Current.Dispatcher.Invoke(() => this.DisplayText = Properties.Language.UpdateDialog_Unzipping);
                        try
                        {
                            foreach (string it in Directory.GetFiles(dir, "*", SearchOption.AllDirectories))
                            {
                                File.Delete(it);
                            }
                            foreach (string it in Directory.GetDirectories(dir).OrderBy((it) => it.Length))
                            {
                                Directory.Delete(it);
                            }
                            Directory.Delete(dir);
                        }
                        catch (Exception ex)
                        {
                            App.DisplayOperationFailed(ex);
                        }
                    }
                    System.IO.Compression.ZipFile.ExtractToDirectory(file, dir);
                    RunBatchScript(dir);
                });
                this.OKButtonEnabled = true;
            });
        }
    }
}

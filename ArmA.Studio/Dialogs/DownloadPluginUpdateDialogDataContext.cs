using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ArmA.Studio.Data.UI.Commands;
using ArmA.Studio.Plugin;

namespace ArmA.Studio.Dialogs
{
    public class DownloadPluginUpdateDialogDataContext : INotifyPropertyChanged, IDialogContext
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ICommand CmdOKButtonPressed => new RelayCommand((p) =>
        {
            this.DialogResult = true;
        });
        public ICommand CmdInitialized { get { return new RelayCommandAsync((p) => this.Window_Initialized()); } }

        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;

        public string WindowHeader => string.Format(Properties.Localization.DownloadingX, this.CurrentPlugin?.Name);
        public string OKButtonText => Properties.Localization.InstallUpdate;

        public bool OKButtonEnabled { get { return this._OKButtonEnabled; } set { this._OKButtonEnabled = value; this.RaisePropertyChanged(); } }
        private bool _OKButtonEnabled;

        public double OverallPluginProgress => this.CurrentPluginToUpdate / (double)this.TotalPluginsToUpdate;
        public int CurrentPluginToUpdate { get { return this._CurrentPluginToUpdate; } set { this._CurrentPluginToUpdate = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(this.OverallPluginProgress)); } }
        private int _CurrentPluginToUpdate;
        public int TotalPluginsToUpdate { get { return this._TotalPluginsToUpdate; } set { this._TotalPluginsToUpdate = value; this.RaisePropertyChanged(); } }
        private int _TotalPluginsToUpdate;

        public double CurrentPluginProgress => this.CurrentDownloadProgressInKiloBytes / (double)this.FileSizeInKiloBytes;
        public long CurrentDownloadProgressInKiloBytes { get { return this._CurrentDownloadProgressInKiloBytes; } set { this._CurrentDownloadProgressInKiloBytes = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(this.CurrentPluginProgress)); } }
        private long _CurrentDownloadProgressInKiloBytes;
        public long FileSizeInKiloBytes { get { return this._FileSizeInKiloBytes; } set { this._FileSizeInKiloBytes = value; this.RaisePropertyChanged(); } }
        private long _FileSizeInKiloBytes;

        public IUpdatingPlugin CurrentPlugin { get { return this._CurrentPlugin; } set { this._CurrentPlugin = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(this.WindowHeader)); } }
        private IUpdatingPlugin _CurrentPlugin;
        private readonly IEnumerable<IUpdatingPlugin> PluginsToUpdate;

        public DownloadPluginUpdateDialogDataContext(IEnumerable<IUpdatingPlugin> pluginsToUpdate)
        {
            this.PluginsToUpdate = pluginsToUpdate;
            this.CurrentPlugin = pluginsToUpdate.FirstOrDefault();
        }

        public async Task Window_Initialized()
        {
            if(!this.PluginsToUpdate.Any())
            {
                Logger.Warn("No plugins to update passed. Exiting Plugin Updater");
                this.DialogResult = false;
                return;
            }
            var list = new List<Tuple<string, string>>();
            foreach(var it in this.PluginsToUpdate)
            {
                Logger.Info($"Updating {it.Name}...");
                this.CurrentPlugin = it;
                string resultPath = await Task.Run(() => it.DownloadUpdate(new Progress<Tuple<long, long>>((t) =>
                {
                    this.CurrentDownloadProgressInKiloBytes = t.Item1;
                    this.FileSizeInKiloBytes = t.Item2;
                })));
                list.Add(new Tuple<string, string>(resultPath, Path.GetFileName(it.GetType().Assembly.Location)));
            }
            Logger.Info($"Done");
            foreach (var it in list)
            {
                var finalPath = Path.Combine(App.PluginsPath, string.Concat(it.Item2, App.CONST_UPDATESUFFIX));
                Logger.Info($"Moving temporary patch file from '{it.Item1}' to '{finalPath}'");
                File.Move(it.Item1, finalPath);
            }
            this.OKButtonEnabled = true;
        }
    }
}

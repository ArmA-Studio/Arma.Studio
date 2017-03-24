using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ArmA.Studio.Data.UI.Commands;
using ArmA.Studio.Plugin;

namespace ArmA.Studio
{
    public sealed class SplashScreenDataContext : INotifyPropertyChanged
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        #region BindableProperties
        public Version CurrentVersion { get { return App.CurrentVersion; } }

        public ICommand CmdInitialized => new RelayCommand((p) => Task.Run(() => RunSplash()));

        public double ProgressValue { get { return this._ProgressValue; } set { if (this._ProgressValue == value) return; this._ProgressValue = value; RaisePropertyChanged(); } }
        private double _ProgressValue;

        public string ProgressText { get { return this._ProgressText; } set { if (this._ProgressText == value) return; this._ProgressText = value; RaisePropertyChanged(); } }
        private string _ProgressText;

        public bool ProgressIndeterminate { get { return this._ProgressIndeterminate; } set { if (this._ProgressIndeterminate == value) return; this._ProgressIndeterminate = value; RaisePropertyChanged(); } }
        private bool _ProgressIndeterminate;
        #endregion

        public SplashScreenDataContext()
        {

        }

        private async Task RunSplashAsync() => await Task.Run(() => this.RunSplash());
        private void RunSplash()
        {
            var setIndeterminate = new Action<bool>((v) => App.Current.Dispatcher.Invoke(() => this.ProgressIndeterminate = v));
            var setDisplayText = new Action<string>((v) => App.Current.Dispatcher.Invoke(() => this.ProgressText = v));
            var setProgress = new Action<double>((v) => App.Current.Dispatcher.Invoke(() => this.ProgressValue = v));
            var reset = new Action(() => App.Current.Dispatcher.Invoke(() =>
            {
                this.ProgressIndeterminate = false;
                this.ProgressText = string.Empty;
                this.ProgressValue = 0;
            }));

            if (!Splash_LoadPlugins(setIndeterminate, setDisplayText, setProgress))
                return;
            reset();
            if (!Splash_CheckUpdate(setIndeterminate, setDisplayText, setProgress))
                return;
            reset();

            foreach(var splashActivityPlugin in from plugin in App.Plugins where plugin is ISplashActivityPlugin select plugin as ISplashActivityPlugin)
            {
                reset();
                bool terminate;
                splashActivityPlugin.PerformSplashActivity(App.Current.Dispatcher, setIndeterminate, setDisplayText, setProgress, out terminate);
                if (terminate)
                    return;
            }
            reset();

            if (!Splash_Workspace(setIndeterminate, setDisplayText, setProgress))
                return;
        }

        private static bool Splash_Workspace(Action<bool> SetIndeterminate, Action<string> SetDisplayText, Action<double> SetProgress, out Workspace w)
        {
            w = null;
            SetIndeterminate(true);
            string workspace;
            string solutionPath = string.Empty;

            #region set/get workspace path
            SetDisplayText(Properties.Localization.Splash_SettingWorkspace);
            Logger.Info("Trying to receive workspace...");
            workspace = string.IsNullOrWhiteSpace(ConfigHost.App.WorkspacePath) ? Dialogs.WorkspaceSelectorDialog.GetWorkspacePath(string.Empty) : ConfigHost.App.WorkspacePath;
            if (string.IsNullOrWhiteSpace(workspace))
            {
                Logger.Info("No workspace set, exiting");
                App.Current.Dispatcher.Invoke(() =>
                MessageBox.Show(Properties.Localization.WorkspaceSelectorDialog_NoWorkspaceSelected, Studio.Properties.Localization.Whoops, MessageBoxButton.OK, MessageBoxImage.Error));
                return true;
            }
            else
            {
                Logger.Info($"Selected workspace: {workspace}");
                ConfigHost.App.WorkspacePath = workspace;
            }
            if (!Directory.Exists(workspace))
            {
                Logger.Info($"Creating Directory for workspace ...");
                Directory.CreateDirectory(workspace);
            }
            #endregion

            w = new Workspace();
            w.PathUri = new Uri(workspace, UriKind.Absolute);

            #region Prepare Solution
            Logger.Info("Searching solution file in workspace");
            SetDisplayText(Properties.Localization.Splash_SearchingSolutionFile);
            foreach (var file in Directory.EnumerateFiles(workspace, string.Concat('*', App.CONST_SOLUTIONEXTENSION)))
            {
                solutionPath = file;
                break;
            }


            SetDisplayText(Properties.Localization.Splash_PreparingSolution);
            try
            {
                if (string.IsNullOrWhiteSpace(solutionPath))
                {
                    solutionPath = Path.Combine(workspace, string.Concat("solution", App.CONST_SOLUTIONEXTENSION));
                    Logger.Info($"No solution file found, creating new at '{solutionPath}'");
                    w.Solution = new Data.Solution();
                    using (var stream = File.OpenWrite(solutionPath))
                    {
                        Data.Solution.Serialize(w.Solution, stream);
                    }
                }
                else
                {
                    Logger.Info($"Solution file found: {solutionPath}");
                    using (var stream = File.OpenRead(solutionPath))
                    {
                        w.Solution = Data.Solution.Deserialize(stream, new Uri(solutionPath, UriKind.Absolute));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Exception while trying to get Solution");
                MessageBox.Show(string.Format(Properties.Localization.MessageBoxOperationFailed_Body, ex.Message, ex.GetType().FullName, ex.StackTrace), Properties.Localization.MessageBoxOperationFailed_Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }

            #endregion

            SetIndeterminate(false);


            return false;
        }

        private static bool Splash_LoadPlugins(Action<bool> SetIndeterminate, Action<string> SetDisplayText, Action<double> SetProgress)
        {
            SetDisplayText(Properties.Localization.Splash_ApplyingPossiblyAvailablePluginUpdates);
            foreach(var fileName in Directory.EnumerateFiles(App.PluginsPath, string.Concat("*.dll", App.CONST_UPDATESUFFIX)))
            {
                var fileNameWithoutSuffix = fileName.Remove(fileName.LastIndexOf(App.CONST_UPDATESUFFIX));
                Logger.Info($"Applying update for {Path.GetFileName(fileNameWithoutSuffix)}...");
                File.Delete(fileNameWithoutSuffix);
                File.Move(fileName, fileNameWithoutSuffix);
            }

            SetDisplayText(Properties.Localization.Splash_LoadingPlugins);
            var pManager = new PluginManager<IPlugin>();
            var plugins = Directory.EnumerateFiles(App.PluginsPath, "*.dll");
            pManager.LoadPlugins(plugins, new Progress<double>((d) =>
            {
                SetProgress(d);
            }), (ex) =>
            {
                Logger.Error(ex, "Error while loading plugin.");
                return true;
            });
            Logger.Info($"Loaded {pManager.Plugins.Count()} plugins:");
            foreach (var p in pManager.Plugins)
            {
                Logger.Info($"\t- {p.Name} <{p.GetType().AssemblyQualifiedName}>");
            }
            App.Plugins = pManager.Plugins;
            return true;
        }
        private static bool Splash_CheckUpdate(Action<bool> SetIndeterminate, Action<string> SetDisplayText, Action<double> SetProgress)
        {
            var doShutdown = false;
            SetIndeterminate(true);
            SetDisplayText(Properties.Localization.Splash_CheckingForToolUpdates);
            SetProgress(1);
            Logger.Info("Checking for tool updates...");
#if DEBUG
            (App.Current as App).UpdateDownloadInfo = UpdateHelper.GetDownloadInfo().Result;
            if ((App.Current as App).UpdateDownloadInfo.available)
            {
                SetIndeterminate(false);
                SetDisplayText(Properties.Localization.Splash_UpdateAvailable);
                Logger.Info($"Update {(App.Current as App).UpdateDownloadInfo.version} is available.");
                App.Current.Dispatcher.Invoke(() =>
                {
                    var msgboxresult = MessageBox.Show(
                          string.Format(Properties.Localization.SoftwareUpdateAvailable_Body, App.CurrentVersion, (App.Current as App).UpdateDownloadInfo.version),
                          string.Format(Properties.Localization.SoftwareUpdateAvailable_Title, (App.Current as App).UpdateDownloadInfo.version),
                          MessageBoxButton.YesNo,
                          MessageBoxImage.Information
                    );
                    if (msgboxresult == MessageBoxResult.Yes)
                    {
                        Logger.Info("Applying update.");
                        App.Shutdown(App.ExitCodes.Updating);
                        doShutdown = true;
                    }
                    else
                    {
                        Logger.Info("Ignoring update.");
                    }
                });
            }
            else
            {
                Logger.Info("No update available.");
            }
#endif
            var updatingPlugins = from plugin in App.Plugins where plugin is IUpdatingPlugin select plugin as IUpdatingPlugin;
            if (updatingPlugins.Any())
            {
                var list = new List<IUpdatingPlugin>();
                Logger.Info("Checking for plugin updates.");
                foreach (var p in updatingPlugins)
                {
                    Logger.Info($"Checking plugin {p.Name} for updates...");
                    SetDisplayText(string.Format(Properties.Localization.Splash_CheckingForPluginXUpdate, p.Name));
                    bool result = p.CheckUpdateAvailable();
                    if (result)
                    {
                        Logger.Info($"{p.Name} has an update available.");
                        list.Add(p);
                    }
                    else
                    {
                        Logger.Info($"{p.Name} has no update.");
                    }
                }
                if (list.Any())
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        var msgboxresult = MessageBox.Show(
                              string.Format(Properties.Localization.SoftwareUpdateAvailable_Body, App.CurrentVersion, (App.Current as App).UpdateDownloadInfo.version),
                              string.Format(Properties.Localization.SoftwareUpdateAvailable_Title, (App.Current as App).UpdateDownloadInfo.version),
                              MessageBoxButton.YesNo,
                              MessageBoxImage.Information
                        );
                        if (msgboxresult == MessageBoxResult.Yes)
                        {
                            Logger.Info("Applying plugin updates.");
                            var dlgdc = new Dialogs.DownloadPluginUpdateDialogDataContext(list);
                            var dlg = new Dialogs.DownloadPluginUpdateDialog(dlgdc);
                            var dlgResult = dlg.ShowDialog();
                            if (dlgResult.HasValue && dlgResult.Value)
                            {
                                doShutdown = true;
                                App.Shutdown(App.ExitCodes.RestartPluginUpdate);
                            }
                            else
                            {
                                Logger.Info("Ignoring plugin updates.");
                            }
                        }
                        else
                        {
                            Logger.Info("Ignoring plugin updates.");
                        }
                    });
                }
            }
            return doShutdown;
        }
    }
}

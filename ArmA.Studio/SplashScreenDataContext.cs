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

        private void RunSplash()
        {
            var setIndeterminate = new Action<bool>((v) => App.Current.Dispatcher.Invoke(() => this.ProgressIndeterminate = v));
            var setDisplayText = new Action<string>((v) => App.Current.Dispatcher.Invoke(() => this.ProgressText = v));
            var setProgress = new Action<double>((v) => App.Current.Dispatcher.Invoke(() => this.ProgressValue = v));

            Splash_LoadPlugins(setIndeterminate, setDisplayText, setProgress);
            Splash_CheckUpdate(setIndeterminate, setDisplayText, setProgress);
        }

        private static void Splash_LoadPlugins(Action<bool> SetIndeterminate, Action<string> SetDisplayText, Action<double> SetProgress)
        {
            var pManager = new PluginManager<IPlugin>();
            var plugins = Directory.EnumerateFiles(App.PluginsPath, "*.dll");
            pManager.LoadPlugins(plugins, new Progress<double>((d) =>
            {
                SetProgress(d);
                SetDisplayText(Properties.Localization.Splash_LoadingPlugins);
            }), (ex) =>
            {
                Logger.Error(ex, "Error while loading plugin.");
                return true;
            });
            Logger.Info($"Loaded {pManager.Plugins.Count()} plugins:");
            foreach(var p in pManager.Plugins)
            {
                Logger.Info($"\t- {p.Name} <{p.GetType().AssemblyQualifiedName}>");
            }
            App.Plugins = pManager.Plugins;
        }
        private static void Splash_CheckUpdate(Action<bool> SetIndeterminate, Action<string> SetDisplayText, Action<double> SetProgress)
        {
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
                Logger.Info("Checking for plugin updates.");
                foreach (var p in updatingPlugins)
                {
                    Logger.Info($"Checking plugin {p.Name} for updates.");
                    SetDisplayText(string.Format(Properties.Localization.Splash_CheckingForPluginXUpdate, p.Name));
                    bool result = p.CheckUpdateAvailable();
                    if(result)
                    {
                        Logger.Info($"Plugin.");
                    }
                    else
                    {

                    }
                }
            }
            SetIndeterminate(false);
        }
    }
}

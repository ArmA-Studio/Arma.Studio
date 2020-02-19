using Arma.Studio;
using Arma.Studio.Data.UI;
using Arma.Studio.UI.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Arma.Studio.UI.Windows
{
    public class SplashScreenDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") => App.Current.Dispatcher.Invoke(() => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)));

        public SplashScreenDataContext()
        {
            this.ProgressIndeterminate = true;
            this.Source = new CancellationTokenSource();
        }

        #region BindableProperties
        public Version CurrentVersion => App.CurrentVersion;
        private bool IsValidClose = false;
        public ICommand CmdLoaded => new RelayCommandAsync<Window>(async (window) =>
        {
            try
            {
                App.MWContext = new MainWindowDataContext();
                if (await this.RunSplash())
                {
                    var mw = App.Current.Dispatcher.Invoke(() => new MainWindow(App.MWContext));
                    this.ProgressText = Properties.Language.SplashScreen_PreparingUserInterface;
                    this.ProgressIndeterminate = true;
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        this.IsValidClose = true;
                        window.Close();
                        mw.Show();
                    });
                }
                else
                {
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                App.DisplayOperationFailed(ex);
                App.Current.Dispatcher.Invoke(() => App.Current.Shutdown(-1));
            }
        });
        public ICommand CmdClosed => new RelayCommand((p) =>
        {
            if (!this.IsValidClose)
            {
                App.Current.Shutdown(-1);
            }
        });

        public double ProgressValue { get { return this._ProgressValue; } set { if (this._ProgressValue == value) { return; } this._ProgressValue = value; this.RaisePropertyChanged(); } }
        private double _ProgressValue;

        public string ProgressText { get { return this._ProgressText; } set { if (this._ProgressText == value) { return; } this._ProgressText = value; this.RaisePropertyChanged(); } }
        private string _ProgressText;

        public bool ProgressIndeterminate { get { return this._ProgressIndeterminate; } set { if (this._ProgressIndeterminate == value) { return; } this._ProgressIndeterminate = value; this.RaisePropertyChanged(); } }
        private bool _ProgressIndeterminate;
        #endregion

        public CancellationTokenSource Source { get; }

        private IEnumerable<string> Directories(string path, Func<bool> func = null)
        {
            if (System.IO.Directory.Exists(path) && (func == null || func()))
            {
                foreach(var it in System.IO.Directory.EnumerateDirectories(path))
                {
                    yield return it;
                }
            }
        }
        private async Task<bool> RunSplash()
        {
            // Check for updates
            if (true) // (ConfigHost.App.EnableAutoToolUpdates)
            {
                this.ProgressIndeterminate = true;
                this.ProgressText = Properties.Language.SplashScreen_CheckingForUpdates;
                var downloadInfo = UpdateHelper.GetDownloadInfoAsync().Result;
                if (downloadInfo.available)
                {
                    this.ProgressIndeterminate = false;
                    this.ProgressValue = 1;
                    this.ProgressText = Properties.Language.SplashScreen_UpdateAvailable;
                    var msgboxresult = Application.Current.Dispatcher.Invoke(() => MessageBox.Show(
                            Properties.Language.SoftwareUpdateAvailable_Body,
                            Properties.Language.SoftwareUpdateAvailable_Caption,
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Information
                    ));
                    if (msgboxresult == MessageBoxResult.Yes)
                    {
                        App.Update(downloadInfo);
                        return false;
                    }
                }
            }
            // Loading plugins
            {
                this.ProgressIndeterminate = true;
                this.ProgressText = Properties.Language.SplashScreen_LoadingPlugins;
                var plugins = this.Directories(App.PluginDir_Executable)
                    .Concat(this.Directories(App.PluginDir_Data))
                    .Concat(this.Directories(App.PluginDir_RoamingUser, () => true /* ToDo: Add property to disable plugin loading from user dir */));
                var pluginCount = plugins.Count();

                foreach (var plugin in plugins)
                {
                    try
                    {
                        PluginManager.Instance.LoadPlugin(plugin);
                    }
                    catch (PluginManager.NoPluginPresentException) { /* EMPTY */ }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format(Properties.Language.FailedToLoadPlugin_Body, plugin), Properties.Language.FailedToLoadPlugin_Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                { // Handle Data.Log Plugins setters
                    this.ProgressText = Properties.Language.SplashScreen_PreparingLoggers;
                    this.ProgressIndeterminate = true;
                    var loggersList = new List<Data.Log.Logger>();
                    foreach (var it in PluginManager.Instance.GetPlugins<Data.Log.ILogger>())
                    {
                        var logger = new Data.Log.Logger(it);
                        it.SetLogger(logger);
                        loggersList.Add(logger);
                    }
                    var loggersReadOnlyCollection = new Data.Log.LoggerCollection(loggersList);
                    foreach (var it in PluginManager.Instance.GetPlugins<Data.Log.ILogHandler>())
                    {
                        it.SetLogCollection(loggersReadOnlyCollection);
                    }
                }

                var count = PluginManager.Instance.Plugins.Count;
                int num = 0;
                this.ProgressValue = 0;
                this.ProgressIndeterminate = false;
                foreach (var plugin in PluginManager.Instance.Plugins)
                {
                    var actualPlugin = plugin.Plugin();
                    this.ProgressValue = (num++ / (double)count);
                    this.ProgressText = String.Format(Properties.Language.SplashScreen_InitializingPlugin_0Name, actualPlugin.Name);

                    await actualPlugin.Initialize(plugin.Folder, this.Source.Token);
                }

                {
                    // Receive TextEditor Providers
                    foreach (var it in PluginManager.Instance.GetPlugins<Data.TextEditor.ITextEditorProvider>())
                    {
                        App.MWContext.TextEditorsAvailable.AddRange(it.TextEditorInfos);
                    }
                }
            }
            return true;
        }
    }
}

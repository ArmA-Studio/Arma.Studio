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
        }

        #region BindableProperties
        public Version CurrentVersion => App.CurrentVersion;
        private bool IsValidClose = false;
        public ICommand CmdLoaded => new RelayCommand((p) =>
        {
            var task = Task.Run(() => this.RunSplash());
            task.ContinueWith((t) =>
            {
                var window = p as Window;
                App.DisplayOperationFailed(t.Exception);
                App.Current.Dispatcher.Invoke(() => window.Close());
            }, TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith((t) =>
            {
                var window = p as Window;
                this.IsValidClose = true;
                if (t.IsFaulted)
                {
                    App.DisplayOperationFailed(t.Exception);
                    App.Current.Shutdown(-1);
                    return;
                }
                App.Current.Dispatcher.Invoke(() => window.Close());
                App.Current.Dispatcher.Invoke(() =>
                {
                    var mwdc = new MainWindowDataContext();
                    var mw = new MainWindow(mwdc);
                    mw.Show();
                });
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
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
        private async Task RunSplash()
        {
            // ToDo: Implement auto-update-check and remove this
            await Task.Delay(100);

            // Loading plugins
            {
                var plugins = Directories(App.PluginDir_Executable)
                    .Concat(Directories(App.PluginDir_Data))
                    .Concat(Directories(App.PluginDir_RoamingUser, () => true /* ToDo: Add property to disable plugin loading from user dir */));
                var pluginCount = plugins.Count();

                foreach (var plugin in plugins)
                {
                    try
                    {
                        PluginManager.Instance.LoadPlugin(plugin);
                    }
                    catch (PluginManager.NoPluginPresentException) { /* EMPTY */ }
                    catch
                    {
                        MessageBox.Show(String.Format(Properties.Language.FailedToLoadPlugin_Body, plugin), Properties.Language.FailedToLoadPlugin_Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

    }
}

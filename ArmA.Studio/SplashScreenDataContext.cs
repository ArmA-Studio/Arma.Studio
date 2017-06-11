using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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

        public ICommand CmdLoaded => new RelayCommandAsync((p) => RunSplashAsync(p as Window));

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

        private async Task RunSplashAsync(Window wind) => await Task.Run(() => this.RunSplash(wind));
        private void RunSplash(Window wind)
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
            try
            {


                if (Splash_LoadPlugins(setIndeterminate, setDisplayText, setProgress))
                {
                    App.Shutdown(App.ExitCodes.OK);
                    return;
                }
                reset();
                try
                {
                    if (Splash_CheckUpdate(setIndeterminate, setDisplayText, setProgress))
                    {
                        App.Shutdown(App.ExitCodes.OK);
                        return;
                    }
                }
                catch (Exception ex)
                {
                }
                reset();

                foreach (var splashActivityPlugin in from plugin in App.Plugins where plugin is ISplashActivityPlugin select plugin as ISplashActivityPlugin)
                {
                    reset();
                    bool terminate;
                    splashActivityPlugin.PerformSplashActivity(App.Current.Dispatcher, setIndeterminate, setDisplayText, setProgress, out terminate);
                    if (terminate)
                    {
                        App.Shutdown(App.ExitCodes.OK);
                        return;
                    }
                }
                reset();

                Workspace w;
                if (Splash_Workspace(setIndeterminate, setDisplayText, setProgress, out w))
                {
                    App.Shutdown(App.ExitCodes.OK);
                    return;
                }
                reset();

#if !DEBUG
                if (Splash_DoInitialLint(setIndeterminate, setDisplayText, setProgress))
                {
                    App.Shutdown(App.ExitCodes.OK);
                    return;
                }
                reset();
#endif
                App.Current.Dispatcher.Invoke(() =>
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    App.Current.MainWindow = mainWindow;
                    wind.Close();
                });
            }
            catch (Exception ex)
            {
                App.ShowOperationFailedMessageBox(ex);
                App.Shutdown(App.ExitCodes.SplashError);
                return;
            }
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

            var dataTemplateSelector = new UI.GenericDataTemplateSelector();
            w = new Workspace(dataTemplateSelector);
            dataTemplateSelector.AddAllDataTemplatesInAssembly(Assembly.GetExecutingAssembly(), (s) => s.StartsWith("ArmA.Studio.UI.DataTemplates."));
            foreach (var dt in App.GetPlugins<IDocumentProviderPlugin>().SelectMany((p) => p.DocumentDataTemplates))
            {
                dataTemplateSelector.AddDataTemplate(dt);
            }
            foreach (var p in App.GetPlugins<IPaneProviderPlugin>())
            {
                foreach (var dt in p.PaneDataTemplates)
                {
                    dataTemplateSelector.AddDataTemplate(dt);
                }
                foreach (var paneType in p.PaneDataContextTypes)
                {
                    var pane = Activator.CreateInstance(paneType) as Data.UI.PanelBase;
                    w.AvailablePanels.Add(pane);
                }
            }
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
                    w.Solution.FileUri = new Uri(solutionPath, UriKind.Absolute);
                    using (var stream = File.Open(solutionPath, FileMode.Create))
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
            catch (UnauthorizedAccessException ex)
            {
                ConfigHost.App.WorkspacePath = string.Empty;
                Logger.Error(ex, "Exception while trying to get Solution");
                App.ShowOperationFailedMessageBox(ex);
                App.Shutdown(App.ExitCodes.NoWorkspaceSelected);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Exception while trying to get Solution");
                App.ShowOperationFailedMessageBox(ex);
                App.Shutdown(App.ExitCodes.ConfigError);
                return true;
            }
            SetDisplayText(Properties.Localization.Splash_LoadingBreakpointInformations);
            { //Load Breakpoints
                var filePath = Path.ChangeExtension(solutionPath, App.CONST_BREAKPOINTINFOEXTENSION);
                if (File.Exists(filePath))
                {
                    try
                    {
                        using (var stream = File.OpenRead(filePath))
                        {
                            w.BreakpointManager.LoadBreakpoints(stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        App.ShowOperationFailedMessageBox(ex);
                    }
                }
                else
                {
                    using (var stream = File.Open(filePath, FileMode.Create))
                    {
                        w.BreakpointManager.SaveBreakpoints(stream);
                    }
                }
            }
            { //Load Watch Variables
                var filePath = Path.ChangeExtension(solutionPath, App.CONST_WATCHEXTENSION);
                if (File.Exists(filePath))
                {
                    try
                    {
                        using (var stream = File.OpenRead(filePath))
                        {
                            var vvp = w.AvailablePanels.First((it) => it is DataContext.VariablesViewPane) as DataContext.VariablesViewPane;
                            vvp.LoadVariables(stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        App.ShowOperationFailedMessageBox(ex, Properties.Localization.VariablesView_FailedToLoadExistingWatchEntries);
                    }
                }
                else
                {
                    using (var stream = File.Open(filePath, FileMode.Create))
                    {
                        var vvp = w.AvailablePanels.First((it) => it is DataContext.VariablesViewPane) as DataContext.VariablesViewPane;
                        vvp.SaveVariables(stream);
                    }
                }
            }
            #endregion

            SetIndeterminate(false);


            return false;
        }
        private static bool Splash_LoadPlugins(Action<bool> SetIndeterminate, Action<string> SetDisplayText, Action<double> SetProgress)
        {
            SetDisplayText(Properties.Localization.Splash_ApplyingPossiblyAvailablePluginUpdates);
            foreach (var fileName in Directory.EnumerateFiles(App.PluginsPath, string.Concat("*.dll", App.CONST_UPDATESUFFIX)))
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
                Logger.Error($"Error while loading plugin: {ex.Message}");
                if(ex is ReflectionTypeLoadException)
                {
                    var refl = ex as ReflectionTypeLoadException;
                    foreach(var loaderEx in refl.LoaderExceptions)
                    {
                        Logger.Error($"-> loaderEx.Message");
                    }
                }
                return true;
            });
            App.Plugins.AddRange(pManager.Plugins);
            Logger.Info($"Loaded {App.Plugins.Count} plugins:");
            foreach (var p in App.Plugins)
            {
                Logger.Info($"\t- {p.Name} <{p.GetType().AssemblyQualifiedName}>");
                if (p is IAccessCallsPlugin)
                {
                    var acp = p as IAccessCallsPlugin;
                    acp.GetAllPlugins = () => App.Plugins;
                    acp.GetCurrentDocument = () => Workspace.Instance?.GetCurrentDocument();
                    acp.GetSolution = () => Workspace.Instance?.Solution;
                    acp.ShowOperationFailed = (ex) => App.ShowOperationFailedMessageBox(ex);
                }
                else if (p is IStorageAccessPlugin)
                {
                    var sap = p as IStorageAccessPlugin;
                    ConfigHost.Instance.PreparePlugin(sap);
                }
                else if (p is IDocumentProviderPlugin)
                {
                    var dpp = p as IDocumentProviderPlugin;
                    var extensions = dpp.FileTypes.Select((ft) => ft.DefaultExtension);
                    Data.Project.ValidFileExtensions.AddRange(extensions);
                }
            }
            return false;
        }
        private static bool Splash_CheckUpdate(Action<bool> SetIndeterminate, Action<string> SetDisplayText, Action<double> SetProgress)
        {
            //ToDo: Fix cert issue causes appclose
            var doShutdown = false;
            SetIndeterminate(true);
            SetDisplayText(Properties.Localization.Splash_CheckingForToolUpdates);
            SetProgress(1);
            Logger.Info("Checking for tool updates...");
            if (ConfigHost.App.EnableAutoToolUpdates)
            {
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
            }
            if (ConfigHost.App.EnableAutoPluginsUpdate)
            {
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

            }
            return doShutdown;
        }
        private static bool Splash_DoInitialLint(Action<bool> SetIndeterminate, Action<string> SetDisplayText, Action<double> SetProgress)
        {
            var doShutdown = false;
            var files = Workspace.Instance.Solution.Projects.SelectMany((p) => p);
            double count = files.Count();
            var index = 0;
            foreach (var it in files)
            {
                index++;
                SetDisplayText(string.Format(Properties.Localization.Splash_DoingInitialLint, index, (int)count, it.FileName));
                SetProgress(index / count);

                var fileType = Workspace.Instance.GetFileType(it.FileUri);
                if (fileType == null || fileType.Linter == null)
                    continue;
                try
                {
                    using (var stream = File.OpenRead(it.FilePath))
                    {
                        DataContext.ErrorListPane.Instance.LinterDictionary[it.FilePath] = fileType.Linter.Lint(stream, it);
                    }
                }
                catch(Exception ex)
                {
                    App.ShowOperationFailedMessageBox(ex);
                }
            }
            return doShutdown;
        }
    }
}

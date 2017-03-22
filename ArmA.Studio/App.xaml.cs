using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Diagnostics;
using System.Xml;
using NLog;
using ArmA.Studio.LoggerTargets;
using NLog.Config;
using ArmA.Studio.Plugin;

namespace ArmA.Studio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string CONST_UPDATESUFFIX = ".update";

        public enum ExitCodes
        {
            ConfigError = -2,
            NoWorkspaceSelected = -1,
            OK = 0,
            Restart = 1,
            Updating = 2,
            RestartPluginUpdate = 3
        }
        public static string ExecutablePath { get { return Path.GetDirectoryName(ExecutableFile); } }
        public static string ExecutableFile { get { return Assembly.GetExecutingAssembly().GetName().CodeBase.Substring("file:///".Length); } }
        public static string SyntaxFilesPath { get { return Path.Combine(ExecutablePath, "SyntaxFiles"); } }
        public static string PluginsPath { get { return Path.Combine(ExecutablePath, "Plugins"); } }
        public static string ConfigPath { get { return Path.Combine(ApplicationDataPath, "Configuration"); } }
        public static string FileTemplatePath { get { return Path.Combine(ApplicationDataPath, "Templates"); } }
        public static string TempPath { get { return Path.Combine(Path.GetTempPath(), @"ArmA.Studio"); } }
        public static string CommonApplicationDataPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"ArmA.Studio"); } }
        public static string ApplicationDataPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"ArmA.Studio"); } }
        public static Version CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public static SubscribableTarget SubscribableLoggerTarget { get; private set; }
        public static IEnumerable<IPlugin> Plugins { get; set; }
        internal UpdateHelper.DownloadInfo UpdateDownloadInfo;

        private void SetupNLog()
        {
            //this.TraceListenerInstance = new TraceListener();
            //System.Diagnostics.Trace.Listeners.Add(this.TraceListenerInstance);
            SubscribableLoggerTarget = new SubscribableTarget();
            ConfigurationItemFactory.Default.Targets.RegisterDefinition("SubscribableTarget", typeof(SubscribableTarget));
            LogManager.Configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, SubscribableLoggerTarget));
            LogManager.Configuration.AddTarget(SubscribableLoggerTarget);
            LogManager.ReconfigExistingLoggers();
        }

        private void CreateUserDirectories()
        {
            if (!Directory.Exists(ConfigPath))
            {
                Directory.CreateDirectory(ConfigPath);
            }
            if (!Directory.Exists(FileTemplatePath))
            {
                Directory.CreateDirectory(FileTemplatePath);
            }
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
#if !DEBUG
            Task.Run(() => {
                DownloadInfo = UpdateHelper.GetDownloadInfo().Result;
                if(DownloadInfo.available)
                {
                    Dispatcher.Invoke(() =>
                    {
                        var msgboxresult = MessageBox.Show(
                              string.Format(Studio.Properties.Localization.SoftwareUpdateAvailable_Body, CurrentVersion, DownloadInfo.version),
                              string.Format(Studio.Properties.Localization.SoftwareUpdateAvailable_Title, DownloadInfo.version),
                              MessageBoxButton.YesNo,
                              MessageBoxImage.Information
                        );
                        if (msgboxresult == MessageBoxResult.Yes)
                        {
                            App.Shutdown(ExitCodes.Updating);
                        }
                    });
                }
            });
#endif
            this.CreateUserDirectories();
            try
            {
                //Invoke getter, will never be null
                if (ConfigHost.Instance == null)
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException != null ? ex.InnerException.Message : ex.Message, "FATAL ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Shutdown(ExitCodes.ConfigError);
                return;
            }
            this.SetupNLog();
            var workspace = ConfigHost.App.WorkspacePath;
            if (string.IsNullOrWhiteSpace(workspace) && !SwitchWorkspace())
            {
                MessageBox.Show(Studio.Properties.Localization.WorkspaceSelectorDialog_NoWorkspaceSelected, Studio.Properties.Localization.Whoops, MessageBoxButton.OK, MessageBoxImage.Error);
                this.Shutdown((int)ExitCodes.NoWorkspaceSelected);
                return;
            }
            workspace = ConfigHost.App.WorkspacePath;
            Workspace.CurrentWorkspace = new Workspace(workspace);
            var mwnd = new MainWindow();
            mwnd.Show();
        }

        public static bool SwitchWorkspace()
        {
            var dlgDc = new Dialogs.WorkspaceSelectorDialogDataContext();
            var dlg = new Dialogs.WorkspaceSelectorDialog(dlgDc);
            var dlgResult = dlg.ShowDialog();
            if (!dlgResult.HasValue || !dlgResult.Value)
            {
                return false;
            }
            var workspace = dlgDc.CurrentPath;
            ConfigHost.App.WorkspacePath = workspace;
            return true;
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
#if DEBUG
            System.Diagnostics.Debugger.Break();
#endif
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (e.ApplicationExitCode == (int)ExitCodes.ConfigError)
                return;
            Workspace.CurrentWorkspace = null;
            if (e.ApplicationExitCode == (int)ExitCodes.Restart || e.ApplicationExitCode == (int)ExitCodes.RestartPluginUpdate)
            {
                Process.Start(ExecutableFile);
            }
            ConfigHost.Instance.ExecSave();
        }

        public static void Shutdown(ExitCodes code)
        {
            if (code == ExitCodes.Updating)
            {
                var dlgdc = new Dialogs.DownloadDialogDataContext((App.Current as App).UpdateDownloadInfo);
                var dlg = new Dialogs.DownloadDialog(dlgdc);
                dlg.ShowDialog();
            }
            App.Current.Shutdown((int)code);
        }

        public static Stream GetStreamFromEmbeddedResource(string path)
        {
            var ass = Assembly.GetExecutingAssembly();
            var resNames = from name in ass.GetManifestResourceNames() where name.Equals(path) select name;
            foreach (var res in resNames)
            {
                return ass.GetManifestResourceStream(res);
            }
            throw new FileNotFoundException();
        }
        public static object GetXamlObjectFromEmbeddedResource(string path)
        {
            using (var stream = GetStreamFromEmbeddedResource(path))
            {
                try
                {
                    return System.Windows.Markup.XamlReader.Load(stream);
                }
                catch (FileNotFoundException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new Exception("Unknown", ex);
                }
            }
        }
    }
}
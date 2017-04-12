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
using System.Text;

namespace ArmA.Studio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public const string CONST_UPDATESUFFIX = ".update";
        public const string CONST_SOLUTIONEXTENSION = ".asln";
        public const string CONST_BREAKPOINTINFOEXTENSION = ".asbp";
        public const string CONST_CONFIGURATION = "Configuration";


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
        public static string ConfigPath { get { return Path.Combine(ApplicationDataPath, CONST_CONFIGURATION); } }
        public static string FileTemplatePath { get { return Path.Combine(ApplicationDataPath, "Templates"); } }
        public static string TempPath { get { return Path.Combine(Path.GetTempPath(), @"ArmA.Studio"); } }
        public static string CommonApplicationDataPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"ArmA.Studio"); } }
        public static string ApplicationDataPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"ArmA.Studio"); } }
        public static Version CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public static SubscribableTarget SubscribableLoggerTarget { get; private set; }
        public static List<IPlugin> Plugins { get; private set; }

        static App()
        {
            Plugins = new List<IPlugin>();
            Plugins.Add(new DefaultPlugin.PluginMain());
        }

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
                Logger.Info($"Creating new directory at {ConfigPath}.");
            }
            if (!Directory.Exists(FileTemplatePath))
            {
                Directory.CreateDirectory(FileTemplatePath);
                Logger.Info($"Creating new directory at {FileTemplatePath}.");
            }
            if (!Directory.Exists(PluginsPath))
            {
                Directory.CreateDirectory(PluginsPath);
                Logger.Info($"Creating new directory at {PluginsPath}.");
            }
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.SetupNLog();
            DataContext.OutputPane.Initialize();
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
            var dc = new SplashScreenDataContext();
            var dlg = new SplashScreen(dc);
            dlg.Show();
        }

        internal static void ShowOperationFailedMessageBox(Exception ex)
        {
            App.Current.Dispatcher.Invoke(() => MessageBox.Show(string.Format(Studio.Properties.Localization.MessageBoxOperationFailed_Body, ex.Message, ex.GetType().FullName, ex.StackTrace), Studio.Properties.Localization.MessageBoxOperationFailed_Title, MessageBoxButton.OK, MessageBoxImage.Warning));
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
#if !DEBUG
            System.Diagnostics.Debugger.Break();
#else
            SendExceptionReport(e.Exception);
#endif
            e.Handled = true;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (e.ApplicationExitCode == (int)ExitCodes.ConfigError)
                return;
            ConfigHost.Instance.ExecSave();
            if (e.ApplicationExitCode == (int)ExitCodes.Restart || e.ApplicationExitCode == (int)ExitCodes.RestartPluginUpdate)
            {
                Process.Start(ExecutableFile);
            }
        }

        public static void Shutdown(ExitCodes code)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (code == ExitCodes.Updating)
                {
                    var dlgdc = new Dialogs.DownloadToolUpdateDialogDataContext((App.Current as App).UpdateDownloadInfo);
                    var dlg = new Dialogs.DownloadToolUpdateDialog(dlgdc);
                    dlg.ShowDialog();
                }
                App.Current.Shutdown((int)code);
            });
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
        public static IEnumerable<T> GetPlugins<T>() where T : Plugin.IPlugin
        {
            return from plugin in App.Plugins where plugin is T select (T)plugin;
        }

        private static void SendExceptionReport(Exception ex)
        {
            if (!ConfigHost.App.AutoReportException)
                return;
            using (var memStream = new MemoryStream())
            {
                var writer = new XmlTextWriter(new StreamWriter(memStream));

                writer.Formatting = Formatting.Indented;

                writer.WriteStartDocument();
                writer.WriteStartElement("root");
                #region <version>
                writer.WriteStartElement("version");
                writer.WriteString(App.CurrentVersion.ToString());
                writer.WriteEndElement();

                #endregion
                #region <report>
                {
                    writer.WriteStartElement("report");

                    var dlgdc = new Dialogs.ReportDialogDataContext();
                    var dlg = new Dialogs.ReportDialog(dlgdc);
                    var dlgresult = dlg.ShowDialog();
                    if (dlgresult.HasValue && dlgresult.Value)
                    {
                        writer.WriteCData(dlgdc.ReportText);
                    }
                    writer.WriteEndElement();
                }
                #endregion
                #region <stacktrace>
                writer.WriteStartElement("stacktrace");
                var builder = new StringBuilder();
                int tabCount = 0;
                var tmpEx = ex;
                while (tmpEx != null)
                {
                    builder.AppendLine(tmpEx.Message);
                    builder.AppendLine(tmpEx.StackTrace.Replace("\r\n", string.Concat("\r\n", new string('\t', tabCount))));
                    tmpEx = tmpEx.InnerException;
                    tabCount++;
                }
                writer.WriteCData(builder.ToString());
                writer.WriteEndElement();
                #endregion
                #region <trace>
                writer.WriteStartElement("trace");
                //ToDo: Add trace listening
                //foreach (var it in this.TraceListenerInstance.StringQueue)
                //{
                //    writer.WriteStartElement("log");
                //    writer.WriteString(it.Replace("\r", ""));
                //    writer.WriteEndElement();
                //}
                writer.WriteEndElement();
                #endregion

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();


                memStream.Seek(0, SeekOrigin.Begin);
                using (var client = new System.Net.Http.HttpClient())
                {
                    var content = new System.Net.Http.FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("content", new StreamReader(memStream).ReadToEnd()) });
                    using (var response = client.PostAsync("https://x39.io/api.php?action=report&project=ArmA.Studio", content).Result)
                    { }
                }

                Workspace.Instance?.CmdSaveAll.Execute(null);
            }
        }
    }
}
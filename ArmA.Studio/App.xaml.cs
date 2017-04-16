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
using System.Text;

namespace ArmA.Studio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public enum ExitCodes
        {
            ConfigError = -2,
            NoWorkspaceSelected = -1,
            OK = 0,
            Restart = 1,
            Updating = 2
        }
        public static string ExecutablePath { get { return Path.GetDirectoryName(ExecutableFile); } }
        public static string ExecutableFile { get { return Assembly.GetExecutingAssembly().GetName().CodeBase.Substring("file:///".Length); } }
        public static string SyntaxFilesPath { get { return Path.Combine(ExecutablePath, "SyntaxFiles"); } }
        public static string DebuggerPath { get { return Path.Combine(ExecutablePath, "Debugger"); } }
        public static string ConfigPath { get { return Path.Combine(ApplicationDataPath, "Configuration"); } }
        public static string FileTemplatePath { get { return Path.Combine(ApplicationDataPath, "Templates"); } }
        public static string TempPath { get { return Path.Combine(Path.GetTempPath(), @"ArmA.Studio"); } }
        public static string CommonApplicationDataPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"ArmA.Studio"); } }
        public static string ApplicationDataPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"ArmA.Studio"); } }
        public static Version CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public static SubscribableTarget SubscribableLoggerTarget { get; private set; }
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
        private UpdateHelper.DownloadInfo DownloadInfo;
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

            //Save new Workspace to Prev Workspace List
            List<string> prevWorkSpaces = ConfigHost.App.PrevWorkspacePath;
            if (prevWorkSpaces.Contains(workspace))
            {
                prevWorkSpaces.Remove(workspace);
            }
            prevWorkSpaces.Insert(0, workspace);
            ConfigHost.App.PrevWorkspacePath = prevWorkSpaces;

            return true;
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
#if DEBUG
            System.Diagnostics.Debugger.Break();
#else
            SendExceptionReport(e.Exception);
#endif
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (e.ApplicationExitCode == (int)ExitCodes.ConfigError)
                return;
            Workspace.CurrentWorkspace = null;
            if (e.ApplicationExitCode == (int)ExitCodes.Restart)
            {
                Process.Start(ExecutableFile);
            }
            ConfigHost.Instance.ExecSave();
        }

        public static void Shutdown(ExitCodes code)
        {
            if (code == ExitCodes.Updating)
            {
                var dlgdc = new Dialogs.DownloadDialogDataContext((App.Current as App).DownloadInfo);
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

        private static void SendExceptionReport(Exception ex)
        {
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

                Workspace.CurrentWorkspace?.CmdSaveAll.Execute(null);
            }
        }
    }
}
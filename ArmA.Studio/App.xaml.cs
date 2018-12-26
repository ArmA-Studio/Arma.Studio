using ArmA.Studio.Data;
using ArmA.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ArmA.Studio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IApp
    {
        public const string CONST_DOCKING_MANAGER_LAYOUT_NAME = "layout.xml";
        public const string CONST_DOCKING_MANAGER_LAYOUT_JSON_NAME = "layout.json";
        public const string CONST_CONFIG_NAME = "config.xml";
        public const string CONST_VENDOR = "X39";
        public const string CONST_APPLICATIONNAME = "ArmA.Studio";
        public const string CONST_PLUGINS = "Plugins";

        public static readonly string ExecutableFile = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Substring("file:///".Length);
        public static readonly string ExecutablePath = System.IO.Path.GetDirectoryName(ExecutableFile);
        public static readonly string ApplicationDataPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), CONST_VENDOR, CONST_APPLICATIONNAME);
        public static readonly string TempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), CONST_VENDOR, CONST_APPLICATIONNAME);
        public static readonly string CommonApplicationDataPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), CONST_VENDOR, CONST_APPLICATIONNAME);
        public static readonly string ConfigPath = System.IO.Path.Combine(ApplicationDataPath, CONST_CONFIG_NAME);
        public static readonly string LayoutFilePath = System.IO.Path.Combine(ApplicationDataPath, CONST_DOCKING_MANAGER_LAYOUT_NAME);
        public static readonly string LayoutConfigFilePath = System.IO.Path.Combine(ApplicationDataPath, CONST_DOCKING_MANAGER_LAYOUT_JSON_NAME);
        public static readonly Version CurrentVersion = typeof(App).Assembly.GetName().Version;

        public static readonly string PluginDir_Executable = System.IO.Path.Combine(ExecutablePath, CONST_PLUGINS);
        public static readonly string PluginDir_RoamingUser = System.IO.Path.Combine(CommonApplicationDataPath, CONST_PLUGINS);
        public static readonly string PluginDir_Data = System.IO.Path.Combine(ApplicationDataPath, CONST_PLUGINS);

        public static UI.Windows.MainWindowDataContext MWContext => App.Current.MainWindow.DataContext as UI.Windows.MainWindowDataContext;

        IMainWindow IApp.MainWindow => MWContext;

        /// <summary>
        /// Displays generic error messagebox for given exception.
        /// </summary>
        /// <param name="ex">Exception to display.</param>
        public static void DisplayOperationFailed(Exception ex)
        {
            Current.Dispatcher.Invoke(() => MessageBox.Show(String.Format(ArmA.Studio.Properties.Language.App_GenericOperationFailedMessageBox_Body, ex.Message, ex.GetType().FullName, ex.StackTrace), ArmA.Studio.Properties.Language.App_GenericOperationFailedMessageBox_Title, MessageBoxButton.OK, MessageBoxImage.Warning));
        }
        /// <summary>
        /// Displays generic error messagebox for given exception.
        /// Will display the <paramref name="body"/> in front of the exception.
        /// </summary>
        /// <param name="ex">Exception to display.</param>
        /// <param name="body">The Text to display in front of the exception.</param>
        public static void DisplayOperationFailed(Exception ex, string body)
        {
            App.Current.Dispatcher.Invoke(() => MessageBox.Show(String.Concat(body, '\n', String.Format(ArmA.Studio.Properties.Language.App_GenericOperationFailedMessageBox_Body, ex.Message, ex.GetType().FullName, ex.StackTrace)), ArmA.Studio.Properties.Language.App_GenericOperationFailedMessageBox_Title, MessageBoxButton.OK, MessageBoxImage.Warning));
        }
        void IApp.DisplayOperationFailed(Exception ex) => DisplayOperationFailed(ex);
        void IApp.DisplayOperationFailed(Exception ex, string body) => DisplayOperationFailed(ex, body);

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/system.windows.application.dispatcherunhandledexception(v=vs.110).aspx
        /// </summary>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
#if DEBUG
            System.Diagnostics.Debugger.Break();
#endif
        }
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/system.windows.application.exit(v=vs.110).aspx
        /// </summary>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Configuration.Save(ConfigPath);
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            foreach(var dll in System.IO.Directory.GetFiles(App.ExecutablePath, "*.dll"))
            {
                PluginManager.Instance.LoadAssemblySafe(dll);
            }
            Configuration.Load(ConfigPath);
            var splashDataContext = new UI.Windows.SplashScreenDataContext();
            var splash = new UI.Windows.SplashScreen(splashDataContext);
            splash.Show();
        }
    }
}

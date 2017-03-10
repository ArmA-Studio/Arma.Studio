using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ArmA.Studio
{
    public class DebuggerContext : INotifyPropertyChanged
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public bool IsDebuggerAttached { get { return this._IsDebuggerAttached; } set { this._IsDebuggerAttached = value; this.RaisePropertyChanged(); } }
        private bool _IsDebuggerAttached;

        public bool IsPaused { get { return this._IsPaused; } set { this._IsPaused = value; this.RaisePropertyChanged(); } }
        private bool _IsPaused;

        private readonly Debugger.IDebugger DebuggerInstance;

        public ICommand CmdRunDebuggerClick { get; private set; }
        public ICommand CmdPauseDebugger { get; private set; }
        public ICommand CmdStopDebugger { get; private set; }
        public ICommand CmdStepInto { get; private set; }
        public ICommand CmdStepOver { get; private set; }
        public ICommand CmdStepOut { get; private set; }

        public DebuggerContext()
        {
            this._IsDebuggerAttached = false;
            this._IsPaused = false;


            //ToDo: Find debugger dynamically
            this.DebuggerInstance = GetDebuggerInstance();
            if (this.DebuggerInstance == null)
            {
                MessageBox.Show(Properties.Localization.DebuggerContext_NoDebuggerAvailable_Body, Properties.Localization.DebuggerContext_NoDebuggerAvailable_Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                this.DebuggerInstance.OnHalt += DebuggerInstance_OnHalt;
                this.DebuggerInstance.OnConnectionClosed += DebuggerInstance_OnConnectionClosed;
                this.DebuggerInstance.OnError += DebuggerInstance_OnError;
                this.DebuggerInstance.OnException += DebuggerInstance_OnException;
                this.CmdRunDebuggerClick = new UI.Commands.RelayCommand((p) =>
                {
                    if (!this.IsDebuggerAttached)
                    {
                        Logger.Log(NLog.LogLevel.Info, "Attaching debugger...");
                        this.IsDebuggerAttached = this.DebuggerInstance.Attach();
                        if (this.IsDebuggerAttached)
                        {
                            Logger.Log(NLog.LogLevel.Info, "Debugger got attached.");
                        }
                        else
                        {
                            var reason = this.DebuggerInstance.GetLastError();
                            Logger.Log(NLog.LogLevel.Info, string.Format("Failed to attach debugger: {0}.", reason));
                            MessageBox.Show(string.Format(Properties.Localization.DebuggerContext_AttachFailed_Body, reason), Properties.Localization.DebuggerContext_AttachFailed_Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else if(this.IsPaused)
                    {
                        this.IsPaused = false;
                        ExecuteOperation(Debugger.EOperation.Continue);
                    }
                });
                this.CmdStopDebugger = new UI.Commands.RelayCommand((p) =>
                {
                    Logger.Log(NLog.LogLevel.Info, "Detaching debugger...");
                    this.DebuggerInstance.Detach();
                });
                this.CmdPauseDebugger = new UI.Commands.RelayCommand((p) => ExecuteOperation(Debugger.EOperation.Pause));
                this.CmdStepInto = new UI.Commands.RelayCommand((p) => ExecuteOperation(Debugger.EOperation.StepInto));
                this.CmdStepOver = new UI.Commands.RelayCommand((p) => ExecuteOperation(Debugger.EOperation.StepOver));
                this.CmdStepOut = new UI.Commands.RelayCommand((p) => ExecuteOperation(Debugger.EOperation.StepOut));
            }
        }

        public void ExecuteOperation(Debugger.EOperation operation)
        {
            Logger.Log(NLog.LogLevel.Info, string.Format("Executing '{0}' on debugger", Enum.GetName(typeof(Debugger.EOperation), operation)));
            if (!this.DebuggerInstance.Perform(operation))
            {
                var reason = this.DebuggerInstance.GetLastError();
                MessageBox.Show(string.Format(Properties.Localization.DebuggerContext_OperationFailed_Body, Enum.GetName(typeof(Debugger.EOperation), operation), reason), Properties.Localization.DebuggerContext_OperationFailed_Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DebuggerInstance_OnException(object sender, Debugger.OnExceptionEventArgs e)
        {
            Logger.Log(NLog.LogLevel.Info, "OnException got raised.");
            //ToDo: Do something
            MessageBox.Show("DebuggerInstance_OnException");
            System.Diagnostics.Debugger.Break();
        }

        private void DebuggerInstance_OnError(object sender, Debugger.OnErrorEventArgs e)
        {
            Logger.Log(NLog.LogLevel.Info, "OnError got raised.");
            //ToDo: Do something
            MessageBox.Show("DebuggerInstance_OnException");
            System.Diagnostics.Debugger.Break();
        }

        private void DebuggerInstance_OnConnectionClosed(object sender, Debugger.OnConnectionLostEventArgs e)
        {
            Logger.Log(NLog.LogLevel.Info, "Debugger got detached.");
            this.IsDebuggerAttached = false;
        }

        private void DebuggerInstance_OnHalt(object sender, Debugger.OnHaltEventArgs e)
        {
            Logger.Log(NLog.LogLevel.Info, "Execution was halted.");
            this.IsPaused = true;
        }
        /// <summary>
        /// Finds first instance of any debugger in DebuggerPath
        /// </summary>
        /// <returns>Instance of the debugger or null.</returns>
        private static Debugger.IDebugger GetDebuggerInstance()
        {
            Logger.Log(NLog.LogLevel.Info, "Trying to find debugger...");
            var path = App.DebuggerPath;
            if(!Directory.Exists(path))
            {
                Logger.Log(NLog.LogLevel.Error, string.Format("Directory '{0}' is not existing, cannot continue debugger dll search.", path));
                return null;
            }
            foreach (var file in Directory.EnumerateFiles(path, "*.dll"))
            {
                var ass = System.Reflection.Assembly.LoadFile(file);
                var assTypes = ass.GetTypes();
                foreach(var type in assTypes)
                {
                    if(typeof(Debugger.IDebugger).IsAssignableFrom(type))
                    {
                        Logger.Log(NLog.LogLevel.Info, string.Format("Using '{0}' as debugger.", file));
                        return Activator.CreateInstance(type) as Debugger.IDebugger;
                    }
                }
            }
            Logger.Log(NLog.LogLevel.Error, "No debugger found!");
            return null;
        }
    }
}
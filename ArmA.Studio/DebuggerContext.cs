using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ArmA.Studio.Data;
using ArmA.Studio.Data.UI;
using ArmA.Studio.Data.UI.Commands;
using ArmA.Studio.Debugger;
using ArmA.Studio.Plugin;

namespace ArmA.Studio
{
    public class DebuggerContext : INotifyPropertyChanged
    {
        public static DebuggerContext Instance { get; private set; }
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public bool IsDebuggerAttached { get { return this._IsDebuggerAttached; } set { this._IsDebuggerAttached = value; this.RaisePropertyChanged(); } }
        private bool _IsDebuggerAttached;

        public bool IsPaused { get { return this._IsPaused; } set { this._IsPaused = value; this.RaisePropertyChanged(); } }
        private bool _IsPaused;

        public IEnumerable<CallstackItem> CallStack { get { return this._CallStack; } set { this._CallStack = value; this.RaisePropertyChanged(); } }
        private IEnumerable<CallstackItem> _CallStack;

        public IDebuggerPlugin DebuggerInstance { get; private set; }

        public ICommand CmdRunDebuggerClick { get; private set; }
        public ICommand CmdPauseDebugger { get; private set; }
        public ICommand CmdStopDebugger { get; private set; }
        public ICommand CmdStepInto { get; private set; }
        public ICommand CmdStepOver { get; private set; }
        public ICommand CmdStepOut { get; private set; }

        public int CurrentLine { get; private set; }
        public int CurrentColumn { get; private set; }
        public DocumentBase CurrentDocument { get; private set; }

        private CodeEditorBaseDataContext LastEditorContext;

        public DebuggerContext()
        {
            Instance = this;
            this._IsDebuggerAttached = false;
            this._IsPaused = false;
            
            this.DebuggerInstance = App.GetPlugins<IDebuggerPlugin>().FirstOrDefault();
            Logger.Info(this.DebuggerInstance == null ? "Could not locate debugger plugin." : $"Using '{this.DebuggerInstance.Name}' as debugger.");

            if (this.DebuggerInstance == null)
            {
                this.CmdRunDebuggerClick = new RelayCommand((p) => MessageBox.Show(Properties.Localization.DebuggerContext_NoDebuggerAvailable_Body, Properties.Localization.DebuggerContext_NoDebuggerAvailable_Title, MessageBoxButton.OK, MessageBoxImage.Information));
            }
            else
            {
                this.DebuggerInstance.OnHalt += this.DebuggerInstance_OnHalt;
                this.DebuggerInstance.OnConnectionClosed += this.DebuggerInstance_OnConnectionClosed;
                this.DebuggerInstance.OnError += this.DebuggerInstance_OnError;
                this.DebuggerInstance.OnException += this.DebuggerInstance_OnException;
                this.DebuggerInstance.OnContinue += this.DebuggerInstance_OnContinue;
                this.CmdRunDebuggerClick = new RelayCommandAsync(async (p) =>
                {
                    if (!this.IsDebuggerAttached)
                    {
                        Logger.Log(NLog.LogLevel.Info, "Attaching debugger...");
                        this.IsDebuggerAttached = await Task.Run(() => this.DebuggerInstance.Attach());
                        if (this.IsDebuggerAttached)
                        {
                            Logger.Log(NLog.LogLevel.Info, "Debugger got attached.");
                            this.AddAllBreakpointsToDebugger();
                        }
                        else
                        {
                            var reason = this.DebuggerInstance.GetLastError();
                            Logger.Log(NLog.LogLevel.Info, string.Format("Failed to attach debugger: {0}.", reason));
                            MessageBox.Show(string.Format(Properties.Localization.DebuggerContext_AttachFailed_Body, reason), Properties.Localization.DebuggerContext_AttachFailed_Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else if (this.IsPaused)
                    {
                        this.IsPaused = false;
                        await this.ExecuteOperationAsync(EOperation.Continue);
                    }
                });
                this.CmdStopDebugger = new RelayCommandAsync(async (p) =>
                {
                    Logger.Log(NLog.LogLevel.Info, "Detaching debugger...");
                    await Task.Run(() => this.DebuggerInstance.Detach());
                });
                this.CmdPauseDebugger = new RelayCommandAsync((p) => this.ExecuteOperationAsync(EOperation.Pause));
                this.CmdStepInto = new RelayCommandAsync((p) => this.ExecuteOperationAsync(EOperation.StepInto));
                this.CmdStepOver = new RelayCommandAsync((p) => this.ExecuteOperationAsync(EOperation.StepOver));
                this.CmdStepOut = new RelayCommandAsync((p) => this.ExecuteOperationAsync(EOperation.StepOut));

                Workspace.Instance.BreakpointManager.OnBreakPointsChanged += this.BreakpointManager_OnBreakPointsChanged;
            }
        }

        private void BreakpointManager_OnBreakPointsChanged(object sender, BreakpointManager.BreakPointsChangedEventArgs e)
        {
            if (!this.IsDebuggerAttached)
            {
                return;
            }
            switch (e.Mode)
            {
                case BreakpointManager.BreakPointsChangedEventArgs.EMode.Add:
                        this.DebuggerInstance.AddBreakpoint(e.Breakpoint);
                    break;
                case BreakpointManager.BreakPointsChangedEventArgs.EMode.Remove:
                        this.DebuggerInstance.RemoveBreakpoint(e.Breakpoint);
                    break;
                case BreakpointManager.BreakPointsChangedEventArgs.EMode.Update:
                    this.DebuggerInstance.UpdateBreakpoint(e.Breakpoint);
                    break;
                case BreakpointManager.BreakPointsChangedEventArgs.EMode.DrasticChange:
                    this.DebuggerInstance.ClearBreakpoints();
                    this.AddAllBreakpointsToDebugger();
                    break;
            }
        }

        private void AddAllBreakpointsToDebugger()
        {
            foreach (var bpi in Workspace.Instance.BreakpointManager)
            {
                this.DebuggerInstance.AddBreakpoint(bpi);
            }
        }

        #region Event callback methods
        private void DebuggerInstance_OnException(object sender, OnExceptionEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Logger.Log(NLog.LogLevel.Info, "OnException got raised.");
                //ToDo: Do something
                MessageBox.Show("DebuggerInstance_OnException");
                System.Diagnostics.Debugger.Break();
                this.CurrentDocument?.RefreshVisuals();
            }, System.Windows.Threading.DispatcherPriority.Send);
        }

        private void DebuggerInstance_OnError(object sender, OnErrorEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Logger.Log(NLog.LogLevel.Info, string.Format("Error was caught: {0}", e.Message));
                this.CurrentDocument?.RefreshVisuals();
            }, System.Windows.Threading.DispatcherPriority.Send);
        }

        private void DebuggerInstance_OnConnectionClosed(object sender, OnConnectionClosedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Logger.Log(NLog.LogLevel.Info, "Debugger got detached.");
                this.CurrentDocument?.RefreshVisuals();
                this.IsDebuggerAttached = false;
                this.CallStack = Enumerable.Empty<CallstackItem>();
            }, System.Windows.Threading.DispatcherPriority.Send);
        }

        private void DebuggerInstance_OnHalt(object sender, OnHaltEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Logger.Log(NLog.LogLevel.Info, "Execution was halted.");
                this.IsPaused = true;
                this.CurrentLine = e.Line;
                this.CurrentColumn = e.Column;
                this.CallStack = this.DebuggerInstance.GetCallstack();

                Application.Current.MainWindow.Activate();

                var pff = Workspace.Instance.Solution.GetProjectFileFromArmAPath(e.DocumentPath);
                DocumentBase doc;
                if (pff == null)
                {
                    var content = e.DocumentContent;
                    if(string.IsNullOrWhiteSpace(content))
                    {
                        content = "NA";
                    }
                    this.CurrentDocument = doc = Workspace.Instance.CreateOrFocusTemporaryDocument(e.DocumentPath, content, ".sqf");
                    Logger.Info($"Created temporary document for {e.DocumentPath}");
                }
                else
                {
                    this.CurrentDocument = doc = Workspace.Instance.CreateOrFocusDocument(pff);
                }
                doc.RefreshVisuals();
                this.LastEditorContext = doc as CodeEditorBaseDataContext;
            }, System.Windows.Threading.DispatcherPriority.Send);
        }

        private void DebuggerInstance_OnContinue(object sender, OnContinueEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Logger.Log(NLog.LogLevel.Info, "Execution was continued.");
                this.IsPaused = false;
                this.CallStack = Enumerable.Empty<CallstackItem>();
                this.CurrentDocument?.RefreshVisuals();
                if (this.LastEditorContext != null && this.LastEditorContext.IsTemporary)
                {
                    this.LastEditorContext.Close();
                }
                else
                {
                    this.LastEditorContext?.RefreshVisuals();
                }
            }, System.Windows.Threading.DispatcherPriority.Send);
        }
        #endregion

        public async Task UpdateBreakpointAsync(BreakpointInfo bp)
        {
            if (!this.IsDebuggerAttached)
                return;
            await this.DebuggerInstance?.UpdateBreakpointAsync(bp);
        }
        public async Task ExecuteOperationAsync(EOperation operation)
        {
            Logger.Log(NLog.LogLevel.Info, string.Format("Executing '{0}' on debugger", Enum.GetName(typeof(EOperation), operation)));
            if (!await this.DebuggerInstance.PerformAsync(operation))
            {
                var reason = this.DebuggerInstance.GetLastError();
                MessageBox.Show(string.Format(Properties.Localization.DebuggerContext_OperationFailed_Body, Enum.GetName(typeof(EOperation), operation), reason), Properties.Localization.DebuggerContext_OperationFailed_Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public async Task<IEnumerable<Variable>> GetVariablesAsync(EVariableNamespace scope = EVariableNamespace.All, params string[] names)
        {
            if (!this.IsDebuggerAttached)
                return new Variable[0];
            return await this.DebuggerInstance.GetVariablesAsync(scope, names);
        }
        public async Task SetVariableAsync(Variable variable)
        {
            if (!this.IsDebuggerAttached)
                return;
            await this.DebuggerInstance.SetVariableAsync(variable);
        }
        internal void Close()
        {
            this.DebuggerInstance?.Dispose();
        }
    }
}
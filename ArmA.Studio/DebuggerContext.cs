using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ArmA.Studio.SolutionUtil;

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
        public DocumentBase CurrentDocument { get; internal set; }
        public int CurrentLine { get; internal set; }
        public int CurrentColumn { get; private set; }

        public DebuggerContext()
        {
            this._IsDebuggerAttached = false;
            this._IsPaused = false;


            //ToDo: Find debugger dynamically
            this.DebuggerInstance = GetDebuggerInstance();
            if (this.DebuggerInstance == null)
            {
                this.CmdRunDebuggerClick = new UI.Commands.RelayCommand((p) => MessageBox.Show(Properties.Localization.DebuggerContext_NoDebuggerAvailable_Body, Properties.Localization.DebuggerContext_NoDebuggerAvailable_Title, MessageBoxButton.OK, MessageBoxImage.Information));
            }
            else
            {
                this.DebuggerInstance.OnHalt += DebuggerInstance_OnHalt;
                this.DebuggerInstance.OnConnectionClosed += DebuggerInstance_OnConnectionClosed;
                this.DebuggerInstance.OnError += DebuggerInstance_OnError;
                this.DebuggerInstance.OnException += DebuggerInstance_OnException;
                this.DebuggerInstance.OnContinue += DebuggerInstance_OnContinue;
                this.CmdRunDebuggerClick = new UI.Commands.RelayCommandAsync(async (p) =>
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
                        ExecuteOperation(Debugger.EOperation.Continue);
                    }
                });
                this.CmdStopDebugger = new UI.Commands.RelayCommandAsync(async (p) =>
                {
                    Logger.Log(NLog.LogLevel.Info, "Detaching debugger...");
                    await Task.Run(() => this.DebuggerInstance.Detach());
                });
                this.CmdPauseDebugger = new UI.Commands.RelayCommand((p) => ExecuteOperation(Debugger.EOperation.Pause));
                this.CmdStepInto = new UI.Commands.RelayCommand((p) => ExecuteOperation(Debugger.EOperation.StepInto));
                this.CmdStepOver = new UI.Commands.RelayCommand((p) => ExecuteOperation(Debugger.EOperation.StepOver));
                this.CmdStepOut = new UI.Commands.RelayCommand((p) => ExecuteOperation(Debugger.EOperation.StepOut));

                DataContext.BreakpointsPane.Breakpoints.CollectionChanged += Breakpoints_CollectionChanged;
            }
        }

        private void Breakpoints_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!this.IsDebuggerAttached)
            {
                return;
            }
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (DataContext.BreakpointsPaneUtil.Breakpoint it in e.NewItems)
                        this.DebuggerInstance.AddBreakpoint(it);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (DataContext.BreakpointsPaneUtil.Breakpoint it in e.OldItems)
                        this.DebuggerInstance.RemoveBreakpoint(it);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    foreach (DataContext.BreakpointsPaneUtil.Breakpoint it in e.OldItems)
                        this.DebuggerInstance.AddBreakpoint(it);
                    foreach (DataContext.BreakpointsPaneUtil.Breakpoint it in e.NewItems)
                        this.DebuggerInstance.AddBreakpoint(it);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    this.DebuggerInstance.ClearBreakpoints();
                    this.AddAllBreakpointsToDebugger();
                    break;
            }
        }

        private void AddAllBreakpointsToDebugger()
        {
            foreach (var it in DataContext.BreakpointsPane.Breakpoints)
            {
                this.DebuggerInstance.AddBreakpoint(it);
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
            App.Current.Dispatcher.Invoke(() =>
            {
                Logger.Log(NLog.LogLevel.Info, "OnException got raised.");
                //ToDo: Do something
                MessageBox.Show("DebuggerInstance_OnException");
                System.Diagnostics.Debugger.Break();
            }, System.Windows.Threading.DispatcherPriority.Send);
        }

        private void DebuggerInstance_OnError(object sender, Debugger.OnErrorEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
        {
            Logger.Log(NLog.LogLevel.Info, string.Format("Error was caught: {0}", e.Message));
        }, System.Windows.Threading.DispatcherPriority.Send);
        }

        private void DebuggerInstance_OnConnectionClosed(object sender, Debugger.OnConnectionClosedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                Logger.Log(NLog.LogLevel.Info, "Debugger got detached.");
                this.IsDebuggerAttached = false;
            }, System.Windows.Threading.DispatcherPriority.Send);
        }

        private void DebuggerInstance_OnHalt(object sender, Debugger.OnHaltEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                Logger.Log(NLog.LogLevel.Info, "Execution was halted.");
                this.IsPaused = true;
                this.CurrentLine = e.Line;
                this.CurrentColumn = e.Col;

                App.Current.MainWindow.Activate();
                SolutionFile sf = null;
                if (!SolutionFileBase.WalkThrough(Workspace.CurrentWorkspace.CurrentSolution.FilesCollection, (sfb) =>
                 {
                     var f = sfb as SolutionFile;
                     if (f != null && f.ArmAPath == e.DocumentPath)
                     {
                         sf = f;
                         return true;
                     }
                     return false;
                 }))
                {
                    Logger.Log(NLog.LogLevel.Info, string.Format("Could not locate document {0}.", e.DocumentPath));
                    return;
                }
                Workspace.CurrentWorkspace.OpenOrFocusDocument(sf);
                var doc = Workspace.CurrentWorkspace.GetDocumentOfSolutionFileBase(sf) as DataContext.TextEditorDocument;
                if (doc == null)
                {
                    Logger.Log(NLog.LogLevel.Info, string.Format("Document {0} is no TextEditorDocument?", sf.RelativePath));
                    return;
                }
                //ToDo: Remove when document gets closed
                this.CurrentDocument = doc;
            }, System.Windows.Threading.DispatcherPriority.Send);
        }

        private void DebuggerInstance_OnContinue(object sender, Debugger.OnContinueEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                Logger.Log(NLog.LogLevel.Info, "Execution was continued.");
                this.IsPaused = false;
            }, System.Windows.Threading.DispatcherPriority.Send);
        }

        /// <summary>
        /// Finds first instance of any debugger in DebuggerPath
        /// </summary>
        /// <returns>Instance of the debugger or null.</returns>
        private static Debugger.IDebugger GetDebuggerInstance()
        {
            Logger.Log(NLog.LogLevel.Info, "Trying to find debugger...");
            var path = App.DebuggerPath;
            if (!Directory.Exists(path))
            {
                Logger.Log(NLog.LogLevel.Error, string.Format("Directory '{0}' is not existing, cannot continue debugger dll search.", path));
                return null;
            }
            foreach (var file in Directory.EnumerateFiles(path, "*.dll"))
            {
                var ass = System.Reflection.Assembly.LoadFile(file);
                var assTypes = ass.GetTypes();
                foreach (var type in assTypes)
                {
                    if (typeof(Debugger.IDebugger).IsAssignableFrom(type))
                    {
                        Logger.Log(NLog.LogLevel.Info, string.Format("Using '{0}' as debugger.", file));
                        return Activator.CreateInstance(type) as Debugger.IDebugger;
                    }
                }
            }
            Logger.Log(NLog.LogLevel.Error, "No debugger found!");
            return null;
        }

        public void UpdateBreakpoint(DataContext.BreakpointsPaneUtil.Breakpoint bp)
        {
            this.DebuggerInstance?.UpdateBreakpoint(bp);
        }

        internal void Close()
        {
            this.DebuggerInstance?.Dispose();
        }
    }
}
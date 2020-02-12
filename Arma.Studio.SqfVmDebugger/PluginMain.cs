using Arma.Studio.Data;
using Arma.Studio.Data.Debugging;
using Arma.Studio.Data.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arma.Studio.SqfVmDebugger
{
    public class PluginMain : IPlugin, IDebugger, ILogger
    {
        #region IPlugin
        public Version Version => new Version(1, 0, 0);
        public string Name => Properties.Language.SqfVmDebugger_Name;
        public Task<IUpdateInfo> CheckForUpdate(CancellationToken cancellationToken)
        {
            return Task.FromResult(default(IUpdateInfo));
        }

        public Task Initialize(string pluginPath, CancellationToken cancellationToken)
        {
            Logger.Trace($"Creating Virtualmachine.");
            this.Virtualmachine = new sqfvm.ClrVirtualmachine();
            return Task.CompletedTask;
        }
        #endregion
        #region IDebugger
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callee = "")
        {
            this.GetApplication().GetDispatcher().Invoke(() => { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee)); });
        }
        public IEnumerable<EDebugAction> SupportedActions => new EDebugAction[] {
            EDebugAction.Start,
            EDebugAction.StepInto,
            EDebugAction.Resume,
            EDebugAction.Pause,
            EDebugAction.Stop,
            EDebugAction.StepOut
        };
        #region Property: State (Arma.Studio.Data.Debugging.EDebugState)
        public EDebugState State
        {
            get => this._State;
            set
            {
                if (value == this._State)
                {
                    return;
                }
                Logger.Trace($"Changing state from {{{Enum.GetName(typeof(EDebugState), this._State)}}} to {{{Enum.GetName(typeof(EDebugState), value)}}}.");
                this._State = value;
                this.RaisePropertyChanged();
            }
        }
        private EDebugState _State;
        #endregion
        #region Property: Virtualmachine (sqfvm.ClrVirtualmachine)
        public sqfvm.ClrVirtualmachine Virtualmachine
        {
            get => this._Virtualmachine;
            set
            {
                if (value == this._Virtualmachine)
                {
                    return;
                }
                this._Virtualmachine = value;
                this.RaisePropertyChanged();
            }
        }
        private sqfvm.ClrVirtualmachine _Virtualmachine;
        #endregion

        public async Task Execute(EDebugAction action)
        {
            Logger.Diagnostic($"async Task Execute(action: {nameof(EDebugAction)}.{Enum.GetName(typeof(EDebugAction), action)})");
            await Task.Run(() =>
            {
                void printLog()
                {
                    var infoContents = this.Virtualmachine.InfoContents();
                    var warnContents = this.Virtualmachine.WarningContents();
                    var errContents = this.Virtualmachine.ErrorContents();

                    if (infoContents.Length > 0 || warnContents.Length > 0 || errContents.Length > 0)
                    {
                        var builder = new StringBuilder();
                        builder.AppendLine("Execution Result:");
                        if (infoContents.Length > 0)
                        {
                            builder.AppendLine("--- INFO ---");
                            builder.AppendLine(infoContents);
                        }
                        if (warnContents.Length > 0)
                        {
                            builder.AppendLine("--- WARNING ---");
                            builder.AppendLine(warnContents);
                        }
                        if (errContents.Length > 0)
                        {
                            builder.AppendLine("--- ERROR ---");
                            builder.AppendLine(errContents);
                        }
                        Logger.Info(builder);
                    }
                    else
                    {
                        Logger.Info("Executed without any result.");
                    }
                }
                bool execResult = false;
                switch (action)
                {
                    case EDebugAction.Start:
                        var textEditorDocuments = this.GetApplication().MainWindow.ActiveDockable as Data.UI.ITextDocument;
                        if (textEditorDocuments == null)
                        {
                            Logger.Error($"Failed to receive TextDocument via this.GetApplication().MainWindow.ActiveDockable.");
                            throw new NotSupportedException();
                        }
                        var text = textEditorDocuments.GetContents();
                        this.Virtualmachine.ParseSqf(text, textEditorDocuments.TextEditorInstance.File.FullPath);
                        this.State = EDebugState.Running;
                        execResult = this.Virtualmachine.Start();
                        Logger.Diagnostic($"Result of Start: {execResult}");
                        printLog();
                        // ToDo: Determine wether we are really "halted" or actually "NA" (stopped)
                        this.State = EDebugState.Halted;
                        break;
                    case EDebugAction.Stop:
                        execResult = this.Virtualmachine.Abort();
                        Logger.Diagnostic($"Result of Abort: {execResult}");
                        break;
                    case EDebugAction.Pause:
                        execResult = this.Virtualmachine.Stop();
                        Logger.Diagnostic($"Result of Stop: {execResult}");
                        break;
                    case EDebugAction.Resume:
                        this.State = EDebugState.Running;
                        execResult = this.Virtualmachine.Start();
                        Logger.Diagnostic($"Result of Start: {execResult}");
                        printLog();
                        this.State = EDebugState.Halted;
                        break;
                    case EDebugAction.StepOut:
                        this.State = EDebugState.Running;
                        execResult = this.Virtualmachine.LeaveScope();
                        Logger.Diagnostic($"Result of LeaveScope: {execResult}");
                        printLog();
                        this.State = EDebugState.Halted;
                        break;
                    case EDebugAction.StepInto:
                        this.State = EDebugState.Running;
                        execResult = this.Virtualmachine.AssemblyStep();
                        Logger.Diagnostic($"Result of AssemblyStep: {execResult}");
                        printLog();
                        this.State = EDebugState.Halted;
                        break;


                    case EDebugAction.NA:
                    case EDebugAction.StepOver:
                    default:
                        throw new NotSupportedException();
                }
            });
        }

        public IEnumerable<HaltInfo> GetHaltInfos()
        {
            Logger.Diagnostic($"IEnumerable<HaltInfo> GetHaltInfos()");
            var callstack = this.Virtualmachine.GetCallstack();
            var res = callstack.Select((it) => new HaltInfo(it.File)
            {
                Column = (int)it.Column,
                Line = (int)it.Line,
                Content = it.DebugInformations
            }).ToArray();
            return res;
        }

        public Task RemoveBreakpoint(IBreakpoint breakpoint)
        {
            Logger.Diagnostic($"Task RemoveBreakpoint(breakpoint: {{{breakpoint}}})");
            throw new NotSupportedException();
        }

        public Task SetBreakpoint(IBreakpoint breakpoint)
        {
            Logger.Diagnostic($"Task SetBreakpoint(breakpoint: {{{breakpoint}}})");
            throw new NotSupportedException();
        }

        #endregion
        #region ILogger
        internal static Logger Logger { get; private set; }
        public string TargetName => Properties.Language.LoggerName;
        public void SetLogger(Logger logger)
        {
            Logger = logger;
        }
        #endregion
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                }

                // free unmanaged resources (unmanaged objects).
                this.Virtualmachine.Dispose();

                this.disposedValue = true;
            }
        }
        ~PluginMain()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
        }
        #endregion
    }
}

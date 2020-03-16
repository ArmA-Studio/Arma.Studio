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
        public string CfgFunctionsScript { get; private set; }


        public Data.IO.IFileManagement FileManagement => this.GetApplication().MainWindow.FileManagement;


        #region IPlugin
        public Version Version => new Version(1, 0, 0);
        public string Name => Properties.Language.SqfVmDebugger_Name;
        public string Description => String.Empty;
        public Task<IUpdateInfo> CheckForUpdate(CancellationToken cancellationToken)
        {
            return Task.FromResult(default(IUpdateInfo));
        }

        public Task Initialize(string pluginPath, CancellationToken cancellationToken)
        {
            Logger.Trace($"Loading CfgFunctions method.");
            using (var stream = typeof(PluginMain).Assembly.GetManifestResourceStream(typeof(PluginMain).Assembly.GetName().Name + ".CfgFunctions.sqf"))
            using (var reader = new System.IO.StreamReader(stream))
            {
                var text = reader.ReadToEnd().Trim();
                this.CfgFunctionsScript = text;
            }
            return Task.CompletedTask;
        }

        private void Virtualmachine_OnLog(object sender, SqfVm.LogEventArgs eventArgs)
        {
            switch (eventArgs.Severity)
            {
                case SqfVm.ESeverity.Fatal:
                    Logger.Log(ESeverity.Error, eventArgs.Message);
                    break;
                case SqfVm.ESeverity.Error:
                    Logger.Log(ESeverity.Error, eventArgs.Message);
                    break;
                case SqfVm.ESeverity.Warning:
                    Logger.Log(ESeverity.Warning, eventArgs.Message);
                    break;
                case SqfVm.ESeverity.Info:
                    Logger.Log(ESeverity.Info, eventArgs.Message);
                    break;
                case SqfVm.ESeverity.Verbose:
                    Logger.Log(ESeverity.Trace, eventArgs.Message);
                    break;
                case SqfVm.ESeverity.Trace:
                    Logger.Log(ESeverity.Diagnostic, eventArgs.Message);
                    break;
                default:
                    break;
            }
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
            EDebugAction.Step,
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
        public SqfVm.ClrVirtualmachine Virtualmachine
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
        private SqfVm.ClrVirtualmachine _Virtualmachine;
        #endregion
        public async Task Execute(EDebugAction action)
        {
            if ((this.Virtualmachine == null && action != EDebugAction.Start) || (this.Virtualmachine != null && action == EDebugAction.Start))
            {
                throw new InvalidOperationException();
            }
            Logger.Diagnostic($"async Task Execute(action: {nameof(EDebugAction)}.{Enum.GetName(typeof(EDebugAction), action)})");
            await Task.Run(() =>
            {
                bool execResult = false;
                switch (action)
                {
                    case EDebugAction.Start:
                        Logger.Trace($"Creating Virtualmachine.");
                        this.Virtualmachine = new SqfVm.ClrVirtualmachine();
                        this.Virtualmachine.OnLog += this.Virtualmachine_OnLog;
                        bool hasConfig = false;
                        // Prepare all PBO virtual paths
                        foreach (var pbo in this.FileManagement.Where((it) => it is Data.IO.PBO).Cast<Data.IO.PBO>())
                        {
                            if (pbo.Prefix is null)
                            {
                                this.Virtualmachine.AddPhysicalBoundary(pbo.FullPath);
                            }
                            else
                            {
                                this.Virtualmachine.AddVirtualMapping(pbo.Prefix, pbo.FullPath);
                            }
                            // Parse config.cpp/description.ext
                            var config = pbo.GetAll((file) => file.Name == "config.cpp" || file.Name == "description.ext");
                            if (config.Any())
                            {
                                foreach (var file in config)
                                {
                                    var fileContents = file.GetText();
                                    var filePreprocessed = this.Virtualmachine.PreProcess(fileContents, file.FullPath);
                                    this.Virtualmachine.ParseConfig(filePreprocessed, file.FullPath);
                                }
                                hasConfig = true;
                            }
                            else
                            {
                                Logger.Warning($"No config file located for PBO {pbo.Name}.");
                            }
                        }
                        // Apply Breakpoints
                        foreach (var breakpoint in this.GetApplication().MainWindow.BreakpointManager.Breakpoints)
                        {
                            this.SetBreakpoint(breakpoint);
                        }
                        if (hasConfig)
                        {
                            // Run CfgFunctions script
                            var text = this.CfgFunctionsScript;
                            var preprocessed = this.Virtualmachine.PreProcess(text, "SqfVmDebugger/CfgFunctions.sqf");
                            this.Virtualmachine.ParseSqf(preprocessed, "SqfVmDebugger/CfgFunctions.sqf");
                            this.State = EDebugState.Running;
                            execResult = this.Virtualmachine.Start();
                            Logger.Diagnostic($"Result of Start: {execResult}");
                        }
                        else
                        {
                            Logger.Warning("No config files available. Trying to use currently open document.");
                            var editorDocument = this.GetApplication().MainWindow.ActiveDockable as Data.UI.IEditorDocument;
                            if (editorDocument != null && editorDocument.File.Extension == ".sqf")
                            {
                                var text = editorDocument.GetContents();
                                var preprocessed = this.Virtualmachine.PreProcess(text, editorDocument.TextEditorInstance.File.FullPath);
                                this.Virtualmachine.ParseSqf(preprocessed, editorDocument.TextEditorInstance.File.FullPath);
                                this.State = EDebugState.Running;
                                execResult = this.Virtualmachine.Start();
                                Logger.Diagnostic($"Result of Start: {execResult}");
                            }
                            else
                            {
                                Logger.Error("Found nothing to run.");
                            }
                        }
                        break;
                    case EDebugAction.Stop:
                        execResult = this.Virtualmachine.Abort();
                        Logger.Diagnostic($"Result of Abort: {execResult}");
                        SpinWait.SpinUntil(() => !this.Virtualmachine.IsVirtualmachineRunning);
                        break;
                    case EDebugAction.Pause:
                        execResult = this.Virtualmachine.Stop();
                        Logger.Diagnostic($"Result of Stop: {execResult}");
                        break;
                    case EDebugAction.Resume:
                        this.State = EDebugState.Running;
                        execResult = this.Virtualmachine.Start();
                        Logger.Diagnostic($"Result of Start: {execResult}");
                        break;
                    case EDebugAction.StepOut:
                        this.State = EDebugState.Running;
                        execResult = this.Virtualmachine.LeaveScope();
                        Logger.Diagnostic($"Result of LeaveScope: {execResult}");
                        break;
                    case EDebugAction.Step:
                        this.State = EDebugState.Running;
                        execResult = this.Virtualmachine.AssemblyStep();
                        Logger.Diagnostic($"Result of AssemblyStep: {execResult}");
                        break;
                    case EDebugAction.StepInto:
                        this.State = EDebugState.Running;
                        execResult = this.Virtualmachine.LineStep();
                        Logger.Diagnostic($"Result of AssemblyStep: {execResult}");
                        break;
                    case EDebugAction.NA:
                    case EDebugAction.StepOver:
                    default:
                        throw new NotSupportedException();
                }
                this.State = (this.Virtualmachine?.IsVirtualmachineRunning ?? false) ? EDebugState.Running : (this.Virtualmachine?.IsVirtualmachineDone ?? true) ? EDebugState.NA : EDebugState.Halted;
                if (this.State == EDebugState.Halted)
                {
                    var callstack = this.Virtualmachine.GetCallstack();
                    var firstCallstackItem = callstack.First();
                    if (this.FileManagement.ContainsKey(firstCallstackItem.File))
                    {
                        var file = this.FileManagement[callstack.First().File] as Data.IO.File;
                        this.GetApplication().MainWindow.OpenFile(file).ContinueWith((textDocument) =>
                        {
                            textDocument.Result.ScrollTo((int)firstCallstackItem.Line, (int)firstCallstackItem.Column);
                        }, TaskContinuationOptions.OnlyOnRanToCompletion);
                    }
                }
                if (this.State == EDebugState.NA)
                {
                    Logger.Trace($"Destroying Virtualmachine.");
                    this.Virtualmachine.OnLog -= this.Virtualmachine_OnLog;
                    this.Virtualmachine.Dispose();
                    this.Virtualmachine = null;
                }
            });
        }

        public IEnumerable<HaltInfo> GetHaltInfos()
        {
            if (this.Virtualmachine == null)
            {
                return Array.Empty<HaltInfo>();
            }
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
            this.Virtualmachine?.RemoveBreakpoint(breakpoint.Line, breakpoint.File);
            return Task.CompletedTask;
        }

        public Task SetBreakpoint(IBreakpoint breakpoint)
        {
            Logger.Diagnostic($"Task SetBreakpoint(breakpoint: {{{breakpoint}}})");
            this.Virtualmachine?.SetBreakpoint(breakpoint.Line, breakpoint.File);
            return Task.CompletedTask;
        }

        public IEnumerable<VariableInfo> GetLocalVariables()
        {
            if (this.Virtualmachine == null)
            {
                throw new InvalidOperationException();
            }
            Logger.Diagnostic($"IEnumerable<VariableInfo> GetLocalVariables()");
            return this.Virtualmachine.GetLocalVariables().Select((it) => new VariableInfo
            {
                Data = it.Data,
                DataType = it.DataType,
                ScopeIndex = it.ScopeIndex,
                ScopeName = it.ScopeName,
                VariableName = it.VariableName
            });
        }

        public bool SetVariable(string variableName, string data, ENamespace @namespace)
        {
            if (this.Virtualmachine == null)
            {
                throw new InvalidOperationException();
            }
            Logger.Diagnostic($"bool SetVariable(variableName: {{{variableName}}}, data: {{{data}}}, @namespace: {{{@namespace}}})");
            return this.Virtualmachine.SetVariable(variableName, data, @namespace switch
            {
                ENamespace.Default => "",
                ENamespace.MissionNamespace => "missionNamespace",
                ENamespace.ParsingNamespace => "parsingNamespace",
                ENamespace.ProfileNamespace => "profileNamespace",
                ENamespace.UINamespace => "uiNamespace",
                _ => throw new NotImplementedException()
            });
        }

        public VariableInfo GetVariable(string variableName, ENamespace @namespace)
        {
            if (this.Virtualmachine == null)
            {
                throw new InvalidOperationException();
            }
            Logger.Diagnostic($"bool VariableInfo(variableName: {{{variableName}}}, @namespace: {{{@namespace}}})");
            var varref = this.Virtualmachine.GetVariable(variableName, @namespace switch {
                ENamespace.Default => "",
                ENamespace.MissionNamespace => "missionNamespace",
                ENamespace.ParsingNamespace => "parsingNamespace",
                ENamespace.ProfileNamespace => "profileNamespace",
                ENamespace.UINamespace => "uiNamespace",
                _ => throw new NotImplementedException()
            });
            return new VariableInfo
            {
                Data = varref.Data,
                DataType = varref.DataType,
                ScopeIndex = varref.ScopeIndex,
                ScopeName = varref.ScopeName,
                VariableName = varref.VariableName
            };
        }

        public SqfValue Evaluate(string text)
        {
            if (this.Virtualmachine == null)
            {
                throw new InvalidOperationException();
            }
            var res = this.Virtualmachine.Evaluate(text);
            return new SqfValue { Data = res.Data, DataType = res.DataType };
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
                this.Virtualmachine?.Dispose();

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

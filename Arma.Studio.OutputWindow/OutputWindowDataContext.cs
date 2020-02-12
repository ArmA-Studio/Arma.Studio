using Arma.Studio.Data;
using Arma.Studio.Data.IO;
using Arma.Studio.Data.Log;
using Arma.Studio.Data.TextEditor;
using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Arma.Studio.OutputWindow
{
    public class OutputWindowDataContext : DockableBase, IDisposable
    {
        private static readonly OutputTarget NullTarget = new OutputTarget("NULL");
        public class OutputTarget
        {
            public string Title { get; set; }
            public ICSharpCode.AvalonEdit.Document.TextDocument TextDocument { get; }
            public OutputTarget(string title)
            {
                this.Title = title;
                this.TextDocument = new ICSharpCode.AvalonEdit.Document.TextDocument();
                this.TextDocument.UndoStack.SizeLimit = 0;
            }
            public void OnLog(object sender, LogEventArgs e)
            {
                var message = String.Concat(e.Severity switch
                {
                    ESeverity.Diagnostic => "[DGN]  ",
                    ESeverity.Trace => "[TRC]  ",
                    ESeverity.Info => "[OUT]  ",
                    ESeverity.Warning => "[WRN]  ",
                    ESeverity.Error => "[ERR]  ",
                    _ => throw new NotImplementedException()
                }, e.Message, Environment.NewLine);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.TextDocument.Insert(this.TextDocument.TextLength, message);
                    var ex = e.Exception;
                    while (ex != null)
                    {
                        this.TextDocument.Insert(this.TextDocument.TextLength, e.Exception.Message);
                        if (e.Exception.Data != null && e.Exception.Data.Count > 0)
                        {
                            // ToDo: Localize
                            this.TextDocument.Insert(this.TextDocument.TextLength, "    With Data: ");
                            try
                            {
                                foreach (System.Collections.DictionaryEntry datapair in e.Exception.Data)
                                {
                                    this.TextDocument.Insert(this.TextDocument.TextLength, String.Concat("        ", Convert.ToString(datapair.Key), ": ", Convert.ToString(datapair.Value)));
                                }
                            }
                            catch (Exception)
                            {
                                // ToDo: Localize
                                this.TextDocument.Insert(this.TextDocument.TextLength, "    -- Failed to stringify all data --");
                            }
                        }
                        this.TextDocument.Insert(this.TextDocument.TextLength, e.Exception.StackTrace);
                        ex = e.Exception;
                        if (ex != null)
                        {
                            this.TextDocument.Insert(this.TextDocument.TextLength, new string('-', 32));
                        }
                        
                    }
                });
            }
        }
        #region Property: SelectedOutputTarget (OutputTarget)
        public OutputTarget SelectedOutputTarget
        {
            get => this._SelectedOutputTarget;
            set
            {
                this._SelectedOutputTarget = value;
                this.RaisePropertyChanged();
            }
        }
        private OutputTarget _SelectedOutputTarget;
        #endregion
        #region Property: IsWordWrapToggled (System.Nullable<System.Boolean>)
        public bool? IsWordWrapToggled
        {
            get => this._IsWordWrapToggled;
            set
            {
                this._IsWordWrapToggled = value;
                this.RaisePropertyChanged();
            }
        }
        private bool? _IsWordWrapToggled;
        #endregion
        #region Property: IsWordWrapToggled (System.Nullable<System.Boolean>)
        public bool? AutoScroll
        {
            get => this._AutoScroll;
            set
            {
                this._AutoScroll = value;
                this.RaisePropertyChanged();
            }
        }
        private bool? _AutoScroll;
        #endregion

        #region Collection: AvailableOutputTargets
        public ObservableCollection<OutputTarget> AvailableOutputTargets { get; }
        #endregion
        #region Dictionary: DictionaryOfOutputTargets
        public Dictionary<IPlugin, OutputTarget> DictionaryOfOutputTargets { get; }
        #endregion

        public OutputWindowDataContext()
        {
            this.AvailableOutputTargets = new ObservableCollection<OutputTarget>();
            this.DictionaryOfOutputTargets = new Dictionary<IPlugin, OutputTarget>();
            foreach (var logger in PluginMain.Loggers)
            {
                var outputTarget = new OutputTarget(logger.RelatedPlugin.Name);
                this.DictionaryOfOutputTargets[logger.RelatedPlugin] = outputTarget;
                this.AvailableOutputTargets.Add(outputTarget);
                logger.OnLog += outputTarget.OnLog;
            }
            this.SelectedOutputTarget = this.AvailableOutputTargets.Any() ? this.AvailableOutputTargets.First() : NullTarget;
            this._IsWordWrapToggled = true;
            this._AutoScroll = true;
        }

        public ICommand CmdClearOutputWindow => new RelayCommand(() => Application.Current.Dispatcher.Invoke(() => this.SelectedOutputTarget.TextDocument.Text = string.Empty));
        public override string Title { get => Properties.Language.OutputWindow; set { throw new NotSupportedException(); } }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    foreach (var logger in PluginMain.Loggers)
                    {
                        var outputTarget = this.DictionaryOfOutputTargets[logger.RelatedPlugin];
                        logger.OnLog -= outputTarget.OnLog;
                    }
                }
                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
        #endregion
    }
}

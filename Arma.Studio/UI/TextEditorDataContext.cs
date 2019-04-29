﻿using Arma.Studio.Data;
using Arma.Studio.Data.IO;
using Arma.Studio.Data.TextEditor;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Arma.Studio.UI
{
    public class TextEditorDataContext : DockableBase, INotifyPropertyChanged, Data.UI.AttachedProperties.IOnInitialized, IDisposable
    {
        private static readonly TimeSpan LintTimeout = new TimeSpan(0, 0, 0, 0, 200);


        public TextDocument TextDocument { get; }
        public ITextEditor TextEditorInstance
        {
            get => this._TextEditorInstance;
            set
            {
                this._TextEditorInstance = value;
                this.CreateHighlightingDefinition(value.SyntaxFile);
                value.File = this.File;
                this.RaisePropertyChanged();
            }
        }
        private ITextEditor _TextEditorInstance;
        public TextEditor TextEditorControl
        {
            get => this._TextEditorControl;
            set { this._TextEditorControl = value; this.RaisePropertyChanged(); }
        }
        private TextEditor _TextEditorControl;
        //IHighlightingDefinition

        public bool IsReadOnly
        {
            get => this._IsReadOnly;
            set { this._IsReadOnly = value; this.RaisePropertyChanged(); }
        }
        private bool _IsReadOnly;

        public IHighlightingDefinition SyntaxHighlightingDefinition
        {
            get => this._SyntaxHighlightingDefinition;
            set { this._SyntaxHighlightingDefinition = value; this.RaisePropertyChanged(); }
        }
        private IHighlightingDefinition _SyntaxHighlightingDefinition;

        private CancellationTokenSource LintingCancellationTokenSource { get; set; }
        private Task LintingTask { get; set; }
        private CancellationTokenSource FoldingCancellationTokenSource { get; set; }
        private Task FoldingTask { get; set; }
        private TextEditorDataContext()
        {
            this.LintingCancellationTokenSource = new CancellationTokenSource();
            this.FoldingCancellationTokenSource = new CancellationTokenSource();
            this.LintingTask = Task.CompletedTask;
            this.FoldingTask = Task.CompletedTask;
            this.TextChangedTimer = new DispatcherTimer();
            this.TextChangedTimer.Tick += this.TextChangedTimer_Tick;
            this.TextChangedTimer.Interval = LintTimeout;
            this.TextDocument = new TextDocument();
            this.TextDocument.Changed += this.TextDocument_Changed;
        }
        public TextEditorDataContext(ITextEditor textEditor, File file) : this()
        {
            this.TextEditorInstance = textEditor;
            this.File = file;
            this.Title = this.File.Name;
        }
        public override void LayoutSaveCallback(dynamic section)
        {
            section.file = this.File.FullPath;
            section.type = this.TextEditorInstance.GetType().FullName;
        }
        public override void LayoutLoadCallback(dynamic section)
        {
            this.File = (File)App.MWContext.FileManagement[section.file as string];
            var type = section.type as string;

            if (System.IO.File.Exists(this.File.FullPath))
            {
                using (var reader = new System.IO.StreamReader(this.File.FullPath))
                {
                    this.TextDocument.Text = reader.ReadToEnd();
                    this.TextDocument.UndoStack.ClearAll();
                }
            }
            else
            {
                MessageBox.Show(this.File.FullPath, "File Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
                return;
            }

            var textEditor = App.MWContext.TextEditorsAvailable.FirstOrDefault((tei) => tei.Type.FullName == type);
            if (textEditor.IsAsync)
            {
                textEditor.CreateAsyncFunc().ContinueWith((t) =>
                {
                    this.TextEditorInstance = t.Result;
                });
            }
            else
            {
                this.TextEditorInstance = textEditor.CreateFunc();
            }
        }

        private LintInfo[] LastLintInfos = new LintInfo[0];
        private FoldingInfo[] LastFoldingInfos = new FoldingInfo[0];
        public IEnumerable<LintInfo> GetLintInfos() => this.LastLintInfos;

        #region Property: LastChangeTimestamp (System.DateTime)
        public DateTime LastChangeTimestamp
        {
            get => this._LastChangeTimestamp;
            set { this._LastChangeTimestamp = value; this.RaisePropertyChanged(); }
        }
        private DateTime _LastChangeTimestamp;
        #endregion
        #region Property: File (Arma.Studio.Data.IO.File)
        public File File
        {
            get => this._File;
            set
            {
                if (this._File != null)
                {
                    this._File.PropertyChanged -= this.File_PropertyChanged;
                }
                this._File = value;
                if (this._File != null)
                {
                    this._File.PropertyChanged += this.File_PropertyChanged;
                }
                if (this.TextEditorInstance != null)
                {
                    this.TextEditorInstance.File = value;
                }
                this.RaisePropertyChanged();
            }
        }
        private File _File;
        #endregion
        #region Property: FoldingManager (ICSharpCode.AvalonEdit.Folding.FoldingManager)
        public FoldingManager FoldingManager { get; private set; }
        #endregion
        #region Field: TextChangedTimer [readonly] (System.Windows.Threading.DispatcherTimer)
        private readonly DispatcherTimer TextChangedTimer;
        #endregion

        private void TextChangedTimer_Tick(object sender, EventArgs e)
        {
            this.TextChangedTimer.Stop();
            if (this.TextEditorInstance is ILintable lintable)
            {
                if (!this.LintingTask.IsCompleted)
                {
                    this.LintingCancellationTokenSource.Cancel();
                }
                this.LintingCancellationTokenSource = new CancellationTokenSource();
                this.LintingTask = Task.Run(async () =>
                {
                    var res = await lintable.GetLintInfos(App.Current.Dispatcher.Invoke(() => this.TextDocument.Text), this.LintingCancellationTokenSource.Token);
                    this.LastLintInfos = res.ToArray();
                    App.Current.Dispatcher.Invoke(() => this.TextEditorControl.TextArea.TextView.InvalidateLayer(ICSharpCode.AvalonEdit.Rendering.KnownLayer.Background));
                });
            }
            if (this.TextEditorInstance is IFoldable foldable)
            {
                if (!this.FoldingTask.IsCompleted)
                {
                    this.FoldingCancellationTokenSource.Cancel();
                }
                this.FoldingCancellationTokenSource = new CancellationTokenSource();
                this.FoldingTask = Task.Run(async () =>
                {
                    var res = await foldable.GetFoldings(App.Current.Dispatcher.Invoke(() => this.TextDocument.Text), this.FoldingCancellationTokenSource.Token);
                    this.LastFoldingInfos = res.ToArray();
                    var foldings = new List<NewFolding>(this.LastFoldingInfos.Length);
                    foreach (var it in this.LastFoldingInfos)
                    {
                        if (it.StartOffset.HasValue)
                        {
                            foldings.Add(new NewFolding { StartOffset = it.StartOffset.Value, EndOffset = it.StartOffset.Value + it.Length });
                        }
                        else
                        {
                            var off = App.Current.Dispatcher.Invoke(() => this.TextDocument.GetOffset(it.LineStart.Value, it.ColumnStart.Value));
                            foldings.Add(new NewFolding { StartOffset = off, EndOffset = off + it.Length });
                        }
                    }
                    foldings.Sort((l, r) => l.StartOffset.CompareTo(r.StartOffset));
                    App.Current.Dispatcher.Invoke(() => this.FoldingManager.UpdateFoldings(foldings, -1));
                });
            }
        }



        private void TextDocument_Changed(object sender, DocumentChangeEventArgs e)
        {
            this.LastChangeTimestamp = DateTime.Now;
            if (!this.LintingCancellationTokenSource.IsCancellationRequested)
            {
                this.LintingCancellationTokenSource.Cancel();
            }
            if (!this.FoldingCancellationTokenSource.IsCancellationRequested)
            {
                this.FoldingCancellationTokenSource.Cancel();
            }
            this.TextChangedTimer.Stop();
            this.TextChangedTimer.Start();
            if (e.InsertedText.Text.Contains('\n'))
            {
                var count = e.InsertedText.Text.Count((c) => c == '\n');
                var line = this.TextDocument.GetLineByOffset(e.Offset);
                var lineNumber = line.LineNumber;
                if (this.File != null)
                {
                    var breakpoints = App.MWContext.BreakpointManager.GetBreakpoints(this.File, (bp) => bp.Line > lineNumber);
                    foreach (var bp in breakpoints)
                    {
                        bp.Line += count;
                    }
                }
            }
            if (e.RemovedText.Text.Contains('\n'))
            {
                var count = e.RemovedText.Text.Count((c) => c == '\n');
                var line = this.TextDocument.GetLineByOffset(e.Offset);
                var lineNumber = line.LineNumber;
                if (this.File != null)
                {
                    var breakpoints = App.MWContext.BreakpointManager.GetBreakpoints(this.File, (bp) => bp.Line > lineNumber);
                    foreach (var bp in breakpoints)
                    {
                        if (bp.Line <= lineNumber + count)
                        {
                            App.MWContext.BreakpointManager.RemoveBreakpoint(this.File, bp);
                        }
                        else
                        {
                            bp.Line -= count;
                        }
                    }
                }
            }
        }

        private void CreateHighlightingDefinition(SyntaxFile syntaxFile)
        {
            using (var memstream = new System.IO.MemoryStream())
            {
                using (var writer = System.Xml.XmlWriter.Create(memstream))
                {
                    writer.WriteStartElement("SyntaxDefinition");
                    writer.WriteAttributeString("name", "Config");
                    {
                        writer.WriteStartElement("Digits");
                        writer.WriteAttributeString("name", "Digits");
                        writer.WriteAttributeString("color", syntaxFile.DigitsColor.ToString());
                        writer.WriteEndElement();

                        writer.WriteStartElement("RuleSets");
                        writer.WriteStartElement("RuleSet");
                        writer.WriteAttributeString("ignorecase", syntaxFile.IgnoreCase.ToString().ToLower());
                        {
                            writer.WriteElementString("Delimiters", syntaxFile.Delimeters);

                            int i = 0;
                            foreach (var it in syntaxFile.Enclosures)
                            {
                                writer.WriteStartElement("Span");
                                writer.WriteAttributeString("name", $"OpenClose{i++}");
                                writer.WriteAttributeString("color", it.Color.ToString());
                                if (String.IsNullOrWhiteSpace(it.Close))
                                {
                                    writer.WriteAttributeString("stopateol", "true");
                                }
                                writer.WriteElementString("Begin", it.Open);
                                if (!String.IsNullOrWhiteSpace(it.Close))
                                {
                                    writer.WriteElementString("End", it.Close);
                                }
                                writer.WriteEndElement();
                            }

                            i = 0;
                            foreach(var it in syntaxFile.Keywords)
                            {
                                writer.WriteStartElement("KeyWords");
                                writer.WriteAttributeString("name", $"ControlStructure{i++}");
                                writer.WriteAttributeString("color", it.Color.ToString());
                                writer.WriteAttributeString("bold", it.IsBold.ToString().ToLower());
                                foreach (var key in it)
                                {
                                    writer.WriteStartElement("Key");
                                    writer.WriteAttributeString("word", syntaxFile.IgnoreCase ? key.ToLower() : key);
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                memstream.Seek(0, System.IO.SeekOrigin.Begin);
                using (var reader = System.Xml.XmlReader.Create(memstream))
                {
                    this.SyntaxHighlightingDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
        }


        private void File_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Title = this.File.Name;
        }



        public void OnInitialized(FrameworkElement sender, EventArgs e)
        {
            if (sender is TextEditor textEditor)
            {
                this.TextEditorControl = textEditor;
                var bpm = new BreakPointMargin(this);
                textEditor.TextArea.TextView.BackgroundRenderers.Add(new UnderlineBackgroundRenderer(this));
                textEditor.TextArea.TextView.BackgroundRenderers.Add(bpm);
                textEditor.TextArea.TextView.BackgroundRenderers.Add(new LineHighlighterBackgroundRenderer(textEditor));
                textEditor.TextArea.LeftMargins.Insert(0, bpm);
                textEditor.TextArea.LeftMargins.Insert(1, new RuntimeExecutionMargin(this));
                this.FoldingManager = ICSharpCode.AvalonEdit.Folding.FoldingManager.Install(textEditor.TextArea);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.FoldingTask?.Dispose();
                    this.LintingTask?.Dispose();
                    (this.TextEditorInstance as IDisposable)?.Dispose();
                    this.File.PropertyChanged -= this.File_PropertyChanged;
                }


                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
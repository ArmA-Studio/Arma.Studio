using Arma.Studio.Data;
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
    public class TextEditorDataContext : DockableBase, INotifyPropertyChanged, Data.UI.AttachedProperties.IOnInitialized
    {
        private static readonly TimeSpan LintTimeout = new TimeSpan(0, 0, 0, 0, 200);


        public TextDocument TextDocument { get; }
        public ITextEditor TextEditorInstance
        {
            get => this._TextEditorInstance;
            set { this._TextEditorInstance = value; this.CreateHighlightingDefinition(value.SyntaxFile); this.RaisePropertyChanged(); }
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
        public TextEditorDataContext(ITextEditor textEditor)
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
            this.TextEditorInstance = textEditor;
            this.File = new File() { Name = "tmp" };
            this.TextDocument.Text =
                new string('a', 15) + "\n" +
                new string('b', 15) + "\n" +
                new string('c', 15) + "\n" +
                new string('d', 15) + "\n" +
                new string('e', 15) + "\n" +
                new string('f', 15) + "\n" +
                new string('g', 15) + "\n" +
                new string('h', 15) + "\n" +
                new string('i', 15) + "\n" +
                new string('j', 15) + "\n" +
                new string('k', 15) + "\n" +
                new string('l', 15) + "\n" +
                new string('m', 15) + "\n" +
                new string('n', 15) + "\n";
        }

        private LintInfo[] LastLintInfos = new LintInfo[0];
        private FoldingInfo[] LastFoldingInfos = new FoldingInfo[0];
        public IEnumerable<LintInfo> GetLintInfos() => this.LastLintInfos;
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
                    App.Current.Dispatcher.Invoke(() => this.FoldingManager.UpdateFoldings(
                        this.LastFoldingInfos
                        .Where((it) => it.StartOffset >= 0 && it.EndOffset <= this.TextDocument.TextLength)
                        .Select((it) => new NewFolding { StartOffset = it.StartOffset, EndOffset = it.EndOffset }), -1));
                });
            }
        }

        public DateTime LastChangeTimestamp
        {
            get => this._LastChangeTimestamp;
            set { this._LastChangeTimestamp = value; this.RaisePropertyChanged(); }
        }
        private DateTime _LastChangeTimestamp;

        private readonly DispatcherTimer TextChangedTimer;

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
                var breakpoints = App.MWContext.BreakpointManager.GetBreakpoints(this.File, (bp) => bp.Line > lineNumber);
                foreach (var bp in breakpoints)
                {
                    bp.Line += count;
                }
            }
            if (e.RemovedText.Text.Contains('\n'))
            {
                var count = e.RemovedText.Text.Count((c) => c == '\n');
                var line = this.TextDocument.GetLineByOffset(e.Offset);
                var lineNumber = line.LineNumber;
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
        
        public File File
        {
            get => this._File;
            set { this._File = value; this.RaisePropertyChanged(); }
        }
        private File _File;

        public FoldingManager FoldingManager { get; private set; }


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

    }
}

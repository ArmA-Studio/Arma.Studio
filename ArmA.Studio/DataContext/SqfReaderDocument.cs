using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ArmA.Studio.DataContext.TextEditorUtil;
using ArmA.Studio.Debugger;
using ArmA.Studio.UI;
using ArmA.Studio.UI.Commands;
using ICSharpCode.AvalonEdit.Highlighting;
using RealVirtuality.SQF.Parser;

namespace ArmA.Studio.DataContext
{
    public class SQFReaderDocument : TextEditorDocument
    {
        public sealed class VariableContainer : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

            public ICommand CmdContentClicked { get { return new RelayCommand((p) => this.EditMode = true); } }
            public ICommand CmdLostKeyboardFocus { get { return new RelayCommand((p) => this.EditMode = false); } }

            public string Name { get { return this.InternalVariable.Name; } }

            public bool EditMode { get { return this._EditMode; } set { this._EditMode = value; this.RaisePropertyChanged(); } }
            private bool _EditMode;

            public string Value
            {
                get
                {
                    return this.InternalVariable.Value;
                }
                set
                {
                    //ToDo: Validate correct type remains and pick correct one if not
                    if (this.InternalVariable.Value == value)
                        return;
                    var variable = this.InternalVariable;
                    variable.Value = value;
                    //ToDo: Check if proper async maybe is required due to UI lag
                    Workspace.CurrentWorkspace.DebugContext.SetVariable(variable).Wait();
                    this.InternalVariable = variable;
                }
            }

            public Variable InternalVariable { get { return this._InternalVariable; } set { this._InternalVariable = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(this.Name)); this.RaisePropertyChanged(nameof(this.Value)); } }
            private Variable _InternalVariable;
        }
        private static IHighlightingDefinition ThisSyntaxName { get; set; }
        private static Popup SqfVariableViewPopup { get; set; }
        static SQFReaderDocument()
        {
            ThisSyntaxName = LoadAvalonEditSyntaxFiles(Path.Combine(App.SyntaxFilesPath, "sqf.xshd"));
            SqfVariableViewPopup = App.GetXamlObjectFromEmbeddedResource("ArmA.Studio.UI.SqfVariableViewPopup.xaml") as Popup;
            SqfVariableViewPopup.DataContext = new VariableContainer();
            SqfVariableViewPopup.Placement = PlacementMode.Relative;
        }
        public override string[] SupportedFileExtensions { get { return new string[] { ".sqf" }; } }
        public override IHighlightingDefinition SyntaxDefinition { get { return ThisSyntaxName; } }

        public BreakPointMargin BreakPointMargin { get; private set; }

        protected override void OnTextEditorSet()
        {
            var sf = Workspace.CurrentWorkspace.CurrentSolution.GetOrCreateFileReference(this.FilePath) as SolutionUtil.SolutionFile;
            this.BreakPointMargin = new BreakPointMargin(sf);
            this.Editor.TextArea.TextView.BackgroundRenderers.Add(this.BreakPointMargin);
            this.Editor.TextArea.LeftMargins.Insert(0, BreakPointMargin);
            this.Editor.TextArea.LeftMargins.Insert(1, new RuntimeExecutionMargin());
        }
        protected override IEnumerable<LinterInfo> GetLinterInformations(MemoryStream memstream)
        {
            var inputStream = new Antlr4.Runtime.AntlrInputStream(memstream);

            var lexer = new RealVirtuality.SQF.ANTLR.Parser.sqfLexer(inputStream);
            var commonTokenStream = new Antlr4.Runtime.CommonTokenStream(lexer);
            var p = new RealVirtuality.SQF.ANTLR.Parser.sqfParser(commonTokenStream, ConfigHost.Instance.SqfDefinitions);
            var listener = new RealVirtuality.SQF.ANTLR.SqfListener();
            p.AddParseListener(listener);
            memstream.Seek(0, SeekOrigin.Begin);
            p.RemoveErrorListeners();
#if DEBUG
            p.BuildParseTree = true;
            p.Context = new Antlr4.Runtime.ParserRuleContext();
#endif
            
            var se = new List<LinterInfo>();
            p.AddErrorListener(new RealVirtuality.SQF.ANTLR.ErrorListener((recognizer, token, line, charPositionInLine, msg, ex) =>
            {
                switch (ex == null ? null : p.RuleNames[ex.Context.RuleIndex])
                {

                    case "binaryexpression":
                        se.Add(new LinterInfo()
                        {
                            StartOffset = token.StartIndex,
                            Length = token.Text.Length,
                            Message = string.Concat("Invalid binary expression: ", msg),
                            Severity = ESeverity.Error,
                            Line = line,
                            LineOffset = charPositionInLine,
                            FilePath = this.FilePath
                        });
                        break;
                    case "unaryexpression":
                        se.Add(new LinterInfo()
                        {
                            StartOffset = token.StartIndex,
                            Length = token.Text.Length,
                            Message = string.Concat("Invalid unary expression: ", msg),
                            Severity = ESeverity.Error,
                            Line = line,
                            LineOffset = charPositionInLine,
                            FilePath = this.FilePath
                        });
                        break;
                    case "nularexpression":
                        se.Add(new LinterInfo()
                        {
                            StartOffset = token.StartIndex,
                            Length = token.Text.Length,
                            Message = string.Concat("Invalid nular expression: ", msg),
                            Severity = ESeverity.Error,
                            Line = line,
                            LineOffset = charPositionInLine,
                            FilePath = this.FilePath
                        });
                        break;
                    default:
                        se.Add(new LinterInfo()
                        {
                            StartOffset = token.StartIndex,
                            Length = token.Text.Length,
                            Message = msg,
                            Severity = ESeverity.Error,
                            Line = line,
                            LineOffset = charPositionInLine,
                            FilePath = this.FilePath
                        });
                        break;
                }
            }));
            p.sqf();
            return se;
        }
        protected override bool OnHoverText(int textOffset, Point p)
        {
            if (!Workspace.CurrentWorkspace.DebugContext.IsDebuggerAttached)
                return false;
            if (!Workspace.CurrentWorkspace.DebugContext.IsPaused)
                return false;
            var child = SqfVariableViewPopup.Child as FrameworkElement;
            if (child == null)
                return false;
            if (!SqfVariableViewPopup.IsOpen)
            {
                var varname = this.Editor.Document.GetWordAround(textOffset);
                if (varname.Length <= 1)
                    return false;
                if (!ConfigHost.Instance.SqfDefinitions.All((it) => it.Name != varname))
                    return false;
                //ToDo: Check if proper async maybe is required due to UI lag
                var result = Workspace.CurrentWorkspace.DebugContext.GetVariables(EVariableNamespace.All, varname).Result;
                if (result.Count() < 1)
                    return false;
                var container = SqfVariableViewPopup.DataContext as VariableContainer;
                container.InternalVariable = result.First();
                container.EditMode = false;
                SqfVariableViewPopup.PlacementTarget = this.Editor;
                SqfVariableViewPopup.HorizontalOffset = p.X + 10;
                SqfVariableViewPopup.VerticalOffset = p.Y;
                SqfVariableViewPopup.IsOpen = true;
                
                return true;
            }
            return false;
        }
        protected override void OnMouseMove(int textOffset, Point p)
        {
            if (!SqfVariableViewPopup.IsOpen)
                return;
            var child = SqfVariableViewPopup.Child as FrameworkElement;
            if (child == null)
                return;
            const int maxdist = 20;
            if (
                (SqfVariableViewPopup.HorizontalOffset - maxdist > p.X) ||
                (SqfVariableViewPopup.HorizontalOffset + child.ActualWidth + maxdist < p.X) ||
                (SqfVariableViewPopup.VerticalOffset - maxdist > p.Y) ||
                (SqfVariableViewPopup.VerticalOffset + child.ActualHeight + maxdist < p.Y)
                )
            {
                SqfVariableViewPopup.IsOpen = false;
            }
        }
    }
}
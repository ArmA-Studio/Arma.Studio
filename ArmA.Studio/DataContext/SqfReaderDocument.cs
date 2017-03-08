using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ArmA.Studio.DataContext.TextEditorUtil;
using ArmA.Studio.UI;
using ICSharpCode.AvalonEdit.Highlighting;
using RealVirtuality.SQF.Parser;

namespace ArmA.Studio.DataContext
{
    public class SQFReaderDocument : TextEditorDocument
    {
        private static IHighlightingDefinition ThisSyntaxName { get; set; }
        static SQFReaderDocument()
        {
            ThisSyntaxName = LoadAvalonEditSyntaxFiles(Path.Combine(App.SyntaxFilesPath, "sqf.xshd"));
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
                            FileName = Path.GetFileName(this.FilePath)
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
                            FileName = Path.GetFileName(this.FilePath)
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
                            FileName = Path.GetFileName(this.FilePath)
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
                            FileName = Path.GetFileName(this.FilePath)
                        });
                        break;
                }
            }));
            p.sqf();
            return se;
        }
    }
}
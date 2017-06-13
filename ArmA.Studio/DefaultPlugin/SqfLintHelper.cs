using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data;
using ArmA.Studio.Data.Lint;

namespace ArmA.Studio.DefaultPlugin
{
    internal sealed class SqfLintHelper : ILinterHost
    {
        public IEnumerable<LintInfo> LinterInfo { get; set; }

        public void LintWriteCache(Stream stream, ProjectFile file) => this.LinterInfo = Lint(stream, file);

        public IEnumerable<LintInfo> Lint(Stream stream, ProjectFile file)
        {
            var helper = new RealVirtuality.SQF.Parser.v2.PreProcessingStreamHelper(stream);
            var inputStream = new Antlr4.Runtime.AntlrInputStream(helper);
            var lexer = new RealVirtuality.SQF.Parser.v2.sqf2Lexer(inputStream);
            var tokenStream = new RealVirtuality.SQF.Parser.v2.PreProcessingTokenStream(lexer, helper);
            var p = new RealVirtuality.SQF.Parser.v2.sqf2Parser(tokenStream, ConfigHost.Instance.SqfDefinitions, new string[] { "private" });
            var listener = new RealVirtuality.SQF.Parser.v2.Sqf2Listener();
            p.AddParseListener(listener);
            p.RemoveErrorListeners();
#if DEBUG
            p.BuildParseTree = true;
            p.Context = new Antlr4.Runtime.ParserRuleContext();
#endif

            var se = new List<LintInfo>();
            p.AddErrorListener(new RealVirtuality.SQF.Parser.v1.ErrorListener((recognizer, token, line, charPositionInLine, msg, ex) =>
            {
                switch (ex == null ? null : p.RuleNames[ex.Context.RuleIndex])
                {

                    case "binaryexpression":
                        se.Add(new LintInfo(file)
                        {
                            StartOffset = token.StartIndex,
                            Length = token.Text.Length,
                            Message = string.Concat("Invalid binary expression: ", msg),
                            Severity = ESeverity.Error,
                            Line = line,
                            LineOffset = charPositionInLine
                        });
                        break;
                    case "unaryexpression":
                        se.Add(new LintInfo(file)
                        {
                            StartOffset = token.StartIndex,
                            Length = token.Text.Length,
                            Message = string.Concat("Invalid unary expression: ", msg),
                            Severity = ESeverity.Error,
                            Line = line,
                            LineOffset = charPositionInLine
                        });
                        break;
                    case "nularexpression":
                        se.Add(new LintInfo(file)
                        {
                            StartOffset = token.StartIndex,
                            Length = token.Text.Length,
                            Message = string.Concat("Invalid nular expression: ", msg),
                            Severity = ESeverity.Error,
                            Line = line,
                            LineOffset = charPositionInLine
                        });
                        break;
                    default:
                        se.Add(new LintInfo(file)
                        {
                            StartOffset = token.StartIndex,
                            Length = token.Text.Length,
                            Message = msg,
                            Severity = ESeverity.Error,
                            Line = line,
                            LineOffset = charPositionInLine
                        });
                        break;
                }
            }));
            try
            {
                p.sqf2();
            }
            catch(Exception ex)
            {
                App.ShowOperationFailedMessageBox(ex);
                while(ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    App.ShowOperationFailedMessageBox(ex);
                }
                System.Diagnostics.Debugger.Break();
            }
            return se;
        }
    }
}

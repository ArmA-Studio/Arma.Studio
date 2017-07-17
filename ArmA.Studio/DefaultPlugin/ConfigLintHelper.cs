using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data;
using ArmA.Studio.Data.Lint;
using RealVirtuality.Config.Parser;

namespace ArmA.Studio.DefaultPlugin
{
    internal sealed class ConfigLintHelper : ILinterHost
    {
        public IEnumerable<LintInfo> LinterInfo { get; set; }

        public void LintWriteCache(Stream stream, ProjectFile file) => this.LinterInfo = this.Lint(stream, file);

        public IEnumerable<LintInfo> Lint(Stream stream, ProjectFile file)
        {
            var inputStream = new Antlr4.Runtime.AntlrInputStream(stream);
            var lexer = new RealVirtuality.Config.Parser.ANTLR.armaconfigLexer(inputStream);
            var commonTokenStream = new Antlr4.Runtime.CommonTokenStream(lexer);
            var p = new RealVirtuality.Config.Parser.ANTLR.armaconfigParser(commonTokenStream);
            var listener = new RealVirtuality.Config.Parser.ANTLR.ArmaConfigListener();
            p.AddParseListener(listener);
            p.RemoveErrorListeners();
#if DEBUG
            p.BuildParseTree = true;
            p.Context = new Antlr4.Runtime.ParserRuleContext();
#endif

            var se = new List<LintInfo>();
            p.AddErrorListener(new RealVirtuality.Config.Parser.ANTLR.ErrorListener((recognizer, token, line, charPositionInLine, msg, ex) =>
            {
                se.Add(new LintInfo(file)
                {
                    StartOffset = token.StartIndex,
                    Length = token.Text.Length,
                    Message = msg,
                    Severity = ESeverity.Error,
                    Line = line,
                    LineOffset = charPositionInLine
                });
            }));
            try
            {
                p.armaconfig();
            }
            catch (Exception ex)
            {
                App.ShowOperationFailedMessageBox(ex);
                while (ex.InnerException != null)
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

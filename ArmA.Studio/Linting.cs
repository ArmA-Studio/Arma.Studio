using System;
using System.Collections.Generic;
using System.IO;
using ArmA.Studio.Data;
using ArmA.Studio.Data.Lint;

namespace ArmA.Studio
{
    public static class Linting
    {
        internal static IEnumerable<LintInfo> SQF_GetLintInfo(Stream content, string filePath)
        {
            var inputStream = new Antlr4.Runtime.AntlrInputStream(content);
            var lexer = new RealVirtuality.SQF.ANTLR.Parser.sqfLexer(inputStream);
            var commonTokenStream = new Antlr4.Runtime.CommonTokenStream(lexer);
            var p = new RealVirtuality.SQF.ANTLR.Parser.sqfParser(commonTokenStream, ConfigHost.Instance.SqfDefinitions);
            var listener = new RealVirtuality.SQF.ANTLR.SqfListener();
            p.AddParseListener(listener);
            p.RemoveErrorListeners();
#if DEBUG
            p.BuildParseTree = true;
            p.Context = new Antlr4.Runtime.ParserRuleContext();
#endif

            var se = new List<LintInfo>();
            p.AddErrorListener(new RealVirtuality.SQF.ANTLR.ErrorListener((recognizer, token, line, charPositionInLine, msg, ex) =>
            {
                switch (ex == null ? null : p.RuleNames[ex.Context.RuleIndex])
                {

                    //case "binaryexpression":
                    //    se.Add(new LintInfo()
                    //    {
                    //        StartOffset = token.StartIndex,
                    //        Length = token.Text.Length,
                    //        Message = string.Concat("Invalid binary expression: ", msg),
                    //        Severity = ESeverity.Error,
                    //        Line = line,
                    //        LineOffset = charPositionInLine,
                    //        FilePath = filePath
                    //    });
                    //    break;
                    //case "unaryexpression":
                    //    se.Add(new LintInfo()
                    //    {
                    //        StartOffset = token.StartIndex,
                    //        Length = token.Text.Length,
                    //        Message = string.Concat("Invalid unary expression: ", msg),
                    //        Severity = ESeverity.Error,
                    //        Line = line,
                    //        LineOffset = charPositionInLine,
                    //        FilePath = filePath
                    //    });
                    //    break;
                    //case "nularexpression":
                    //    se.Add(new LintInfo()
                    //    {
                    //        StartOffset = token.StartIndex,
                    //        Length = token.Text.Length,
                    //        Message = string.Concat("Invalid nular expression: ", msg),
                    //        Severity = ESeverity.Error,
                    //        Line = line,
                    //        LineOffset = charPositionInLine,
                    //        FilePath = filePath
                    //    });
                    //    break;
                    //default:
                    //    se.Add(new LintInfo()
                    //    {
                    //        StartOffset = token.StartIndex,
                    //        Length = token.Text.Length,
                    //        Message = msg,
                    //        Severity = ESeverity.Error,
                    //        Line = line,
                    //        LineOffset = charPositionInLine,
                    //        FilePath = filePath
                    //    });
                    //    break;
                }
            }));
            p.sqf();
            return se;
        }
    }
}
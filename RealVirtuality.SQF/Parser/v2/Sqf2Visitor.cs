using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RealVirtuality.SQF.Parser.v2;

namespace RealVirtuality.SQF.Parser.v2
{
    public class Sqf2Visitor : Isqf2ParserVisitor<string>
    {
        public string Visit(IParseTree tree)
        {
            throw new NotImplementedException();
        }

        public string VisitArray([NotNull] sqf2Parser.ArrayContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitAssignment([NotNull] sqf2Parser.AssignmentContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitBinaryexpression([NotNull] sqf2Parser.BinaryexpressionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitChildren(IRuleNode node)
        {
            throw new NotImplementedException();
        }

        public string VisitCode([NotNull] sqf2Parser.CodeContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitDefine([NotNull] sqf2Parser.DefineContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitElse([NotNull] sqf2Parser.ElseContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitEndif([NotNull] sqf2Parser.EndifContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitErrorNode(IErrorNode node)
        {
            throw new NotImplementedException();
        }

        public string VisitGlobalvariable([NotNull] sqf2Parser.GlobalvariableContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitIf([NotNull] sqf2Parser.IfContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitInclude([NotNull] sqf2Parser.IncludeContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitMacro([NotNull] sqf2Parser.MacroContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitNularexpression([NotNull] sqf2Parser.NularexpressionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitOperator([NotNull] sqf2Parser.OperatorContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitPrimaryexpression([NotNull] sqf2Parser.PrimaryexpressionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitPrivatevariable([NotNull] sqf2Parser.PrivatevariableContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitRoundbrackets([NotNull] sqf2Parser.RoundbracketsContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitSqf2([NotNull] sqf2Parser.Sqf2Context context)
        {
            throw new NotImplementedException();
        }

        public string VisitStatement([NotNull] sqf2Parser.StatementContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitTerminal(ITerminalNode node)
        {
            throw new NotImplementedException();
        }

        public string VisitUnaryexpression([NotNull] sqf2Parser.UnaryexpressionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitVariable([NotNull] sqf2Parser.VariableContext context)
        {
            throw new NotImplementedException();
        }
    }
}

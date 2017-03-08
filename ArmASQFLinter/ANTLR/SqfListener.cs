using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RealVirtuality.SQF.ANTLR.Parser;


namespace RealVirtuality.SQF.ANTLR
{
    public class SqfListener : IsqfListener
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public struct SyntaxError
        {
            public int Line { get; set; }
            public int Col { get; set; }

            public int Length { get; set; }
            public int StartOffset { get; set; }
            public string Message { get; set; }
        }
#if DEBUG
        private readonly SqfNode Original;
#endif
        public SqfNode Current { get; set; }
        public readonly List<SyntaxError> OtherSyntaxErrors;

        public SqfListener()
        {
            this.OtherSyntaxErrors = new List<SyntaxError>();
            this.Current = new SqfCode(null);
#if DEBUG
            this.Original = this.Current;
#endif
        }
        private void EnterGeneric(SqfNode node, [System.Runtime.CompilerServices.CallerMemberName]string caller = "")
        {
            Logger.Info(string.Format("ENTER:{0}{1}", new string('\t', node.ParentCount()), caller.Remove(0, "Enter".Length)));
            Current.AddChild(node);
            Current = node;
        }
        private T ExitGeneric<T>(ParserRuleContext ctx, [System.Runtime.CompilerServices.CallerMemberName]string caller = "") where T : SqfNode
        {
            var node = Current;
            Current = node.GetParent();

            Logger.Info(string.Format("EXIT :{0:#####}{1}{2}: {3}", ctx.start.Line, new string('\t', node.ParentCount()), caller.Remove(0, "Exit".Length), ctx.GetText()));
            node.StartOffset = ctx.Start.StartIndex;
            node.Line = ctx.Start.Line;
            node.Length = ctx.Stop == null ? ctx.GetText().Length : ctx.Stop.StopIndex - node.StartOffset;
            node.Col = ctx.start.Column;

            return node as T;
        }

        #region Enter
        public void EnterEveryRule(ParserRuleContext ctx) { }
        public void EnterSqf([NotNull] sqfParser.SqfContext context) { }

        public void EnterAssignment([NotNull] sqfParser.AssignmentContext context)
        {
            this.EnterGeneric(new SqfAssignment(this.Current));
        }

        public void EnterBinaryexpression([NotNull] sqfParser.BinaryexpressionContext context)
        {
            this.EnterGeneric(new SqfBinaryExpression(this.Current));
        }

        public void EnterCode([NotNull] sqfParser.CodeContext context)
        {
            this.EnterGeneric(new SqfCode(this.Current));
        }

        public void EnterNularexpression([NotNull] sqfParser.NularexpressionContext context)
        {
            this.EnterGeneric(new SqfNularExpression(this.Current));
        }

        public void EnterPrimaryexpression([NotNull] sqfParser.PrimaryexpressionContext context)
        {
            this.EnterGeneric(new SqfPrimaryExpression(this.Current));
        }

        public void EnterStatement([NotNull] sqfParser.StatementContext context)
        {
            this.EnterGeneric(new SqfRemovable(this.Current));
        }

        public void EnterUnaryexpression([NotNull] sqfParser.UnaryexpressionContext context)
        {
            this.EnterGeneric(new SqfUnaryExpression(this.Current));
        }

        public void EnterVariable([NotNull] sqfParser.VariableContext context)
        {
            this.EnterGeneric(new SqfVariable(this.Current));
        }

        public void EnterRoundbrackets([NotNull] sqfParser.RoundbracketsContext context)
        {
            this.EnterGeneric(new SqfRemovable(this.Current));
        }
        public void EnterArray([NotNull] sqfParser.ArrayContext context)
        {
            this.EnterGeneric(new SqfArray(this.Current));
        }

        #endregion
        #region Exit
        public void ExitEveryRule(ParserRuleContext ctx) { }
        public void ExitSqf([NotNull] sqfParser.SqfContext context) { }

        public void ExitAssignment([NotNull] sqfParser.AssignmentContext context)
        {
            var node = this.ExitGeneric<SqfAssignment>(context);
            node.HasPrivateKeyword = context.IDENTIFIER().Length > 1;
            if (node.HasPrivateKeyword)
            {
                if (!context.IDENTIFIER(0).GetText().Equals("private", StringComparison.InvariantCultureIgnoreCase))
                {
                    this.OtherSyntaxErrors.Add(new SyntaxError()
                    {
                        StartOffset = context.Start.StartIndex,
                        Col = context.Start.Column,
                        Length = context.GetText().Length,
                        Line = context.Start.Line,
                        Message = "Expected { 'private' }"
                    });
                    context.AddErrorNode(context.IDENTIFIER(0).Symbol);
                }
                node.VariableName = context.IDENTIFIER(1).GetText();
            }
            else
            {
                node.VariableName = context.IDENTIFIER(0).GetText();
            }
            

            if (node.Children.Count == 0)
            {
                this.OtherSyntaxErrors.Add(new SyntaxError()
                {
                    StartOffset = context.Start.StartIndex,
                    Col = context.Start.Column,
                    Length = context.GetText().Length,
                    Line = context.Start.Line,
                    Message = "Unknown Error"
                });
                context.AddErrorNode(context.Start);
            }
            else
            {
                node.AssignedExpression = node.Children[0];
            }
        }

        public void ExitBinaryexpression([NotNull] sqfParser.BinaryexpressionContext context)
        {
            var node = this.ExitGeneric<SqfBinaryExpression>(context);
            var idents = context.IDENTIFIER();
            var ops = context.OPERATOR();
            var terminalList = new List<ITerminalNode>(idents.Length + ops.Length);
            var identIndex = 0;
            var opIndex = 0;
            while(true)
            {
                var ident = idents.Length >= identIndex ? null : idents[identIndex];
                var op = ops.Length >= opIndex ? null : ops[opIndex];
                if(op == null && ident == null)
                {
                    break;
                }
                if (op == null || ident.Symbol.StartIndex < op.Symbol.StartIndex)
                {
                    terminalList.Add(ident);
                }
                else if(ident == null || ident.Symbol.StartIndex > op.Symbol.StartIndex)
                {
                    terminalList.Add(op);
                }
                else
                {
                    break;
                }
            }
            if (terminalList.Count == 0)
            {
                node.RemoveFromTree();
            }
            else
            {
                int index = 0;
                SqfBinaryExpression lastExp = node;
                while (node.Children.Count > 1)
                {
                    SqfBinaryExpression exp = new SqfBinaryExpression(node);
                    exp.LValue = node.Children[0];
                    exp.RValue = node.Children[1];
                    exp.AddChild(exp.LValue);
                    exp.AddChild(exp.RValue);
                    exp.Length = exp.RValue.StartOffset - lastExp.StartOffset + exp.RValue.Length;
                    exp.Operation = terminalList[index++].GetText();
                    exp.Line = exp.LValue.Line;
                    node.Children.Remove(exp.LValue);
                    node.Children.Remove(exp.RValue);
                    node.Children.Insert(0, exp);
                    lastExp = exp;
                }
                node.RemoveFromTree();
            }
        }

        public void ExitCode([NotNull] sqfParser.CodeContext context)
        {
            var node = this.ExitGeneric<SqfCode>(context);
        }

        public void ExitNularexpression([NotNull] sqfParser.NularexpressionContext context)
        {
            var node = this.ExitGeneric<SqfNularExpression>(context);
            node.Identifier = context.GetText();
        }

        public void ExitPrimaryexpression([NotNull] sqfParser.PrimaryexpressionContext context)
        {
            var node = this.ExitGeneric<SqfPrimaryExpression>(context);

            if (context.NUMBER() != null)
            {
                var value = new SqfNumber(node.GetParent());
                var valContext = context.NUMBER();
                value.Value = valContext.GetText();
                value.StartOffset = node.StartOffset;
                value.Length = node.Length;
                value.Col = node.Col;
                node.GetParent().Children[node.GetParent().Children.IndexOf(node)] = value;
            }
            else if (context.STRING() != null)
            {
                var value = new SqfString(node.GetParent());
                var valContext = context.STRING();
                value.Value = valContext.GetText();
                value.StartOffset = node.StartOffset;
                value.Length = node.Length;
                value.Col = node.Col;
                node.GetParent().Children[node.GetParent().Children.IndexOf(node)] = value;
            }
            else
            {
                if (context.GetToken(sqfLexer.CURLYOPEN, 0) != null)
                {
                    if (node.Children.Count > 0)
                    {
                        node.RemoveFromTree();
                    }
                    else
                    {
                        var value = new SqfCode(node.GetParent());
                        var numberContext = context.STRING();
                        value.StartOffset = node.StartOffset;
                        value.Length = node.Length;
                        value.Col = node.Col;
                        node.GetParent().Children[node.GetParent().Children.IndexOf(node)] = value;
                    }
                }
                else
                {
                    node.RemoveFromTree();
                }
            }
        }

        public void ExitStatement([NotNull] sqfParser.StatementContext context)
        {
            var node = this.ExitGeneric<SqfRemovable>(context);
            node.RemoveFromTree();
        }

        public void ExitUnaryexpression([NotNull] sqfParser.UnaryexpressionContext context)
        {
            var node = this.ExitGeneric<SqfUnaryExpression>(context);
            node.Operator = (context.IDENTIFIER()??context.NEGATION()).GetText();
            try
            {
                node.Expression = node.Children[0];
            }
            catch
            {
                this.OtherSyntaxErrors.Add(new SyntaxError() { Col = context.Start.Column, Length = context.GetText().Length, Line = context.Start.Line, Message = "Unknown Error", StartOffset = context.Start.StartIndex });
            }
        }

        public void ExitVariable([NotNull] sqfParser.VariableContext context)
        {
            var node = this.ExitGeneric<SqfVariable>(context);
            node.Identifier = context.GetText();
        }
        public void ExitRoundbrackets([NotNull] sqfParser.RoundbracketsContext context)
        {
            var node = this.ExitGeneric<SqfRemovable>(context);
            node.RemoveFromTree();
        }


        public void ExitArray([NotNull] sqfParser.ArrayContext context)
        {
            var node = this.ExitGeneric<SqfArray>(context);
        }
        #endregion

        public void VisitErrorNode(IErrorNode node)
        {
            
        }

        public void VisitTerminal(ITerminalNode node)
        {
        }
        
    }
}

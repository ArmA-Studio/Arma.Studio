using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace RealVirtuality.SQF.ANTLR
{
    public class ErrorListener : IAntlrErrorListener<IToken>
    {
        public ErrorListener(Action<IRecognizer, IToken, int, int, string, RecognitionException> fnc)
        {
            this.Func = fnc;
        }

        public Action<IRecognizer, IToken, int, int, string, RecognitionException> Func { get; private set; }

        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            this.Func?.Invoke(recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        }
    }
}

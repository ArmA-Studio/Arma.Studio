using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;

namespace Utility
{
    public static class TextDocumentExtensionMethods
    {
        public static string GetWordAround(this TextDocument doc, int offset)
        {
            if (offset < 0 || offset >= doc.TextLength)
                return string.Empty;
            int start, end;

            //find start
            for (start = offset; start >= 0; start--)
            {
                var c = doc.GetCharAt(start);
                if (!char.IsLetterOrDigit(c) && c != '_')
                {
                    start++;
                    break;
                }
            }
            //find end
            for (end = start; end < doc.TextLength; end++)
            {
                var c = doc.GetCharAt(end);
                if (!char.IsLetterOrDigit(c) && c != '_')
                {
                    break;
                }
            }
            return doc.GetText(start, end - start);
        }

    }
}

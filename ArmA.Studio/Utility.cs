using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio
{
    public static class Utility
    {
        public static string GetWordAround(this ICSharpCode.AvalonEdit.Document.TextDocument doc, int offset)
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
        public static IEnumerable<string> AllIdents(this ICSharpCode.AvalonEdit.Document.TextDocument doc, int minLength = 2)
        {
            var builder = new StringBuilder();
            bool isString = false;
            char stringchar = '\0';
            for(int i = 0; i < doc.TextLength; i++)
            {
                var c = doc.GetCharAt(i);
                if(isString)
                {
                    if (c == stringchar)
                        isString = false;
                }
                else if ((builder.Length == 0 ? char.IsLetter(c) : char.IsLetterOrDigit(c)) || c == '_')
                {
                    builder.Append(c);
                }
                else if(c == '"' || c == '\'')
                {
                    stringchar = c;
                    isString = true;
                }
                else
                {
                    if (builder.Length >= minLength)
                    {
                        yield return builder.ToString();
                    }
                    builder.Clear();
                }
            }
        }

        public static bool Contains(this string s, string cont, bool ignoreCasing)
        {
            if (cont.Length == 0)
                return true;
            int contIndex = 0;
            if (ignoreCasing)
            {
                foreach (var c in s)
                {
                    if (char.ToUpper(c) == char.ToUpper(cont[contIndex]))
                    {
                        contIndex++;
                        if (contIndex >= cont.Length)
                            return true;
                    }
                    else
                    {
                        contIndex = 0;
                    }
                }
            }
            else
            {
                foreach (var c in s)
                {
                    if (c == cont[contIndex])
                    {
                        contIndex++;
                        if (contIndex >= cont.Length)
                            return true;
                    }
                    else
                    {
                        contIndex = 0;
                    }
                }
            }
            return false;
        }
    }
}

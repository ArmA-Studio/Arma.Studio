using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace RealVirtuality
{
    public static class Utility
    {
        /// <summary>
        /// Parses provided string in SQF string format to normal string.
        /// </summary>
        /// <param name="s">SQF-Formatted string</param>
        /// <returns>Normal formatted string</returns>
        public static string FromSqfString(this string s)
        {
            s = s.Substring(1, s.Length - 2);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c == '"')
                {
                    if (s[i + 1] == '"')
                        i++;
                    builder.Append(c);
                }
                else
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }
        /// <summary>
        /// Parses provided string into SQF-Formatted string.
        /// </summary>
        /// <param name="s">Normal formatted string<param>
        /// <returns>SQF-Formatted string</returns>
        public static string ToSqfString(this string s, bool outerStringSpecifier = true)
        {
            StringBuilder builder = new StringBuilder(s.Length + 2);
            if (outerStringSpecifier)
                builder.Append('"');
            foreach (var c in s)
            {
                switch (c)
                {
                    case '"':
                        builder.Append("\"\"");
                        break;
                    default:
                        builder.Append(c);
                        break;
                    case '\r':
                    case '\n':
                        //Ignore theese chars
                        break;
                }
            }
            if (outerStringSpecifier)
                builder.Append('"');
            return builder.ToString();
        }
        public static TextPointer GetPointerFromCharOffset(this TextPointer tPtr, int charOffset)
        {
            //https://social.msdn.microsoft.com/Forums/vstudio/en-US/bc67d8c5-41f0-48bd-8d3d-79159e86b355/textpointer-into-a-flowdocument-based-on-character-index?forum=wpf
            if (charOffset == 0)
            {
                return tPtr;
            }

            var navigator = tPtr;
            var counter = 0;
            var runMoveMode = true;
            while (navigator != null && counter < charOffset)
            {
                var ptrContext = navigator.GetPointerContext(LogicalDirection.Forward);
                if (runMoveMode)
                {
                    switch (ptrContext)
                    {
                        case TextPointerContext.ElementEnd:
                            if (navigator.GetAdjacentElement(LogicalDirection.Forward) is Paragraph)
                                counter += 2;
                            navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
                            break;
                        case TextPointerContext.ElementStart:
                        case TextPointerContext.EmbeddedElement:
                        case TextPointerContext.None:
                            navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
                            break;
                        case TextPointerContext.Text:
                            var len = navigator.GetTextRunLength(LogicalDirection.Forward);
                            if(counter + len >= charOffset)
                            {
                                runMoveMode = false;
                            }
                            else
                            {
                                counter += len;
                                navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
                            }
                            break;
                    }
                }
                else
                {
                    if (ptrContext == TextPointerContext.Text)
                    {
                        counter++;
                    }
                    navigator = navigator.GetNextInsertionPosition(LogicalDirection.Forward);
                }
            }

            return navigator;
        }


    }
}

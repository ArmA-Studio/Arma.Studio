using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data.UI;
using ICSharpCode.AvalonEdit.Document;

namespace ArmA.Studio
{
    public static class Utility
    {


        public static IEnumerable<string> GetAllSqfIdents(this TextDocument doc, int minLength = 2)
        {
            var builder = new StringBuilder();
            bool isString = false;
            bool isInComment = false;
            bool isInMultiLineComment = false;
            char stringchar = '\0';
            for (int i = 0; i < doc.TextLength; i++)
            {
                var c = doc.GetCharAt(i);
                if (isInMultiLineComment)
                {
                    if (c == '*' && doc.GetCharAt(i + 1) == '/')
                        isInMultiLineComment = false;
                }
                else if (isInComment)
                {
                    if (c == '\n')
                        isInComment = false;
                }
                else if (isString)
                {
                    if (c == stringchar)
                        isString = false;
                }
                else if ((builder.Length == 0 ? char.IsLetter(c) : char.IsLetterOrDigit(c)) || c == '_')
                {
                    builder.Append(c);
                }
                else if (c == '"' || c == '\'')
                {
                    stringchar = c;
                    isString = true;
                }
                else if (c == '/' && doc.GetCharAt(i + 1) == '*')
                {
                    isInMultiLineComment = true;
                }
                else if (c == '/' && doc.GetCharAt(i + 1) == '/')
                {
                    isInComment = true;
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


        public static void Close(this DocumentBase doc)
        {
            if (Workspace.Instance.AvalonDockDocuments.Contains(doc))
            {
                Workspace.Instance.AvalonDockDocuments.Remove(doc);
            }
        }
        public static void Close(this PanelBase panel)
        {
            if (Workspace.Instance.AvalonDockPanels.Contains(panel))
            {
                Workspace.Instance.AvalonDockPanels.Remove(panel);
            }
        }
    }
}

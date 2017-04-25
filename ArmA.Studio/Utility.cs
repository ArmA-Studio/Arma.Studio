using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data;
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
        public static string GetTemplate(this FileType ftype)
        {
            //ToDo: make templates customizable
            return ftype.FileTemplate;
        }

        /// <summary>
        /// Loads given type <typeparamref name="T"/> from provided <paramref name="path"/>.
        /// Will throw <see cref="System.IO.FileNotFoundException"/> in case of the <paramref name="path"/> being not existing.
        /// Will throw <see cref="InvalidCastException"/> in case of the item behind <paramref name="path"/> is not of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="class"/> type which will be loaded from <paramref name="assembly"/>.</typeparam>
        /// <param name="assembly">The <see cref="System.Reflection.Assembly"/> where to look the <paramref name="path"/> up.</param>
        /// <param name="path">Path to the xaml file. 'Example: ArmA.Studio.Data.Configuration.StringItem.xaml' </param>
        /// <returns></returns>
        public static T LoadFromEmbeddedResource<T>(System.Reflection.Assembly assembly, string path) where T : class
        {
            var resNames = from name in assembly.GetManifestResourceNames() where name.EndsWith(".xaml") where name.Equals(path) select name;
            foreach (var res in resNames)
            {
                var resSplit = res.Split('.');
                var header = resSplit[resSplit.Count() - 2];
                using (var stream = assembly.GetManifestResourceStream(res))
                {
                    try
                    {
                        var obj = System.Windows.Markup.XamlReader.Load(stream);
                        if (!(obj is T))
                        {
                            throw new InvalidCastException();
                        }
                        return obj as T;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            throw new System.IO.FileNotFoundException();
        }

        public static string GetContentAsString(this ProjectFile pf)
        {
            var doc = Workspace.Instance.AvalonDockDocuments.FirstOrDefault((d) => d.FileReference == pf);
            if(doc != null && doc is TextEditorBaseDataContext)
            {
                return (doc as TextEditorBaseDataContext).Document.Text;
            }
            using (var stream = new System.IO.StreamReader(System.IO.File.OpenRead(pf.FilePath)))
            {
                return stream.ReadToEnd();
            }
        }
        public static void SetContentAsString(this ProjectFile pf, string replaceText)
        {
            var doc = Workspace.Instance.AvalonDockDocuments.FirstOrDefault((d) => d.FileReference == pf);
            if (doc != null && doc is TextEditorBaseDataContext)
            {
                (doc as TextEditorBaseDataContext).Document.Text = replaceText;
            }
            using (var stream = new System.IO.StreamWriter(System.IO.File.Open(pf.FilePath, System.IO.FileMode.Create)))
            {
                stream.Write(replaceText);
            }
        }
    }
}

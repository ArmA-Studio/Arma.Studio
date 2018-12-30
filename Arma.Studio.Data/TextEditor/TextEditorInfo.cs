using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.TextEditor
{
    public class TextEditorInfo
    {
        public string Name { get; }
        public string IconSource { get; }
        public string FileExtension { get; }
        public Func<ITextEditor> CreateFunc { get; }
        public Type Type { get; }

        private TextEditorInfo(string name, string iconSource, Func<ITextEditor> func, Type type, string fileExtension)
        {
            this.Name = name;
            this.IconSource = iconSource;
            this.CreateFunc = func;
            this.FileExtension = fileExtension.StartsWith(".") ? fileExtension : String.Concat(".", fileExtension);
            this.Type = type;
        }

        public static TextEditorInfo Create<T>(string name, string iconSource, Func<T> func, string fileExtension) where T : ITextEditor =>
            new TextEditorInfo(name, iconSource, () => func(), typeof(T), fileExtension);
        public static IEnumerable<TextEditorInfo> Create<T>(string name, string iconSource, Func<T> func, params string[] fileExtensions) where T : ITextEditor =>
            fileExtensions.Select((fileExtension) => new TextEditorInfo(name, iconSource, () => func(), typeof(T), fileExtension));
    }
}

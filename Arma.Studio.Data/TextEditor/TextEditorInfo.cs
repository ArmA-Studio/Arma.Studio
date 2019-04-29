using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.Data.TextEditor
{
    public class TextEditorInfo
    {
        /// <summary>
        /// The localized name of the dockable to describe.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The method to create the actual <see cref="ITextEditor"/>.
        /// </summary>
        public Func<ITextEditor> CreateFunc { get; }

        /// <summary>
        /// The method to create the actual <see cref="ITextEditor"/>.
        /// </summary>
        public Func<Task<ITextEditor>> CreateAsyncFunc { get; }

        /// <summary>
        /// Bool determining wether the <see cref="CreateFunc"/> (false)
        /// or the <see cref="CreateAsyncFunc"/> (true) method has to be used.
        /// </summary>
        public bool IsAsync { get; }

        /// <summary>
        /// The actual type of the <see cref="ITextEditor"/>.
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// The file extensions supported by this <see cref="ITextEditor"/> per default.
        /// </summary>
        public string[] Extensions { get; }

        /// <summary>
        /// Creates a new <see cref="TextEditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="func">The method to create the actual <see cref="ITextEditor"/>.</param>
        /// <param name="type">The actual type of the <see cref="ITextEditor"/>.</param>
        internal TextEditorInfo(string name, Func<ITextEditor> func, Type type, params string[] extensions)
        {
            this.Name = name;
            this.CreateFunc = func;
            this.IsAsync = false;
            this.Type = type;
            this.Extensions = extensions;
        }

        /// <summary>
        /// Creates a new <see cref="TextEditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="func">The method to create the actual <see cref="ITextEditor"/>.</param>
        /// <param name="type">The actual type of the <see cref="ITextEditor"/>.</param>
        /// <param name="extensions">The extensions supported by default.</param>
        internal TextEditorInfo(string name, Func<Task<ITextEditor>> func, Type type, params string[] extensions)
        {
            this.Name = name;
            this.CreateAsyncFunc = func;
            this.IsAsync = true;
            this.Type = type;
            this.Extensions = extensions;
        }

        /// <summary>
        /// Creates a new <see cref="TextEditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="creationMode">The types this <see cref="TextEditorInfo"/> can create.</param>
        /// <param name="func">The method to create the actual <see cref="ITextEditor"/>.</param>
        /// <param name="extensions">The extensions supported by default.</param>
        /// <returns>The created <see cref="TextEditorInfo"/> instance.</returns>
        public static TextEditorInfo Create<T>(string name, Func<T> func, params string[] extensions) where T : ITextEditor =>
            new TextEditorInfo(name, () => func() as ITextEditor, typeof(T), extensions);

        /// <summary>
        /// Creates a new <see cref="TextEditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="func">The method to create the actual <see cref="ITextEditor"/>.</param>
        /// <param name="extensions">The extensions supported by default.</param>
        /// <returns>The created <see cref="TextEditorInfo"/> instance.</returns>
        public static TextEditorInfo Create<T>(string name, Func<Task<T>> func, params string[] extensions) where T : ITextEditor =>
            new TextEditorInfo(name, async () => await func(), typeof(T), extensions);


        /// <summary>
        /// Creates a new <see cref="TextEditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">An instance of some <see cref="DrawingBrush"/>.</param>
        /// <param name="func">The method to create the actual <see cref="ITextEditor"/>.</param>
        /// <returns>The created <see cref="TextEditorInfo"/> instance.</returns>
        public static TextEditorInfoDrawingBrush Create<T>(string name, DrawingBrush iconSource, Func<T> func) where T : ITextEditor =>
            new TextEditorInfoDrawingBrush(name, iconSource, () => func() as ITextEditor, typeof(T));

        /// <summary>
        /// Creates a new <see cref="TextEditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">An instance of some <see cref="DrawingBrush"/>.</param>
        /// <param name="func">The method to create the actual <see cref="ITextEditor"/>.</param>
        /// <returns>The created <see cref="TextEditorInfo"/> instance.</returns>
        public static TextEditorInfoDrawingBrush Create<T>(string name, DrawingBrush iconSource, Func<Task<T>> func) where T : ITextEditor =>
            new TextEditorInfoDrawingBrush(name, iconSource, async () => await func(), typeof(T));

        /// <summary>
        /// Creates a new <see cref="TextEditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">The path to an icon for the dockable to describe.
        /// Please try to use proper resource URI, eg.:
        /// pack://application:,,,/assemblyname;component/path/to/file.png</param>
        /// <param name="func">The method to create the actual <see cref="ITextEditor"/>.</param>
        /// <returns>The created <see cref="TextEditorInfo"/> instance.</returns>
        public static TextEditorInfoIcon Create<T>(string name, string iconSource, Func<T> func) where T : ITextEditor =>
            new TextEditorInfoIcon(name, iconSource, () => func() as ITextEditor, typeof(T));

        /// <summary>
        /// Creates a new <see cref="TextEditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">The path to an icon for the dockable to describe.
        /// Please try to use proper resource URI, eg.:
        /// pack://application:,,,/assemblyname;component/path/to/file.png</param>
        /// <param name="func">The method to create the actual <see cref="ITextEditor"/>.</param>
        /// <returns>The created <see cref="TextEditorInfo"/> instance.</returns>
        public static TextEditorInfoIcon Create<T>(string name, string iconSource, Func<Task<T>> func) where T : ITextEditor =>
            new TextEditorInfoIcon(name, iconSource, async () => await func(), typeof(T));
    }
}

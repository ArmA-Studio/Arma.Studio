using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.Data.UI
{
    public class EditorInfo
    {
        /// <summary>
        /// The localized name of the dockable to describe.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The method to create the actual <see cref="IEditorDocument"/>.
        /// </summary>
        public Func<IO.File, IEditorDocument> CreateFunc { get; }

        /// <summary>
        /// The method to create the actual <see cref="IEditorDocument"/>.
        /// </summary>
        public Func<IO.File, Task<IEditorDocument>> CreateAsyncFunc { get; }

        /// <summary>
        /// Bool determining wether the <see cref="CreateFunc"/> (false)
        /// or the <see cref="CreateAsyncFunc"/> (true) method has to be used.
        /// </summary>
        public bool IsAsync { get; }

        /// <summary>
        /// The actual type of the <see cref="IEditorDocument"/>.
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// The file extensions supported by this <see cref="IEditorDocument"/> per default.
        /// </summary>
        public string[] Extensions { get; }

        /// <summary>
        /// Creates a new <see cref="EditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="func">The method to create the actual <see cref="IEditorDocument"/>.</param>
        /// <param name="type">The actual type of the <see cref="IEditorDocument"/>.</param>
        internal EditorInfo(string name, Func<IO.File, IEditorDocument> func, Type type, params string[] extensions)
        {
            this.Name = name;
            this.CreateFunc = func;
            this.IsAsync = false;
            this.Type = type;
            this.Extensions = extensions;
        }

        /// <summary>
        /// Creates a new <see cref="EditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="func">The method to create the actual <see cref="IEditorDocument"/>.</param>
        /// <param name="type">The actual type of the <see cref="IEditorDocument"/>.</param>
        /// <param name="extensions">The extensions supported by default.</param>
        internal EditorInfo(string name, Func<IO.File, Task<IEditorDocument>> func, Type type, params string[] extensions)
        {
            this.Name = name;
            this.CreateAsyncFunc = func;
            this.IsAsync = true;
            this.Type = type;
            this.Extensions = extensions;
        }

        /// <summary>
        /// Creates a new <see cref="EditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="creationMode">The types this <see cref="EditorInfo"/> can create.</param>
        /// <param name="func">The method to create the actual <see cref="IEditorDocument"/>.</param>
        /// <param name="extensions">The extensions supported by default.</param>
        /// <returns>The created <see cref="EditorInfo"/> instance.</returns>
        public static EditorInfo Create<T>(string name, Func<IO.File, T> func, params string[] extensions) where T : IEditorDocument =>
            new EditorInfo(name, (file) => func(file) as IEditorDocument, typeof(T), extensions);

        /// <summary>
        /// Creates a new <see cref="EditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="func">The method to create the actual <see cref="IEditorDocument"/>.</param>
        /// <param name="extensions">The extensions supported by default.</param>
        /// <returns>The created <see cref="EditorInfo"/> instance.</returns>
        public static EditorInfo Create<T>(string name, Func<IO.File, Task<T>> func, params string[] extensions) where T : IEditorDocument =>
            new EditorInfo(name, async (file) => await func(file), typeof(T), extensions);


        /// <summary>
        /// Creates a new <see cref="EditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">An instance of some <see cref="DrawingBrush"/>.</param>
        /// <param name="func">The method to create the actual <see cref="IEditorDocument"/>.</param>
        /// <returns>The created <see cref="EditorInfo"/> instance.</returns>
        public static EditorInfoDrawingBrush Create<T>(string name, DrawingBrush iconSource, Func<IO.File, T> func, params string[] extensions) where T : IEditorDocument =>
            new EditorInfoDrawingBrush(name, iconSource, (file) => func(file) as IEditorDocument, typeof(T), extensions);

        /// <summary>
        /// Creates a new <see cref="EditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">An instance of some <see cref="DrawingBrush"/>.</param>
        /// <param name="func">The method to create the actual <see cref="IEditorDocument"/>.</param>
        /// <returns>The created <see cref="EditorInfo"/> instance.</returns>
        public static EditorInfoDrawingBrush Create<T>(string name, DrawingBrush iconSource, Func<IO.File, Task<T>> func, params string[] extensions) where T : IEditorDocument =>
            new EditorInfoDrawingBrush(name, iconSource, async (file) => await func(file), typeof(T), extensions);

        /// <summary>
        /// Creates a new <see cref="EditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">The path to an icon for the dockable to describe.
        /// Please try to use proper resource URI, eg.:
        /// pack://application:,,,/assemblyname;component/path/to/file.png</param>
        /// <param name="func">The method to create the actual <see cref="IEditorDocument"/>.</param>
        /// <returns>The created <see cref="EditorInfo"/> instance.</returns>
        public static EditorInfoIcon Create<T>(string name, string iconSource, Func<IO.File, T> func, params string[] extensions) where T : IEditorDocument =>
            new EditorInfoIcon(name, iconSource, (file) => func(file) as IEditorDocument, typeof(T), extensions);

        /// <summary>
        /// Creates a new <see cref="EditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">The path to an icon for the dockable to describe.
        /// Please try to use proper resource URI, eg.:
        /// pack://application:,,,/assemblyname;component/path/to/file.png</param>
        /// <param name="func">The method to create the actual <see cref="IEditorDocument"/>.</param>
        /// <returns>The created <see cref="EditorInfo"/> instance.</returns>
        public static EditorInfoIcon Create<T>(string name, string iconSource, Func<IO.File, Task<T>> func, params string[] extensions) where T : IEditorDocument =>
            new EditorInfoIcon(name, iconSource, async (file) => await func(file), typeof(T), extensions);
    }
}

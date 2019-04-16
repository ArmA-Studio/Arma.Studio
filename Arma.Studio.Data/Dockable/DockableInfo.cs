using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.Data.Dockable
{
    public class DockableInfo
    {
        /// <summary>
        /// The localized name of the dockable to describe.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The method to create the actual <see cref="DockableBase"/>.
        /// </summary>
        public Func<DockableBase> CreateFunc { get; }

        /// <summary>
        /// The method to create the actual <see cref="DockableBase"/>.
        /// </summary>
        public Func<Task<DockableBase>> CreateAsyncFunc { get; }

        /// <summary>
        /// Bool determining wether the <see cref="CreateFunc"/> (false)
        /// or the <see cref="CreateAsyncFunc"/> (true) method has to be used.
        /// </summary>
        public bool IsAsync { get; }

        /// <summary>
        /// The actual type of the <see cref="DockableBase"/>.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Wether or not this can be an Anchorable.
        /// </summary>
        public bool IsAnchorable { get; }

        /// <summary>
        /// Wether or not this can be a Document.
        /// </summary>
        public bool IsDocument { get; }

        /// <summary>
        /// Creates a new <see cref="DockableInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="isAnchorable">Wether or not this can be an Anchorable.</param>
        /// <param name="isDocument">Wether or not this can be a Document.</param>
        /// <param name="func">The method to create the actual <see cref="DockableBase"/>.</param>
        /// <param name="type">The actual type of the <see cref="DockableBase"/>.</param>
        internal DockableInfo(string name, bool isAnchorable, bool isDocument, Func<DockableBase> func, Type type)
        {
            this.Name = name;
            this.IsAnchorable = isAnchorable;
            this.IsDocument = isDocument;
            this.CreateFunc = func;
            this.IsAsync = false;
            this.Type = type;
        }

        /// <summary>
        /// Creates a new <see cref="DockableInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="isAnchorable">Wether or not this can be an Anchorable.</param>
        /// <param name="isDocument">Wether or not this can be a Document.</param>
        /// <param name="func">The method to create the actual <see cref="DockableBase"/>.</param>
        /// <param name="type">The actual type of the <see cref="DockableBase"/>.</param>
        internal DockableInfo(string name, bool isAnchorable, bool isDocument, Func<Task<DockableBase>> func, Type type)
        {
            this.Name = name;
            this.IsAnchorable = isAnchorable;
            this.IsDocument = isDocument;
            this.CreateAsyncFunc = func;
            this.IsAsync = true;
            this.Type = type;
        }

        /// <summary>
        /// Creates a new <see cref="DockableInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="creationMode">The types this <see cref="DockableInfo"/> can create.</param>
        /// <param name="func">The method to create the actual <see cref="DockableBase"/>.</param>
        /// <returns>The created <see cref="DockableInfo"/> instance.</returns>
        public static DockableInfo Create<T>(string name, ECreationMode creationMode, Func<T> func) where T : DockableBase =>
            new DockableInfo(name, creationMode.HasFlag(ECreationMode.Anchorable), creationMode.HasFlag(ECreationMode.Document), func, typeof(T));

        /// <summary>
        /// Creates a new <see cref="DockableInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="creationMode">The types this <see cref="DockableInfo"/> can create.</param>
        /// <param name="func">The method to create the actual <see cref="DockableBase"/>.</param>
        /// <returns>The created <see cref="DockableInfo"/> instance.</returns>
        public static DockableInfo Create<T>(string name, ECreationMode creationMode, Func<Task<T>> func) where T : DockableBase =>
            new DockableInfo(name, creationMode.HasFlag(ECreationMode.Anchorable), creationMode.HasFlag(ECreationMode.Document), async () => await func(), typeof(T));

        /// <summary>
        /// Creates a new <see cref="DockableInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">An instance of some <see cref="DrawingBrush"/>.</param>
        /// <param name="creationMode">The types this <see cref="DockableInfo"/> can create.</param>
        /// <param name="func">The method to create the actual <see cref="DockableBase"/>.</param>
        /// <returns>The created <see cref="DockableInfo"/> instance.</returns>
        public static DockableInfoDrawingBrush Create<T>(string name, DrawingBrush iconSource, ECreationMode creationMode, Func<T> func) where T : DockableBase =>
            new DockableInfoDrawingBrush(name, iconSource, creationMode.HasFlag(ECreationMode.Anchorable), creationMode.HasFlag(ECreationMode.Document), func, typeof(T));

        /// <summary>
        /// Creates a new <see cref="DockableInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">An instance of some <see cref="DrawingBrush"/>.</param>
        /// <param name="creationMode">The types this <see cref="DockableInfo"/> can create.</param>
        /// <param name="func">The method to create the actual <see cref="DockableBase"/>.</param>
        /// <returns>The created <see cref="DockableInfo"/> instance.</returns>
        public static DockableInfoDrawingBrush Create<T>(string name, DrawingBrush iconSource, ECreationMode creationMode, Func<Task<T>> func) where T : DockableBase =>
            new DockableInfoDrawingBrush(name, iconSource, creationMode.HasFlag(ECreationMode.Anchorable), creationMode.HasFlag(ECreationMode.Document), async () => await func(), typeof(T));

        /// <summary>
        /// Creates a new <see cref="DockableInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">The path to an icon for the dockable to describe.
        /// Please try to use proper resource URI, eg.:
        /// pack://application:,,,/assemblyname;component/path/to/file.png</param>
        /// <param name="creationMode">The types this <see cref="DockableInfo"/> can create.</param>
        /// <param name="func">The method to create the actual <see cref="DockableBase"/>.</param>
        /// <returns>The created <see cref="DockableInfo"/> instance.</returns>
        public static DockableInfoIcon Create<T>(string name, string iconSource, ECreationMode creationMode, Func<T> func) where T : DockableBase =>
            new DockableInfoIcon(name, iconSource, creationMode.HasFlag(ECreationMode.Anchorable), creationMode.HasFlag(ECreationMode.Document), func, typeof(T));

        /// <summary>
        /// Creates a new <see cref="DockableInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">The path to an icon for the dockable to describe.
        /// Please try to use proper resource URI, eg.:
        /// pack://application:,,,/assemblyname;component/path/to/file.png</param>
        /// <param name="creationMode">The types this <see cref="DockableInfo"/> can create.</param>
        /// <param name="func">The method to create the actual <see cref="DockableBase"/>.</param>
        /// <returns>The created <see cref="DockableInfo"/> instance.</returns>
        public static DockableInfoIcon Create<T>(string name, string iconSource, ECreationMode creationMode, Func<Task<T>> func) where T : DockableBase =>
            new DockableInfoIcon(name, iconSource, creationMode.HasFlag(ECreationMode.Anchorable), creationMode.HasFlag(ECreationMode.Document), async () => await func(), typeof(T));
    }
}

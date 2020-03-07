using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.UI
{
    public class EditorInfoIcon : EditorInfo
    {
        /// <summary>
        /// The path to an icon for the dockable to describe.
        /// </summary>
        public string IconSource { get; }
        /// <summary>
        /// Creates a new <see cref="TextEditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">The path to an icon for the dockable to describe.
        /// Please try to use proper resource URI, eg.:
        /// pack://application:,,,/assemblyname;component/path/to/file.png</param>
        /// <param name="func">The method to create the actual <see cref="ITextEditor"/>.</param>
        /// <param name="type">The actual type of the <see cref="ITextEditor"/>.</param>
        internal EditorInfoIcon(string name, string iconSource, Func<IO.File, IEditorDocument> func, Type type, params string[] extensions) : base(name, func, type, extensions)
        {
            this.IconSource = iconSource;
        }

        /// <summary>
        /// Creates a new <see cref="TextEditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">The path to an icon for the dockable to describe.
        /// Please try to use proper resource URI, eg.:
        /// pack://application:,,,/assemblyname;component/path/to/file.png</param>
        /// <param name="func">The method to create the actual <see cref="ITextEditor"/>.</param>
        /// <param name="type">The actual type of the <see cref="ITextEditor"/>.</param>
        internal EditorInfoIcon(string name, string iconSource, Func<IO.File, Task<IEditorDocument>> func, Type type, params string[] extensions) : base(name, func, type, extensions)
        {
            this.IconSource = iconSource;
        }
    }
}

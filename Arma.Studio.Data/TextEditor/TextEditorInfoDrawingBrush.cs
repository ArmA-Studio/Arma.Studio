using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.Data.TextEditor
{
    public class TextEditorInfoDrawingBrush : TextEditorInfo
    {
        /// <summary>
        /// A <see cref="DrawingBrush"/> that represents an icon.
        /// </summary>
        public DrawingBrush IconSource { get; }

        /// <summary>
        /// Creates a new <see cref="TextEditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">An instance of some <see cref="DrawingBrush"/>.</param>
        /// <param name="func">The method to create the actual <see cref="ITextEditor"/>.</param>
        /// <param name="type">The actual type of the <see cref="ITextEditor"/>.</param>
        internal TextEditorInfoDrawingBrush(string name, DrawingBrush iconSource, Func<ITextEditor> func, Type type) : base(name, func, type)
        {
            this.IconSource = iconSource;
        }

        /// <summary>
        /// Creates a new <see cref="TextEditorInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">An instance of some <see cref="DrawingBrush"/>.</param>
        /// <param name="func">The method to create the actual <see cref="ITextEditor"/>.</param>
        /// <param name="type">The actual type of the <see cref="ITextEditor"/>.</param>
        internal TextEditorInfoDrawingBrush(string name, DrawingBrush iconSource, Func<Task<ITextEditor>> func, Type type) : base(name, func, type)
        {
            this.IconSource = iconSource;
        }
    }
}

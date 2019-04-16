using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.Data.Dockable
{
    public class DockableInfoDrawingBrush : DockableInfo
    {
        /// <summary>
        /// A <see cref="DrawingBrush"/> that represents an icon.
        /// </summary>
        public DrawingBrush IconSource { get; }

        /// <summary>
        /// Creates a new <see cref="DockableInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">An instance of some <see cref="DrawingBrush"/>.</param>
        /// <param name="isAnchorable">Wether or not this can be an Anchorable.</param>
        /// <param name="isDocument">Wether or not this can be a Document.</param>
        /// <param name="func">The method to create the actual <see cref="DockableBase"/>.</param>
        /// <param name="type">The actual type of the <see cref="DockableBase"/>.</param>
        internal DockableInfoDrawingBrush(string name, DrawingBrush iconSource, bool isAnchorable, bool isDocument, Func<DockableBase> func, Type type) : base(name, isAnchorable, isDocument, func, type)
        {
            this.IconSource = iconSource;
        }

        /// <summary>
        /// Creates a new <see cref="DockableInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">An instance of some <see cref="DrawingBrush"/>.</param>
        /// <param name="isAnchorable">Wether or not this can be an Anchorable.</param>
        /// <param name="isDocument">Wether or not this can be a Document.</param>
        /// <param name="func">The method to create the actual <see cref="DockableBase"/>.</param>
        /// <param name="type">The actual type of the <see cref="DockableBase"/>.</param>
        internal DockableInfoDrawingBrush(string name, DrawingBrush iconSource, bool isAnchorable, bool isDocument, Func<Task<DockableBase>> func, Type type) : base(name, isAnchorable, isDocument, func, type)
        {
            this.IconSource = iconSource;
        }
    }
}

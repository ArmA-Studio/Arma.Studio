using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Dockable
{
    public class DockableInfoIcon : DockableInfo
    {
        /// <summary>
        /// The path to an icon for the dockable to describe.
        /// </summary>
        public string IconSource { get; }
        /// <summary>
        /// Creates a new <see cref="DockableInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">The path to an icon for the dockable to describe.
        /// Please try to use proper resource URI, eg.:
        /// pack://application:,,,/assemblyname;component/path/to/file.png</param>
        /// <param name="isAnchorable">Wether or not this can be an Anchorable.</param>
        /// <param name="isDocument">Wether or not this can be a Document.</param>
        /// <param name="func">The method to create the actual <see cref="DockableBase"/>.</param>
        /// <param name="type">The actual type of the <see cref="DockableBase"/>.</param>
        internal DockableInfoIcon(string name, string iconSource, bool isAnchorable, bool isDocument, Func<DockableBase> func, Type type) : base(name, isAnchorable, isDocument, func, type)
        {
            this.IconSource = iconSource;
        }

        /// <summary>
        /// Creates a new <see cref="DockableInfo"/> instance.
        /// </summary>
        /// <param name="name">The localized name of the dockable to describe</param>
        /// <param name="iconSource">The path to an icon for the dockable to describe.
        /// Please try to use proper resource URI, eg.:
        /// pack://application:,,,/assemblyname;component/path/to/file.png</param>
        /// <param name="isAnchorable">Wether or not this can be an Anchorable.</param>
        /// <param name="isDocument">Wether or not this can be a Document.</param>
        /// <param name="func">The method to create the actual <see cref="DockableBase"/>.</param>
        /// <param name="type">The actual type of the <see cref="DockableBase"/>.</param>
        internal DockableInfoIcon(string name, string iconSource, bool isAnchorable, bool isDocument, Func<Task<DockableBase>> func, Type type) : base(name, isAnchorable, isDocument, func, type)
        {
            this.IconSource = iconSource;
        }
    }
}

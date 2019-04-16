using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Dockable
{
    public interface IDockableProvider
    {
        /// <summary>
        /// Called at program start.
        /// Is expected to contain all documents and anchorables a user can open.
        /// </summary>
        /// <returns>DockableInfos that contain the documents a user can open.</returns>
        IEnumerable<DockableInfo> Dockables { get; }
        /// <summary>
        /// Called at program start.
        /// Supposed to add the datatemplates to the <see cref="GenericDataTemplateSelector"/>.
        /// </summary>
        /// <param name="selector">The <see cref="GenericDataTemplateSelector"/> instance.</param>
        void AddDataTemplates(GenericDataTemplateSelector selector);
    }
}

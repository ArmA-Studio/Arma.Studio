using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Plugin
{
    public interface IDockableProvider
    {
        /// <summary>
        /// Called at program start.
        /// Is expected to return all documents a user can open.
        /// </summary>
        /// <returns>DockableInfos that contain the documents a user can open.</returns>
        IEnumerable<DockableInfo> GetDocuments();
        /// <summary>
        /// Called at program start.
        /// Is expected to return all anchorables a user can open.
        /// </summary>
        /// <returns>DockableInfos that contain the anchorables a user can open.</returns>
        IEnumerable<DockableInfo> GetAnchorables();
        /// <summary>
        /// Supposed to add the datatemplates to the <see cref="GenericDataTemplateSelector"/>.
        /// </summary>
        /// <param name="selector">The <see cref="GenericDataTemplateSelector"/> instance.</param>
        void AddDataTemplates(GenericDataTemplateSelector selector);
    }
}

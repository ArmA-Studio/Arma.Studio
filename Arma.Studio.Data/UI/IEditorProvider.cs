using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.UI
{
    public interface IEditorProvider
    {
        /// <summary>
        /// Called at program start.
        /// Is expected to contain all Editors a user can open.
        /// </summary>
        /// <returns>EditorInfo that contain the documents a user can open.</returns>
        IEnumerable<EditorInfo> EditorInfos { get; }
    }
}

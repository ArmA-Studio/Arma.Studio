using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.TextEditor
{
    public interface ITextEditorProvider
    {
        /// <summary>
        /// Called at program start.
        /// Is expected to contain all Text Editors a user can open.
        /// </summary>
        /// <returns>TextEditorInfo that contain the documents a user can open.</returns>
        IEnumerable<TextEditorInfo> TextEditorInfos { get; }
    }
}

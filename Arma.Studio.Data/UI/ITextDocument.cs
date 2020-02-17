using Arma.Studio.Data.TextEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.UI
{
    public interface ITextDocument
    {
        ITextEditor TextEditorInstance { get; }
        DateTime LastChangeTimestamp { get; }
        bool IsReadOnly { get; set; }
        string GetContents();
        void ScrollTo(int line, int column);
    }
}

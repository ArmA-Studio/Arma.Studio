
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.Data.TextEditor
{
    public interface ICodeCompletionInfo
    {
        ImageSource ImageSource { get; }

        string Text { get; }

        object Content { get; }

        object Description { get; }

        double Priority { get; }

        abstract string Complete(string input);
    }
}

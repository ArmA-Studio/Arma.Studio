using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.TextEditor
{
    public interface ITextEditor
    {
        SyntaxFile SyntaxFile { get; }
        bool ShowLineNumbers { get; }
    }
}

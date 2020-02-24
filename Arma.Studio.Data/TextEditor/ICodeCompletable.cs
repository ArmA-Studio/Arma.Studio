using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.TextEditor
{
    public interface ICodeCompletable
    {
        IEnumerable<ICodeCompletionInfo> GetAutoCompleteInfos(string text, int caretOffset);
        bool IsSeparatorCharacter(char c);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arma.Studio.Data.TextEditor
{
    public interface IFoldable
    {
        Task<IEnumerable<FoldingInfo>> GetFoldings(string text, CancellationToken cancellationToken);
    }
}

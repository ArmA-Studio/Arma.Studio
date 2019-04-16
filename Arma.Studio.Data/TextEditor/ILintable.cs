using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arma.Studio.Data.TextEditor
{
    public interface ILintable
    {
        Task<IEnumerable<LintInfo>> GetLintInfos(string text, CancellationToken cancellationToken);
    }
}

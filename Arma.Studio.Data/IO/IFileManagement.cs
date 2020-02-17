using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.IO
{
    public interface IFileManagement : ICollection<PBO>
    {
        FileFolderBase this[string key] { get; set; }
        bool ContainsKey(string key);
        bool TryGetValue(string fullkey, out FileFolderBase fileFolderBase);
    }
}

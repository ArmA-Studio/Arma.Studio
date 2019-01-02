using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.IO
{
    public interface IFileManagement
    {
        IEnumerable<PBO> PBOs { get; }
        FileFolderBase this[string key] { get; set; }
        void Add(PBO pbo);
    }
}

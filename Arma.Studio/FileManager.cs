using Arma.Studio.Data.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio
{
    public class FileManager : IFileManagement
    {
        public FileFolderBase this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IEnumerable<PBO> PBOs => throw new NotImplementedException();

        public void Add(PBO pbo) => throw new NotImplementedException();
    }
}

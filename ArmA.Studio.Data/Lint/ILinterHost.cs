using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Data.Lint
{
    public interface ILinterHost
    {
        IEnumerable<LintInfo> LinterInfo { get; }
        void DoLinting(Stream reader);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data
{
    public enum ESeverity : sbyte
    {
        Diagnostic = -2,
        Trace = -1,
        Info = 0,
        Warning,
        Error
    }
}

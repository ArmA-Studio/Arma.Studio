using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Debugging
{
    public interface IBreakpoint : INotifyPropertyChanged
    {
        string File { get; }
        int Line { get; set; }
        int Column { get; set; }
        bool IsActive { get; set; }
        int HitCount { get; set; }
        EBreakpointKind Kind { get; set; }
    }
}

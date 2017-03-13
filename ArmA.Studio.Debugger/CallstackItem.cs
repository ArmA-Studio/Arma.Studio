using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Debugger
{
    public sealed class CallstackItem
    {
        public string ContentSample { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public string FileName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Data
{
    public struct BreakpointInfo
    {
        public int Line { get; set; }
        public ProjectFile FileFolder { get; set; }

        public string SqfCondition { get; set; }
        public bool IsEnabled { get; set; }
    }
}

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
        public ProjectFile FileRef { get; set; }

        public string SqfCondition { get; set; }
        public bool IsEnabled { get; set; }

        public override string ToString()
        {
            //ToDo: Improve ToString formatting
            return $"{{{(!this.IsEnabled ? "INACTIVE" : string.Empty)} Line {this.Line} {this.SqfCondition}}}";
        }
    }
}

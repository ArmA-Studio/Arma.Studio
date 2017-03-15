using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Debugger
{
    [Flags]
    public enum EVariableNamespace
    {
        All = Callstack | LocalEvaluator | MissionNamespace | UiNamespace | ProfileNamespace | ParsingNamespace,
        Callstack = 1,
        LocalEvaluator = 2,
        MissionNamespace = 4,
        UiNamespace = 8,
        ProfileNamespace = 16,
        ParsingNamespace = 32
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Debugger
{
    public partial struct Variable
    {
        public string Name;
        public string Value;
        public ValueType VariableType;
        public EVariableNamespace Namespace;
    }

}
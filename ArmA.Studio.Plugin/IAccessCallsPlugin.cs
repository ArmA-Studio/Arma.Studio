using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data;
using ArmA.Studio.Data.Configuration;
using ArmA.Studio.Data.UI;

namespace ArmA.Studio.Plugin
{
    public interface IAccessCallsPlugin : IPlugin
    {
        Func<DocumentBase> GetCurrentDocument { set; }
        Func<IEnumerable<IPlugin>> GetAllPlugins { set; }
        Func<Solution> GetSolution { set; }
        Action<Exception> ShowOperationFailed { set; }
    }
}

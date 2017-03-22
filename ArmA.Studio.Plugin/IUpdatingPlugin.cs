using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Plugin
{
    public interface IUpdatingPlugin : IPlugin
    {
        bool CheckUpdateAvailable();
    }
}

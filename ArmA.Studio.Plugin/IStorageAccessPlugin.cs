using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data.Configuration;
using IniParser.Model;

namespace ArmA.Studio.Plugin
{
    public interface IStorageAccessPlugin : IPlugin
    {
        IniData ProjectStorage { set; }
        bool ProjectStorageHasChanges { get; }
        IniData ToolStorage { set; }
        bool ToolStorageHasChanges { get; }
    }
}

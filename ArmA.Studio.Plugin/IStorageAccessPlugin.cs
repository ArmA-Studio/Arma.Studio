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
        IniData ProjectStorage { get; set; }
        bool ProjectStorageHasChanges { get; }
        IniData ToolStorage { get; set; }
        bool ToolStorageHasChanges { get; }
    }
}

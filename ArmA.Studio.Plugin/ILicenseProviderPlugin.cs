using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data;

namespace ArmA.Studio.Plugin
{
    /// <summary>
    /// Class to provide info of the used licenses.
    /// Should not contain the plugins license itself but the licenses of stuff used by the plugin.
    /// </summary>
    public interface ILicenseProviderPlugin : IPlugin
    {
        IEnumerable<LicenseInfo> LicenseInfos { get; }
    }
}

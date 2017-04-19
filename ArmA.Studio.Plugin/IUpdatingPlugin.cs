using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Plugin
{
    public interface IUpdatingPlugin : IPlugin
    {
        /// <summary>
        /// Checks if any update is available for the current plugin.
        /// </summary>
        /// <returns><see cref="true"/> if any plugin is available. false in all other cases including target not reachable.</returns>
        bool CheckUpdateAvailable();

        /// <summary>
        /// Creates a new temporary local file and downloads the latest plugin version into it.
        /// Always will download latest, even if current is equal to latest.
        /// Recommended way to download: HTTP(S)
        /// </summary>
        /// <param name="progress">Progress reporter receiving (Item1: CurrentDownloadProgressInbytes, Item2: FileSizeInBytes).</param>
        /// <returns>Path to the temporary local file created.</returns>
        string DownloadUpdate(IProgress<Tuple<long, long>> progress);
    }
}

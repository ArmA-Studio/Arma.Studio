using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Plugin
{
    public interface IPlugin
    {
        /// <summary>
        /// Name of this plugin
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Text describing what this plugin is about.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// URL where the current version can be received from.
        /// Version number must consist of 1 - 4 integers separated by a dot.
        /// Example page content:
        /// 1.234.5.6
        /// </summary>
        string UpdateUrl { get; }


    }
}

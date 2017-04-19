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
    }
}

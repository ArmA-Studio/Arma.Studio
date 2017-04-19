using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArmA.Studio.Data;
using ArmA.Studio.Data.Configuration;
using ArmA.Studio.Data.UI;

namespace ArmA.Studio.Plugin
{
    public interface IPaneProviderPlugin : IPlugin
    {
        IEnumerable<Type> PaneDataContextTypes { get; set; }
        IEnumerable<DataTemplate> PaneDataTemplates { get; }
    }
}

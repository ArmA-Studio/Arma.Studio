using Arma.Studio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.UI.Windows
{
    public class AboutDialogDataContext
    {
        public string ToolVersion => App.CurrentVersion.ToString();


        public IPlugin[] Plugins { get; }
        public AboutDialogDataContext()
        {

            this.Plugins = PluginManager.Instance.Plugins.Select((it) => it.Plugin()).ToArray();
        }
    }
}

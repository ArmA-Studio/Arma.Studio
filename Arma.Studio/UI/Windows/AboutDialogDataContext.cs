using Arma.Studio.Data;
using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Arma.Studio.UI.Windows
{
    public class AboutDialogDataContext
    {
        public string ToolVersion => App.GetReleaseInfos();
        public ICommand CmdCopyReleaseInfo => new RelayCommand(() => Clipboard.SetText(App.GetReleaseInfos(), TextDataFormat.UnicodeText));
        public IPlugin[] Plugins { get; }
        public AboutDialogDataContext()
        {

            this.Plugins = PluginManager.Instance.Plugins.Select((it) => it.Plugin()).OrderBy((it) => it.Name).ToArray();
        }
    }
}

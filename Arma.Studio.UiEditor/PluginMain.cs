using Arma.Studio.Data;
using Arma.Studio.Data.Dockable;
using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arma.Studio.UiEditor
{
    public class PluginMain : IPlugin, IDockableProvider
    {
        public Version Version => new Version(1, 0, 0);
        public string Name => Properties.Language.UIEditor_Name;

        public IEnumerable<DockableInfo> Dockables => new DockableInfo[] {
            DockableInfo.Create(Properties.Language.UIEditor_Toolbox, ECreationMode.Anchorable, () => new EditorToolboxDataContext()),
            DockableInfo.Create(Properties.Language.UIEditor_Document, ECreationMode.Document, () => new EditorDataContext())
        };

        public Task<IUpdateInfo> CheckForUpdate(CancellationToken cancellationToken) => Task.Run(() => default(IUpdateInfo));
        public Task Initialize(string pluginPath, CancellationToken cancellationToken) => Task.CompletedTask;

        public void AddDataTemplates(GenericDataTemplateSelector selector)
        {
            selector.AddAllDataTemplatesInAssembly(typeof(PluginMain).Assembly, (s) => s.StartsWith("Arma.Studio.UiEditor"));
        }
    }
}

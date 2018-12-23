using ArmA.Studio.Data;
using ArmA.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArmA.Studio.UiEditor
{
    public class PluginMain : IPlugin
    {
        public Version Version => new Version(1, 0, 0);
        public string Name => Properties.Language.UIEditor_Name;

        public Task<IUpdateInfo> CheckForUpdate(CancellationToken cancellationToken) => Task.Run(() => default(IUpdateInfo));
        public Task Initialize(CancellationToken cancellationToken) => Task.CompletedTask;
        public IEnumerable<DockableInfo> GetAnchorables()
        {
            yield break;
        }

        public IEnumerable<DockableInfo> GetDocuments()
        {
            yield return DockableInfo.Create(Properties.Language.UIEditor_Document, "", () => new EditorDataContext());
        }

        public void AddDataTemplates(GenericDataTemplateSelector selector)
        {
            selector.AddAllDataTemplatesInAssembly(typeof(PluginMain).Assembly, (s) => s.StartsWith("ArmA.Studio.UiEditor"));
        }
    }
}

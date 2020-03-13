using Arma.Studio.Data;
using Arma.Studio.Data.Dockable;
using Arma.Studio.Data.TextEditor;
using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Arma.Studio.ConfigEditor
{
    public class PluginMain : IPlugin, ITextEditorProvider
    {
        #region IPlugin
        public Version Version => new Version(1, 0, 0);
        public string Name => Properties.Language.ConfigEditor_Name;
        public string Description => String.Empty;
        public Task<IUpdateInfo> CheckForUpdate(CancellationToken cancellationToken) => Task.Run(() => default(IUpdateInfo));
        public Task Initialize(string pluginPath, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        #endregion
        #region ITextEditorProvider
        public IEnumerable<TextEditorInfo> TextEditorInfos => new TextEditorInfo[] {
            TextEditorInfo.Create(Properties.Language.ConfigEditor_Document, () => new ConfigEditor(), ".cpp", ".ext", ".hpp")
        };
        #endregion
    }
}

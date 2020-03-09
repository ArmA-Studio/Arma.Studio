using Arma.Studio.Data;
using Arma.Studio.Data.Dockable;
using Arma.Studio.Data.Log;
using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arma.Studio.UiEditor
{
    public class PluginMain : IPlugin, IDockableProvider, IEditorProvider, ILogger
    {
        public Version Version => new Version(1, 0, 0);
        public string Name => Properties.Language.UIEditor_Name;
        public string Description => String.Empty;

        public IEnumerable<DockableInfo> Dockables => new DockableInfo[] {
            DockableInfo.Create(Properties.Language.UIEditor_Toolbox, ECreationMode.Anchorable, () => new EditorToolboxDataContext())
        };

        public IEnumerable<EditorInfo> EditorInfos => new EditorInfo[]
        {
            EditorInfo.Create(Properties.Language.UIEditor_Document, (file) => new UI.UiEditorDataContext(file), ".ui")
        };

        public Task<IUpdateInfo> CheckForUpdate(CancellationToken cancellationToken) => Task.Run(() => default(IUpdateInfo));
        public static string ParentClassesConfig { get; private set; }
        public Task Initialize(string pluginPath, CancellationToken cancellationToken)
        {
            Logger.Trace($"Loading ParentClasses config.");
            using (var stream = typeof(PluginMain).Assembly.GetManifestResourceStream(typeof(PluginMain).Assembly.GetName().Name + ".ParentClasses.cpp"))
            using (var reader = new System.IO.StreamReader(stream))
            {
                var text = reader.ReadToEnd().Trim();
                ParentClassesConfig = text;
            }
            return Task.CompletedTask;
        }

        public void AddDataTemplates(GenericDataTemplateSelector selector)
        {
            selector.AddAllDataTemplatesInAssembly(typeof(PluginMain).Assembly, (s) => s.StartsWith("Arma.Studio.UiEditor"));
        }


        #region ILogger
        internal static Logger Logger { get; private set; }
        public string TargetName => Properties.Language.LoggerName;

        public void SetLogger(Logger logger)
        {
            Logger = logger;
        }
        #endregion
    }
}

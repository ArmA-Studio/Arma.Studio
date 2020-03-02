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

namespace Arma.Studio.ErrorWindow
{
    public class PluginMain : IPlugin, IDockableProvider
    {
        internal static LoggerCollection Loggers { get; private set; }
        #region IPlugin
        public Version Version => new Version(1, 0, 0, 0);
        public string Name => Properties.Language.ErrorWindow_Name;
        public string Description => String.Empty;
        public Task<IUpdateInfo> CheckForUpdate(CancellationToken cancellationToken) => Task.Run(() => default(IUpdateInfo));
        public Task Initialize(string pluginPath, CancellationToken cancellationToken)
        {
            Instanceable<PluginMain>.Instance = this;
            this.GetApplication().MainWindow.FileManagement.CollectionChanged += this.FileManagement_CollectionChanged;
            return Task.CompletedTask;
        }
        #endregion
        #region IDockableProvider
        public IEnumerable<DockableInfo> Dockables => new DockableInfo[] {
            DockableInfo.Create(Properties.Language.ErrorWindow, ECreationMode.Anchorable, () => new ErrorWindowDataContext())
        };
        public void AddDataTemplates(GenericDataTemplateSelector selector)
        {
            selector.AddAllDataTemplatesInAssembly(typeof(PluginMain).Assembly, (s) => s.StartsWith("Arma.Studio.ErrorWindow"));
        }
        #endregion

        public System.Collections.ObjectModel.ObservableCollection<Data.TextEditor.LintInfo> LintInfos { get; } = new System.Collections.ObjectModel.ObservableCollection<Data.TextEditor.LintInfo>();

        private void FileManagement_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (Data.IO.PBO pbo in e.NewItems??Array.Empty<Data.IO.PBO>())
            {
                this.GetApplication().MainWindow.BusyContainerManager.Run(async (cancellationToken, busyContainer) => {
                    var editor = this.GetApplication().MainWindow.TextEditorInfos.FirstOrDefault((tei) => tei.Extensions.Contains(".sqf"));
                    if (editor is null)
                    {
                        return;
                    }
                    var files = pbo.GetAll((file) => file.Extension == ".sqf").ToArray();
                    foreach (var file in files)
                    {
                        Data.TextEditor.ITextEditor instance;
                        if (editor.IsAsync)
                        {
                            instance = await editor.CreateAsyncFunc();
                        }
                        else
                        {
                            instance = editor.CreateFunc();
                        }
                        if (!(instance is Data.TextEditor.ILintable lintable))
                        {
                            return;
                        }
                        instance.File = file;
                        var doc = this.GetApplication().MainWindow
                                  .GetDocuments<DockableBase>()
                                  .Where((it) => it is ITextDocument)
                                  .Cast<ITextDocument>()
                                  .FirstOrDefault((d) => d.TextEditorInstance.File == file);
                        if (doc != null)
                        {
                            continue;
                        }
                        var lintinfos = await lintable.GetLintInfos(file.GetText(), CancellationToken.None);
                        foreach (var it in lintinfos)
                        {
                            this.LintInfos.Add(it);
                        }
                    }
                }, String.Format(Properties.Language.PBO_LintingFiles_0file, pbo.Name));
            }
        }
    }
}

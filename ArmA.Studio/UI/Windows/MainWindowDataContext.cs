using Arma.Studio.Data;
using Arma.Studio.Data.Debugging;
using Arma.Studio.Data.IO;
using Arma.Studio.Data.Plugin;
using Arma.Studio.Data.UI;
using Arma.Studio.UI.AvalonDock;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace Arma.Studio.UI.Windows
{

    public class MainWindowDataContext : INotifyPropertyChanged, IMainWindow
    {
        private const string CONST_INI_TYPES_STRING = "Types";
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callee = "") => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee));

        #region IMainWindow
        public Window OwningWindow { get; private set; }
        public void SetStatusLabel(string s) => App.Current.Dispatcher.Invoke(() => this.StatusLabel = s);
        public DockableBase FirstDocumentOrDefault(Func<DockableBase, bool> predicate) => this.Documents.FirstOrDefault((it) => predicate(it));
        public DockableBase FirstAnchorableOrDefault(Func<DockableBase, bool> predicate) => this.Anchorables.FirstOrDefault((it) => predicate(it));
        public DockableBase SelectedDockable { get => this.AvalonDockActiveContent as DockableBase; set => this.AvalonDockActiveContent = value; }
        public void AddDocument(DockableBase dockableBase)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                dockableBase.OnDockableClose += this.Dockable_OnDocumentClosing;
                this.Documents.Add(dockableBase);
                dockableBase.IsActive = true;
                dockableBase.IsSelected = true;
                dockableBase.IsAnchorable = false;
                this.AvalonDockActiveContent = dockableBase;
            });
        }
        public void AddAnchorable(DockableBase dockableBase)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var t = dockableBase.GetType();
                if (this.Anchorables.Any((db) => db.GetType().IsEquivalentTo(t)))
                {
                    throw new Exception();
                }

                dockableBase.OnDockableClose += this.Dockable_OnDocumentClosing;
                this.Anchorables.Add(dockableBase);
                dockableBase.IsActive = true;
                dockableBase.IsSelected = true;
                dockableBase.IsAnchorable = true;
                this.AvalonDockActiveContent = dockableBase;
            });
        }
        public IEnumerable<T> GetDocuments<T>() where T : DockableBase
        {
            foreach (var it in this.Documents)
            {
                if (it is T t)
                {
                    yield return t;
                }
            }
        }
        public T GetAnchorable<T>() where T : DockableBase
        {
            foreach (var it in this.Anchorables)
            {
                if (it is T t)
                {
                    return t;
                }
            }
            return null;
        }
        public IFileManagement FileManagement => this.Solution.FileManager;
        public IBreakpointManager BreakpointManager => this.Solution.BreakpointManager;
        #endregion

        public Solution Solution { get; }

        public object AvalonDockActiveContent
        {
            get => this._AvalonDockActiveContent;
            set
            {
                if (this._AvalonDockActiveContent == value) { return; }
                this._AvalonDockActiveContent = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.SelectedDockable));
            }
        }
        private object _AvalonDockActiveContent;

        public IDebugger Debugger { get { return this._Debugger; } set { this._Debugger = value; this.RaisePropertyChanged(); } }
        private IDebugger _Debugger;

        public bool DebuggerIsRunning { get { return this._DebuggerIsRunning; } set { this._DebuggerIsRunning = value; this.RaisePropertyChanged(); } }
        private bool _DebuggerIsRunning;

        public ICommand CmdDebuggerAction => new RelayCommand<EDebugAction>((p) => { });

        public string StatusLabel { get { return this._StatusLabel; } set { this._StatusLabel = value; this.RaisePropertyChanged(); } }
        private string _StatusLabel;

        public Xceed.Wpf.AvalonDock.DockingManager WindowsDockingManager { get; private set; }
        public GenericDataTemplateSelector LayoutItemTemplateSelector { get; private set; }

        public ICommand CmdQuit => new RelayCommand((p) => this.OwningWindow.Close());
        public ICommand CmdWindowClosing => new RelayCommand((p) =>
        {
            this.SaveAvalonDockLayout();
        });
        public ICommand CmdActiveContentChanged => new RelayCommand((p) => { });
        public ICommand CmdWindowClosed => new RelayCommand((p) => { App.Current.Shutdown(0); });
        public ICommand CmdWindowInitialized => new RelayCommand<Window>((window) =>
        {
            if (this.OwningWindow == null)
            {
                this.OwningWindow = window;
                this.Initialized();
            }
            else
            {
                this.OwningWindow = window;
            }
        });
        public ICommand CmdDockingManagerInitialized => new RelayCommand((p) =>
        {
            this.WindowsDockingManager = p as Xceed.Wpf.AvalonDock.DockingManager;
            this.LoadAvalonDockLayout();
        });
        public ICommand CmdCreateDocument => new RelayCommand<DockableInfo>((info) => this.AddDocument(info.CreateFunc()));
        public ICommand CmdCreateAnchorable => new RelayCommand<DockableInfo>((info) =>
        {
            var anch = this.Anchorables.FirstOrDefault((db) => db.GetType().IsEquivalentTo(info.Type));
            if (anch == null)
            {
                this.AddAnchorable((DockableBase)Activator.CreateInstance(info.Type));
            }
            else
            {
                anch.IsActive = true;
                anch.IsSelected = true;
            }
        });

        public ObservableCollection<DockableBase> Anchorables { get { return this._Anchorables; } set { this._Anchorables = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<DockableBase> _Anchorables;

        public ObservableCollection<DockableBase> Documents { get { return this._Documents; } set { this._Documents = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<DockableBase> _Documents;

        public ObservableCollection<DockableInfo> AnchorablesAvailable { get { return this._AnchorablesAvailable; } set { this._AnchorablesAvailable = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<DockableInfo> _AnchorablesAvailable;

        public ObservableCollection<DockableInfo> DocumentsAvailable { get { return this._DocumentsAvailable; } set { this._DocumentsAvailable = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<DockableInfo> _DocumentsAvailable;
        
        private Newtonsoft.Json.Linq.JObject LayoutJsonNode;
        private void LayoutSerializer_LayoutSerializationCallback(object sender, Xceed.Wpf.AvalonDock.Layout.Serialization.LayoutSerializationCallbackEventArgs e)
        {
            var contentid = e.Model.ContentId;
            dynamic types = this.LayoutJsonNode[CONST_INI_TYPES_STRING];
            string typestring = types[contentid];
            if (String.IsNullOrWhiteSpace(typestring))
            {
                e.Cancel = true;
                return;
            }
            var type = Type.GetType(typestring, false);
            if (type == null || !typeof(DockableBase).IsAssignableFrom(type))
            {
                e.Cancel = true;
                return;
            }
            var dockable = Activator.CreateInstance(type) as DockableBase;
            e.Content = dockable;
            dockable.ContentId = e.Model.ContentId;


            dockable.LayoutLoadCallback(this.LayoutJsonNode[contentid]);

            if (e.Model is Xceed.Wpf.AvalonDock.Layout.LayoutAnchorable)
            {
                this.AddAnchorable(dockable);
            }
            else if (e.Model is Xceed.Wpf.AvalonDock.Layout.LayoutDocument)
            {
                this.AddDocument(dockable);
            }
        }
        private void LoadAvalonDockLayout()
        {
            if (System.IO.File.Exists(App.LayoutConfigFilePath))
            {
                try
                {
                    using (var reader = System.IO.File.OpenRead(App.LayoutConfigFilePath))
                    {
                        this.LayoutJsonNode = Newtonsoft.Json.JsonConvert.DeserializeObject(new System.IO.StreamReader(reader).ReadToEnd()) as Newtonsoft.Json.Linq.JObject;
                    }
                }
                catch (Exception ex)
                {
                    App.DisplayOperationFailed(ex);
                }
            }
            if (!System.IO.File.Exists(App.LayoutFilePath))
            {
                return;
            }
            try
            {
                using (var reader = System.IO.File.OpenRead(App.LayoutFilePath))
                {
                    var layoutSerializer = new Xceed.Wpf.AvalonDock.Layout.Serialization.XmlLayoutSerializer(this.WindowsDockingManager);
                    layoutSerializer.LayoutSerializationCallback += this.LayoutSerializer_LayoutSerializationCallback;
                    layoutSerializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                App.DisplayOperationFailed(ex);
            }
        }
        private void SaveAvalonDockLayout()
        {
            var dir = System.IO.Path.GetDirectoryName(App.LayoutFilePath);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            using (var writer = System.IO.File.Open(App.LayoutFilePath, System.IO.FileMode.Create))
            {
                var layoutSerializer = new Xceed.Wpf.AvalonDock.Layout.Serialization.XmlLayoutSerializer(this.WindowsDockingManager);
                layoutSerializer.Serialize(writer);
            }
            this.LayoutJsonNode = new Newtonsoft.Json.Linq.JObject(new Newtonsoft.Json.Linq.JProperty(CONST_INI_TYPES_STRING, new Newtonsoft.Json.Linq.JObject()));
            var types = this.LayoutJsonNode[CONST_INI_TYPES_STRING];

            foreach (var it in this.Anchorables.Concat(this.Documents))
            {
                var token = types.GetOrCreateProperty(it.ContentId);
                types[it.ContentId] = new Newtonsoft.Json.Linq.JValue(it.GetType().FullName);
                it.LayoutSaveCallback(this.LayoutJsonNode.GetOrCreateProperty(it.ContentId));
            }

            using (var writer = System.IO.File.Open(App.LayoutConfigFilePath, System.IO.FileMode.Create))
            {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                var swriter = new System.IO.StreamWriter(writer);
#if DEBUG
                serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
#endif
                serializer.Serialize(swriter, this.LayoutJsonNode);
                swriter.Flush();
            }
        }

        public MainWindowDataContext()
        {
            this.LayoutItemTemplateSelector = new GenericDataTemplateSelector();
            this.Anchorables = new ObservableCollection<DockableBase>();
            this.Documents = new ObservableCollection<DockableBase>();
            this.AnchorablesAvailable = new ObservableCollection<DockableInfo>();
            this.DocumentsAvailable = new ObservableCollection<DockableInfo>();
            this.Solution = new Solution();
            this.LayoutJsonNode = new Newtonsoft.Json.Linq.JObject(new Newtonsoft.Json.Linq.JProperty(CONST_INI_TYPES_STRING, new Newtonsoft.Json.Linq.JObject()));
        }
        private void Initialized()
        {
            this.LayoutItemTemplateSelector.AddAllDataTemplatesInAssembly(typeof(MainWindowDataContext).Assembly, (s) => s.StartsWith("Arma.Studio.UI"));
            foreach (var it in PluginManager.Instance.GetPlugins<IDockableProvider>())
            {
                it.AddDataTemplates(this.LayoutItemTemplateSelector);
                this.AnchorablesAvailable.AddRange(it.GetAnchorables());
                this.DocumentsAvailable.AddRange(it.GetDocuments());
            }
        }
        private void Dockable_OnDocumentClosing(object sender, EventArgs e)
        {
            var dockable = sender as DockableBase;
            dockable.OnDockableClose -= this.Dockable_OnDocumentClosing;
            if (this.Documents.Contains(dockable))
            {
                try
                {
                    this.Documents.Remove(dockable);
                }
                catch (NullReferenceException) { } //AvalonDock ...
            }
            else if (this.Anchorables.Contains(dockable))
            {
                try
                {
                    this.Anchorables.Remove(dockable);
                }
                catch (NullReferenceException) { } //AvalonDock ...
            }
        }
    }
}

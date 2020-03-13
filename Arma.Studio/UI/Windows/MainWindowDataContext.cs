using Arma.Studio.Data;
using Arma.Studio.Data.Debugging;
using Arma.Studio.Data.Dockable;
using Arma.Studio.Data.IO;
using Arma.Studio.Data.TextEditor;
using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace Arma.Studio.UI.Windows
{

    public class MainWindowDataContext : INotifyPropertyChanged, IMainWindow, IDisposable
    {
        private const string CONST_INI_TYPES_STRING = "Types";
        private const string CONST_INI_PBOPATHS_STRING = "PBO-Paths";
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callee = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee));
        }

        #region IMainWindow
        public IKeyInteractible KeyInteractible
        {
            get => this._KeyInteractible;
            set
            {
                this._KeyInteractible = value;
                this.RaisePropertyChanged();
            }
        }
        private IKeyInteractible _KeyInteractible;
        public IPropertyHost PropertyHost
        {
            get => this._PropertyHost;
            set
            {
                if (this._PropertyHost == value) { return; }
                this._PropertyHost = value;
                this.RaisePropertyChanged();
            }
        }
        private IPropertyHost _PropertyHost;
        public IEnumerable<Data.UI.EditorInfo> EditorInfos => this.EditorsAvailable;
        public IEnumerable<Data.TextEditor.TextEditorInfo> TextEditorInfos => this.TextEditorsAvailable;
        public BusyContainerManager BusyContainerManager { get; }
        public Window OwningWindow { get; private set; }
        public void SetStatusLabel(string s)
        {
            App.Current.Dispatcher.Invoke(() => this.StatusLabel = s);
        }

        public DockableBase FirstDocumentOrDefault(Func<DockableBase, bool> predicate)
        {
            return this.Documents.FirstOrDefault((it) => predicate(it));
        }

        public DockableBase FirstAnchorableOrDefault(Func<DockableBase, bool> predicate)
        {
            return this.Anchorables.FirstOrDefault((it) => predicate(it));
        }

        public DockableBase ActiveDockable { get => this.AvalonDockActiveContent as DockableBase; set => this.AvalonDockActiveContent = value; }
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
        public IFileManagement FileManagement => this.Solution;
        public IBreakpointManager BreakpointManager => this.Solution.BreakpointManager;

        public Task<Data.UI.IEditorDocument> OpenFile(File file)
        {
            var doc = this.Documents.Where((d) => d is Data.UI.IEditorDocument).Cast<Data.UI.IEditorDocument>().FirstOrDefault((d) => d.File.Equals(file));
            if (doc != null)
            {
                (doc as DockableBase).Focus(); ;
                return Task.FromResult(doc);
            }
            string ext = file.Extension;
            var editorInfo = this.EditorInfos.FirstOrDefault((tei) => tei.Extensions.Any((s) => s.Equals(ext, StringComparison.InvariantCultureIgnoreCase)));
            if (editorInfo == null)
            {
                // ToDo: Localize
                MessageBox.Show("File extension unknown");
                return Task.FromException<Data.UI.IEditorDocument>(new Exception());
            }
            if (editorInfo.IsAsync)
            {
                return Task.Run(async () =>
                {
                    var editorDocument = await editorInfo.CreateAsyncFunc(file);
                    this.AddDocument((DockableBase)editorDocument);
                    return editorDocument;
                });
            }
            else
            {
                var editorDocument = editorInfo.CreateFunc(file);
                this.AddDocument((DockableBase)editorDocument);
                return Task.FromResult(editorDocument);
            }
        }
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
                this.RaisePropertyChanged(nameof(this.ActiveDockable));
            }
        }
        private object _AvalonDockActiveContent;

        public IDebugger Debugger
        {
            get => this._Debugger;
            set
            {
                if (this._Debugger != null)
                {
                    this._Debugger.PropertyChanged -= this.Debugger_PropertyChanged;
                    this.BreakpointManager.BreakpointAdded -= this.Debugger_BreakpointManager_BreakpointAdded;
                    this.BreakpointManager.BreakpointRemoved -= this.Debugger_BreakpointManager_BreakpointRemoved;
                    this.BreakpointManager.BreakpointUpdated -= this.Debugger_BreakpointManager_BreakpointUpdated;
                    foreach (var breakpoint in this.BreakpointManager.Breakpoints)
                    {
                        this.Debugger.RemoveBreakpoint(breakpoint);
                    }
                }
                this._Debugger = value;
                if (this._Debugger != null)
                {
                    this._Debugger.PropertyChanged += this.Debugger_PropertyChanged;
                    this.BreakpointManager.BreakpointAdded += this.Debugger_BreakpointManager_BreakpointAdded;
                    this.BreakpointManager.BreakpointRemoved += this.Debugger_BreakpointManager_BreakpointRemoved;
                    this.BreakpointManager.BreakpointUpdated += this.Debugger_BreakpointManager_BreakpointUpdated;
                    foreach (var breakpoint in this.BreakpointManager.Breakpoints)
                    {
                        this._Debugger.SetBreakpoint(breakpoint);
                    }
                }
                this.RaisePropertyChanged();
            }
        }

        public event EventHandler<EventArgs> DebuggerStateChanged;

        private void Debugger_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            foreach (var it in this.Documents.Where((it) => it is UI.TextEditorDataContext).Cast<UI.TextEditorDataContext>())
            {
                // Update Left-Margins
                if (it.TextEditorInstance != null && it.CurrentVisibility == Visibility.Visible)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        it.TextEditorControl.TextArea.TextView.InvalidateVisual();
                        foreach (var leftMargin in it.TextEditorControl.TextArea.LeftMargins)
                        {
                            leftMargin.InvalidateVisual();
                        }
                    });
                }
            }
            if (e.PropertyName == nameof(IDebugger.State))
            {
                this.DebuggerStateChanged?.Invoke(sender, new EventArgs());
            }
        }

        private void Debugger_BreakpointManager_BreakpointUpdated(object sender, BreakpointUpdatedEventArgs e)
        {
            this.Debugger.RemoveBreakpoint(e.BreakpointOld);
            this.Debugger.SetBreakpoint(e.BreakpointNew);
        }

        private void Debugger_BreakpointManager_BreakpointRemoved(object sender, BreakpointEventArgs e)
        {
            this.Debugger.RemoveBreakpoint(e.Breakpoint);
        }

        private void Debugger_BreakpointManager_BreakpointAdded(object sender, BreakpointEventArgs e)
        {
            this.Debugger.SetBreakpoint(e.Breakpoint);
        }

        private IDebugger _Debugger;

        public ICommand CmdDebuggerAction => new RelayCommand<EDebugAction>((p) =>
        {
            if (this.Debugger is null)
            {
                var debuggerPlugins = PluginManager.Instance.GetPlugins<IDebugger>().ToArray();
                if (debuggerPlugins.Length > 1)
                {
                    // ToDo: Show user a selection and let the user pick it ONCE per session
                }
                else if (debuggerPlugins.Length == 1)
                {
                    this.Debugger = debuggerPlugins.First();
                }
                else
                {
                    SystemSounds.Asterisk.Play();
                    MessageBox.Show("No Debugger Present");
                    return;
                }
            }
            if (p == EDebugAction.NA)
            {
                return;
            }
            this.Debugger.Execute(p);
        });

        public string StatusLabel { get => this._StatusLabel; set { this._StatusLabel = value; this.RaisePropertyChanged(); } }
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
        public ICommand CmdCreateDocument => new RelayCommand((info) =>
        {
            if (info is DockableInfo dinfo)
            {
                this.AddDocument(dinfo.CreateFunc());
            }
            else if (info is TextEditorInfo teinfo)
            {
                this.AddDocument(new TextEditorDataContext(teinfo.CreateFunc(), new File()));
            }
            else
            {
                throw new NotSupportedException();
            }
        });
        public ICommand CmdCreateAnchorable => new RelayCommand<DockableInfo>((info) =>
        {
            var anch = this.Anchorables.FirstOrDefault((db) => db.GetType().IsEquivalentTo(info.Type));
            if (anch == null)
            {
                this.AddAnchorable((DockableBase)Activator.CreateInstance(info.Type));
            }
            else
            {
                anch.Focus();
            }
        });
        public ICommand CmdUserIdentificationDialog => new RelayCommand(() =>
        {
            bool currentOptOut = Configuration.Instance.OptOutOfReportingAndUpdates;
            var dlgdc = new UI.Windows.UserIdentificationDialogDataContext();
            var dlg = new UI.Windows.UserIdentificationDialog(dlgdc);
            dlg.ShowDialog();
            if (currentOptOut != Configuration.Instance.OptOutOfReportingAndUpdates)
            {
                var res = MessageBox.Show(Properties.Language.MainWindow_RestartRequired_Body,
                    Properties.Language.MainWindow_RestartRequired_Caption,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);
                if (res == MessageBoxResult.Yes)
                {
                    App.Restart();
                }
            }

        });

        public ICommand CmdUndo => new RelayCommand(() =>
        {
            if (this.ActiveDockable is IInteractionUndoRedo interactionUndoRedo)
            {
                Task.Run(() => interactionUndoRedo.Undo(CancellationToken.None));
            }
        });
        public ICommand CmdRedo => new RelayCommand(() =>
        {
            if (this.ActiveDockable is IInteractionUndoRedo interactionUndoRedo)
            {
                Task.Run(() => interactionUndoRedo.Redo(CancellationToken.None));
            }
        });
        public ICommand CmdSaveDocument => new RelayCommand(() =>
        {
            if (this.ActiveDockable is IInteractionSave interactionSave)
            {
                Task.Run(() => interactionSave.Save(CancellationToken.None));
            }
        });
        public ICommand CmdShowAbout => new RelayCommand(() =>
        {
            var dlgdc = new AboutDialogDataContext();
            var dlg = new AboutDialog(dlgdc);
            dlg.ShowDialog();
        });
        public ICommand CmdSaveAllDocuments => new RelayCommand(() =>
        {
            foreach (var it in this.Documents)
            {
                if (it is IInteractionSave interactionSave)
                {
                    Task.Run(() => interactionSave.Save(CancellationToken.None));
                }
            }
        });

        public ObservableCollection<DockableBase> Anchorables { get => this._Anchorables; set { this._Anchorables = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<DockableBase> _Anchorables;

        public ObservableCollection<DockableBase> Documents { get => this._Documents; set { this._Documents = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<DockableBase> _Documents;

        public ObservableCollection<DockableInfo> AnchorablesAvailable { get => this._AnchorablesAvailable; set { this._AnchorablesAvailable = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<DockableInfo> _AnchorablesAvailable;

        public ObservableCollection<DockableInfo> DocumentsAvailable { get => this._DocumentsAvailable; set { this._DocumentsAvailable = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<DockableInfo> _DocumentsAvailable;

        public ObservableCollection<EditorInfo> EditorsAvailable { get => this._EditorsAvailable; set { this._EditorsAvailable = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<EditorInfo> _EditorsAvailable;
        public ObservableCollection<TextEditorInfo> TextEditorsAvailable { get => this._TextEditorsAvailable; set { this._TextEditorsAvailable = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<TextEditorInfo> _TextEditorsAvailable;

        private Newtonsoft.Json.Linq.JObject LayoutJsonNode;
        private void LayoutSerializer_LayoutSerializationCallback(object sender, Xceed.Wpf.AvalonDock.Layout.Serialization.LayoutSerializationCallbackEventArgs e)
        {
            string contentid = e.Model.ContentId;
            dynamic types = this.LayoutJsonNode[CONST_INI_TYPES_STRING];
            string typestring = types[contentid];
            if (String.IsNullOrWhiteSpace(typestring))
            {
                e.Cancel = true;
                return;
            }
            var type = Type.GetType(typestring,
                throwOnError: false,
                ignoreCase: false,
                assemblyResolver: (assemblyName) =>
                {
                    var plugin = PluginManager.Instance.Plugins.FirstOrDefault((it) => it.Assembly.FullName == assemblyName.FullName);
                    return plugin.Assembly ?? Type.GetType(typestring, throwOnError: false, ignoreCase: false)?.Assembly;
                },
                typeResolver: (assembly, typename, caseSensitiveSearch) => assembly.GetType(typename, false, caseSensitiveSearch));
            if (type == null || !typeof(DockableBase).IsAssignableFrom(type))
            {
                MessageBox.Show(
                    String.Format(Properties.Language.MainWindow_FailedToLoad_Body_0typestring, typestring),
                    Properties.Language.MainWindow_FailedToLoad_Caption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                e.Cancel = true;
                return;
            }
            DockableBase dockable;
            try
            {
                dockable = Activator.CreateInstance(type, true) as DockableBase;
                e.Content = dockable;
                dockable.ContentId = e.Model.ContentId;
                bool close = false;
                void OnDockableClose(object sender, EventArgs e)
                {
                    close = true;
                }
                try
                {
                    dockable.OnDockableClose += OnDockableClose;
                    dockable.LayoutLoadCallback(this.LayoutJsonNode[contentid]);
                }
                finally
                {
                    dockable.OnDockableClose -= OnDockableClose;
                }
                if (close)
                {
                    e.Cancel = true;
                    return;
                }
            }
            catch
            {
                MessageBox.Show(
                    String.Format(Properties.Language.MainWindow_FailedToLoad_Body_0typestring, typestring),
                    Properties.Language.MainWindow_FailedToLoad_Caption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                e.Cancel = true;
                return;
            }

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
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(App.LayoutConfigFilePath)))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(App.LayoutConfigFilePath));
            }
            if (!System.IO.File.Exists(App.LayoutConfigFilePath))
            {
                System.IO.File.WriteAllText(App.LayoutConfigFilePath, @"{
  ""Types"": {
    ""9a8ff5b9-8c6c-4d4e-8666-0ed9ef0b549e"": ""Arma.Studio.OutputWindow.OutputWindowDataContext, Arma.Studio.OutputWindow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"",
    ""bbf6042f-4675-48d9-97cf-7396bf45d407"": ""Arma.Studio.CallstackWindow.CallstackWindowDataContext, Arma.Studio.CallstackWindow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"",
    ""8327bf63-354e-4de4-b70b-c6ff2f6e2a43"": ""Arma.Studio.LocalsWindow.LocalsWindowDataContext, Arma.Studio.LocalsWindow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"",
    ""bcf81bff-3d6c-4d33-8464-3351484011ff"": ""Arma.Studio.SolutionExplorer.SolutionExplorerDataContext, Arma.Studio.SolutionExplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"",
    ""4cee9c6a-c41f-472f-a9d0-883c260545ea"": ""Arma.Studio.ImmediateWindow.ImmediateWindowDataContext, Arma.Studio.ImmediateWindow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null""
  },
  ""PBO-Paths"": [],
  ""9a8ff5b9-8c6c-4d4e-8666-0ed9ef0b549e"": {},
  ""bbf6042f-4675-48d9-97cf-7396bf45d407"": {},
  ""8327bf63-354e-4de4-b70b-c6ff2f6e2a43"": {},
  ""bcf81bff-3d6c-4d33-8464-3351484011ff"": {},
  ""4cee9c6a-c41f-472f-a9d0-883c260545ea"": {}
}");
            }
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(App.LayoutFilePath)))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(App.LayoutFilePath));
            }
            if (!System.IO.File.Exists(App.LayoutFilePath))
            {
                System.IO.File.WriteAllText(App.LayoutFilePath, @"<?xml version=""1.0""?>
<LayoutRoot>
  <RootPanel Orientation=""Horizontal"">
    <LayoutPanel Orientation=""Vertical"">
      <LayoutPanel Orientation=""Horizontal"" DockHeight=""25*"">
        <LayoutPanel Orientation=""Horizontal"">
          <LayoutDocumentPane />
        </LayoutPanel>
      </LayoutPanel>
      <LayoutAnchorablePaneGroup Orientation=""Horizontal"" DockWidth=""200"" DockHeight=""221"" FloatingWidth=""210"" FloatingHeight=""974"" FloatingLeft=""952"" FloatingTop=""1014"">
        <LayoutAnchorablePane DockHeight=""221"" FloatingWidth=""210"" FloatingHeight=""974"" FloatingLeft=""823"" FloatingTop=""922"">
          <LayoutAnchorable AutoHideMinWidth=""100"" AutoHideMinHeight=""100"" Title=""Output Window"" IsSelected=""True"" ContentId=""9a8ff5b9-8c6c-4d4e-8666-0ed9ef0b549e"" FloatingLeft=""823"" FloatingTop=""922"" FloatingWidth=""210"" FloatingHeight=""974"" CanClose=""False"" LastActivationTimeStamp=""02/20/2020 00:59:29"" PreviousContainerId=""4cdc16d9-4d60-4f82-b06e-e9cc2928346b"" PreviousContainerIndex=""2"" />
        </LayoutAnchorablePane>
        <LayoutAnchorablePane Id=""4cdc16d9-4d60-4f82-b06e-e9cc2928346b"" DockHeight=""221"" FloatingWidth=""210"" FloatingHeight=""974"" FloatingLeft=""952"" FloatingTop=""1014"">
          <LayoutAnchorable AutoHideMinWidth=""100"" AutoHideMinHeight=""100"" Title=""Callstack"" IsSelected=""True"" ContentId=""bbf6042f-4675-48d9-97cf-7396bf45d407"" FloatingLeft=""952"" FloatingTop=""1014"" FloatingWidth=""210"" FloatingHeight=""974"" CanClose=""False"" LastActivationTimeStamp=""02/20/2020 00:59:43"" />
          <LayoutAnchorable AutoHideMinWidth=""100"" AutoHideMinHeight=""100"" Title=""Locals"" ContentId=""8327bf63-354e-4de4-b70b-c6ff2f6e2a43"" FloatingLeft=""952"" FloatingTop=""1014"" FloatingWidth=""210"" FloatingHeight=""974"" CanClose=""False"" LastActivationTimeStamp=""02/20/2020 00:59:41"" />
          <LayoutAnchorable AutoHideMinWidth=""100"" AutoHideMinHeight=""100"" Title=""Immediate Window"" ContentId=""4cee9c6a-c41f-472f-a9d0-883c260545ea"" FloatingLeft=""772"" FloatingTop=""543"" FloatingWidth=""210"" FloatingHeight=""974"" CanClose=""False"" LastActivationTimeStamp=""02/20/2020 00:59:40"" PreviousContainerId=""a3aadb84-4241-4b01-b085-396cce19b466"" PreviousContainerIndex=""1"" />
        </LayoutAnchorablePane>
      </LayoutAnchorablePaneGroup>
    </LayoutPanel>
    <LayoutAnchorablePane Id=""a3aadb84-4241-4b01-b085-396cce19b466"" DockWidth=""198"" DockHeight=""482"" FloatingWidth=""210"" FloatingHeight=""974"" FloatingLeft=""1885"" FloatingTop=""547"">
      <LayoutAnchorable AutoHideMinWidth=""100"" AutoHideMinHeight=""100"" Title=""Solution-Explorer"" IsSelected=""True"" ContentId=""bcf81bff-3d6c-4d33-8464-3351484011ff"" FloatingLeft=""1885"" FloatingTop=""547"" FloatingWidth=""210"" FloatingHeight=""974"" CanClose=""False"" LastActivationTimeStamp=""02/20/2020 00:59:37"" PreviousContainerId=""4cdc16d9-4d60-4f82-b06e-e9cc2928346b"" PreviousContainerIndex=""3"" />
    </LayoutAnchorablePane>
  </RootPanel>
  <TopSide />
  <RightSide />
  <LeftSide />
  <BottomSide />
  <FloatingWindows />
  <Hidden />
</LayoutRoot>");
            }
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
            if (this.LayoutJsonNode.ContainsKey(CONST_INI_PBOPATHS_STRING))
            {
                var jarray = (Newtonsoft.Json.Linq.JArray)this.LayoutJsonNode[CONST_INI_PBOPATHS_STRING];
                var pboPaths = jarray.Values<string>();
                foreach (string pboPath in pboPaths)
                {
                    var pbo = new PBO { Name = pboPath };
                    pbo.Rescan();
                    this.Solution.Add(pbo);
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
            string dir = System.IO.Path.GetDirectoryName(App.LayoutFilePath);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            using (var writer = System.IO.File.Open(App.LayoutFilePath, System.IO.FileMode.Create))
            {
                var layoutSerializer = new Xceed.Wpf.AvalonDock.Layout.Serialization.XmlLayoutSerializer(this.WindowsDockingManager);
                layoutSerializer.Serialize(writer);
            }
            this.LayoutJsonNode = new Newtonsoft.Json.Linq.JObject
            {
                new Newtonsoft.Json.Linq.JProperty(CONST_INI_TYPES_STRING, new Newtonsoft.Json.Linq.JObject()),
                { CONST_INI_PBOPATHS_STRING, new Newtonsoft.Json.Linq.JArray(this.Solution.Where((it)=> it is PBO).Cast<PBO>().Select((pbo) => pbo.FullPath)) }
            };
            var types = this.LayoutJsonNode[CONST_INI_TYPES_STRING];

            foreach (var it in this.Anchorables.Concat(this.Documents))
            {
                var token = types.GetOrCreateProperty(it.ContentId);
                types[it.ContentId] = new Newtonsoft.Json.Linq.JValue(it.GetType().AssemblyQualifiedName);
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

        public HotkeyManager HotkeyManager { get; }
        public MainWindowDataContext()
        {
            this.LayoutItemTemplateSelector = new GenericDataTemplateSelector();
            this.Anchorables = new ObservableCollection<DockableBase>();
            this.Documents = new ObservableCollection<DockableBase>();
            this.EditorsAvailable = new ObservableCollection<EditorInfo>();
            this.TextEditorsAvailable = new ObservableCollection<TextEditorInfo>();
            this.AnchorablesAvailable = new ObservableCollection<DockableInfo>();
            this.DocumentsAvailable = new ObservableCollection<DockableInfo>();
            this.Solution = new Solution();
            this.LayoutJsonNode = new Newtonsoft.Json.Linq.JObject(new Newtonsoft.Json.Linq.JProperty(CONST_INI_TYPES_STRING, new Newtonsoft.Json.Linq.JObject()));
            this.BusyContainerManager = new BusyContainerManager();
            this.HotkeyManager = new HotkeyManager();
            App.Current.Dispatcher.Invoke(() => InputManager.Current.PreProcessInput += this.InputManager_PreProcessInput);
        }
        private void Initialized()
        {
            this.LayoutItemTemplateSelector.AddAllDataTemplatesInAssembly(typeof(MainWindowDataContext).Assembly, (s) => s.StartsWith("Arma.Studio.UI"));
            foreach (var it in PluginManager.Instance.GetPlugins<IDockableProvider>())
            {
                it.AddDataTemplates(this.LayoutItemTemplateSelector);
                this.AnchorablesAvailable.AddRange(it.Dockables.Where((di) => di.IsAnchorable));
                this.DocumentsAvailable.AddRange(it.Dockables.Where((di) => di.IsDocument));
            }
        }
        private void Dockable_OnDocumentClosing(object sender, EventArgs e)
        {
            var dockable = sender as DockableBase;
            dockable.OnDockableClose -= this.Dockable_OnDocumentClosing;
            bool wasActive = dockable.IsActive;
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
            if (wasActive && this.AvalonDockActiveContent == dockable)
            {
                this.AvalonDockActiveContent = this.Documents.FirstOrDefault((it) => it.IsActive);
                if (this.AvalonDockActiveContent is null)
                {
                    this.AvalonDockActiveContent = dockable = this.Documents.FirstOrDefault();
                    dockable?.Focus();
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.Solution.Dispose();
                    this.Debugger?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
        #endregion
        #region Key Handling
        private readonly Dictionary<Key, bool> InputManager_PreprocessInput_PressedKeys = new Dictionary<Key, bool>();
        private void InputManager_PreProcessInput(object sender, PreProcessInputEventArgs e)
        {
            var inputEvent = e.StagingItem.Input;

            if (inputEvent is KeyEventArgs eventArgs)
            {
                if (eventArgs.IsUp)
                {
                    this.InputManager_PreprocessInput_PressedKeys[eventArgs.Key] = false;
                    return;
                }
                if (this.InputManager_PreprocessInput_PressedKeys.TryGetValue(eventArgs.Key, out bool flag) && flag)
                {
                    return;
                }
                else
                {
                    this.InputManager_PreprocessInput_PressedKeys[eventArgs.Key] = true;
                }
                var kfocused = Keyboard.FocusedElement;
                if ((kfocused is System.Windows.Controls.TextBox || kfocused is ICSharpCode.AvalonEdit.Editing.TextArea)
                    && (eventArgs.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || eventArgs.KeyboardDevice.IsKeyDown(Key.RightCtrl))
                    && (eventArgs.Key == Key.C || eventArgs.Key == Key.V || eventArgs.Key == Key.X))
                {
                    return;
                }
                if (!(this.KeyInteractible?.KeyDown(eventArgs) ?? false) && !(this.ActiveDockable is IKeyInteractible keyInteractible && keyInteractible.KeyDown(eventArgs)))
                {
                    eventArgs.Handled = this.HotkeyManager.KeyDown(eventArgs);
                }
            }
        }
        #endregion
        #region DllImport
        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);
        [DllImport("user32.dll")]
        private static extern bool GetKeyboardState(byte[] lpKeyState);
        [DllImport("user32.dll")]
        private static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);
        public enum EMapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }
        #endregion
    }
}

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Reflection;
using Xceed.Wpf.AvalonDock;
using Utility;
using Utility.Collections;
using Xceed.Wpf.AvalonDock.Layout;
using RealVirtuality.SQF;
using ArmA.Studio.Data.UI;
using ArmA.Studio.Data.UI.Commands;
using ArmA.Studio.Data;
using ArmA.Studio.Plugin;

namespace ArmA.Studio
{
    public sealed class Workspace : INotifyPropertyChanged
    {
        public const string CONST_DOCKING_MANAGER_LAYOUT_NAME = "docklayout.xml";

        public enum ECreateDocumentModes
        {
            CreateTemporary,
            AddFileReferenceOrTemporary,
            AddOrCreateFileReference,
            AddFileReferenceOrNull
        }

        public static Workspace Instance { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        #region Updating window properties
        public double WindowWidth { get { return this._WindowWidth; } set { this._WindowWidth = value; ConfigHost.App.WindowWidth = value; this.RaisePropertyChanged(); } }
        private double _WindowWidth;

        public double WindowHeight { get { return this._WindowHeight; } set { this._WindowHeight = value; ConfigHost.App.WindowHeight = value; this.RaisePropertyChanged(); } }
        private double _WindowHeight;

        public double WindowTop { get { return this._WindowTop; } set { this._WindowTop = value; ConfigHost.App.WindowTop = value; this.RaisePropertyChanged(); } }
        private double _WindowTop;

        public double WindowLeft { get { return this._WindowLeft; } set { this._WindowLeft = value; ConfigHost.App.WindowLeft = value; this.RaisePropertyChanged(); } }
        private double _WindowLeft;

        public WindowState WindowCurrentState { get { return this._WindowCurrentState; } set { this._WindowCurrentState = value; ConfigHost.App.WindowCurrentState = value; this.RaisePropertyChanged(); } }
        private WindowState _WindowCurrentState;
        #endregion
        #region Commands
        public ICommand CmdDisplayPanel => new RelayCommand((p) =>
        {
            if (p is PanelBase)
            {
                var pb = p as PanelBase;
                if (this.AvalonDockPanels.Contains(p))
                {
                    pb.CurrentVisibility = pb.CurrentVisibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
                }
                else
                {
                    this.AvalonDockPanels.Add(pb);
                }
            }
        });
        public ICommand CmdDisplayLicensesDialog => new RelayCommand((p) =>
        {
            var dlg = new Dialogs.LicenseViewer();
            var dlgResult = dlg.ShowDialog();
        });
        public ICommand CmdDisplayAboutDialog => new RelayCommand((p) =>
        {
            var dlg = new Dialogs.AboutDialog();
            var dlgResult = dlg.ShowDialog();
        });
        public ICommand CmdDockingManagerInitialized => new RelayCommand((p) => this.OnAvalonDockingManagerInitialized(p as DockingManager));
        public ICommand CmdMainWindowClosing => new RelayCommand((p) =>
        {
            SaveLayout(this.AvalonDockDockingManager, Path.Combine(App.ConfigPath, CONST_DOCKING_MANAGER_LAYOUT_NAME));
            foreach (var panel in this.AvailablePanels)
            {
                var iniName = panel.GetType().FullName;
                if (!ConfigHost.Instance.LayoutIni.Sections.ContainsSection(iniName))
                    ConfigHost.Instance.LayoutIni.Sections.AddSection(iniName);
                var section = ConfigHost.Instance.LayoutIni[iniName];
                section["ContentId"] = panel.ContentId;
                section["IsSelected"] = panel.IsSelected.ToString();
            }
            this.CmdSaveAll.Execute(null);

            App.Current.Shutdown((int)App.ExitCodes.OK);
        });
        public ICommand CmdSwitchWorkspace => new RelayCommand((p) =>
        {
            var newWorkspace = Dialogs.WorkspaceSelectorDialog.GetWorkspacePath(this.PathUri.AbsolutePath);
            if (!string.IsNullOrWhiteSpace(newWorkspace))
            {
                this.CmdSaveAll.Execute(null);
                ConfigHost.App.WorkspacePath = newWorkspace;
                App.Shutdown(App.ExitCodes.Restart);
            }
        });
        public ICommand CmdShowProperties => new RelayCommand((p) =>
        {
            var dlgDc = new Dialogs.PropertiesDialogDataContext();
            var dlg = new Dialogs.PropertiesDialog(dlgDc);
            dlg.ShowDialog();
            if (dlgDc.RestartRequired)
            {
                var msgResult = MessageBox.Show(Properties.Localization.ChangesRequireRestart_Body, Properties.Localization.ChangesRequireRestart_Title, MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (msgResult == MessageBoxResult.Yes)
                {
                    App.Shutdown(App.ExitCodes.Restart);
                }
            }
        });
        public ICommand CmdQuit => new RelayCommand((p) => { App.Current.MainWindow.Close(); });
        public ICommand CmdSave => new RelayCommand((p) =>
        {
            var doc = this.GetCurrentDocument();
            if (doc != null && doc.HasChanges)
            {
                doc.SaveDocument();
            }
        });
        public ICommand CmdSaveAll => new RelayCommand((p) =>
        {
            try
            {
                foreach (var doc in this.AvalonDockDocuments)
                {
                    if (doc == null)
                        continue;
                    if (doc.HasChanges)
                    {
                        doc.SaveDocument();
                    }
                }
                using (var stream = File.Open(this.Solution.FileUri.AbsolutePath, FileMode.Create))
                {
                    Solution.Serialize(this.Solution, stream);
                }
                this.SaveBreakpointsToProject();
                ConfigHost.Instance.ExecSave();
            }
            catch (Exception ex)
            {
                App.ShowOperationFailedMessageBox(ex);
            }
        });
        public ICommand CmdActiveContentChanged => new RelayCommand((p) =>
        {
            var dm = p as DockingManager;
            this.CurrentContent = dm.ActiveContent as DockableBase;
        });
        #endregion
        #region Updating Properties
        public ObservableCollection<PanelBase> AvailablePanels
        {
            get { return this._AvailablePanels; }
            set { this._AvailablePanels = value; this.RaisePropertyChanged(); }
        }

        private ObservableCollection<PanelBase> _AvailablePanels;

        public DockableBase CurrentContent
        {
            get { return this._CurrentContent; }
            set { if (this._CurrentContent == value) return; this._CurrentContent = value; this.RaisePropertyChanged(); }
        }
        private DockableBase _CurrentContent;
        #endregion
        #region AvalonDock Layout handling
        public ObservableCollection<PanelBase> AvalonDockPanels { get { return this._AvalonDockPanels; } set { this._AvalonDockPanels = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<PanelBase> _AvalonDockPanels;

        public ObservableCollection<DocumentBase> AvalonDockDocuments { get { return this._AvalonDockDocuments; } set { this._AvalonDockDocuments = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<DocumentBase> _AvalonDockDocuments;

        private DockingManager AvalonDockDockingManager;

        private void OnAvalonDockingManagerInitialized(DockingManager dockingManager)
        {
            this.AvalonDockDockingManager = dockingManager;

            foreach (var panel in this.AvailablePanels)
            {
                var iniName = panel.GetType().FullName;
                if (ConfigHost.Instance.LayoutIni.Sections.ContainsSection(iniName))
                {
                    var section = ConfigHost.Instance.LayoutIni[iniName];
                    if (section.ContainsKey("ContentId"))
                    {
                        panel.ContentId = section["ContentId"];
                    }
                    if (section.ContainsKey("IsSelected"))
                    {
                        bool isSelectedFlag;
                        bool.TryParse(section["IsSelected"], out isSelectedFlag);
                        panel.IsSelected = isSelectedFlag;
                    }
                }
            }

            Workspace.LoadLayout(dockingManager, Path.Combine(App.ConfigPath, CONST_DOCKING_MANAGER_LAYOUT_NAME));
        }


        private static void LoadLayout(DockingManager dm, string v)
        {
            if (!File.Exists(v))
            {
                return;
            }
            try
            {
                using (var reader = File.OpenRead(v))
                {
                    var layoutSerializer = new Xceed.Wpf.AvalonDock.Layout.Serialization.XmlLayoutSerializer(dm);
                    layoutSerializer.LayoutSerializationCallback += LayoutSerializer_LayoutSerializationCallback;
                    layoutSerializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "docklayout.xml");
            }
        }

        private static void LayoutSerializer_LayoutSerializationCallback(object sender, Xceed.Wpf.AvalonDock.Layout.Serialization.LayoutSerializationCallbackEventArgs e)
        {
            if (e.Model is LayoutAnchorable)
            {
                foreach (var panel in Instance.AvailablePanels)
                {
                    if (panel.ContentId != e.Model.ContentId)
                        continue;
                    e.Content = panel;
                    Instance.AvalonDockPanels.Add(panel);
                    break;
                }
            }
            else if (e.Model is LayoutDocument)
            {
                var doc = Instance.CreateDocument(new Uri(e.Model.ContentId, UriKind.RelativeOrAbsolute), ECreateDocumentModes.AddFileReferenceOrNull);
                if (doc == null)
                {
                    e.Cancel = true;
                    return;
                }
                e.Content = doc;
                Instance.AvalonDockDocuments.Add(doc);
            }
        }

        private static void SaveLayout(DockingManager dm, string v)
        {
            var dir = Path.GetDirectoryName(v);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (var writer = File.Open(v, FileMode.Create))
            {
                var layoutSerializer = new Xceed.Wpf.AvalonDock.Layout.Serialization.XmlLayoutSerializer(dm);
                layoutSerializer.Serialize(writer);
            }
        }

        #endregion

        public Uri PathUri { get; internal set; }
        public Solution Solution { get; internal set; }
        public BreakpointManager BreakpointManager { get; internal set; }
        public string ConfigPath { get { return Path.Combine(PathUri.AbsolutePath, App.CONST_CONFIGURATION); } }
        public UI.GenericDataTemplateSelector LayoutItemTemplateSelector { get; private set; }
        public KeyManager KeyManager { get; private set; }
        public DebuggerContext DebugContext { get; internal set; }

        public Workspace(UI.GenericDataTemplateSelector layoutItemTemplateSelector)
        {
            Instance = this;
            this.KeyManager = new KeyManager();
            this.BreakpointManager = new BreakpointManager();
            foreach (var it in App.GetPlugins<IHotKeyPlugin>())
            {
                foreach(var kc in it.GetGlobalHotKeys())
                {
                    this.KeyManager.RegisterKey(kc);
                }
            }

            this.LayoutItemTemplateSelector = layoutItemTemplateSelector;
            this.AvailablePanels = new ObservableCollection<PanelBase>();
            this.AvalonDockDocuments = new ObservableCollection<DocumentBase>();
            this.AvalonDockPanels = new ObservableCollection<PanelBase>();

            this.DebugContext = new DebuggerContext();

            foreach (var t in Assembly.GetExecutingAssembly().DefinedTypes)
            {
                if (typeof(PanelBase).IsAssignableFrom(t))
                {
                    var instance = Activator.CreateInstance(t, true) as PanelBase;
                    this.AvailablePanels.Add(instance);
                }
            }


            const double DEF_WIN_HEIGHT = 512;
            const double DEF_WIN_WIDTH = 1024;
            //Load values
            this._WindowHeight = ConfigHost.App.WindowHeight;
            this._WindowWidth = ConfigHost.App.WindowWidth;
            this._WindowLeft = ConfigHost.App.WindowLeft;
            this._WindowTop = ConfigHost.App.WindowTop;
            this._WindowCurrentState = ConfigHost.App.WindowCurrentState;
            if (this._WindowHeight < 0)
            {
                this._WindowHeight = DEF_WIN_HEIGHT;
            }
            if (this._WindowWidth < 0)
            {
                this._WindowWidth = DEF_WIN_WIDTH;
            }
            if (this._WindowLeft < 0)
            {
                this._WindowLeft = (SystemParameters.PrimaryScreenWidth - DEF_WIN_WIDTH) / 2;
            }
            if (this._WindowTop < 0)
            {
                this._WindowTop = (SystemParameters.PrimaryScreenHeight - DEF_WIN_HEIGHT) / 2;
            }
        }

        public ProjectFile GetProjectFileFolderReference(Uri path)
        {
            foreach (var p in this.Solution.Projects)
            {
                if (!p.FileUri.IsBaseOf(path))
                    continue;
                foreach (var pff in p)
                {
                    if (pff.FileUri.Equals(path))
                    {
                        return pff;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Focuses an existing document or creates a new one if non is yet existing.
        /// </summary>
        /// <param name="fileReference">reference to the file used.</param>
        /// <returns>The document created/focused.</returns>
        public DocumentBase CreateOrFocusDocument(ProjectFile fileReference) => this.CreateOrFocusDocument(fileReference.FileUri);
        /// <summary>
        /// Focuses an existing document or creates a new one if non is yet existing.
        /// </summary>
        /// <param name="filePath">Path to the file to open.</param>
        /// <returns>The document created/focused.</returns>
        public DocumentBase CreateOrFocusDocument(string filePath) => this.CreateOrFocusDocument(new Uri(filePath, UriKind.RelativeOrAbsolute));
        /// <summary>
        /// Focuses an existing document or creates a new one if non is yet existing.
        /// </summary>
        /// <param name="path">Path to the file to open.</param>
        /// <returns>The document created/focused.</returns>
        public DocumentBase CreateOrFocusDocument(Uri path)
        {
            foreach (var doc in this.AvalonDockDocuments)
            {
                if (doc == null)
                    continue;
                if (doc.FileReference.FileUri.Equals(path))
                {
                    doc.IsSelected = true;
                    return doc;
                }
            }
            var tmp = this.CreateDocument(path, ECreateDocumentModes.AddFileReferenceOrTemporary);
            this.AvalonDockDocuments.Add(tmp);
            tmp.IsSelected = true;
            return tmp;
        }

        /// <summary>
        /// Creates a new <see cref="DocumentBase"/>.
        /// Could return null if creation of the <see cref="DocumentBase"/> fails.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="mode">file creation mode which determines some parameters for the file.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">If <see cref="ECreateDocumentModes.AddOrCreateFileReference"/> fails to find a project at the URI.</exception>
        public DocumentBase CreateDocument(Uri path, ECreateDocumentModes mode)
        {
            DocumentBase doc;
            try
            {
                doc = this.CreateNewDocument(path);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
            var reference = this.GetProjectFileFolderReference(path);
            switch (mode)
            {
                case ECreateDocumentModes.AddFileReferenceOrNull:
                    {
                        if (reference == null)
                            return null;
                        doc.FileReference = reference;
                        doc.LoadDocument();
                        return doc;
                    }

                case ECreateDocumentModes.AddFileReferenceOrTemporary:
                    {
                        if (reference == null)
                        {
                            doc.IsTemporary = true;
                        }
                        else
                        {
                            doc.FileReference = reference;
                        }
                        doc.LoadDocument();
                        return doc;
                    }

                case ECreateDocumentModes.AddOrCreateFileReference:
                    {
                        if (reference == null)
                        {
                            foreach (var p in this.Solution.Projects)
                            {
                                if (p.FileUri.IsBaseOf(path))
                                {
                                    var pff = p.GetOrCreateFileFolder(path);
                                    doc.FileReference = pff;
                                    doc.LoadDocument();
                                    return doc;
                                }
                            }
                            throw new KeyNotFoundException(nameof(ECreateDocumentModes.AddOrCreateFileReference));
                        }
                        else
                        {
                            doc.FileReference = reference;
                            doc.LoadDocument();
                            return doc;
                        }
                    }

                case ECreateDocumentModes.CreateTemporary:
                    {
                        doc.IsTemporary = true;
                        doc.LoadDocument();
                        return doc;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        public DocumentBase CreateTemporaryDocument(string content, string extension) => this.CreateTemporaryDocument(content, this.GetFileTypeFromExtension(extension));
        public DocumentBase CreateTemporaryDocument(string content, FileType fileType)
        {
            var doc = App.GetPlugins<IDocumentProviderPlugin>().CreateDocument(fileType);
            if(doc is TextEditorBaseDataContext)
            {
                (doc as TextEditorBaseDataContext).SetText(content);
            }
            else
            {
                throw new Exception("Cannot set text in non-TextEditorBaseDataContext classes");
            }
            doc.IsTemporary = true;
            doc.KeyManager = this.KeyManager;
            return doc;
        }

        /// <summary>
        /// Returns the corresponding <see cref="FileType"/> for given file extension.
        /// extension has to be prefixed by a dot.
        /// </summary>
        /// <param name="ext">File extension prefixed by dot.</param>
        /// <returns>The corresponding <see cref="FileType"/> or null.</returns>
        public FileType GetFileTypeFromExtension(string ext)
        {
            return App.GetPlugins<IDocumentProviderPlugin>().FirstOrDefault((prov) => prov.FileTypes.Any((ft) => ft.IsFileTypeCondition(ext)))?.FileTypes.First((ft) => ft.IsFileTypeCondition(ext));
        }

        /// <summary>
        /// Adds provided <see cref="DocumentBase"/> to the displayed documents.
        /// If already added, <see cref="DocumentBase"/> will be focused.
        /// </summary>
        /// <param name="doc">The document to focus/display</param>
        public void DisplayDocument(DocumentBase doc)
        {
            if (this.AvalonDockDocuments.Contains(doc))
            {
                this.AvalonDockDocuments.Add(doc);
            }
            doc.IsActive = true;
        }



        /// <summary>
        /// Tries to find a <see cref="FileType"/> from provided path.
        /// If <see cref="Uri"/> is not found, null will be returned.
        /// </summary>
        /// <param name="path">Path to the document.</param>
        /// <returns>Correct <see cref="FileType"/> or null.</returns>
        public FileType GetFileType(Uri path)
        {
            foreach (var p in App.GetPlugins<IDocumentProviderPlugin>())
            {
                var fileType = p.FileTypes.FirstOrDefault((ft) => ft.IsFileType(path));
                if (fileType != null)
                {
                    return fileType;
                }
            }
            return null;
        }
        /// <summary>
        /// User will be prompted to select a <see cref="DocumentBase.DocumentDescribor"/>.
        /// In case he aborts, exception will be thrown.
        /// </summary>
        /// <returns>The <see cref="DocumentBase.DocumentDescribor"/> the user selected.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when user aborted the selection dialog.</exception>
        public DocumentBase.DocumentDescribor GetDocumentDescriborByPrompt()
        {
            DocumentBase.DocumentDescribor descr = null;
            App.Current.Dispatcher.Invoke(() =>
            {
                var dlgdc = new Dialogs.DocumentSelectorDialogDataContext();
                var dlg = new Dialogs.DocumentSelectorDialog(dlgdc);
                var dlgResult = dlg.ShowDialog();
                if (dlgResult.HasValue && dlgResult.Value)
                {
                    descr = dlgdc.SelectedItem as DocumentBase.DocumentDescribor;
                }
            });
            if (descr != null)
                return descr;
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Creates a new document with provided describor.
        /// If describor is not found in any document providers, exception will be thrown.
        /// </summary>
        /// <param name="describor">Describor of some <see cref="DocumentBase"/>.</param>
        /// <returns>new <see cref="DocumentBase"/> instance.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when <see cref="DocumentBase.DocumentDescribor"/> is not found in any DocumentProvider</exception>
        private DocumentBase CreateNewDocument(DocumentBase.DocumentDescribor describor)
        {
            foreach (var p in App.GetPlugins<IDocumentProviderPlugin>())
            {
                if (p.Documents.Contains(describor))
                {
                    var doc = p.CreateDocument(describor);
                    doc.KeyManager = this.KeyManager;
                    doc.OnDocumentClosing += Document_OnDocumentClosing;
                    if(doc is CodeEditorBaseDataContext)
                    {
                        (doc as CodeEditorBaseDataContext).OnLintingInfoUpdated += Workspace_OnLintingInfoUpdated;
                    }
                    if (doc is TextEditorBaseDataContext)
                    {
                        (doc as TextEditorBaseDataContext).GetEditorInstanceAsync().ContinueWith((t) => App.Current.Dispatcher.Invoke(() => t.Result.TextArea.TextView.BackgroundRenderers.Add(new UI.LineHighlighterBackgroundRenderer(t.Result))));
                    }
                    return doc;
                }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Creates a new document with provided describor.
        /// If <see cref="Uri"/> is not found, the User will be prompted to select a <see cref="DocumentBase.DocumentDescribor"/>.
        /// In case he aborts, exception will be thrown.
        /// Should be executed on dialog thread.
        /// </summary>
        /// <param name="path">Path to the document.</param>
        /// <returns>new <see cref="DocumentBase"/> instance.</returns>
        private DocumentBase CreateNewDocument(Uri path)
        {
            var fileType = GetFileType(path);
            if (fileType == null)
            {
                var describor = this.GetDocumentDescriborByPrompt();
                var doc = App.GetPlugins<IDocumentProviderPlugin>().CreateDocument(describor);
                doc.KeyManager = this.KeyManager;
                doc.OnDocumentClosing += Document_OnDocumentClosing;
                if (doc is CodeEditorBaseDataContext)
                {
                    (doc as CodeEditorBaseDataContext).OnLintingInfoUpdated += Workspace_OnLintingInfoUpdated;
                }
                if (doc is TextEditorBaseDataContext)
                {
                    (doc as TextEditorBaseDataContext).GetEditorInstanceAsync().ContinueWith((t) => App.Current.Dispatcher.Invoke(() => t.Result.TextArea.TextView.BackgroundRenderers.Add(new UI.LineHighlighterBackgroundRenderer(t.Result))));
                }
                return doc;
            }
            else
            {
                var doc = App.GetPlugins<IDocumentProviderPlugin>().CreateDocument(fileType);
                doc.KeyManager = this.KeyManager;
                doc.OnDocumentClosing += Document_OnDocumentClosing;
                if (doc is CodeEditorBaseDataContext)
                {
                    (doc as CodeEditorBaseDataContext).OnLintingInfoUpdated += Workspace_OnLintingInfoUpdated;
                }
                if (doc is TextEditorBaseDataContext)
                {
                    (doc as TextEditorBaseDataContext).GetEditorInstanceAsync().ContinueWith((t) => App.Current.Dispatcher.Invoke(() => t.Result.TextArea.TextView.BackgroundRenderers.Add(new UI.LineHighlighterBackgroundRenderer(t.Result))));
                }
                return doc;
            }
        }

        private void Document_OnDocumentClosing(object sender, EventArgs e)
        {
            var doc = sender as DocumentBase;
            doc.OnDocumentClosing -= Document_OnDocumentClosing;
            if (doc is CodeEditorBaseDataContext)
            {
                (doc as CodeEditorBaseDataContext).OnLintingInfoUpdated -= Workspace_OnLintingInfoUpdated;
            }
            if (this.AvalonDockDocuments.Contains(doc))
            {
                try
                {
                    this.AvalonDockDocuments.Remove(doc);
                }
                catch (NullReferenceException) { } //AvalonDock ...
            }
        }
        private void Workspace_OnLintingInfoUpdated(object sender, EventArgs e)
        {
            var editor = sender as CodeEditorBaseDataContext;
            if (editor == null || editor.FileReference == null)
                return;
            DataContext.ErrorListPane.Instance.LinterDictionary[editor.FileReference.FilePath] = editor.Linter.LinterInfo;
        }

        public DocumentBase GetCurrentDocument()
        {
            if (this.CurrentContent is DocumentBase)
                return this.CurrentContent as DocumentBase;

            foreach (var doc in this.AvalonDockDocuments)
            {
                if (doc == null)
                    continue;
                if (doc.IsSelected)
                {
                    return doc;
                }
            }
            return null;
        }


        public void SaveBreakpointsToProject()
        {
            var filePath = Path.ChangeExtension(this.Solution.FileUri.AbsolutePath, App.CONST_BREAKPOINTINFOEXTENSION);
            using (var stream = File.Open(filePath, FileMode.Create))
            {
                this.BreakpointManager.SaveBreakpoints(stream);
            }
        }
    }
}
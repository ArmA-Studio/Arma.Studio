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
        public ICommand CmdDockingManagerInitialized => new RelayCommand((p) => this.OnAvalonDockingManagerInitialized(p as DockingManager));
        public ICommand CmdMainWindowClosing => new RelayCommand((p) =>
        {
            SaveLayout(this.AvalonDockDockingManager, System.IO.Path.Combine(App.ConfigPath, CONST_DOCKING_MANAGER_LAYOUT_NAME));
            App.Current.Shutdown((int)App.ExitCodes.OK);
        });
        public ICommand CmdSwitchWorkspace => new RelayCommand((p) =>
        {
            var newWorkspace = Dialogs.WorkspaceSelectorDialog.GetWorkspacePath(this.PathUri.AbsolutePath);
            if (!string.IsNullOrWhiteSpace(newWorkspace))
            {
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
        public ICommand CmdQuit => new RelayCommand((p) => { App.Shutdown(App.ExitCodes.OK); });
        public ICommand CmdSave => new RelayCommand((p) =>
        {
            var doc = this.GetCurrentDocument();
            if (doc != null && doc.HasChanges)
            {
                doc.SaveDocument(doc.FilePath);
            }
        });
        public ICommand CmdSaveAll => new RelayCommand((p) =>
        {
            try
            {
                foreach (var doc in this.AvalonDockDocuments)
                {
                    if (doc.HasChanges)
                    {
                        doc.SaveDocument(doc.FilePath);
                    }
                }
                using (var stream = File.OpenWrite(this.Solution.FileUri.AbsolutePath))
                {
                    Solution.Serialize(this.Solution, stream);
                }
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

        public Workspace(UI.GenericDataTemplateSelector layoutItemTemplateSelector)
        {
            this.LayoutItemTemplateSelector = layoutItemTemplateSelector;
            this.AvailablePanels = new ObservableCollection<PanelBase>();
            this.AvalonDockDocuments = new ObservableCollection<DocumentBase>();
            this.AvalonDockPanels = new ObservableCollection<PanelBase>();

            foreach (var t in Assembly.GetExecutingAssembly().DefinedTypes)
            {
                if (typeof(PanelBase).IsAssignableFrom(t))
                {
                    var instance = Activator.CreateInstance(t, true) as PanelBase;
                    this.AvailablePanels.Add(instance);
                }
            }
        }

        public ProjectFileFolder GetProjectFileFolderReference(Uri path)
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
                                    return doc;
                                }
                            }
                            throw new KeyNotFoundException(nameof(ECreateDocumentModes.AddOrCreateFileReference));
                        }
                        else
                        {
                            doc.FileReference = reference;
                        }
                        return doc;
                    }

                case ECreateDocumentModes.CreateTemporary:
                    {
                        return doc;
                    }

                default:
                    throw new NotImplementedException();
            }
        }
        public void DisplayDocument(DocumentBase doc)
        {
            if (!this.AvalonDockDocuments.Contains(doc))
            {
                this.AvalonDockDocuments.Add(doc);
            }
        }

        /// <summary>
        /// Creates a new document with provided describor.
        /// If <see cref="Uri"/> is not found, the User will be prompted to select a <see cref="DocumentBase.DocumentDescribor"/>.
        /// In case he aborts, exception will be thrown.
        /// Should be executed on dialog thread.
        /// </summary>
        /// <param name="path">Path to a document.</param>
        /// <returns>new <see cref="DocumentBase"/> instance.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when <see cref="Uri"/> is not found and user aborted the selection dialog.</exception>
        private DocumentBase CreateNewDocument(Uri path)
        {
            Plugin.IDocumentProviderPlugin provider = null;
            FileType fileType = null;
            foreach (var p in App.GetPlugins<Plugin.IDocumentProviderPlugin>())
            {
                fileType = p.FileTypes.FirstOrDefault((ft) => ft.IsFileType(path));
                if (fileType != null)
                {
                    provider = p;
                    break;
                }
            }
            if (provider == null)
            {
                var dlgdc = new Dialogs.DocumentSelectorDialogDataContext();
                var dlg = new Dialogs.DocumentSelectorDialog(dlgdc);
                var dlgResult = dlg.ShowDialog();
                if (dlgResult.HasValue && dlgResult.Value)
                {
                    return this.CreateNewDocument(dlgdc.SelectedItem as DocumentBase.DocumentDescribor);
                }
                throw new KeyNotFoundException();
            }
            else
            {
                return provider.CreateDocument(fileType);
            }
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
            foreach (var p in App.GetPlugins<Plugin.IDocumentProviderPlugin>())
            {
                if (p.Documents.Contains(describor))
                {
                    return p.CreateDocument(describor);
                }
            }

            throw new KeyNotFoundException();
        }

        public DocumentBase GetCurrentDocument()
        {
            if (this.CurrentContent is DocumentBase)
                return this.CurrentContent as DocumentBase;

            foreach (var doc in this.AvalonDockDocuments)
            {
                if (doc.IsSelected)
                {
                    return doc;
                }
            }
            return null;
        }

    }
}
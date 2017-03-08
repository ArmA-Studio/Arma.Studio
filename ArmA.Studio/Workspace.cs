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
using ArmA.Studio.UI.Commands;
using System.Reflection;
using Xceed.Wpf.AvalonDock;
using Utility;
using Utility.Collections;
using Xceed.Wpf.AvalonDock.Layout;
using RealVirtuality.SQF;

namespace ArmA.Studio
{
    public sealed class Workspace : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }
        public const string CONST_DOCKING_MANAGER_LAYOUT_NAME = "docklayout.xml";

        public double WindowWidth { get { return this._WindowWidth; } set { this._WindowWidth = value; ConfigHost.App.WindowWidth = value;  this.RaisePropertyChanged(); } }
        private double _WindowWidth;

        public double WindowHeight { get { return this._WindowHeight; } set { this._WindowHeight = value; ConfigHost.App.WindowHeight = value; this.RaisePropertyChanged(); } }
        private double _WindowHeight;

        public double WindowTop { get { return this._WindowTop; } set { this._WindowTop = value; ConfigHost.App.WindowTop = value; this.RaisePropertyChanged(); } }
        private double _WindowTop;

        public double WindowLeft { get { return this._WindowLeft; } set { this._WindowLeft = value; ConfigHost.App.WindowLeft = value; this.RaisePropertyChanged(); } }
        private double _WindowLeft;

        public WindowState WindowCurrentState { get { return this._WindowCurrentState; } set { this._WindowCurrentState = value; ConfigHost.App.WindowCurrentState = value; this.RaisePropertyChanged(); } }
        private WindowState _WindowCurrentState;





        public static Workspace CurrentWorkspace { get { return _CurrentWorkspace; } set { if (_CurrentWorkspace != null) _CurrentWorkspace.Close(); _CurrentWorkspace = value; value?.Open(); } }
        private static Workspace _CurrentWorkspace;

        public DebuggerContext DebugContext { get { return this._DebugContext; } set { this._DebugContext = value; this.RaisePropertyChanged(); } }
        private DebuggerContext _DebugContext;

        public SolutionUtil.Solution CurrentSolution { get { return this._CurrentSolution; } set { this._CurrentSolution = value; this.RaisePropertyChanged(); } }
        private SolutionUtil.Solution _CurrentSolution;
        public UI.ViewModel.IPropertyDatatemplateProvider CurrentSelectedProperty { get { return this._CurrentSelectedProperty; } set { this._CurrentSelectedProperty = value; this.RaisePropertyChanged(); } }
        public DataTemplate CurrentSelectedPropertyTemplate { get { return this._CurrentSelectedProperty?.PropertiesTemplate; } }
        private UI.ViewModel.IPropertyDatatemplateProvider _CurrentSelectedProperty;

        public ObservableCollection<PanelBase> PanelsDisplayed
        {
            get { return this._PanelsDisplayed; }
            set
            {
                if(this._PanelsDisplayed != null)
                    this._PanelsDisplayed.CollectionChanged -= PanelsDisplayed_CollectionChanged;
                this._PanelsDisplayed = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("CurrentSelectedPropertyTemplate");

                this._PanelsDisplayed.CollectionChanged += PanelsDisplayed_CollectionChanged;
            }
        }
        private ObservableCollection<PanelBase> _PanelsDisplayed;

        public ObservableCollection<PanelBase> PanelsAvailable { get { return this._PanelsAvailable; } set { this._PanelsAvailable = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<PanelBase> _PanelsAvailable;

        public ObservableCollection<DocumentBase> DocumentsDisplayed { get { return this._DocumentsDisplayed; } set { this._DocumentsDisplayed = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<DocumentBase> _DocumentsDisplayed;

        public ObservableCollection<DocumentBase> DocumentsAvailable { get { return this._DocumentsAvailable; } set { this._DocumentsAvailable = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<DocumentBase> _DocumentsAvailable;

        public ICommand CmdDisplayPanel { get; private set; }
        public ICommand CmdDisplayLicensesDialog { get; private set; }
        public ICommand CmdDockingManagerInitialized { get; private set; }
        public ICommand CmdMainWindowClosing { get; private set; }
        public ICommand CmdSwitchWorkspace { get; private set; }
        public ICommand CmdShowProperties { get; private set; }
        public ICommand CmdQuit { get; private set; }
        public ICommand CmdSave { get; private set; }
        public ICommand CmdSaveAll { get; private set; }

        public string WorkingDir { get; private set; }

        private DockingManager MWDockingManager;

        public DocumentBase GetDocumentOfSolutionFileBase(SolutionUtil.SolutionFileBase sfb)
        {
            foreach (var it in this.DocumentsDisplayed)
            {
                if (it.FilePath == sfb.FullPath)
                {
                    return it;
                }
            }
            return null;
        }

        public Workspace(string path)
        {
            this._DebugContext = new DebuggerContext();
            this.WorkingDir = path;
            this._PanelsAvailable = new ObservableCollection<PanelBase>(FindAllAnchorablePanelsInAssembly());
            this.PanelsDisplayed = new ObservableCollection<PanelBase>();
            this._DocumentsDisplayed = new ObservableCollection<DocumentBase>();
            this._DocumentsAvailable = new ObservableCollection<DocumentBase>(FindAllDocumentsInAssembly());
            this.CmdDisplayPanel = new RelayCommand((p) =>
            {
                if (p is PanelBase)
                {
                    var pb = p as PanelBase;
                    if (this.PanelsDisplayed.Contains(p))
                    {
                        pb.CurrentVisibility = pb.CurrentVisibility == System.Windows.Visibility.Visible ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        this.PanelsDisplayed.Add(pb);
                    }
                }
            });
            this.CmdDisplayLicensesDialog = new RelayCommand((p) =>
            {
                var dlg = new Dialogs.LicenseViewer();
                var dlgResult = dlg.ShowDialog();
            });
            this.CmdDockingManagerInitialized = new RelayCommand((p) => this.DockingMangerInitialized(p as DockingManager));
            this.CmdMainWindowClosing = new RelayCommand((p) =>
            {
                SaveLayout(this.MWDockingManager, Path.Combine(App.ConfigPath, CONST_DOCKING_MANAGER_LAYOUT_NAME));
                App.Current.Shutdown((int)App.ExitCodes.OK);
            });
            this.CmdSwitchWorkspace = new RelayCommand((p) => { if (App.SwitchWorkspace()) App.Shutdown(App.ExitCodes.Restart); });
            this.CmdShowProperties = new RelayCommand((p) =>
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
            this.CmdQuit = new RelayCommand((p) => { App.Shutdown(App.ExitCodes.OK); });
            this.CmdSave = new RelayCommand((p) =>
            {
                foreach (var doc in this.DocumentsDisplayed)
                {
                    if (doc.IsSelected)
                    {
                        if (!doc.HasChanges)
                            break;
                        doc.SaveDocument(doc.FilePath);
                        break;
                    }
                }
            });
            this.CmdSaveAll = new RelayCommand((p) =>
            {
                foreach (var doc in this.DocumentsDisplayed)
                {
                    if (doc.HasChanges)
                    {
                        doc.SaveDocument(doc.FilePath);
                    }
                }
                this.SaveSolution();
            });

            const double DEF_WIN_HEIGHT = 512;
            const double DEF_WIN_WIDTH = 1024;
            this._WindowHeight = DEF_WIN_HEIGHT;
            this._WindowWidth = DEF_WIN_WIDTH;
            this._WindowLeft = (SystemParameters.PrimaryScreenWidth - DEF_WIN_WIDTH) / 2;
            if (this._WindowLeft < 0)
            {
                this._WindowLeft = 0;
            }
            this._WindowTop = (SystemParameters.PrimaryScreenHeight - DEF_WIN_HEIGHT) / 2;
            if (this._WindowTop < 0)
            {
                this._WindowTop = 0;
            }
            this._WindowCurrentState = WindowState.Normal;

            
        }

        private void PanelsDisplayed_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("PanelsDisplayed");
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
                foreach (var panel in CurrentWorkspace.PanelsAvailable)
                {
                    if (panel.ContentId != e.Model.ContentId)
                        continue;
                    e.Content = panel;
                    CurrentWorkspace.PanelsDisplayed.Add(panel);
                    break;
                }
            }
            else if(e.Model is LayoutDocument)
            {
                var doc = CurrentWorkspace.GetNewDocument(e.Model.ContentId);
                e.Content = doc;
                CurrentWorkspace.DocumentsDisplayed.Add(doc);
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

        public void OpenOrFocusDocument(string path)
        {
            path = path.Trim('/', '\\');
            if (Path.IsPathRooted(path))
            {
                path = path.Substring(this.WorkingDir.Length + 1);
            }
            var fullPath = Path.Combine(this.WorkingDir, path);
            //Check if document is already open and select instead of open
            foreach (var doc in DocumentsDisplayed)
            {
                if(doc.FilePath == fullPath)
                {
                    doc.IsSelected = true;
                    return;
                }
            }

            var instance = this.GetNewDocument(path);
            this.DocumentsDisplayed.Add(instance);
            instance.IsSelected = true;
        }
        public DocumentBase GetNewDocument(string path)
        {
            path = path.Trim('/', '\\');
            if (Path.IsPathRooted(path))
            {
                path = path.Substring(this.WorkingDir.Length + 1);
            }
            var fullPath = Path.Combine(this.WorkingDir, path);
            Type docType = null;
            var fExt = Path.GetExtension(path);
            foreach (var doc in DocumentsAvailable)
            {
                if (doc.SupportedFileExtensions.Contains(fExt))
                {
                    docType = doc.GetType();
                }
            }
            if (docType == null)
            {
                //ToDo: Let user decide how to open this document
                MessageBox.Show("No matching editor context found. Selecting is not yet implemented.");
                return null;
            }
            var instance = Activator.CreateInstance(docType) as DocumentBase;
            instance.OpenDocument(fullPath);
            return instance;
        }

        private void DockingMangerInitialized(DockingManager dm)
        {
            if (dm == null)
                return;
            this.MWDockingManager = dm;
            LoadLayout(dm, Path.Combine(App.ConfigPath, CONST_DOCKING_MANAGER_LAYOUT_NAME));
        }

        private void Open()
        {
            var solutionFile = Directory.Exists(this.WorkingDir) ? Directory.EnumerateFiles(this.WorkingDir, "*.assln").FirstOrDefault() : string.Empty;
            if (!string.IsNullOrWhiteSpace(solutionFile))
            {
                try
                {
                    this.CurrentSolution = solutionFile.XmlDeserialize<SolutionUtil.Solution>();
                    this.CurrentSolution.RestoreFromXml();
                    this.CurrentSolution.Prepare(this);
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                        ex = ex.InnerException;
                    MessageBox.Show(string.Format(Properties.Localization.MessageBoxOperationFailed_Body, ex.Message, ex.GetType().FullName, ex.StackTrace), Properties.Localization.MessageBoxOperationFailed_Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.CurrentSolution = new SolutionUtil.Solution();
                    this.CurrentSolution.Prepare(this);
                    this.CurrentSolution.ReScan();
                }
            }
            else
            {
                //Create new solution as no existing is present
                this.CurrentSolution = new SolutionUtil.Solution();
                this.CurrentSolution.Prepare(this);
                this.CurrentSolution.ReScan();
            }
            this.PanelsAvailable.Add(this.CurrentSolution);

            foreach (var panel in this.PanelsAvailable)
            {
                var iniName = panel.GetType().Name;
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
            double d;
            d = ConfigHost.App.WindowWidth;
            if(d >= 0)
            {
                this.WindowWidth = d;
            }
            d = ConfigHost.App.WindowHeight;
            if (d >= 0)
            {
                this.WindowHeight = d;
            }
            d = ConfigHost.App.WindowLeft;
            if (d >= 0)
            {
                this.WindowLeft = d;
            }
            d = ConfigHost.App.WindowTop;
            if (d >= 0)
            {
                this.WindowTop = d;
            }
            this.WindowCurrentState = ConfigHost.App.WindowCurrentState;
        }

        private void Close()
        {
            //Save Layout GUIDs of the panels
            foreach (var panel in PanelsAvailable)
            {
                var iniName = panel.GetType().Name;
                if (!ConfigHost.Instance.LayoutIni.Sections.ContainsSection(iniName))
                    ConfigHost.Instance.LayoutIni.Sections.AddSection(iniName);
                var section = ConfigHost.Instance.LayoutIni[iniName];
                section["ContentId"] = panel.ContentId;
                section["IsSelected"] = panel.IsSelected.ToString();
            }
            this.SaveSolution();
        }

        public void SaveSolution()
        {
            var solutionFile = Directory.EnumerateFiles(this.WorkingDir, "*.assln").FirstOrDefault();
            this.CurrentSolution.XmlSerialize(Path.Combine(this.WorkingDir, solutionFile == null ? string.Concat(Path.GetFileName(this.WorkingDir), ".assln") : solutionFile));
        }

        private static IEnumerable<PanelBase> FindAllAnchorablePanelsInAssembly()
        {
            var list = new List<PanelBase>();
            foreach (var t in Assembly.GetExecutingAssembly().DefinedTypes)
            {
                if (!t.IsEquivalentTo(typeof(PanelBase)) && typeof(PanelBase).IsAssignableFrom(t))
                {
                    var instance = Activator.CreateInstance(t, true) as PanelBase;
                    if (instance.AutoAddPanel)
                        list.Add(instance);
                }
            }
            return list;
        }
        private static IEnumerable<DocumentBase> FindAllDocumentsInAssembly()
        {
            var list = new List<DocumentBase>();
            foreach (var t in Assembly.GetExecutingAssembly().DefinedTypes)
            {
                if (!t.IsEquivalentTo(typeof(DocumentBase)) && typeof(DocumentBase).IsAssignableFrom(t))
                {
                    var instance = Activator.CreateInstance(t) as DocumentBase;
                    list.Add(instance);
                }
            }
            return list;
        }
    }
}
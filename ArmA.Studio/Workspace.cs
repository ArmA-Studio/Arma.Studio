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

        #region Bindable window properties
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
            if(!string.IsNullOrWhiteSpace(newWorkspace))
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
            if(doc != null && doc.HasChanges)
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
        #region Bindable Properties
        public ObservableCollection<PanelBase> AvailablePanels
        {
            get { return this._AvailablePanels; }
            set { this._AvailablePanels = value; this.RaisePropertyChanged(); }
        }
        private ObservableCollection<PanelBase> _AvailablePanels;

        public ObservableCollection<DocumentBase> AvailableDocuments
        {
            get { return this._AvailableDocuments; }
            set { this._AvailableDocuments = value; this.RaisePropertyChanged(); }
        }
        private ObservableCollection<DocumentBase> _AvailableDocuments;

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
                if(doc == null)
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



        public DocumentBase CreateDocument(Uri path, ECreateDocumentModes mode)
        {
            throw new NotImplementedException();
        }
        private DocumentBase CreateNewDocument(Uri path)
        {
            foreach(var p in App.GetPlugins<Plugin.IDocumentProviderPlugin>())
            {
                
            }
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
using Arma.Studio.Data;
using Arma.Studio.Data.IO;
using Arma.Studio.Data.TextEditor;
using Arma.Studio.Data.UI;
using Arma.Studio.Data.UI.AttachedProperties;
using Arma.Studio.UiEditor.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Arma.Studio.UiEditor.UI
{
    public class UiEditorDataContext : DockableBase,
        IOnDragEnter,
        IOnDragLeave,
        IOnDragOver,
        IOnDrop,
        IEditorDocument
    {
        #region Collection: CanvasManager (CanvasManager)
        public CanvasManager CanvasManager
        {
            get => this._CanvasManager;
            set
            {
                if (this._CanvasManager == value)
                {
                    return;
                }
                this._CanvasManager = value;
                this.RaisePropertyChanged();
            }
        }
        private CanvasManager _CanvasManager;
        #endregion


        #region Property: IDD (System.Int32)
        /// <summary>
        /// The unique ID number of this dialog. can be -1 if you don't require access to the dialog itself from within a script.
        /// </summary>
        public int IDD
        {
            get => this._IDD;
            set
            {
                this._IDD = value;
                this.RaisePropertyChanged();
            }
        }
        private int _IDD;
        #endregion
        #region Property: EnableSimulation (System.Boolean)
        /// <summary>
        /// Specifies whether the game continues while the dialog is shown or not.
        /// </summary>
        public bool EnableSimulation
        {
            get => this._EnableSimulation;
            set
            {
                this._EnableSimulation = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _EnableSimulation;
        #endregion
        #region Collection: BackgroundControls (ObservableCollection<IControlElement>)
        public ObservableCollection<IControlElement> BackgroundControls
        {
            get => this._BackgroundControls;
            set
            {
                if (this._BackgroundControls == value)
                {
                    return;
                }
                this._BackgroundControls = value;
                this.RaisePropertyChanged();
            }
        }
        private ObservableCollection<IControlElement> _BackgroundControls;
        #endregion
        #region Collection: ForegroundControls (ObservableCollection<IControlElement>)
        public ObservableCollection<IControlElement> ForegroundControls
        {
            get => this._ForegroundControls;
            set
            {
                if (this._ForegroundControls == value)
                {
                    return;
                }
                this._ForegroundControls = value;
                this.RaisePropertyChanged();
            }
        }
        private ObservableCollection<IControlElement> _ForegroundControls;
        #endregion
        #region Property: InterfaceSize (InterfaceSize)
        /// <summary>
        /// Specifies whether the game continues while the dialog is shown or not.
        /// </summary>
        public InterfaceSize InterfaceSize
        {
            get => this._InterfaceSize;
            set
            {
                this._InterfaceSize = value;
                this.RaisePropertyChanged();
            }
        }
        private InterfaceSize _InterfaceSize;
        #endregion

        public UiEditorDataContext()
        {
            this.IDD = -1;
            this.EnableSimulation = true;
            this.BackgroundControls = new ObservableCollection<IControlElement>();
            this.ForegroundControls = new ObservableCollection<IControlElement>();
            this.CanvasManager = new CanvasManager(this);
            this.InterfaceSize = InterfaceSize.Small;
        }
        public UiEditorDataContext(File file) : this()
        {
            this.File = file;
        }
        public void OnDragEnter(UIElement sender, DragEventArgs e)
        {
            if (e.GetInfo().HasData<EditorToolboxItem>())
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }
        public void OnDragOver(UIElement sender, DragEventArgs e)
        {
            if (e.GetInfo().HasData<EditorToolboxItem>())
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }
        public void OnDrop(UIElement sender, DragEventArgs e)
        {
            var data = e.GetInfo().GetDataOrDefault<EditorToolboxItem>();
            if (data == null)
            {
                return;
            }
            var position = e.GetPosition(sender);
            switch (data.Type)
            {
                case EControlType.CT_STATIC:
                    this.ForegroundControls.Add(new ControlStatic()
                    {
                        Left = position.X - 25,
                        Top = position.Y - 25,
                        Width = 50,
                        Height = 50,
                        Text = "ControlStatic"
                    });
                    break;
                case EControlType.CT_BUTTON:
                case EControlType.CT_EDIT:
                case EControlType.CT_SLIDER:
                case EControlType.CT_COMBO:
                case EControlType.CT_LISTBOX:
                case EControlType.CT_TOOLBOX:
                case EControlType.CT_CHECKBOXES:
                case EControlType.CT_PROGRESS:
                case EControlType.CT_HTML:
                case EControlType.CT_STATIC_SKEW:
                case EControlType.CT_ACTIVETEXT:
                case EControlType.CT_TREE:
                case EControlType.CT_STRUCTURED_TEXT:
                case EControlType.CT_CONTEXT_MENU:
                case EControlType.CT_CONTROLS_GROUP:
                case EControlType.CT_SHORTCUTBUTTON:
                case EControlType.CT_HITZONES:
                case EControlType.CT_VEHICLETOGGLES:
                case EControlType.CT_CONTROLS_TABLE:
                case EControlType.CT_XKEYDESC:
                case EControlType.CT_XBUTTON:
                case EControlType.CT_XLISTBOX:
                case EControlType.CT_XSLIDER:
                case EControlType.CT_XCOMBO:
                case EControlType.CT_ANIMATED_TEXTURE:
                case EControlType.CT_MENU:
                case EControlType.CT_MENU_STRIP:
                case EControlType.CT_CHECKBOX:
                case EControlType.CT_OBJECT:
                case EControlType.CT_OBJECT_ZOOM:
                case EControlType.CT_OBJECT_CONTAINER:
                case EControlType.CT_OBJECT_CONT_ANIM:
                case EControlType.CT_LINEBREAK:
                case EControlType.CT_USER:
                case EControlType.CT_MAP:
                case EControlType.CT_MAP_MAIN:
                case EControlType.CT_LISTNBOX:
                case EControlType.CT_ITEMSLOT:
                case EControlType.CT_LISTNBOX_CHECKABLE:
                case EControlType.CT_VEHICLE_DIRECTION:
                default:
                    return;
            }

            e.Handled = true;
        }
        public void OnDragLeave(UIElement sender, DragEventArgs e)
        {
            if (e.GetInfo().HasData<EditorToolboxItem>())
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }
        public override string Title { get => false ? this.File?.Name : String.Concat(this.File?.Name, '*'); set => throw new InvalidOperationException(); }

        #region Interface: IEditorDocument
        public File File
        {
            get => this._File;
            set
            {
                if (this._File != null)
                {
                    this._File.PropertyChanged -= this.File_PropertyChanged;
                }
                this._File = value;
                if (this._File != null)
                {
                    this._File.PropertyChanged += this.File_PropertyChanged;
                }
                if (this.TextEditorInstance != null)
                {
                    this.TextEditorInstance.File = value;
                }
                this.RaisePropertyChanged();
            }
        }
        private File _File;
        private void File_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(nameof(this.Title));
        }

        public ITextEditor TextEditorInstance => null;

        public DateTime LastChangeTimestamp
        {
            get => this._LastChangeTimestamp;
            set
            {
                this._LastChangeTimestamp = value;
                this.RaisePropertyChanged();
            }
        }
        private DateTime _LastChangeTimestamp;

        public bool IsReadOnly
        {
            get => this._IsReadOnly;
            set
            {
                this._IsReadOnly = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _IsReadOnly;

        public string GetContents() => this.File.GetText();
        public void ScrollTo(int line, int column) { }
        #endregion

        public override void LayoutSaveCallback(dynamic section)
        {
            section.fullpath = this.File.FullPath;
            section.name = this.File.Name;
        }
        public override void LayoutLoadCallback(dynamic section)
        {
            if ((Application.Current as IApp).MainWindow.FileManagement.ContainsKey((string)section.fullpath))
            {
                this.File = (File)(Application.Current as IApp).MainWindow.FileManagement[(string)section.fullpath];
            }
            else
            {
                this.File = new File { Name = (string)section.fullpath };
            }
            this.RaisePropertyChanged(nameof(this.Title));
            var type = (string)section.type;

            if (this.File?.FullPath != null && System.IO.File.Exists(this.File.FullPath))
            {
                using (var reader = new System.IO.StreamReader(this.File.FullPath))
                {
                    // ToDo: Parse config
                    //this.TextDocument.Text = reader.ReadToEnd();
                }
            }
            else
            {
                MessageBox.Show(this.File?.FullPath?.ToString() ?? "NULL", "File Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
                return;
            }
        }
    }
}

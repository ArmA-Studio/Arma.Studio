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
using System.Threading;
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
        IEditorDocument,
        Studio.Data.UI.IInteractionSave,
        IPropertyHost
    {
        private const string dlg_idd = "idd";
        private const string dlg_movingEnable = "movingEnable";
        private const string dlg_enableSimulation = "enableSimulation";
        private const string dlg_controlsBackground = "controlsBackground";
        private const string dlg_controls = "controls";

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
        [ArmaName("idd")]
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
        [ArmaName("enableSimulation")]
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
        #region Property: ClassName (System.String)
        [Property("Classname")]
        public string ClassName
        {
            get => this._ClassName;
            set
            {
                this._ClassName = value;
                this.RaisePropertyChanged();
            }
        }
        private string _ClassName;
        #endregion
        #region SerializationProperties
        [ArmaName("ArmaStudio_InterfaceSize", Display = false)]
        public string InterfaceSizeSerialized
        {
            get => this.InterfaceSize.Key;
            set => this.InterfaceSize = InterfaceSize.InterfaceSizes.FirstOrDefault((it) => it.Key == value) ?? InterfaceSize.Small;
        }
        [ArmaName("ArmaStudio_GridSize", Display = false)]
        public int GridSizeSerialized
        {
            get => this.CanvasManager.GridSize;
            set => this.CanvasManager.GridSize = value;
        }
        [ArmaName("ArmaStudio_Width", Display = false)]
        public double WidthSerialized
        {
            get => this.CanvasManager.Width;
            set => this.CanvasManager.Width = value;
        }
        [ArmaName("ArmaStudio_Height", Display = false)]
        public double HeightSerialized
        {
            get => this.CanvasManager.Height;
            set => this.CanvasManager.Height = value;
        }
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
            this.ClassName = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            this.Load();
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

            if (this.File?.FullPath != null && System.IO.File.Exists(this.File.PhysicalPath))
            {
                this.Load();
            }
            else
            {
                MessageBox.Show(this.File?.FullPath?.ToString() ?? "NULL", "File Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
                return;
            }
        }

        public void Load()
        {
            var code = System.IO.File.ReadAllText(this.File.PhysicalPath);
            FileFolderBase ffb = this.File;
            while (ffb.Parent != null) { ffb = ffb.Parent; }
            var pbo = ffb as PBO;
            if (pbo is null)
            {
                // ToDo: Localize & enhance message
                throw new InvalidOperationException("No PBO found.");
            }
            using (var vm = new SqfVm.ClrVirtualmachine())
            {
                if (!String.IsNullOrWhiteSpace(pbo.Prefix))
                {
                    vm.AddVirtualMapping(pbo.Prefix, pbo.FullPath);
                }
                vm.OnLog += this.ClrVirtualmachine_OnLog;
                vm.ParseSqf(
                    "{ 1 } provide__ ['safezoneW'];" +
                    "{ 1 } provide__ ['safezoneH'];" +
                    "{ 0 } provide__ ['safezoneX'];" +
                    "{ 0 } provide__ ['safezoneY'];" +
                    "(profilenamespace getvariable ['GUI_BCG_RGB_R',0.13]);" +
                    "(profilenamespace getvariable ['GUI_BCG_RGB_G',0.54]);" +
                    "(profilenamespace getvariable ['GUI_BCG_RGB_B',0.21]);",
                    "__UiEdtior");
                vm.Start();
                var parentClassesPreproc = vm.PreProcess(PluginMain.ParentClassesConfig, "/Arma.Studio/UiEditor/parentClasses.cpp");
                var parentClasses = vm.ParseIntoConfig(parentClassesPreproc, "/Arma.Studio/UiEditor/parentClasses.cpp");
                if (parentClasses is null)
                {
                    // unknown cause
                    throw new Exception();
                }
                var preproc = vm.PreProcess(code, this.File.FullPath);
                var configBin = vm.ParseIntoConfig(preproc, this.File.FullPath);
                if (configBin is null)
                {
                    // parse failed
                    throw new Exception();
                }
                if (configBin.Count == 0)
                {
                    // empty
                    return;
                }
                var dialogConfig = configBin.Values.FirstOrDefault((it) =>
                {
                    if (it.ContainsKey(dlg_idd) ||
                        it.ContainsKey(dlg_movingEnable) || 
                        it.ContainsKey(dlg_enableSimulation) || 
                        it.ContainsKey(dlg_controls))
                    {
                        return true;
                    }
                    return false;
                });
                if (dialogConfig is null)
                {
                    dialogConfig = configBin[configBin.Count - 1];
                }
                configBin.MergeWith(parentClasses);

                List<SqfVm.Config> getControls(SqfVm.Config node)
                {
                    List<SqfVm.Config> controls;
                    if (node.NodeType == SqfVm.EConfigNodeType.Array)
                    {
                        var arrayList = (System.Collections.ArrayList)node.Value;
                        controls = new List<SqfVm.Config>(arrayList.Count);
                        foreach (var it in arrayList)
                        {
                            if (it is string str)
                            {
                                var sub = dialogConfig[str];
                                if (sub != null)
                                {
                                    controls.Add(sub);
                                }
                            }
                        }
                    }
                    else
                    {
                        controls = node.Values.ToList();
                    }
                    return controls;
                }

                this.ClassName = dialogConfig.Name;

                var backgroundControls = dialogConfig[dlg_controlsBackground];
                var foregroundControls = dialogConfig[dlg_controls];
                if (backgroundControls != null)
                {
                    var controls = getControls(backgroundControls);
                    foreach(var node in controls)
                    {
                        var control = LoadControl(vm, node);
                        if (control != null)
                        {
                            this.BackgroundControls.Add(control);
                        }
                    }
                }
                if (foregroundControls != null)
                {
                    var controls = getControls(foregroundControls);
                    foreach (var node in controls)
                    {
                        var control = LoadControl(vm, node);
                        if (control != null)
                        {
                            this.ForegroundControls.Add(control);
                        }
                    }
                }
            }
        }
        private ControlBase LoadControl(SqfVm.ClrVirtualmachine vm, SqfVm.Config node)
        {
            var typeNode = node["type"];
            if (typeNode is null || typeNode.NodeType != SqfVm.EConfigNodeType.Scalar)
            {
                return null;
            }
            var type = (EControlType)(int)(double)typeNode.Value;
            var field = typeof(EControlType).GetField(Enum.GetName(typeof(EControlType), type));
            var attributes = field.GetCustomAttributes(typeof(ControlTypeAttribute), true).Cast<ControlTypeAttribute>().ToArray();
            if (!attributes.Any())
            {
                return null;
            }
            var targetType = attributes.First().TargetType;
            var instance = targetType.CreateInstance<ControlBase>();
            instance.ClassName = node.Name;
            foreach (var propertyInfo in targetType.GetProperties().Where((it) => it.SetMethod != null))
            {
                var armaNameAttributes = Attribute.GetCustomAttributes(propertyInfo, typeof(ArmaNameAttribute), true).Cast<ArmaNameAttribute>().ToArray();
                if (!armaNameAttributes.Any())
                {
                    continue;
                }
                var armaNameAttribute = armaNameAttributes.First();
                var value = node[armaNameAttribute.Title]?.Value;
                if (value is null)
                {
                    continue;
                }
                if (propertyInfo.PropertyType.IsEquivalentTo(typeof(System.Windows.Media.Color)))
                {
                    var arraylist = value as System.Collections.ArrayList;
                    propertyInfo.SetValue(instance,
                        System.Windows.Media.Color.FromArgb(
                            (byte)(255 * Convert.ToDouble(arraylist.Count < 4 ? 0 : arraylist[3], System.Globalization.CultureInfo.InvariantCulture)),
                            (byte)(255 * Convert.ToDouble(arraylist.Count < 1 ? 0 : arraylist[0], System.Globalization.CultureInfo.InvariantCulture)),
                            (byte)(255 * Convert.ToDouble(arraylist.Count < 2 ? 0 : arraylist[1], System.Globalization.CultureInfo.InvariantCulture)),
                            (byte)(255 * Convert.ToDouble(arraylist.Count < 3 ? 0 : arraylist[2], System.Globalization.CultureInfo.InvariantCulture))
                            ),
                        null);
                }
                else if (propertyInfo.PropertyType.IsEnum)
                {
                    propertyInfo.SetValue(instance, Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture), null);
                }
                else if (value is string s)
                {
                    if (propertyInfo.PropertyType.IsEquivalentTo(typeof(string)))
                    {
                        propertyInfo.SetValue(instance, Convert.ChangeType(s.Trim('"'), propertyInfo.PropertyType, System.Globalization.CultureInfo.InvariantCulture), null);
                    }
                    else
                    {
                        var res = vm.Evaluate(s);
                        var resval = Convert.ChangeType(res.Data.Trim('"'), propertyInfo.PropertyType, System.Globalization.CultureInfo.InvariantCulture);
                        if (new string[] { "x", "y", "w", "h" }.Contains(armaNameAttribute.Title))
                        {
                            switch (armaNameAttribute.Title)
                            {
                                case "x":
                                case "w":
                                    propertyInfo.SetValue(instance, this.CanvasManager.Width * (double)resval, null);
                                    break;
                                case "y":
                                case "h":
                                    propertyInfo.SetValue(instance, this.CanvasManager.Height * (double)resval, null);
                                    break;
                            }
                            continue;
                        }
                        else
                        {
                            propertyInfo.SetValue(instance, resval, null);
                        }
                    }
                }
                else
                {
                    propertyInfo.SetValue(instance, Convert.ChangeType(value, propertyInfo.PropertyType, System.Globalization.CultureInfo.InvariantCulture), null);
                }
            }
            return instance;
        }

        private void ClrVirtualmachine_OnLog(object sender, SqfVm.LogEventArgs eventArgs)
        {
            switch (eventArgs.Severity)
            {
                case SqfVm.ESeverity.Fatal:
                    PluginMain.Logger.Log(ESeverity.Error, eventArgs.Message);
                    break;
                case SqfVm.ESeverity.Error:
                    PluginMain.Logger.Log(ESeverity.Error, eventArgs.Message);
                    break;
                case SqfVm.ESeverity.Warning:
                    PluginMain.Logger.Log(ESeverity.Warning, eventArgs.Message);
                    break;
                case SqfVm.ESeverity.Info:
                    PluginMain.Logger.Log(ESeverity.Info, eventArgs.Message);
                    break;
                case SqfVm.ESeverity.Verbose:
                    PluginMain.Logger.Log(ESeverity.Trace, eventArgs.Message);
                    break;
                case SqfVm.ESeverity.Trace:
                    PluginMain.Logger.Log(ESeverity.Diagnostic, eventArgs.Message);
                    break;
                default:
                    break;
            }
        }

        public Task Save(CancellationToken cancellationToken)
        {
            using (var stream = new System.IO.StreamWriter(this.File.PhysicalPath))
            {
                int tabstop = 0;
                string tabs() { return new string(' ', tabstop * 4); }
                stream.Write("class ");
                stream.Write(this.ClassName);
                stream.WriteLine(" {");
                tabstop++;

                foreach (var propertyInfo in typeof(UiEditorDataContext).GetProperties())
                {
                    var armaNameAttributes = Attribute.GetCustomAttributes(propertyInfo, typeof(ArmaNameAttribute), true).Cast<ArmaNameAttribute>().ToArray();
                    if (!armaNameAttributes.Any() && !new string[] { dlg_controls, dlg_controlsBackground }.Contains(propertyInfo.Name))
                    {
                        continue;
                    }
                    var armaNameAttribute = armaNameAttributes.First();
                    stream.Write(tabs());
                    stream.Write(armaNameAttribute.Title);
                    var value = propertyInfo.GetValue(this, null);
                    if (new string[] { "x", "y", "w", "h" }.Contains(armaNameAttribute.Title))
                    {
                        switch (armaNameAttribute.Title)
                        {
                            case "x":
                                stream.WriteLine($" = \"safezoneX + ({value} / 1920) * safezoneW\";");
                                break;
                            case "y":
                                stream.WriteLine($" = \"safezoneY + ({value} / 1920) * safezoneH\";");
                                break;
                            case "w":
                                stream.WriteLine($" = \"({value} / 1920) * safezoneW\";");
                                break;
                            case "h":
                                stream.WriteLine($" = \"({value} / 1920) * safezoneH\";");
                                break;
                        }
                        continue;
                    }
                    if (propertyInfo.PropertyType.IsEquivalentTo(typeof(string)))
                    {
                        stream.Write(" = ");
                        stream.Write('"');
                        stream.Write(value);
                        stream.Write('"');
                    }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(System.Windows.Media.Color)))
                    {
                        stream.Write("[] = { ");
                        var color = (System.Windows.Media.Color)value;
                        stream.Write(color.R / 255.0);
                        stream.Write(", ");
                        stream.Write(color.G / 255.0);
                        stream.Write(", ");
                        stream.Write(color.B / 255.0);
                        stream.Write(", ");
                        stream.Write(color.A / 255.0);
                        stream.Write(" }");
                    }
                    else if (propertyInfo.PropertyType.IsEnum)
                    {
                        stream.Write(" = ");
                        stream.Write(Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        stream.Write(" = ");
                        stream.Write(value);
                    }
                    stream.WriteLine(";");
                }

                stream.Write(tabs());
                stream.WriteLine("class " + dlg_controlsBackground + " {");
                tabstop++;
                foreach (var control in this.BackgroundControls)
                {
                    stream.Write(tabs());
                    stream.WriteLine("class " + control.ClassName + " {");
                    tabstop++;
                    foreach (var propertyInfo in control.GetType().GetProperties())
                    {
                        var armaNameAttributes = Attribute.GetCustomAttributes(propertyInfo, typeof(ArmaNameAttribute), true).Cast<ArmaNameAttribute>().ToArray();
                        if (!armaNameAttributes.Any())
                        {
                            continue;
                        }
                        var armaNameAttribute = armaNameAttributes.First();
                        stream.Write(tabs());
                        stream.Write(armaNameAttribute.Title);
                        var value = propertyInfo.GetValue(control, null);
                        if (new string[] { "x", "y", "w", "h" }.Contains(armaNameAttribute.Title))
                        {
                            switch (armaNameAttribute.Title)
                            {
                                case "x":
                                    stream.WriteLine($" = \"safezoneX + ({value} / 1920) * safezoneW\";");
                                    break;
                                case "y":
                                    stream.WriteLine($" = \"safezoneY + ({value} / 1920) * safezoneH\";");
                                    break;
                                case "w":
                                    stream.WriteLine($" = \"({value} / 1920) * safezoneW\";");
                                    break;
                                case "h":
                                    stream.WriteLine($" = \"({value} / 1920) * safezoneH\";");
                                    break;
                            }
                            continue;
                        }
                        if (propertyInfo.PropertyType.IsEquivalentTo(typeof(string)))
                        {
                            stream.Write(" = ");
                            stream.Write('"');
                            stream.Write(value);
                            stream.Write('"');
                        }
                        else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(System.Windows.Media.Color)))
                        {
                            stream.Write("[] = { ");
                            var color = (System.Windows.Media.Color)value;
                            stream.Write(color.R / 255.0);
                            stream.Write(", ");
                            stream.Write(color.G / 255.0);
                            stream.Write(", ");
                            stream.Write(color.B / 255.0);
                            stream.Write(", ");
                            stream.Write(color.A / 255.0);
                            stream.Write(" }");
                        }
                        else if (propertyInfo.PropertyType.IsEnum)
                        {
                            stream.Write(" = ");
                            stream.Write(Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            stream.Write(" = ");
                            stream.Write(value);
                        }
                        stream.WriteLine(";");
                    }
                    tabstop--;
                    stream.Write(tabs());
                    stream.WriteLine("};");
                }
                tabstop--;
                stream.Write(tabs());
                stream.WriteLine("};");

                stream.Write(tabs());
                stream.WriteLine("class " + dlg_controls + " {");
                tabstop++;
                foreach (var control in this.ForegroundControls)
                {
                    stream.Write(tabs());
                    stream.WriteLine("class " + control.ClassName + " {");
                    tabstop++;
                    foreach (var propertyInfo in control.GetType().GetProperties())
                    {
                        var armaNameAttributes = Attribute.GetCustomAttributes(propertyInfo, typeof(ArmaNameAttribute), true).Cast<ArmaNameAttribute>().ToArray();
                        if (!armaNameAttributes.Any())
                        {
                            continue;
                        }
                        var armaNameAttribute = armaNameAttributes.First();
                        stream.Write(tabs());
                        stream.Write(armaNameAttribute.Title);
                        var value = propertyInfo.GetValue(control, null);
                        if (new string[] { "x", "y", "w", "h" }.Contains(armaNameAttribute.Title))
                        {
                            switch (armaNameAttribute.Title)
                            {
                                case "x":
                                    stream.WriteLine($" = \"safezoneX + ({value} / 1920) * safezoneW\";");
                                    break;
                                case "y":
                                    stream.WriteLine($" = \"safezoneY + ({value} / 1920) * safezoneH\";");
                                    break;
                                case "w":
                                    stream.WriteLine($" = \"({value} / 1920) * safezoneW\";");
                                    break;
                                case "h":
                                    stream.WriteLine($" = \"({value} / 1920) * safezoneH\";");
                                    break;
                            }
                            continue;
                        }
                        if (propertyInfo.PropertyType.IsEquivalentTo(typeof(string)))
                        {
                            stream.Write(" = ");
                            stream.Write('"');
                            stream.Write(value);
                            stream.Write('"');
                        }
                        else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(System.Windows.Media.Color)))
                        {
                            stream.Write("[] = { ");
                            var color = (System.Windows.Media.Color)value;
                            stream.Write(color.R / 255.0);
                            stream.Write(", ");
                            stream.Write(color.G / 255.0);
                            stream.Write(", ");
                            stream.Write(color.B / 255.0);
                            stream.Write(", ");
                            stream.Write(color.A / 255.0);
                            stream.Write(" }");
                        }
                        else if (propertyInfo.PropertyType.IsEnum)
                        {
                            stream.Write(" = ");
                            stream.Write(Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            stream.Write(" = ");
                            stream.Write(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
                        }
                        stream.WriteLine(";");
                    }
                    tabstop--;
                    stream.Write(tabs());
                    stream.WriteLine("};");
                }
                tabstop--;
                stream.Write(tabs());
                stream.WriteLine("};");

                tabstop--;
                stream.Write(tabs());
                stream.WriteLine("};");
            }
            return Task.CompletedTask;
        }
    }
}

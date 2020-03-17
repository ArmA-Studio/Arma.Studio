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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Arma.Studio.UiEditor.UI
{
    public class UiEditorDataContext : DockableBase,
        IOnDragEnter,
        IOnDragLeave,
        IOnDragOver,
        IOnDrop,
        IEditorDocument,
        IInteractionSave,
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
        private const string InterfaceSizeSerializedKey = "ArmaStudio_InterfaceSize";
        [ArmaName(InterfaceSizeSerializedKey, Display = false)]
        public string InterfaceSizeSerialized
        {
            get => this.InterfaceSize.Key;
            set => this.InterfaceSize = InterfaceSize.InterfaceSizes.FirstOrDefault((it) => it.Key == value) ?? InterfaceSize.Small;
        }
        private const string GridSizeSerializedKey = "ArmaStudio_GridSize";
        [ArmaName(GridSizeSerializedKey, Display = false)]
        public int GridSizeSerialized
        {
            get => this.CanvasManager.GridSize;
            set => this.CanvasManager.GridSize = value;
        }
        private const string WidthSerializedKey = "ArmaStudio_Width";
        [ArmaName(WidthSerializedKey, Display = false)]
        public double WidthSerialized
        {
            get => this.CanvasManager.Width;
            set => this.CanvasManager.Width = value;
        }
        private const string HeightSerializedKey = "ArmaStudio_Height";
        [ArmaName(HeightSerializedKey, Display = false)]
        public double HeightSerialized
        {
            get => this.CanvasManager.Height;
            set => this.CanvasManager.Height = value;
        }
        #endregion


        public override string Title { get => this.HasChanges ? String.Concat(this.File?.Name, '*') : this.File?.Name; set => throw new InvalidOperationException(); }
        public bool HasChanges
        {
            get => this._HasChanges;
            set
            {
                if (this._HasChanges == value)
                {
                    return;
                }
                this._HasChanges = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.Title));
            }
        }
        private bool _HasChanges;


        public UiEditorDataContext()
        {
            this.IDD = -1;
            this.EnableSimulation = true;
            this.BackgroundControls = new ObservableCollection<IControlElement>();
            this.ForegroundControls = new ObservableCollection<IControlElement>();
            this.CanvasManager = new CanvasManager(this);
            this.InterfaceSize = InterfaceSize.Small;
            this.HasChanges = false;


            this.BackgroundControls.CollectionChanged += this.Controls_CollectionChanged;
            this.ForegroundControls.CollectionChanged += this.Controls_CollectionChanged;
        }

        private void Controls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.HasChanges = true;
            foreach (IControlElement controlElement in e.NewItems ?? Array.Empty<object>())
            {
                controlElement.PropertyChanged += (sender, e) => this.HasChanges = true;
            }
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
            var controlTypeAttribute = typeof(EControlType)
                .GetField(Enum.GetName(typeof(EControlType), data.Type))
                .GetCustomAttributes(typeof(ControlTypeAttribute), true)
                .FirstOrDefault() as ControlTypeAttribute;
            if (controlTypeAttribute is null)
            {
                return;
            }
            var controlBase = controlTypeAttribute.TargetType.CreateInstance<ControlBase>();
            controlBase.Left = position.X - 25;
            controlBase.Top = position.Y - 25;
            controlBase.Width = 50;
            controlBase.Height = 50;
            this.ForegroundControls.Add(controlBase);
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
                var preproc = Utility.CatchYield(() => vm.PreProcess(code, this.File.FullPath));
                if (preproc is null)
                {
                    // parse failed
                    throw new Exception();
                }
                var configBin = Utility.CatchYield(() => vm.ParseIntoConfig(preproc, this.File.FullPath));
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
                try
                {
                    if (dialogConfig.ContainsKey("idd")) { this.IDD = Convert.ToInt32(dialogConfig["idd"].Value); }
                    if (dialogConfig.ContainsKey(InterfaceSizeSerializedKey)) { this.InterfaceSizeSerialized = Convert.ToString(dialogConfig[InterfaceSizeSerializedKey].Value); }
                    if (dialogConfig.ContainsKey(GridSizeSerializedKey)) { this.GridSizeSerialized = Convert.ToInt32(dialogConfig[GridSizeSerializedKey].Value); }
                    if (dialogConfig.ContainsKey(WidthSerializedKey)) { this.WidthSerialized = Convert.ToDouble(dialogConfig[WidthSerializedKey].Value); }
                    if (dialogConfig.ContainsKey(HeightSerializedKey)) { this.HeightSerialized = Convert.ToDouble(dialogConfig[HeightSerializedKey].Value); }
                }
                catch { }

                var backgroundControls = dialogConfig[dlg_controlsBackground];
                var foregroundControls = dialogConfig[dlg_controls];
                var failedToLoadList = new List<string>();
                if (backgroundControls != null)
                {
                    var controls = getControls(backgroundControls);
                    foreach (var node in controls)
                    {
                        var control = LoadControl(vm, node);
                        if (control != null)
                        {
                            this.BackgroundControls.Add(control);
                        }
                        else
                        {
                            failedToLoadList.Add(node.Name);
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
                        else
                        {
                            failedToLoadList.Add(node.Name);
                        }
                    }
                }
                if (failedToLoadList.Any())
                {
                    MessageBox.Show(
                        String.Format(Properties.Language.UiEditor_FailedToLoad_Body_0names, String.Concat("\n- ", String.Join("\n- ", failedToLoadList))),
                        Properties.Language.UiEditor_FailedToLoad_Caption,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            this.HasChanges = false;
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
            try
            {
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
            catch
            { // Any conversion exception is a load-fail, return null.
                return null;
            }
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

        public async Task Save(CancellationToken cancellationToken)
        {
            using (var stream = new System.IO.StreamWriter(this.File.PhysicalPath))
            {
                int tabstop = 0;
                string tabs() { return new string(' ', tabstop * 4); }
                async Task WriteOutConfig(Type t, object obj)
                {
                    foreach (var propertyInfo in t.GetProperties())
                    {
                        var armaNameAttributes = Attribute.GetCustomAttributes(propertyInfo, typeof(ArmaNameAttribute), true).Cast<ArmaNameAttribute>().ToArray();
                        if (!armaNameAttributes.Any() && !new string[] { dlg_controls, dlg_controlsBackground }.Contains(propertyInfo.Name))
                        {
                            continue;
                        }
                        var armaNameAttribute = armaNameAttributes.First();
                        await stream.WriteAsync(tabs());
                        await stream.WriteAsync(armaNameAttribute.Title);
                        var value = propertyInfo.GetValue(obj, null);
                        if (new string[] { "x", "y", "w", "h" }.Contains(armaNameAttribute.Title))
                        {
                            switch (armaNameAttribute.Title)
                            {
                                case "x":
                                    await stream.WriteLineAsync($" = \"safezoneX + ({((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture)} / {this.CanvasManager.Width}) * safezoneW\";");
                                    break;
                                case "y":
                                    await stream.WriteLineAsync($" = \"safezoneY + ({((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture)} / {this.CanvasManager.Height}) * safezoneH\";");
                                    break;
                                case "w":
                                    await stream.WriteLineAsync($" = \"({((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture)} / {this.CanvasManager.Width}) * safezoneW\";");
                                    break;
                                case "h":
                                    await stream.WriteLineAsync($" = \"({((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture)} / {this.CanvasManager.Height}) * safezoneH\";");
                                    break;
                            }
                            continue;
                        }
                        if (propertyInfo.PropertyType.IsEquivalentTo(typeof(string)))
                        {
                            await stream.WriteAsync(" = ");
                            await stream.WriteAsync('"');
                            await stream.WriteAsync(Convert.ToString(value));
                            await stream.WriteAsync('"');
                        }
                        else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(System.Windows.Media.Color)))
                        {
                            await stream.WriteAsync("[] = { ");
                            var color = (System.Windows.Media.Color)value;
                            await stream.WriteAsync(Convert.ToString(color.R / 255.0, System.Globalization.CultureInfo.InvariantCulture));
                            await stream.WriteAsync(", ");
                            await stream.WriteAsync(Convert.ToString(color.G / 255.0, System.Globalization.CultureInfo.InvariantCulture));
                            await stream.WriteAsync(", ");
                            await stream.WriteAsync(Convert.ToString(color.B / 255.0, System.Globalization.CultureInfo.InvariantCulture));
                            await stream.WriteAsync(", ");
                            await stream.WriteAsync(Convert.ToString(color.A / 255.0, System.Globalization.CultureInfo.InvariantCulture));
                            await stream.WriteAsync(" }");
                        }
                        else if (propertyInfo.PropertyType.IsEnum)
                        {
                            await stream.WriteAsync(" = ");
                            await stream.WriteAsync(Convert.ToString(Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture)));
                        }
                        else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(bool)))
                        {
                            await stream.WriteAsync(" = ");
                            await stream.WriteAsync(Convert.ToString(value).ToLower());
                        }
                        else
                        {
                            await stream.WriteAsync(" = ");
                            await stream.WriteAsync(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
                        }
                        await stream.WriteLineAsync(";");
                    }
                }
                await stream.WriteAsync("class ");
                await stream.WriteAsync(this.ClassName);
                await stream.WriteLineAsync(" {");
                tabstop++;

                await WriteOutConfig(typeof(UiEditorDataContext), this);

                await stream.WriteAsync(tabs());
                await stream.WriteLineAsync("class " + dlg_controlsBackground + " {");
                tabstop++;
                foreach (var control in this.BackgroundControls)
                {
                    await stream.WriteAsync(tabs());
                    await stream.WriteLineAsync("class " + control.ClassName + " {");
                    tabstop++;

                    await WriteOutConfig(control.GetType(), control);

                    tabstop--;
                    await stream.WriteAsync(tabs());
                    await stream.WriteLineAsync("};");
                }
                tabstop--;
                await stream.WriteAsync(tabs());
                await stream.WriteLineAsync("};");

                await stream.WriteAsync(tabs());
                await stream.WriteLineAsync("class " + dlg_controls + " {");
                tabstop++;
                foreach (var control in this.ForegroundControls)
                {
                    await stream.WriteAsync(tabs());
                    await stream.WriteLineAsync("class " + control.ClassName + " {");
                    tabstop++;

                    await WriteOutConfig(control.GetType(), control);

                    tabstop--;
                    await stream.WriteAsync(tabs());
                    await stream.WriteLineAsync("};");
                }
                tabstop--;
                await stream.WriteAsync(tabs());
                await stream.WriteLineAsync("};");

                tabstop--;
                await stream.WriteAsync(tabs());
                await stream.WriteLineAsync("};");
            }
            this.HasChanges = false;
        }
    }
}

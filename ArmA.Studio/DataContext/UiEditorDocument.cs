using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;
using ICSharpCode.AvalonEdit.Highlighting;
using RealVirtuality.Config.Control;
using RealVirtuality.Config.Control.Attributes;
using RealVirtuality.Config.Parser;
using RealVirtuality.Config;
using System.Windows.Threading;
using NLog;

namespace ArmA.Studio.DataContext
{
    public class UiEditorDocument : ConfigEditorDocument
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private sealed class ConfigEntryUiControlBinding : UI.CustomBinding<ConfigEntryExpression, ControlBase>
        {
            public ConfigEntryUiControlBinding(PropertyInfo propSource, ConfigEntryExpression source, PropertyInfo propTarget, ControlBase target) : base(propSource, source, propTarget, target) { }
        }
        private static DataTemplate ThisTemplate { get; set; }
        static UiEditorDocument()
        {
            ThisTemplate = GetDataTemplateFromAssemblyRes("ArmA.Studio.UI.DataTemplates.UiEditorDocumentTemplate.xaml");
        }

        public override string[] SupportedFileExtensions { get { return new string[] { ".uic" }; } }

        public FlowDocument VirtualConfigDocument { get { return this._VirtualConfigDocument; } set { this._VirtualConfigDocument = value; this.RaisePropertyChanged(); } }
        private FlowDocument _VirtualConfigDocument;
        public ConfigEntry ConfigTreeRoot { get { return this._ConfigTreeRoot; } set { this._ConfigTreeRoot = value; this.RaisePropertyChanged(); } }
        private ConfigEntry _ConfigTreeRoot;
        public ObservableCollection<ControlBase> Controls { get { return this._Controls; } set { this._Controls = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<ControlBase> _Controls;
        public override DataTemplate Template { get { return ThisTemplate; } }
        //ToDo: Create Controls DataTemplates


        public int CurrentTabIndex
        {
            get
            {
                return this._CurrentTabIndex;
            }
            set
            {
                this._CurrentTabIndex = value;
                this.RaisePropertyChanged();
                if (value == 0)
                {
                    //ToDo: Clear UI-Controls and ConfigTree
                    if (VirtualConfigDocument != null)
                    {
                        var txt = new TextRange(this.VirtualConfigDocument.ContentStart, this.VirtualConfigDocument.ContentEnd).Text;
                        this.VirtualConfigDocument = null;
                        this.ConfigTreeRoot = null;
                        if (this.Document.Text.Equals(txt))
                            return;
                        this.Document.Text = txt;
                    }
                }
                else if(value == 1)
                {
                    //ToDo: Build UI-Controls and ConfigTree
                    this.VirtualConfigDocument = new FlowDocument();
                    new TextRange(this.VirtualConfigDocument.ContentStart, this.VirtualConfigDocument.ContentEnd).Text = this.Document.Text;
                    this.ConfigTreeRoot = new ConfigEntry(null) { };
                    this.BuildConfigTree();
                }
            }
        }
        private int _CurrentTabIndex;
        private string ConfigEntryName;
        private List<ConfigEntryUiControlBinding> Bindings;

        protected override void OnTextChanged(object param)
        {
            base.OnTextChanged(param);
        }

        public UiEditorDocument() : base()
        {
            this._Controls = new ObservableCollection<ControlBase>();
            this.Bindings = new List<ConfigEntryUiControlBinding>();
        }

        public void BuildConfigTree()
        {
            using (var memstream = new MemoryStream())
            {
                { //Load content into MemoryStream
                    var writer = new StreamWriter(memstream);
                    writer.Write(this.Document.Text);
                    writer.Flush();
                    memstream.Seek(0, SeekOrigin.Begin);
                }
                //Setup base requirements for the parser
                var parser = new Parser(new Scanner(memstream));

                parser.Root = this.ConfigTreeRoot;
                parser.doc = this.VirtualConfigDocument;
                parser.Parse();
                if(parser.errors.Count > 0)
                {
                    this.VirtualConfigDocument = null;

                    Application.Current.Dispatcher.BeginInvoke((Action)delegate
                    { this.CurrentTabIndex = 0; }, DispatcherPriority.Render, null);
                    return;
                }
                if(this.ConfigTreeRoot.Children.Count > 1)
                {
                    var dlgContext = new Dialogs.ConfigEntrySelectorDialogDataContext();
                    foreach(var entry in this.ConfigTreeRoot.Children)
                    {
                        dlgContext.ThisCollection.Add(entry);
                    }
                    var dlg = new Dialogs.ConfigEntrySelectorDialog(dlgContext);
                    var dlgResult = dlg.ShowDialog();
                    if(!dlgResult.HasValue || !dlgResult.Value)
                    {
                        this.CurrentTabIndex = 0;
                        return;
                    }
                    this.ConfigEntryName = (dlgContext.SelectedValue as ConfigEntry).Name;
                }
                else if(this.ConfigTreeRoot.Children.Count == 0)
                {
                    MessageBox.Show(Properties.Localization.NoConfigPresent_Body, Properties.Localization.NoConfigPresent_Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    this.ConfigEntryName = this.ConfigTreeRoot.Children.First().Name;
                }
                var controls = this.ConfigTreeRoot[this.ConfigEntryName]["controls"];
                if(controls == null)
                {
                    //ToDo: Inform user that there is no controls class
                    return;
                }
                this.Controls.Clear();

                foreach (var it in controls.Children)
                {
                    //Receive control type
                    int type;
                    var tmpStr = it["type"]?.Content;
                    if (tmpStr == null || !int.TryParse(tmpStr, out type))
                    {
                        //ToDo: Localize
                        logger.Log(LogLevel.Error, string.Format("Cannot find field 'type' on '{0}'", it.Name, tmpStr));
                        continue;
                    }
                    //and check for existing control target
                    ControlInfoAttribute controlInfo = null;
                    foreach (EType eIt in Enum.GetValues(typeof(EType)))
                    {
                        if ((int)eIt != type)
                            continue;
                        var tmpInfo = ControlInfoAttribute.GetAttribute(eIt);
                        if(tmpInfo != null)
                        {
                            controlInfo = tmpInfo;
                            break;
                        }
                    }
                    if(controlInfo == null)
                    {
                        //ToDo: Localize
                        logger.Log(LogLevel.Error, string.Format("Cannot find corresponding ControlInfo for '{0}' for field 'type' => '{1}'", it.Name, type));
                        continue;
                    }
                    //create control
                    var control = Activator.CreateInstance(controlInfo.Type) as ControlBase;
                    //Create CustomBindings
                    foreach(var prop in control.GetType().GetProperties())
                    {
                        var descriptor = ConfigPathDescriptor.GetAttribute(prop);
                        if (descriptor == null)
                            return;
                        var configExpression = new ConfigEntryExpression(it, descriptor.Path);
                        this.Bindings.Add(new ConfigEntryUiControlBinding(
                            typeof(ConfigEntryExpression).GetProperty(nameof(ConfigEntryExpression.Value)),
                            configExpression,
                            typeof(ControlBase).GetProperty(nameof(control.Height)),
                            control)
                        );
                    }
                    //add to controls list
                    this.Controls.Add(control);
                }
            }
        }
    }
}
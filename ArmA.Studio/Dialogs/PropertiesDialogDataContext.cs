using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Windows;
using System.Windows.Input;
using ArmA.Studio.Data.Configuration;
using ArmA.Studio.Data.UI.Commands;
using ArmA.Studio.Properties;
using Localization = System.Windows.Localization;

namespace ArmA.Studio.Dialogs
{
    public class PropertiesDialogDataContext : IDialogContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ICommand CmdOKButtonPressed => new RelayCommand((p) => this.DialogResult = true);

        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;

        public string WindowHeader => Properties.Localization.PropertiesDialog_Header;

        public string OKButtonText => Properties.Localization.OK;

        public bool OKButtonEnabled => true;

        public bool RestartRequired { get; internal set; }

        public IEnumerable<Category> Categories { get { return this._Categories; } set { this._Categories = value; this.RaisePropertyChanged(); } }
        private IEnumerable<Category> _Categories;
        public Category SelectedCategory { get { return this._SelectedCategory; } set { if (this._SelectedCategory == value) return; this._SelectedCategory = value; this.RaisePropertyChanged(); } }
        private Category _SelectedCategory;

        public PropertiesDialogDataContext()
        {
            this.Categories = new ObservableCollection<Category>(this.GetCategories());
            this._SelectedCategory = this.Categories.FirstOrDefault();
        }

        private IEnumerable<Category> GetCategories()
        {
            var list = new List<Category>(App.GetPlugins<Plugin.IPropertiesPlugin>().SelectMany((p) => p.GetCategories()));

            list.Add(new Category(this.GetGeneralSubCategories()) { Name = Properties.Localization.GeneralProperties });
            list.Add(new Category(this.GetColorSubCategories()) { Name = Properties.Localization.ColoringProperties, ImageSource = @"/ArmA.Studio;component/Resources/Pictograms/ColorPalette/ColorPalette_26x.png" });

            return Category.Merge(list);
        }
        private IEnumerable<SubCategory> GetColorSubCategories()
        {
            foreach (var c in typeof(ConfigHost.Coloring).GetNestedTypes())
            {
                yield return new SubCategory(this.GetColorItems(c)) { Name = c.Name, ImageSource = @"/ArmA.Studio;component/Resources/Pictograms/ColorPalette/ColorPalette_26x.png" };
            }
        }
        private IEnumerable<Item> GetColorItems(Type colorType)
        {
            foreach (var prop in colorType.GetProperties())
            {
                yield return new ColorConfigItem(prop.Name, prop);
            }
        }

        private IEnumerable<SubCategory> GetGeneralSubCategories()
        {
            yield return new SubCategory(new Item[]
            {
                new BoolItem(Properties.Localization.Property_General_Updating_EnableAutoToolUpdateAtStart, typeof(ConfigHost.App).GetProperty(nameof(ConfigHost.App.EnableAutoToolUpdates)), null),
                new BoolItem(Properties.Localization.Property_General_Updating_EnableAutoPluginsUpdateAtStart, typeof(ConfigHost.App).GetProperty(nameof(ConfigHost.App.EnableAutoPluginsUpdate)), null),
                new BoolItem(Properties.Localization.Property_General_Updating_UseInDevBuilds, typeof(ConfigHost.App).GetProperty(nameof(ConfigHost.App.UseInDevBuild)), null)
            })
            { Name = Properties.Localization.Property_General_Updating, ImageSource = @"/ArmA.Studio;component/Resources/Logo.ico" };
            yield return new SubCategory(new Item[]
            {
                new BoolItem(Properties.Localization.Property_General_ErrorReporting_AutoReportException, typeof(ConfigHost.App).GetProperty(nameof(ConfigHost.App.AutoReportException)), null)
            })
            { Name = Properties.Localization.Property_General_ErrorReporting, ImageSource = @"/ArmA.Studio;component/Resources/Logo.ico" };
            yield return new SubCategory( new Item[]
            {
                    new ComboBoxItem<int>(LanguageManager.SupportedLanguages(), Properties.Localization.Properties_General_Language, typeof(LanguageManager).GetProperty(nameof(LanguageManager.Language)), null )
            })
            { Name = Properties.Localization.Properties_General_Language, ImageSource = @"/ArmA.Studio;component/Resources/Logo.ico" };
        }
    }
}
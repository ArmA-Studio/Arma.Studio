using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ArmA.Studio.Data.Configuration;
using ArmA.Studio.Data.UI.Commands;
using ArmA.Studio.Dialogs.PropertiesDialogUtil;

namespace ArmA.Studio.Dialogs
{
    public class PropertiesDialogDataContext : IDialogContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ICommand CmdOKButtonPressed => new RelayCommand((p) => this.DialogResult = true);

        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;

        public string WindowHeader { get { return Properties.Localization.PropertiesDialog_Header; } }

        public string OKButtonText { get { return Properties.Localization.OK; } }

        public bool OKButtonEnabled { get { return true; } }

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
            list.Add(new Category(this.GetColorSubCategories()) { Name = Properties.Localization.ColoringProperties, ImageSource = @"/ArmA.Studio;component/Resources/Pictograms/ColorPalette/ColorPalette_26x.png" });
            return list;
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
    }
}
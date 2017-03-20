using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ArmA.Studio.Data.Configuration;
using ArmA.Studio.Dialogs.PropertiesDialogUtil;

namespace ArmA.Studio.Dialogs
{
    public class PropertiesDialogDataContext : IDialogContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ICommand CmdOKButtonPressed { get; private set; }

        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;

        public string SearchText { get { return this._SearchText; } set { this._SearchText = value; this.AvailableCategories.Refresh(); this.RaisePropertyChanged(); } }
        private string _SearchText;

        public string WindowHeader { get { return Properties.Localization.LicenseViewer_Header; } }

        public string OKButtonText { get { return Properties.Localization.OK; } }

        public bool OKButtonEnabled { get { return true; } }

        public bool RestartRequired { get; internal set; }

        public ICollectionView AvailableCategories { get; private set; }

        public ObservableCollection<ConfigCategory> Categories { get; private set; }

        public ConfigCategory SelectedCategory { get { return this._SelectedCategory; } set { this._SelectedCategory = value; this.AvailableCategories.Refresh(); this.RaisePropertyChanged(); } }
        private ConfigCategory _SelectedCategory;
        
        public PropertiesDialogDataContext()
        {
            this.CmdOKButtonPressed = new UI.Commands.RelayCommand(Cmd_OKButtonPressed);
            this.Categories = new ObservableCollection<ConfigCategory>(GetCategories());
            this._SelectedCategory = this.Categories.First();
            this.AvailableCategories = CollectionViewSource.GetDefaultView(this.Categories);
            this.AvailableCategories.Filter = new Predicate<object>(AvailableCategories_Filter);
        }

        private bool AvailableCategories_Filter(object obj)
        {
            var cat = obj as ConfigCategory;
            if (cat == null || this.SearchText == null || cat.Name.Contains(this.SearchText, true) || cat.Any((it) => it.Name.Contains(this.SearchText, true)))
                return true;
            return false;
        }

        public void Cmd_OKButtonPressed(object param)
        {
            this.DialogResult = true;
        }

        private IEnumerable<ConfigCategory> GetCategories()
        {
            yield return new ConfigCategory(Properties.Localization.ColoringProperties, @"Resource\ColorPalette\ColorPalette_26x.png", GetColorConfigEntries());
        }

        private IEnumerable<ConfigEntry> GetColorConfigEntries()
        {
            foreach(var c in typeof(ConfigHost.Coloring).GetNestedTypes())
            {
                foreach(var prop in c.GetProperties())
                {
                    yield return new ColorConfigEntry(c.Name, prop.Name, prop.GetValue(null), prop.SetMethod);
                }
            }
        }
    }
}
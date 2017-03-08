using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using ArmA.Studio.UI.Commands;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Utility.Collections;


namespace ArmA.Studio.DataContext
{
    public class ErrorListPane : PanelBase
    {
        public static ErrorListPane Instance { get; private set; }

        public ObservableDictionary<string, IEnumerable<TextEditorUtil.LinterInfo>> LinterDictionary;


        public override string Title { get { return Properties.Localization.PanelDisplayName_ErrorList; } }

        public bool IsErrorsDisplayed { get { return this._IsErrorsDisplayed; } set { ConfigHost.App.ErrorList_IsErrorsDisplayed = this._IsErrorsDisplayed = value; this.ListView.Refresh(); this.RaisePropertyChanged(); } }
        private bool _IsErrorsDisplayed;

        public bool IsWarningsDisplayed { get { return this._IsWarningsDisplayed; } set { ConfigHost.App.ErrorList_IsWarningsDisplayed = this._IsWarningsDisplayed = value; this.ListView.Refresh(); this.RaisePropertyChanged(); } }
        private bool _IsWarningsDisplayed;

        public bool IsInfosDisplayed { get { return this._IsInfosDisplayed; } set { ConfigHost.App.ErrorList_IsInfosDisplayed = this._IsInfosDisplayed = value; this.ListView.Refresh(); this.RaisePropertyChanged(); } }
        private bool _IsInfosDisplayed;

        public string FileFilter { get { return this._FileFilter; } set { if (this._FileFilter.Equals(value)) return; this._FileFilter = value; this.UpdateListView(); this.RaisePropertyChanged(); } }
        private string _FileFilter;

        public int CurrentErrorCount { get { return this._CurrentErrorCount; } set { this._CurrentErrorCount = value; this.RaisePropertyChanged(); } }
        private int _CurrentErrorCount;

        public int CurrentWarningCount { get { return this._CurrentWarningCount; } set { this._CurrentWarningCount = value; this.RaisePropertyChanged(); } }
        private int _CurrentWarningCount;

        public int CurrentInfoCount { get { return this._CurrentInfoCount; } set { this._CurrentInfoCount = value; this.RaisePropertyChanged(); } }
        private int _CurrentInfoCount;


        public ListCollectionView ListView { get { return this._ListView; } set { this._ListView = value; value.Filter = new Predicate<object>(ListViewFilter); this.RaisePropertyChanged(); } }
        private ListCollectionView _ListView;

        public ICommand CmdOnDoubleClick { get; private set; }

        public ErrorListPane()
        {
            this.LinterDictionary = new ObservableDictionary<string, IEnumerable<TextEditorUtil.LinterInfo>>();
            this.LinterDictionary.CollectionChanged += LinterDictionary_CollectionChanged;
            this.ListView = new ListCollectionView(new List<object>());
            this._IsErrorsDisplayed = ConfigHost.App.ErrorList_IsErrorsDisplayed;
            this._IsWarningsDisplayed = ConfigHost.App.ErrorList_IsWarningsDisplayed;
            this._IsInfosDisplayed = ConfigHost.App.ErrorList_IsInfosDisplayed;
            this.CmdOnDoubleClick = new RelayCommand(Cmd_OnDoubleClick);
            Instance = this;
        }

        private void LinterDictionary_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.UpdateListView();
        }

        public void UpdateListView()
        {
            if (this.FileFilter == null)
            {
                var list = new List<TextEditorUtil.LinterInfo>();
                foreach (var val in this.LinterDictionary.Values)
                {
                    list.AddRange(val);
                }
                this.ListView = new ListCollectionView(list);
                this.CurrentErrorCount = list.Count((it) => it.Severity == ESeverity.Error);
                this.CurrentWarningCount = list.Count((it) => it.Severity == ESeverity.Warning);
                this.CurrentInfoCount = list.Count((it) => it.Severity == ESeverity.Info);
            }
            else
            {
                if (this.LinterDictionary.ContainsKey(this.FileFilter))
                {
                    var list = this.LinterDictionary[this.FileFilter].ToList();
                    this.ListView = new ListCollectionView(list);
                    this.CurrentErrorCount = list.Count((it) => it.Severity == ESeverity.Error);
                    this.CurrentWarningCount = list.Count((it) => it.Severity == ESeverity.Warning);
                    this.CurrentInfoCount = list.Count((it) => it.Severity == ESeverity.Info);
                }
                else
                {
                    this.ListView = new ListCollectionView(new List<object>());
                    this.CurrentErrorCount = 0;
                    this.CurrentWarningCount = 0;
                    this.CurrentInfoCount = 0;
                }
            }
        }

        private bool ListViewFilter(object item)
        {
            var li = item as TextEditorUtil.LinterInfo;
            if (li == null)
                return false;
            return (this.IsErrorsDisplayed ? true : li.Severity != ESeverity.Error) &&
                    (this.IsWarningsDisplayed ? true : li.Severity != ESeverity.Warning) &&
                    (this.IsInfosDisplayed ? true : li.Severity != ESeverity.Info);
        }

        private void Cmd_OnDoubleClick(object param)
        {

        }
    }
}
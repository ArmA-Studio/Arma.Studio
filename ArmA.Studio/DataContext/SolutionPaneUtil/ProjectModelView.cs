using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ArmA.Studio.Data;
using ArmA.Studio.Data.Configuration;
using ArmA.Studio.Data.UI;
using ArmA.Studio.Data.UI.Commands;

namespace ArmA.Studio.DataContext.SolutionPaneUtil
{
    public class ProjectModelView : INotifyPropertyChanged, IList<object>, IPropertyPaneProvider
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public Project Ref { get; private set; }

        private readonly IList<object> InnerList;

        public IEnumerator<object> GetEnumerator() => InnerList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

        public int Count => this.InnerList.Count;
        public bool IsReadOnly => this.InnerList.IsReadOnly;

        public IEnumerable<Item> Items => GetPropertyPaneItems();
        private IEnumerable<Item> GetPropertyPaneItems()
        {
            yield return new StringItem(Properties.Localization.Prop_ArmAPath, nameof(this.Ref.ArmAPath), this.Ref);
            yield return new ComboBoxItem<EProjectType>(new[] {
                new KeyValuePair<string, EProjectType>(nameof(EProjectType.Addon), EProjectType.Addon),
                new KeyValuePair<string, EProjectType>(nameof(EProjectType.Mission), EProjectType.Mission)
            }, Properties.Localization.Prop_ProjectType, nameof(this.Ref.ProjectType), this.Ref);
        }

        public object this[int index] { get { return this.InnerList[index]; } set { this.InnerList[index] = value; } }
        public int IndexOf(object item) => this.InnerList.IndexOf(item);
        public void Insert(int index, object item) => this.InnerList.Insert(index, item);
        public void RemoveAt(int index) => this.InnerList.RemoveAt(index);
        public void Add(object item) => this.InnerList.Add(item);
        public void Clear() => this.InnerList.Clear();
        public bool Contains(object item) => this.InnerList.Contains(item);
        public void CopyTo(object[] array, int arrayIndex) => this.InnerList.CopyTo(array, arrayIndex);
        public bool Remove(object item) => this.InnerList.Remove(item);

        public bool IsInRenameMode { get { return this._IsInRenameMode; } set { this._IsInRenameMode = value; RaisePropertyChanged(); } }
        private bool _IsInRenameMode;

        public bool IsExpanded { get { return this._IsExpanded; } set { this._IsExpanded = value; RaisePropertyChanged(); } }
        private bool _IsExpanded;

        public bool IsSelected { get { return this._IsSelected; } set { this._IsSelected = value; RaisePropertyChanged(); } }
        private bool _IsSelected;

        public ProjectModelView(Project p)
        {
            this.Ref = p;
            this.InnerList = new List<object>();
        }

        public ICommand CmdContextMenuOpening => new RelayCommand((p) =>
        {
            if (this.IsInRenameMode)
                return;
            this.IsSelected = true;
        });

        public ICommand CmdContextMenu_OpenInExplorer => new RelayCommand((p) => { System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", this.Ref.FilePath)); });
        public ICommand CmdContextMenu_Remove => new RelayCommand((p) => { this.Ref.OwningSolution.Projects.Remove(this.Ref); });
        public ICommand CmdMouseDoubleClick => new RelayCommand((p) => { });
        public ICommand CmdContextMenu_Add_NewItem => new RelayCommand((p) =>
        {
            var dlgdc = new Dialogs.CreateNewFileDialogDataContext();
            var dlg = new Dialogs.CreateNewFileDialog(dlgdc);
            var dlgResult = dlg.ShowDialog();
            if (dlgResult.HasValue && dlgResult.Value)
            {
                var file = this.Ref.AddFile(dlgdc.FinalName);
                using (var stream = new StreamWriter(file.FilePath))
                {
                    stream.Write(dlgdc.SelectedFileType.GetTemplate());
                }
                Workspace.Instance.CreateOrFocusDocument(file);
                SolutionPane.Instance.RebuildTree(Workspace.Instance.Solution);
            }
        });
        public ICommand CmdContextMenu_Add_ExistingItem => new RelayCommand((p) =>
        {
            var dlg = new OpenFileDialog() { InitialDirectory = this.Ref.FilePath };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var uri = new Uri(dlg.FileName, UriKind.RelativeOrAbsolute);
                if (!this.Ref.FileUri.IsBaseOf(uri))
                {
                    try
                    {
                        var filePath = Path.Combine(this.Ref.FilePath, Path.GetFileName(dlg.FileName));
                        uri = new Uri(filePath, UriKind.RelativeOrAbsolute);
                        File.Copy(dlg.FileName, Path.Combine(this.Ref.FilePath, filePath));

                    }
                    catch (Exception ex)
                    {
                        App.ShowOperationFailedMessageBox(ex);
                    }
                }

                var file = this.Ref.AddFile(this.Ref.FileUri.MakeRelativeUri(uri).OriginalString);
                Workspace.Instance.CreateOrFocusDocument(file);
                SolutionPane.Instance.RebuildTree(Workspace.Instance.Solution);
            }
        });
        public ICommand CmdContextMenu_Add_NewFolder => new RelayCommand((p) => { this.Add(new ProjectFolderModelView(Properties.Localization.NewFolder, this)); });
        public ICommand CmdContextMenu_Rename => new RelayCommand((p) => { this.IsInRenameMode = true; });
        public ICommand CmdTextBoxLostKeyboardFocus => new RelayCommand((p) => { this.IsInRenameMode = false; });
    }
}

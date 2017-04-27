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
using ArmA.Studio.Data.UI.Commands;

namespace ArmA.Studio.DataContext.SolutionPaneUtil
{
    public class ProjectFolderModelView : INotifyPropertyChanged, IList<object>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        private readonly IList<object> InnerList;

        public IEnumerator<object> GetEnumerator() => InnerList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

        public int Count => this.InnerList.Count;
        public bool IsReadOnly => this.InnerList.IsReadOnly;
        public object this[int index] { get { return this.InnerList[index]; } set { this.InnerList[index] = value; } }
        public int IndexOf(object item) => this.InnerList.IndexOf(item);
        public void Insert(int index, object item) => this.InnerList.Insert(index, item);
        public void RemoveAt(int index) => this.InnerList.RemoveAt(index);
        public void Add(object item) => this.InnerList.Add(item);
        public void Clear() => this.InnerList.Clear();
        public bool Contains(object item) => this.InnerList.Contains(item);
        public void CopyTo(object[] array, int arrayIndex) => this.InnerList.CopyTo(array, arrayIndex);
        public bool Remove(object item) => this.InnerList.Remove(item);

        public ICommand CmdContextMenuOpening => new RelayCommand((p) =>
        {
            if (this.IsInRenameMode)
                return;
            this.IsSelected = true;
        });

        //ToDo: Make folder actually be renamed when this changes
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
                RaisePropertyChanged();
            }
        }
        private string _Name;

        public bool IsExpanded { get { return this._IsExpanded; } set { this._IsExpanded = value; RaisePropertyChanged(); } }
        private bool _IsExpanded;

        public bool IsSelected { get { return this._IsSelected; } set { this._IsSelected = value; RaisePropertyChanged(); } }
        private bool _IsSelected;

        public bool IsInRenameMode { get { return this._IsInRenameMode; } set { this._IsInRenameMode = value; RaisePropertyChanged(); } }
        private bool _IsInRenameMode;

        public object Parent { get; private set; }

        public ProjectFolderModelView(string name, object parent)
        {
            this.Name = name;
            this.InnerList = new List<object>();
            this.Parent = parent;
        }
        public ICommand CmdContextMenu_Add_NewItem => new RelayCommand((p) =>
        {
            var dlgdc = new Dialogs.CreateNewFileDialogDataContext();
            var dlg = new Dialogs.CreateNewFileDialog(dlgdc);
            var dlgResult = dlg.ShowDialog();
            if (dlgResult.HasValue && dlgResult.Value)
            {
                var pathList = new List<string>();
                object cur = this;
                while (!(cur is ProjectModelView))
                {
                    pathList.Add((cur as ProjectFolderModelView).Name);
                    cur = (cur as ProjectFolderModelView).Parent;
                }
                pathList.Reverse();
                pathList.Add(string.Empty);
                var path = string.Join("/", pathList);

                var file = (cur as ProjectModelView).Ref.AddFile(string.Concat(path, dlgdc.FinalName));
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
            var pathList = new List<string>();
            object cur = this;
            while (!(cur is ProjectModelView))
            {
                pathList.Add((cur as ProjectFolderModelView).Name);
                cur = (cur as ProjectFolderModelView).Parent;
            }
            pathList.Reverse();
            pathList.Add(string.Empty);
            var path = string.Join("/", pathList);
            var thisUri = new Uri(Path.Combine((cur as ProjectModelView).Ref.FilePath, path).Replace('/', '\\'));
            var dlg = new OpenFileDialog() { InitialDirectory = thisUri.LocalPath };
            if (dlg.ShowDialog() == DialogResult.OK)
            {

                var uri = new Uri(dlg.FileName, UriKind.RelativeOrAbsolute);
                if (!thisUri.IsBaseOf(uri))
                {
                    try
                    {
                        var filePath = Path.Combine(thisUri.LocalPath, Path.GetFileName(dlg.FileName));
                        uri = new Uri(filePath, UriKind.RelativeOrAbsolute);
                        File.Copy(dlg.FileName, filePath);
                    }
                    catch (Exception ex)
                    {
                        App.ShowOperationFailedMessageBox(ex);
                    }
                }

                var file = (cur as ProjectModelView).Ref.AddFile((cur as ProjectModelView).Ref.FileUri.MakeRelativeUri(uri).OriginalString);
                Workspace.Instance.CreateOrFocusDocument(file);
                SolutionPane.Instance.RebuildTree(Workspace.Instance.Solution);
            }

        });
        public ICommand CmdContextMenu_Add_NewFolder => new RelayCommand((p) => { this.Add(new ProjectFolderModelView(Properties.Localization.NewFolder, this)); });
        public ICommand CmdContextMenu_OpenInExplorer => new RelayCommand((p) => { /* System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", this.Ref.FilePath)); */ });
        public ICommand CmdContextMenu_Delete => new RelayCommand((p) =>
        {
            if (!this.CloseChildDocumentsIfOpen())
                return;
            var pathList = new List<string>();
            object cur = this.Parent;
            while (!(cur is ProjectModelView))
            {
                pathList.Add((cur as ProjectFolderModelView).Name);
                cur = (cur as ProjectFolderModelView).Parent;
            }
            pathList.Reverse();
            var path = string.Join("/", pathList.Concat(new[] { this.Name, string.Empty }));
            var gather = new List<ProjectFile>();
            foreach (var it in (cur as ProjectModelView).Ref)
            {
                if (it.ProjectRelativePath.StartsWith(path, StringComparison.InvariantCultureIgnoreCase))
                {
                    gather.Add(it);
                }
            }
            foreach(var it in gather)
            {
                it.Delete();
            }
            SolutionPane.Instance.RebuildTree(Workspace.Instance.Solution);
        });

        public ICommand CmdMouseDoubleClick => new RelayCommand((p) => { });
        private string NameBeforeRename = null;
        public ICommand CmdContextMenu_Rename => new RelayCommand((p) =>
        {
            if (!this.CloseChildDocumentsIfOpen())
                return;
            this.NameBeforeRename = this.Name;
            this.IsInRenameMode = true;
        });
        
        public ICommand CmdTextBoxLostKeyboardFocus => new RelayCommand((p) =>
        {
            this.IsInRenameMode = false;
            var pathList = new List<string>();
            object cur = this.Parent;
            while (!(cur is ProjectModelView))
            {
                pathList.Add((cur as ProjectFolderModelView).Name);
                cur = (cur as ProjectFolderModelView).Parent;
            }
            pathList.Reverse();
            var pathNew = string.Join("/", pathList.Concat(new[] { this.Name, string.Empty }));
            var pathOld = string.Join("/", pathList.Concat(new[] { this.NameBeforeRename, string.Empty }));
            if (pathNew == pathOld)
                return;
            foreach (var it in (cur as ProjectModelView).Ref)
            {
                if (it.ProjectRelativePath.StartsWith(pathOld, StringComparison.InvariantCultureIgnoreCase))
                {
                    var partialPath = it.ProjectRelativePath.Remove(0, pathOld.Length);
                    it.MoveRelative(string.Concat(pathNew, partialPath));
                }
            }
        });

        private bool CloseChildDocumentsIfOpen()
        {
            foreach (var it in this)
            {
                if (it is ProjectFolderModelView)
                {
                    if (!(it as ProjectFolderModelView).CloseChildDocumentsIfOpen())
                        return false;
                }
                else if(it is ProjectFileModelView)
                {
                    if (!(it as ProjectFileModelView).CloseDocumentIfOpen())
                        return false;
                }
            }
            return true;
        }
    }
}

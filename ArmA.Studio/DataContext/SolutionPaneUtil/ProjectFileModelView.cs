using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ArmA.Studio.Data;
using ArmA.Studio.Data.UI.Commands;

namespace ArmA.Studio.DataContext.SolutionPaneUtil
{
    public class ProjectFileModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ProjectFile Ref { get; private set; }

        public bool IsInRenameMode { get { return this._IsInRenameMode; } set { this._IsInRenameMode = value; RaisePropertyChanged(); } }
        private bool _IsInRenameMode;

        public bool IsSelected { get { return this._IsSelected; } set { this._IsSelected = value; RaisePropertyChanged(); } }
        private bool _IsSelected;

        public bool IsExpanded { get { return this._IsExpanded; } set { this._IsExpanded = value; RaisePropertyChanged(); } }
        private bool _IsExpanded;

        public ICommand CmdMouseDoubleClick => new RelayCommand((p) => { Workspace.Instance.CreateOrFocusDocument((p as ProjectFileModelView).Ref); });
        public ICommand CmdContextMenuOpening => new RelayCommand((p) =>
        {
            if (this.IsInRenameMode)
                return;
            this.IsSelected = true;
        });

        public ICommand CmdContextMenu_Rename => new RelayCommand((p) =>
        {
            var opendoc = Workspace.Instance.AvalonDockDocuments.FirstOrDefault((doc) => doc.FileReference == this.Ref);
            if(opendoc != null)
            {
                opendoc.CmdClosing.Execute(null);
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (Workspace.Instance.AvalonDockDocuments.FirstOrDefault((doc) => doc.FileReference == this.Ref) == null)
                        this.IsInRenameMode = true;
                });
            }
            else
            {
                this.IsInRenameMode = true;
            }
        });
        public ICommand CmdTextBoxLostKeyboardFocus => new RelayCommand((p) => { this.IsInRenameMode = false; });
        public ICommand CmdContextMenu_OpenInExplorer => new RelayCommand((p) => { System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", this.Ref.FilePath.Replace('/', '\\'))); });
        public ICommand CmdContextMenu_Delete => new RelayCommand((p) =>
        {
            if (this.CloseDocumentIfOpen())
            {
                this.Ref.Delete();
                SolutionPane.Instance.RebuildTree(Workspace.Instance.Solution);
            }
        });

        /// <summary>
        /// Closes the open document in case the user currently has it open.
        /// </summary>
        /// <param name="file">The file reference of the document.</param>
        /// <returns>true if file was closed or never has been open, false if user cancelled the close request.</returns>
        public bool CloseDocumentIfOpen()
        {
            var opendoc = Workspace.Instance.AvalonDockDocuments.FirstOrDefault((doc) => doc.FileReference == this.Ref);
            if (opendoc != null)
            {
                opendoc.CmdClosing.Execute(null);
                bool flag = false;
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (Workspace.Instance.AvalonDockDocuments.FirstOrDefault((doc) => doc.FileReference == this.Ref) == null)
                    {
                        flag = true;
                    }
                });
                return flag;
            }
            else
            {
                return true;
            }
        }

        public ProjectFileModelView(ProjectFile pff)
        {
            this.Ref = pff;
        }
    }
}

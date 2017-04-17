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

        public ICommand CmdMouseDoubleClick => new RelayCommand((p) => { Workspace.Instance.CreateOrFocusDocument((p as ProjectFileModelView).Ref); });
        public ICommand CmdContextMenuOpening => new RelayCommand((p) =>
        {
            if (this.IsInRenameMode)
                return;
            this.IsSelected = true;
        });

        public ICommand CmdContextMenu_Rename => new RelayCommand((p) => { this.IsInRenameMode = true; });
        public ICommand CmdContextMenu_OpenInExplorer => new RelayCommand((p) => { System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", this.Ref.FilePath)); });
        public ICommand CmdContextMenu_Delete => new RelayCommand((p) => { });

        public ProjectFileModelView(ProjectFile pff)
        {
            this.Ref = pff;
        }
    }
}

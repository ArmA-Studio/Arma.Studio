using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data;

namespace ArmA.Studio.DataContext.SolutionPaneUtil
{
    public class ProjectFileModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ProjectFile Ref { get; private set; }

        public bool IsInRenameMode { get { return this._IsInRenameMode; } set { this._IsInRenameMode = value; RaisePropertyChanged(); } }
        private bool _IsInRenameMode;

        public ProjectFileModelView(ProjectFile pff)
        {
            this.Ref = pff;
        }

    }
}

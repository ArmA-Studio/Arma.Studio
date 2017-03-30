using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data;

namespace ArmA.Studio.DataContext.SolutionPaneUtil
{
    public class ProjectFileFolderModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public string Name { get; private set; }
        public ProjectFileFolder Ref { get; private set; }
        public List<ProjectFileFolderModelView> Children { get; private set; }

        public bool IsExpaned  { get { return this._IsExpaned; } set { this._IsExpaned = value; RaisePropertyChanged(); } }
        private bool _IsExpaned;

        public ProjectFileFolderModelView(string name)
        {
            this.Name = name;
            this.Ref = null;
            this.Children = new List<ProjectFileFolderModelView>();
        }
        public ProjectFileFolderModelView(ProjectFileFolder pff)
        {
            this.Ref = pff;
            this.Children = new List<ProjectFileFolderModelView>();
            this.Name = pff.FileName;
        }

    }
}

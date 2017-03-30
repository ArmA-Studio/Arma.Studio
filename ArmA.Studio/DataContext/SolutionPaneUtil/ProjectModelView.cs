using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data;

namespace ArmA.Studio.DataContext.SolutionPaneUtil
{
    public class ProjectModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public Project Ref { get; private set; }
        public List<ProjectFileFolderModelView> Children { get; private set; }

        public ProjectModelView(Project p)
        {
            this.Ref = p;
            this.Children = new List<ProjectFileFolderModelView>();
        }
    }
}

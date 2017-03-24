using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.Collections;

namespace ArmA.Studio.Data
{
    public abstract class ProjectFileFolder : IComparable, INotifyPropertyChanged, IEnumerable<ProjectFileFolder>
    {
        public class File : ProjectFileFolder
        {

        }
        public class Folder : ProjectFileFolder
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public string Name { get { return this._Name; } set { if (this._Name == value) return; this._Name = value; RaisePropertyChanged(); } }
        private string _Name;

        public ObservableSortedCollection<ProjectFileFolder> Children { get; private set; }

        public Solution OwningSolution { get { Solution v; this.WeakOwningSolution.TryGetTarget(out v); return v; } set { this.WeakOwningSolution.SetTarget(value); } }
        private WeakReference<Solution> WeakOwningSolution;

        public Project OwningProject { get { Project v; this.WeakOwningProject.TryGetTarget(out v); return v; } set { this.WeakOwningProject.SetTarget(value); } }
        private WeakReference<Project> WeakOwningProject;

        public ProjectFileFolder Parent { get { ProjectFileFolder v; this.WeakParent.TryGetTarget(out v); return v; } set { this.WeakParent.SetTarget(value); } }
        private WeakReference<ProjectFileFolder> WeakParent;

        public ProjectFileFolder()
        {
            this.Children = new ObservableSortedCollection<ProjectFileFolder>();
            this.WeakOwningSolution = new WeakReference<Solution>(null);
            this.WeakOwningProject = new WeakReference<Project>(null);
            this.WeakParent = new WeakReference<ProjectFileFolder>(null);
            this.Name = string.Empty;
        }


        public int CompareTo(object obj)
        {
            return this.Name.CompareTo(obj);
        }

        public IEnumerator<ProjectFileFolder> GetEnumerator()
        {
            return this.Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator().Cast();
        }
    }
}
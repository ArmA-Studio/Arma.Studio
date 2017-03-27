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
    public class ProjectFileFolder : IComparable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public string ProjectRelativePath
        {
            get { return this._ProjectRelativePath; }
            set
            {
                if (this._ProjectRelativePath == value)
                    return;
                this._ProjectRelativePath = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ArmAPath));
                RaisePropertyChanged(nameof(FilePath));
                RaisePropertyChanged(nameof(FileUri));
            }
        }
        private string _ProjectRelativePath;
        public string ArmAPath { get { return string.Concat(this.OwningProject.ArmAPath, '\\', this.ProjectRelativePath); } }
        public string FilePath { get { return string.Concat(this.OwningProject.FilePath, '\\', this.ProjectRelativePath); } }
        public Uri FileUri { get { return new Uri(this.FilePath); } }

        public ObservableSortedCollection<ProjectFileFolder> Children { get; private set; }

        public Solution OwningSolution { get { Solution v; this.WeakOwningSolution.TryGetTarget(out v); return v; } set { this.WeakOwningSolution.SetTarget(value); } }
        private WeakReference<Solution> WeakOwningSolution;

        public Project OwningProject { get { Project v; this.WeakOwningProject.TryGetTarget(out v); return v; } set { this.WeakOwningProject.SetTarget(value); } }
        private WeakReference<Project> WeakOwningProject;


        public bool IsFolder { get { return this._IsFolder; } set { if (this._IsFolder == value) return; this._IsFolder = value; this.RaisePropertyChanged(); } }
        private bool _IsFolder;

        private ProjectFileFolder() : this(string.Empty) { }
        public ProjectFileFolder(string projectRelativePath)
        {
            this.Children = new ObservableSortedCollection<ProjectFileFolder>();
            this.WeakOwningSolution = new WeakReference<Solution>(null);
            this.WeakOwningProject = new WeakReference<Project>(null);
            this.ProjectRelativePath = projectRelativePath;
        }


        public int CompareTo(object obj)
        {
            if(obj is ProjectFileFolder)
            {
                if (this.IsFolder && !(obj as ProjectFileFolder).IsFolder)
                    return 1;
                else if (!this.IsFolder && (obj as ProjectFileFolder).IsFolder)
                    return -1; 
            }
            return this.ProjectRelativePath.CompareTo(obj);
        }
    }
}
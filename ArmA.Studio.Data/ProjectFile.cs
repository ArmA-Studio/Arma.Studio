using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.Collections;

namespace ArmA.Studio.Data
{
    public class ProjectFile : IComparable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public string ProjectRelativePath
        {
            get { return this._ProjectRelativePath; }
            set
            {
                value = value.Replace('\\', '/').TrimStart('/');
                if (this._ProjectRelativePath == value)
                    return;
                this._ProjectRelativePath = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ArmAPath));
                RaisePropertyChanged(nameof(FilePath));
                RaisePropertyChanged(nameof(FileName));
                RaisePropertyChanged(nameof(FileExtension));
                RaisePropertyChanged(nameof(FileUri));
            }
        }
        private string _ProjectRelativePath;
        public string ArmAPath { get { return string.Concat(this.OwningProject.ArmAPath, '\\', this.ProjectRelativePath); } }
        public string FilePath { get { return Path.Combine(this.OwningProject.FilePath, this.ProjectRelativePath); } }
        public string FileName
        {
            get { return Path.GetFileName(this.ProjectRelativePath); }
            set
            {
                var newFilePath = Path.Combine(Path.GetDirectoryName(this.FilePath), value);
                var newRelativePath = Path.Combine(Path.GetDirectoryName(this.ProjectRelativePath), value);
                File.Move(this.FilePath, newFilePath);
                this.ProjectRelativePath = newRelativePath;
            }
        }
        public string FileExtension { get { return Path.GetExtension(this.ProjectRelativePath); } }

        public Uri FileUri { get { return new Uri(this.FilePath); } }

        public Solution OwningSolution { get { Solution v; this.WeakOwningSolution.TryGetTarget(out v); return v; } set { this.WeakOwningSolution.SetTarget(value); } }
        private WeakReference<Solution> WeakOwningSolution;

        public Project OwningProject { get { Project v; this.WeakOwningProject.TryGetTarget(out v); return v; } set { this.WeakOwningProject.SetTarget(value); } }
        private WeakReference<Project> WeakOwningProject;

        internal ProjectFile() : this(string.Empty) { }
        public ProjectFile(string projectRelativePath)
        {
            this.WeakOwningSolution = new WeakReference<Solution>(null);
            this.WeakOwningProject = new WeakReference<Project>(null);
            this.ProjectRelativePath = projectRelativePath;
        }


        public int CompareTo(object obj)
        {
            if (obj is ProjectFile)
            {
                return this.ProjectRelativePath.CompareTo((obj as ProjectFile).ProjectRelativePath);
            }
            return this.ProjectRelativePath.CompareTo(obj);
        }
    }
}
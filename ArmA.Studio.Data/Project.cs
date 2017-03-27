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
    public class Project : IComparable, INotifyPropertyChanged, IEnumerable<ProjectFileFolder>
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public string Name { get { return this._Name; } set { if (this._Name == value) return; this._Name = value; RaisePropertyChanged(); } }
        private string _Name;

        public EProjectType ProjectType { get { return this._ProjectType; } set { if (this._ProjectType == value) return; this._ProjectType = value; RaisePropertyChanged(); } }
        private EProjectType _ProjectType;

        public ObservableSortedCollection<ProjectFileFolder> Children { get; private set; }

        public Solution OwningSolution { get { Solution v; this.WeakOwningSolution.TryGetTarget(out v); return v; } set { this.WeakOwningSolution.SetTarget(value); } }

        public string ProjectPath
        {
            get { return this._ProjectPath; }
            set
            {
                if (this._ProjectPath == value)
                    return;
                this._ProjectPath = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ArmAPath));
                RaisePropertyChanged(nameof(FilePath));
                RaisePropertyChanged(nameof(FileUri));
            }
        }
        private string _ProjectPath;

        public string ArmAPath { get { return this._ArmAPath; } set { if (this._ArmAPath == value) return; this._ArmAPath = value; RaisePropertyChanged(); } }
        private string _ArmAPath;
        public string FilePath { get { return this._FilePath; } set { if (this._FilePath == value) return; this._FilePath = value; RaisePropertyChanged(); } }
        private string _FilePath;
        public Uri FileUri { get { return new Uri(this.FilePath); } }

        private WeakReference<Solution> WeakOwningSolution;

        public Project()
        {
            this.Children = new ObservableSortedCollection<ProjectFileFolder>();
            this.WeakOwningSolution = new WeakReference<Solution>(null);
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

        public ProjectFileFolder FindFileFolder(Uri uri)
        {
            var res = FindFileFolderOrNull(uri);
            if (res == null)
                throw new KeyNotFoundException();
            return res;
        }
        public ProjectFileFolder FindFileFolderOrNull(Uri uri)
        {
            foreach (var it in this)
            {
                if(it.FileUri.Equals(uri))
                {
                    return it;
                }
            }
            return null;
        }

        /// <summary>
        /// Receives or creates a <see cref="ProjectFileFolder"/> inside of this <see cref="Project"/>.
        /// </summary>
        /// <param name="path">Path to the FileFolder.</param>
        /// <returns>reference to a <see cref="ProjectFileFolder"/>.</returns>
        /// <exception cref="ArgumentException">Will be thrown when the provided <see cref="Uri"/> is not leading to this <see cref="Project"/>.</exception>
        public ProjectFileFolder GetOrCreateFileFolder(Uri path)
        {
            if (this.FileUri.IsBaseOf(path))
                throw new ArgumentException("Basepath missmatch", nameof(path));
            foreach(var pff in this)
            {
                if (pff.FileUri.Equals(path))
                    return pff;
            }
            var tmp = new ProjectFileFolder(Uri.UnescapeDataString(this.FileUri.MakeRelativeUri(path).ToString()));
            this.Children.Add(tmp);
            tmp.OwningProject = this;
            tmp.OwningSolution = this.OwningSolution;
            return tmp;
        }
    }
}
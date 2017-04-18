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
    public class Project : IComparable, INotifyPropertyChanged, IEnumerable<ProjectFile>
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

#if DEBUG
        public static readonly List<string> ValidFileExtensions = new List<string>(new [] { ".sqf", ".cpp", ".ext", ".hpp" });
#else
        public static readonly List<string> ValidFileExtensions = new List<string>();
#endif

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public string Name { get { return this._Name; } set { if (this._Name == value) return; this._Name = value; RaisePropertyChanged(); } }
        private string _Name;

        public EProjectType ProjectType { get { return this._ProjectType; } set { if (this._ProjectType == value) return; this._ProjectType = value; RaisePropertyChanged(); } }
        private EProjectType _ProjectType;

        public ObservableSortedCollection<ProjectFile> Children { get; private set; }

        public Solution OwningSolution { get { Solution v; this.WeakOwningSolution.TryGetTarget(out v); return v; } set { this.WeakOwningSolution.SetTarget(value); } }
        private WeakReference<Solution> WeakOwningSolution;

        public string FilePath
        {
            get { return this._FilePath; }
            set
            {
                if (this._FilePath == value)
                    return;
                this._FilePath = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(FilePath));
                RaisePropertyChanged(nameof(FileUri));
            }
        }
        private string _FilePath;

        public string ArmAPath
        {
            get
            {
                return this._ArmAPath;
            }
            set
            {
                if (this._ArmAPath == value)
                    return;
                if (this.ProjectType == EProjectType.Addon)
                {
                    try
                    {
                        using (var stream = new StreamWriter(Path.Combine(this.FilePath, "$PBOPREFIX$")))
                        {
                            stream.Write(value);
                            stream.Flush();
                        }
                        this._ArmAPath = value;
                    }
                    catch(Exception ex)
                    {
                        Virtual.ShowOperationFailedMessageBox(ex);
                    }
                }
                else
                {
                    this._ArmAPath = value;
                }
                RaisePropertyChanged();
            }
        }
        private string _ArmAPath;
        public Uri FileUri { get { return new Uri(string.Concat(this.FilePath, '/')); } }


        public Project()
        {
            this.Children = new ObservableSortedCollection<ProjectFile>();
            this.WeakOwningSolution = new WeakReference<Solution>(null);
            this.Name = string.Empty;
        }


        public int CompareTo(object obj)
        {
            if (obj is Project)
            {
                return this.Name.CompareTo((obj as Project).Name);
            }
            return this.Name.CompareTo(obj);
        }

        public IEnumerator<ProjectFile> GetEnumerator()
        {
            return this.Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator().Cast();
        }

        internal void RemoveFile(ProjectFile projectFile)
        {
            this.Children.Remove(projectFile);
        }

        public ProjectFile FindFileFolder(Uri uri)
        {
            var res = FindFileFolderOrNull(uri);
            if (res == null)
                throw new KeyNotFoundException();
            return res;
        }
        public ProjectFile FindFileFolderOrNull(Uri uri)
        {
            foreach (var it in this)
            {
                if (it.FileUri.Equals(uri))
                {
                    return it;
                }
            }
            return null;
        }

        /// <summary>
        /// Receives or creates a <see cref="ProjectFile"/> inside of this <see cref="Project"/>.
        /// </summary>
        /// <param name="path">Path to the FileFolder.</param>
        /// <returns>reference to a <see cref="ProjectFile"/>.</returns>
        /// <exception cref="ArgumentException">Will be thrown when the provided <see cref="Uri"/> is not leading to this <see cref="Project"/>.</exception>
        public ProjectFile GetOrCreateFileFolder(Uri path)
        {
            if (this.FileUri.IsBaseOf(path))
                throw new ArgumentException("Basepath missmatch", nameof(path));
            foreach (var pff in this)
            {
                if (pff.FileUri.Equals(path))
                    return pff;
            }
            var tmp = new ProjectFile(Uri.UnescapeDataString(this.FileUri.MakeRelativeUri(path).ToString()));
            this.Children.Add(tmp);
            tmp.OwningProject = this;
            tmp.OwningSolution = this.OwningSolution;
            return tmp;
        }

        internal void Scan()
        {
            Logger.Info($"Scanning for project related files in '{this.FilePath}'...");
            var origPathDepth = this.FilePath.Count((c) => Path.DirectorySeparatorChar == c || Path.AltDirectorySeparatorChar == c);

            foreach (var fpath in Directory.EnumerateFiles(this.FilePath, "*", SearchOption.AllDirectories))
            {
                var fname = Path.GetFileName(fpath);
                var ext = Path.GetExtension(fpath);
                var depth = fpath.Count((c) => Path.DirectorySeparatorChar == c || Path.AltDirectorySeparatorChar == c);
                if (depth - origPathDepth == 1 && fname.Equals("$PBOPREFIX$"))
                {
                    Logger.Info($"\tLocated ArmAPath at '{fpath.Remove(0, this.FilePath.Length)}'.");
                    using (var reader = new StreamReader(fpath))
                    {
                        this.ArmAPath = reader.ReadToEnd();
                    }
                }
                else if (ValidFileExtensions.Any((s) => s.Equals(ext, StringComparison.InvariantCultureIgnoreCase)))
                {
                    this.AddFile(fpath.Remove(0, this.FilePath.Length));
                    Logger.Info($"\tAdded '{fpath.Remove(0, this.FilePath.Length)}' to project.");
                }
            }
        }

        public ProjectFile AddFile(string projectRelativePath)
        {
            var pf = new ProjectFile(projectRelativePath) { OwningProject = this, OwningSolution = this.OwningSolution };
            this.Children.Add(pf);
            return pf;
        }

        public override string ToString()
        {
            return $"Project '{this.Name}'";
        }
    }
}
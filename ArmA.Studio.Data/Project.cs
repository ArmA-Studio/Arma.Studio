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
        public const string CONST_PBOPREFIXFILENAME = "$PBOPREFIX$";
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

#if DEBUG
        public static readonly List<string> ValidFileExtensions = new List<string>(new[] { ".sqf", ".cpp", ".ext", ".hpp" });
#else
        public static readonly List<string> ValidFileExtensions = new List<string>();
#endif

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public string Name
        {
            get { return this._Name; }
            set
            {
                if (this._Name == value)
                    return;
                this._Name = value;
                this.RaisePropertyChanged();
            }
        }
        private string _Name;

        public EProjectType ProjectType
        {
            get { return this._ProjectType; }
            set
            {
                if (this._ProjectType == value)
                    return;
                this._ProjectType = value;
                this.RaisePropertyChanged();
            }
        }
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
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.FilePath));
                this.RaisePropertyChanged(nameof(this.FileUri));
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
                        using (var stream = new StreamWriter(Path.Combine(this.FilePath, CONST_PBOPREFIXFILENAME)))
                        {
                            this.SkipPboPrefixUpdate = true;
                            stream.Write(value);
                            stream.Flush();
                        }
                        this._ArmAPath = value;
                    }
                    catch (Exception ex)
                    {
                        Virtual.ShowOperationFailedMessageBox(ex);
                    }
                }
                else
                {
                    this._ArmAPath = value;
                }
                this.RaisePropertyChanged();
            }
        }


        private string _ArmAPath;
        public Uri FileUri => new Uri(string.Concat(this.FilePath, '/'));


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
            var res = this.FindFileFolderOrNull(uri);
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
            var tmp = new ProjectFile(Uri.UnescapeDataString(this.FileUri.MakeRelativeUri(path).LocalPath));
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
                    try
                    {
                        using (var reader = new StreamReader(fpath))
                        {
                            this._ArmAPath = reader.ReadToEnd();
                        }
                    }
                    catch (Exception ex)
                    {
                        Virtual.ShowOperationFailedMessageBox(ex);
                    }
                }
                else if (ValidFileExtensions.Any((s) => s.Equals(ext, StringComparison.InvariantCultureIgnoreCase)))
                {
                    this.AddFile(fpath.Remove(0, this.FilePath.Length));
                    Logger.Info($"\tAdded '{fpath.Remove(0, this.FilePath.Length)}' to project.");
                }
            }
        }

        internal ProjectFile GetFileFromArmAPath(string armaPath)
        {
            if (this.ProjectType == EProjectType.Addon)
            {
                if (!armaPath.StartsWith(this.ArmAPath, StringComparison.InvariantCultureIgnoreCase))
                    return null;
                foreach (var pff in this.Children)
                {
                    if (armaPath.StartsWith(pff.ArmAPath, StringComparison.InvariantCultureIgnoreCase))
                        return pff;
                }
                return null;
            }
            else if (this.ProjectType == EProjectType.Mission)
            {
                //ToDo: https://github.com/ArmA-Studio/ArmA.Studio/issues/20
                //Right now just a copy of the above
                if (!armaPath.StartsWith(this.ArmAPath, StringComparison.InvariantCultureIgnoreCase))
                    return null;
                foreach (var pff in this.Children)
                {
                    if (armaPath.StartsWith(pff.ArmAPath, StringComparison.InvariantCultureIgnoreCase))
                        return pff;
                }
                return null;
            }
            else
            {
                throw new Exception("NOT IMPLEMENTED TYPE");
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


        internal void DeserializeCallback()
        {

            foreach (var it in this.Children)
            {
                if (!File.Exists(it.FilePath))
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() => it.Exists = false);
                }
            }
        }
        internal void AddedCallback()
        {
            this.PrepareFileSystemWatcher();
        }
        internal void RemovedCallback()
        {
            this.RemoveFileSystemWatcher();
        }

        #region FileSystemWatcher
        private FileSystemWatcher FSWatcher;
        private void PrepareFileSystemWatcher()
        {
            if(!Directory.Exists(this.FilePath))
            {
                Directory.CreateDirectory(this.FilePath);
            }
            this.FSWatcher = new FileSystemWatcher(this.FilePath);
            this.FSWatcher.Changed += FileSystemWatcher_Changed;
            this.FSWatcher.Created += FileSystemWatcher_Created;
            this.FSWatcher.Deleted += FileSystemWatcher_Deleted;
            this.FSWatcher.Error += FileSystemWatcher_Error;
            this.FSWatcher.Renamed += FileSystemWatcher_Renamed;
            this.FSWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            this.FSWatcher.Filter = "*";
            this.FSWatcher.EnableRaisingEvents = true;
        }
        private void RemoveFileSystemWatcher()
        {
            this.FSWatcher.Dispose();
        }
        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            var pathOld = new Uri(e.OldFullPath, UriKind.Absolute);
            var pathNew = new Uri(e.FullPath, UriKind.Absolute);
            if (this.FileUri.MakeRelativeUri(pathNew).ToString().Equals(CONST_PBOPREFIXFILENAME, StringComparison.InvariantCultureIgnoreCase))
            {
                this.UpdateArmAPathFromFile(e.FullPath);
                return;
            }
            var pFile = this.Children.FirstOrDefault((pf) => pf.FileUri.Equals(pathOld));
            if (pFile == null)
            {
                Logger.Info($"File got renamed: project '{this.Name}' old '{this.FileUri.MakeRelativeUri(pathOld).ToString()}' new '{this.FileUri.MakeRelativeUri(pathNew).ToString()}'");
                return;
            }
            System.Windows.Application.Current.Dispatcher.Invoke(() => pFile.ProjectRelativePath = this.FileUri.MakeRelativeUri(pathNew).ToString());
        }

        private void FileSystemWatcher_Error(object sender, ErrorEventArgs e)
        {
            Logger.Error(e.GetException());
        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            var path = new Uri(e.FullPath, UriKind.Absolute);
            var pFile = this.Children.FirstOrDefault((pf) => pf.FileUri.Equals(path));
            if (pFile == null)
            {
                Logger.Info($"File got deleted: project '{this.Name}' affected '{this.FileUri.MakeRelativeUri(path).ToString()}'");
                return;
            }
            System.Windows.Application.Current.Dispatcher.Invoke(() => pFile.Exists = false);
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            var path = new Uri(e.FullPath, UriKind.Absolute);
            if (this.FileUri.MakeRelativeUri(path).ToString().Equals(CONST_PBOPREFIXFILENAME, StringComparison.InvariantCultureIgnoreCase))
            {
                this.UpdateArmAPathFromFile(e.FullPath);
                return;
            }
            var pFile = this.Children.FirstOrDefault((pf) => pf.FileUri.Equals(path));
            if (pFile == null)
            {
                Logger.Info($"File got created: project '{this.Name}' affected '{this.FileUri.MakeRelativeUri(path).ToString()}'");
                return;
            }
            System.Windows.Application.Current.Dispatcher.Invoke(() => pFile.Exists = true);
        }
        private bool SkipPboPrefixUpdate = false;
        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            var path = new Uri(e.FullPath, UriKind.Absolute);
            if (this.FileUri.MakeRelativeUri(path).ToString().Equals(CONST_PBOPREFIXFILENAME, StringComparison.InvariantCultureIgnoreCase))
            {
                if(this.SkipPboPrefixUpdate)
                {
                    this.SkipPboPrefixUpdate = false;
                    return;
                }
                this.UpdateArmAPathFromFile(e.FullPath);
                return;
            }
        }
        #endregion

        private void UpdateArmAPathFromFile(string path)
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    this._ArmAPath = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Virtual.ShowOperationFailedMessageBox(ex);
            }
        }
    }
}
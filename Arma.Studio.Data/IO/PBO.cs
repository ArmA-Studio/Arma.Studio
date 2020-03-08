using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arma.Studio.Data.IO
{
    public class PBO : FileFolderBase, ICollection<FileFolderBase>, UI.IPropertyHost, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private readonly ObservableCollection<FileFolderBase> Inner;

        /// <summary>
        /// The PBO Prefix
        /// </summary>
        [UI.Property("$PBOPREFIX$")]
        public string Prefix
        {
            get => this._Prefix;
            set
            {
                value = value.Trim('/', '\\', ' ', '\t', '\r', '\n').Replace('/', '\\');

                if (this._Prefix == value)
                {
                    return;
                }
                this._Prefix = value;
                this.RaisePropertyChanged();
                if (value != null)
                {
                    System.IO.File.WriteAllText(System.IO.Path.Combine(this.FullPath, "$PBOPREFIX$"), value);
                }
            }
        }
        private string _Prefix;


        public PBO()
        {
            this.Inner = new ObservableCollection<FileFolderBase>();
            this.Inner.CollectionChanged += this.Inner_CollectionChanged;
        }

        private void Inner_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }

        public void Sort()
        {
            var sortableList = this.Inner.ToList();
            sortableList.Sort((left, right) =>
            {
                if ((left is File && right is File) || (left is Folder && right is Folder))
                {
                    return left.Name.CompareTo(right.Name);
                }
                else
                {
                    return left is File ? 1 : -1;
                }
            });

            for (int i = 0; i < sortableList.Count; i++)
            {
                this.Inner.Move(this.Inner.IndexOf(sortableList[i]), i);
            }
        }

        public File Add(string text)
        {
            if (this.TryAdd(text, out var file))
            {
                return file;
            }
            else
            {
                throw new Exception("Cannot add to file.");
            }
        }
        public bool TryAdd(string text, out File f)
        {
            text = text.TrimStart('\\', '/');
            var elements = text.Split('\\', '/');
            var ffb = this.FirstOrDefault((it) => it.Name == elements[0]);
            if (elements.Length == 1)
            {
                this.Add(f = new File { Name = elements[0] });
                return true;
            }
            Folder folder;
            if (ffb == null)
            {
                folder = new Folder { Name = elements[0] };
                this.Add(folder);
            }
            else if (ffb is File)
            {
                f = default;
                return false;
            }
            else
            {
                folder = ffb as Folder;
            }
            for (int i = 1; i < elements.Length - 1; i++)
            {
                ffb = folder.FirstOrDefault((it) => it.Name == elements[i]);
                if (ffb == null)
                {
                    var tmp = new Folder { Name = elements[i] };
                    folder.Add(tmp);
                    folder = tmp;
                }
                else if (ffb is File)
                {
                    if (i == elements.Length - 1)
                    {
                        f = ffb as File;
                        return true;
                    }
                    f = default;
                    return false;
                }
                else
                {
                    folder = ffb as Folder;
                }
            }
            folder.Add(f = new File { Name = elements.Last() });
            return true;
        }

        private static IEnumerable<File> GetAllHelper(ICollection<FileFolderBase> col, Func<File, bool> func)
        {
            foreach (var it in col)
            {
                if (it is ICollection<FileFolderBase> othercol)
                {
                    foreach (var it2 in GetAllHelper(othercol, func))
                    {
                        yield return it2;
                    }
                }
                else if (it is File file && func(file))
                {
                    yield return file;
                }
            }
        }
        public IEnumerable<File> GetAll(Func<File, bool> func)
        {
            return GetAllHelper(this, func);
        }

        public delegate void RescanProgressReporter(string message, bool isIntermediate, double progress);
        /// <summary>
        /// Performs a scan of this <see cref="PBO"/>s folder
        /// and removes/adds all file changes.
        /// </summary>
        public void Rescan()
        {
            var filesHit = new List<File>();
            var files = System.IO.Directory.GetFiles(this.FullPath, "*.*", System.IO.SearchOption.AllDirectories);
            for (var i = 0; i < files.Length; i++)
            {
                var path = files[i];
                var relativePath = path.Substring(this.FullPath.Length);
                var extension = System.IO.Path.GetExtension(relativePath);
                var name = System.IO.Path.GetFileName(relativePath);
                if (name == "$PBOPREFIX$")
                {
                    this.Prefix = System.IO.File.ReadAllText(path);
                }
                else
                {
                    switch (extension.ToLower())
                    {
                        default:
                            continue;
                        case ".sqf":
                        case ".cpp":
                        case ".ext":
                        case ".hpp":
                        case ".ui":
                            filesHit.Add(this.Add(relativePath));
                            break;
                    }
                }
            }

            // Remove all unhit files
            bool recursive(FileFolderBase ffb)
            {
                switch (ffb)
                {
                    case File file:
                        return filesHit.Contains(file);
                    case Folder folder:
                        foreach (var it in folder.ToArray())
                        {
                            if (!recursive(it))
                            {
                                folder.Remove(it);
                            }
                        }
                        folder.Sort();
                        return folder.Any();
                    default:
                        throw new Exception("Error Unknown. Expected File or Folder.");
                }
            }
            foreach (var it in this.ToArray())
            {
                if (!recursive(it))
                {
                    this.Remove(it);
                }
            }
            this.Sort();
        }

        #region ICollection<FileFolderBase>
        public int Count => this.Inner.Count;

        public bool IsReadOnly => ((ICollection<FileFolderBase>)this.Inner).IsReadOnly;

        public void Add(FileFolderBase item)
        {
            if (item.Parent != null)
            {
                (item.Parent as ICollection<FileFolderBase>).Remove(item);
            }
            this.Inner.Add(item);
            item.Parent = this;
        }

        public void Clear()
        {
            this.Inner.Clear();
        }

        public bool Contains(FileFolderBase item)
        {
            return this.Inner.Contains(item);
        }

        public void CopyTo(FileFolderBase[] array, int arrayIndex)
        {
            this.Inner.CopyTo(array, arrayIndex);
        }

        public IEnumerator<FileFolderBase> GetEnumerator()
        {
            return ((ICollection<FileFolderBase>)this.Inner).GetEnumerator();
        }

        public bool Remove(FileFolderBase item)
        {
            if (item.Parent == this)
            {
                item.Parent = null;
            }
            return this.Inner.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<FileFolderBase>)this.Inner).GetEnumerator();
        }
        #endregion
    }
}

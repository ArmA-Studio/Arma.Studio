using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Serialization;

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
        /// <summary>
        /// Walks the tree recursive to receive all <see cref="File"/>s
        /// that match the provided condition <paramref name="func"/>.
        /// </summary>
        /// <param name="func">A condition to match files for.</param>
        /// <returns>The <see cref="File"/>s that matched the condition.</returns>
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


        private Dictionary<string, Dictionary<string, object>> ValueStorage = new Dictionary<string, Dictionary<string, object>>();
        /// <summary>
        /// Adds the given value to the assemblies Property Storage.
        /// Note that <typeparamref name="T"/> is required to have the <see cref="SerializableAttribute"/> set and
        /// thus NEEDS TO BE serializable by the <see cref="System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/>.
        /// </summary>
        /// <remarks>
        /// This method should not be used for data, that is in any way important!
        /// The storage may always fail to load or the user may delete it for any reasons.
        /// Save important stuff in proper files.
        /// </remarks>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="key">The key to identify the property.</param>
        /// <param name="value">The value to set.</param>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void SetProperty<T>(string key, T value)
        {
            System.Diagnostics.Contracts.Contract.Requires(typeof(T).IsSerializable);
            var callingAssembly = System.Reflection.Assembly.GetCallingAssembly();
            if (!this.ValueStorage.TryGetValue(callingAssembly.FullName, out var storage))
            {
                storage = new Dictionary<string, object>();
                this.ValueStorage[callingAssembly.FullName] = storage;
            }
            storage[key] = value;
        }
        /// <summary>
        /// Receives a previously set value from the assemblies Property Storage.
        /// Note that proividing incorrect <typeparamref name="T"/>
        /// will result in <paramref name="defaultValue"/> being returned.
        /// </summary>
        /// <remarks>
        /// This method should not be used for data, that is in any way important!
        /// The storage may always fail to load or the user may delete it for any reasons.
        /// Save important stuff in proper files.
        /// </remarks>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="key">The key to identify the property.</param>
        /// <param name="defaultValue">
        /// A default value to return if resolving the
        /// <paramref name="key"/> failed or the value is not <typeparamref name="T"/>
        /// </param>
        /// <returns>The value or <paramref name="defaultValue"/></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public T GetProperty<T>(string key, T defaultValue)
        {
            var callingAssembly = System.Reflection.Assembly.GetCallingAssembly();
            if (!this.ValueStorage.TryGetValue(callingAssembly.FullName, out var storage))
            {
                storage = new Dictionary<string, object>();
                this.ValueStorage[callingAssembly.FullName] = storage;
            }
            if (storage.TryGetValue(key, out var value) && value is T t)
            {
                return t;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Writes all Property Storages to disk.
        /// </summary>
        public void WritePboProperties() => this.WritePboProperties(System.IO.Path.Combine(this.FullPath, ".arma.studio.storage.bin"));
        /// <summary>
        /// Writes all Property Storages to disk.
        /// </summary>
        /// <param name="path">The path to write this information to.</param>
        public void WritePboProperties(string path)
        {
            using (var fstream = System.IO.File.Open(path, System.IO.FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                var helperDictionary = this.ValueStorage
                    .SelectMany((it) => it.Value.Select((it2) => new { Assembly = it.Key, Key = it2.Key, it2.Value }))
                    .ToDictionary((it) => new Tuple<string, string>(it.Assembly, it.Key), (it) => it.Value);
                binaryFormatter.Serialize(fstream, helperDictionary);
            }
        }
        /// <summary>
        /// Reads Property Storages from disk.
        /// </summary>
        public void ReadPboProperties() => this.ReadPboProperties(System.IO.Path.Combine(this.FullPath, ".arma.studio.storage.bin"));
        /// <summary>
        /// Reads Property Storages from disk, replacing the currently active.
        /// </summary>
        /// <param name="path">The path to read this information from.</param>
        public void ReadPboProperties(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return;
            }
            try
            {
                using (var fstream = System.IO.File.OpenRead(path))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var res = (Dictionary<Tuple<string, string>, object>)binaryFormatter.Deserialize(fstream);
                    this.ValueStorage = res.GroupBy((it) => it.Key.Item1)
                        .Select((group) => new KeyValuePair<string, Dictionary<string, object>>(
                            group.Key, group.ToDictionary((it) => it.Key.Item2, (it) => it.Value)))
                        .ToDictionary((it) => it.Key, (it) => it.Value);
                }
            }
            catch { /* We do not care for broken data. Nothing important is supposed to be saved in here. */ }
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

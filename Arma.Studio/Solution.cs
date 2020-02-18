using Arma.Studio.Data.IO;
using Arma.Studio.Data.UI;
using Arma.Studio.Debugging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Arma.Studio
{
    public class Solution : IFileManagement, IDisposable, INotifyCollectionChanged
    {
        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        #region INotifyCollectionChanged
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        #endregion
        #region IFileManagement
        public FileFolderBase this[string fullkey]
        {
            get
            {
                if (this.TryGetValue(fullkey, out var ffb))
                {
                    return ffb;
                }
                throw new KeyNotFoundException();
            }
            set
            {
                var keys = fullkey.Split('/', '\\');
                string tmpkey = keys.First();
                string lastKey = keys.Last();
                FileFolderBase ffb = this._PBOs.First((it) => it.Name.Equals(tmpkey, StringComparison.InvariantCultureIgnoreCase));
                foreach (var key in keys.Skip(1))
                {
                    if (ffb is ICollection<FileFolderBase> collection)
                    {
                        ffb = collection.FirstOrDefault((it) => it.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
                        if (ffb == null)
                        {
                            if (key.Equals(lastKey))
                            {
                                collection.Add(value);
                            }
                            else
                            {
                                collection.Add(new Folder { Name = key });
                            }
                        }
                        else if (key.Equals(lastKey))
                        {
                            collection.Remove(ffb);
                            collection.Add(value);
                        }
                    }
                    throw new InvalidOperationException();
                }
            }
        }
        public bool TryGetValue(string fullkey, out FileFolderBase fileFolderBase)
        {
            if (string.IsNullOrWhiteSpace(fullkey))
            {
                fileFolderBase = default;
                return false;
            }
            fileFolderBase = this._PBOs.FirstOrDefault((it) => fullkey.StartsWith(it.Name, StringComparison.InvariantCultureIgnoreCase));
            if (fileFolderBase == null)
            {
                fileFolderBase = this._PBOs.FirstOrDefault((it) => fullkey.StartsWith(it.Prefix, StringComparison.InvariantCultureIgnoreCase));
                if (fileFolderBase == null)
                {
                    return false;
                }
                else
                {
                    fullkey = fullkey.Substring((fileFolderBase as PBO).Prefix.Length);
                }
            }
            else
            {
                fullkey = fullkey.Substring(fileFolderBase.Name.Length);
            }
            var keys = fullkey.Split('/', '\\');
            foreach (var key in keys.Skip(1))
            {
                if (!(fileFolderBase is ICollection<FileFolderBase> collection) ||
                    (fileFolderBase = collection.FirstOrDefault((it) => it.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase))) == null)
                {
                    fileFolderBase = default;
                    return false;
                }
            }
            return true;
        }
        public bool ContainsKey(string fullkey)
        {
            return this.TryGetValue(fullkey, out _);
        }
        private readonly ObservableCollection<PBO> _PBOs;

        public void Add(PBO item)
        {
            this._PBOs.Add(item);
        }

        public void Clear()
        {
            this._PBOs.Clear();
        }

        public bool Contains(PBO item)
        {
            return this._PBOs.Contains(item);
        }

        public void CopyTo(PBO[] array, int arrayIndex)
        {
            this._PBOs.CopyTo(array, arrayIndex);
        }

        public bool Remove(PBO item)
        {
            return this._PBOs.Remove(item);
        }

        public IEnumerator<PBO> GetEnumerator()
        {
            return this._PBOs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._PBOs.GetEnumerator();
        }
        #endregion
        #region IDisposable
        private bool disposedValue = false; // To detect redundant calls


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this._PBOs.CollectionChanged -= this.PBOs_CollectionChanged;
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
        #endregion

        public BreakpointManager BreakpointManager { get; }

        public int Count => this._PBOs.Count;

        public bool IsReadOnly => ((ICollection<PBO>)this._PBOs).IsReadOnly;

        public Solution()
        {
            this.BreakpointManager = new BreakpointManager();
            this._PBOs = new ObservableCollection<PBO>();
            this._PBOs.CollectionChanged += this.PBOs_CollectionChanged;
        }

        private void PBOs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }

        private const string CONST_FILE = "file";
        private const string CONST_PBO = "pbo";
        private const string CONST_ATT_NAME = "name";
        private const string CONST_ERROR = "ERROR";
    }
}

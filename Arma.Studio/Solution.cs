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
                var keys = fullkey.Split('/', '\\');
                string tmpkey = keys.First();
                FileFolderBase ffb = this._PBOs.First((it) => it.Name.Equals(tmpkey, StringComparison.InvariantCultureIgnoreCase));
                foreach (var key in keys.Skip(1))
                {
                    if (!(ffb is ICollection<FileFolderBase> collection) || (ffb = collection.FirstOrDefault((it) => it.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase))) == null)
                    {
                        throw new KeyNotFoundException();
                    }
                }
                return ffb;
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
        public bool ContainsKey(string fullkey)
        {
            if (string.IsNullOrWhiteSpace(fullkey))
            {
                return false;
            }
            var keys = fullkey.Split('/', '\\');
            string tmpkey = keys.First();
            FileFolderBase ffb = this._PBOs.FirstOrDefault((it) => it.Name.Equals(tmpkey, StringComparison.InvariantCultureIgnoreCase));
            if (ffb is null)
            {
                return false;
            }
            foreach (var key in keys.Skip(1))
            {
                if (!(ffb is ICollection<FileFolderBase> collection) || (ffb = collection.FirstOrDefault((it) => it.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase))) == null)
                {
                    return false;
                }
            }
            return true;
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

        private const string CONST_FILE = "File";
        private const string CONST_PBO = "PBO";
        private const string CONST_ATT_NAME = "name";
        private const string CONST_ERROR = "ERROR";

        public static Solution Deserialize(string path)
        {
            Solution solution = null;
            PBO pbo = null;
            var insidefile = false;
            var breakpoints = new List<Breakpoint>();
            using (var reader = new StreamReader(path))
            using (var xmlreader = XmlReader.Create(reader))
            {
                xmlreader.ReadStartElement(nameof(Solution));
                
                while(xmlreader.Read())
                {
                    switch (xmlreader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (xmlreader.Name)
                            {
                                case nameof(Solution):
                                    if (solution == null)
                                    {
                                        Logger.Info($"Solution start while in Solution.");
                                    }
                                    else
                                    {
                                        Logger.Info($"Found Solution start.");
                                        solution = new Solution();
                                    }
                                    break;
                                case CONST_FILE:
                                    if (pbo == null)
                                    {
                                        Logger.Error($"File start without PBO.");
                                    }
                                    else if (insidefile)
                                    {
                                        Logger.Error($"File start while already inside of a File.");
                                    }
                                    else
                                    {
                                        Logger.Info($"Found File '{xmlreader.Name}' of PBO '{pbo.Name}' start.");
                                        insidefile = true;
                                    }
                                    break;
                                case CONST_PBO:
                                    if (pbo != null)
                                    {
                                        Logger.Error($"PBO start while within a PBO.");
                                    }
                                    else
                                    {
                                        Logger.Info($"Found PBO start.");
                                        var name = xmlreader.GetAttribute(CONST_ATT_NAME);
                                        if(String.IsNullOrWhiteSpace(name))
                                        {
                                            Logger.Error($"PBO is missing {CONST_ATT_NAME}attribute");
                                            name = CONST_ERROR;
                                        }
                                        pbo = new PBO
                                        {
                                            Name = name
                                        };
                                    }
                                    break;
                                default:
                                    Logger.Error($"Unknown XmlElement '{xmlreader.Name}'.");
                                    break;
                            }
                            break;
                        case XmlNodeType.Text:
                            if (insidefile)
                            {
                                var text = xmlreader.Value.Trim();
                                pbo.Add(text);
                            }
                            else
                            {
                                Logger.Error($"Unexpected XmlText '{xmlreader.Value}'.");
                            }
                            break;
                        case XmlNodeType.EndElement:
                            switch (xmlreader.Name)
                            {
                                case CONST_FILE:
                                    if (insidefile)
                                    {
                                        Logger.Error($"File end without file start.");
                                    }
                                    else
                                    {
                                        Logger.Info($"Found File end.");
                                        insidefile = false;
                                    }
                                    break;
                                case CONST_PBO:
                                    if (insidefile)
                                    {
                                        Logger.Error($"PBO end while file still is open.");
                                    }
                                    else if(pbo == null)
                                    {
                                        Logger.Error($"PBO end without file start.");
                                    }
                                    else
                                    {
                                        solution.Add(pbo);
                                        pbo = null;
                                    }
                                    break;
                                default:
                                    Logger.Error($"Unknown XmlElement '{xmlreader.Name}'.");
                                    break;
                            }
                            break;
                    }
                }

                xmlreader.ReadEndElement();
            }
            return solution;
        }
        public Solution Serialize(string path)
        {
            throw new NotImplementedException();
        }

    }
}

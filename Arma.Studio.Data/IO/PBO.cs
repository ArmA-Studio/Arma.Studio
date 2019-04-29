using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.IO
{
    public class PBO : FileFolderBase, ICollection<FileFolderBase>
    {
        private readonly List<FileFolderBase> Inner;

        public PBO()
        {
            this.Inner = new List<FileFolderBase>();
        }


        public void Add(string text)
        {
            var elements = text.Split('\\', '/');
            var ffb = this.FirstOrDefault((it) => it.Name == elements[0]);
            if (elements.Length == 1)
            {
                this.Add(new File { Name = elements[0] });
                return;
            }
            Folder folder;
            if (ffb == null)
            {
                folder = new Folder { Name = elements[0] };
                this.Add(folder);
            }
            else if (ffb is File)
            {
                throw new Exception("Cannot add to file.");
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
                    throw new Exception("Cannot add to file.");
                }
                else
                {
                    folder = ffb as Folder;
                }
            }
            folder.Add(new File { Name = elements.Last() });
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.IO
{
    public class Folder : FileFolderBase, ICollection<FileFolderBase>
    {
        private readonly List<FileFolderBase> Inner;
        public Folder()
        {
            this.Inner = new List<FileFolderBase>();
        }

        public void Sort() => this.Inner.Sort((left, right) =>
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

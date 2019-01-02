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


        #region ICollection<FileFolderBase>
        public int Count => this.Inner.Count;

        public bool IsReadOnly => ((ICollection<FileFolderBase>)this.Inner).IsReadOnly;

        public void Add(FileFolderBase item)
        {
            this.Inner.Add(item);
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
            return this.Inner.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<FileFolderBase>)this.Inner).GetEnumerator();
        }
        #endregion
    }
}

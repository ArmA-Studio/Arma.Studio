using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.IO
{
    public class Folder : FileFolderBase, ICollection<File>
    {
        private readonly List<File> Inner;
        #region ICollection<File>
        public int Count => this.Inner.Count;

        public bool IsReadOnly => ((ICollection<File>)this.Inner).IsReadOnly;

        public void Add(File item)
        {
            this.Inner.Add(item);
        }

        public void Clear()
        {
            this.Inner.Clear();
        }

        public bool Contains(File item)
        {
            return this.Inner.Contains(item);
        }

        public void CopyTo(File[] array, int arrayIndex)
        {
            this.Inner.CopyTo(array, arrayIndex);
        }

        public IEnumerator<File> GetEnumerator()
        {
            return ((ICollection<File>)this.Inner).GetEnumerator();
        }

        public bool Remove(File item)
        {
            return this.Inner.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<File>)this.Inner).GetEnumerator();
        }
        #endregion
    }
}

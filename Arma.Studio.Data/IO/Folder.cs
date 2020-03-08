using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.IO
{
    public class Folder : FileFolderBase, ICollection<FileFolderBase>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private readonly ObservableCollection<FileFolderBase> Inner;
        public Folder()
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

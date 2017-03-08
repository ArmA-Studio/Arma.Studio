using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using System.ComponentModel;

namespace Utility.Collections
{
    public class ObservableSortedCollection<T> : CollectionBase, IList<T>, INotifyCollectionChanged, IEnumerable<T> where T : IComparable
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public T this[int index] { get { return (T)this.InnerList[index]; } set { this.InnerList[index] = value; } }

        public bool IsReadOnly { get { return this.InnerList.IsReadOnly; } }

        public ObservableSortedCollection() { }
        public ObservableSortedCollection(ICollection<T> col)
        {
            this.InnerList.AddRange(col as ICollection);
        }

        public void Add(T item)
        {
            this.InnerList.Add(item);
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }));
            this.Sort();
        }
        public void Sort()
        {
            var len = this.InnerList.Count;
            var hasChange = false;
            for (var i0 = 0; i0 < len; i0++)
            {
                var it0 = (T)this.InnerList[i0];
                for (var i1 = i0; i1 < len; i1++)
                {
                    var it1 = (T)this.InnerList[i1];
                    var compRes = it0.CompareTo(it1);
                    if (compRes < 0)
                    {
                        hasChange = true;
                        this.InnerList[i0] = it1;
                        this.InnerList[i1] = it0;
                    }
                }
            }
            if (hasChange)
            {
                this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        public bool Remove(T item)
        {
            this.InnerList.Remove(item);
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new[] { item }));
            return true;
        }

        public bool Contains(T item)
        {
            return this.InnerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.InnerList.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            return this.InnerList.IndexOf(item);
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new InvalidOperationException();
        }


        public new IEnumerator<T> GetEnumerator()
        {
            return this.InnerList.GetEnumerator().Cast<T>();
        }
    }
}

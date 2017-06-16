using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Collections
{
    public class ActedCollection<T> : ICollection<T>, IEnumerable<T>, IList<T>, IEnumerable
    {
        private List<T> InnerList;

        public int Count => this.InnerList.Count;

        public bool IsReadOnly => false;

        public T this[int index] { get { return this.InnerList[index]; } set { this.OnUpdating?.Invoke(this, new Tuple<T, T>(this.InnerList[index], value)); this.InnerList[index] = value; } }

        public event EventHandler<T> OnAdding;
        public event EventHandler<T> OnRemoving;
        public event EventHandler<Tuple<T, T>> OnUpdating;

        public ActedCollection()
        {
            this.InnerList = new List<T>();
        }

        public void Add(T item)
        {
            this.OnAdding?.Invoke(this, item);
            this.InnerList.Add(item);
        }

        public bool Contains(T item)
        {
            return this.InnerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.InnerList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            this.OnRemoving?.Invoke(this, item);
            this.InnerList.Remove(item);
            return true;
        }

        public int IndexOf(T item)
        {
            return this.InnerList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.OnAdding?.Invoke(this, item);
            this.InnerList.Insert(index, item);
        }

        public IEnumerable<T> FindAll(Predicate<T> p)
        {
            return this.InnerList.FindAll(p);
        }

        public void Clear()
        {
            foreach (var it in this.InnerList)
                this.OnRemoving?.Invoke(this, it);
            this.InnerList.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }

        public void RemoveAt(int index)
        {
            this.OnRemoving?.Invoke(this, this.InnerList[index]);
            this.InnerList.RemoveAt(index);
        }
    }
}

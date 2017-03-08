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
    public class ObservableDictionary<TKey, TValue> : DictionaryBase, INotifyCollectionChanged, IDictionary<TKey, TValue>
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public void RaiseCollectionChanged() { this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)); }

        public ICollection<TKey> Keys { get { return this.Dictionary.Keys.CastCollection<TKey>(); } }
        public ICollection<TValue> Values { get { return this.Dictionary.Values.CastCollection<TValue>(); } }
        public bool IsReadOnly { get { return this.Dictionary.IsReadOnly; } }
        public TValue this[TKey key] { get { return (TValue)this.Dictionary[key]; } set { this.Dictionary[key] = value; } }

        public void Add(TKey key, TValue value) { this.Dictionary.Add(key, value); }
        public bool Remove(TKey key) { this.Dictionary.Remove(key); return true; }
        public void Add(KeyValuePair<TKey, TValue> item) { this.Dictionary.Add(item.Key, item.Value); }
        public bool Contains(KeyValuePair<TKey, TValue> item) { return this.Dictionary.Contains(item); }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) { this.Dictionary.CopyTo(array, arrayIndex); }
        public bool Remove(KeyValuePair<TKey, TValue> item) { this.Dictionary.Remove(item); return true; }
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() { return Dictionary.GetEnumerator().Cast<KeyValuePair<TKey, TValue>>(); }

        public bool ContainsKey(TKey key)
        {
            foreach (var it in this.Dictionary.Keys)
            {
                if (it.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach(var it in this.Dictionary.Keys)
            {
                if(it.Equals(key))
                {
                    value = (TValue)this.Dictionary[it];
                    return true;
                }
            }
            value = default(TValue);
            return false;
        }




        protected override void OnInsertComplete(object key, object value)
        {
            this.RaiseCollectionChanged();
        }
        protected override void OnRemoveComplete(object key, object value)
        {
            this.RaiseCollectionChanged();
        }
        protected override void OnSetComplete(object key, object oldValue, object newValue)
        {
            this.RaiseCollectionChanged();
        }
        protected override void OnClearComplete()
        {
            this.RaiseCollectionChanged();
        }
    }
}

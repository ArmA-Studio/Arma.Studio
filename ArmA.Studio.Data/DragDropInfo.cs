using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Data
{
    public class DragDropInfo : ICollection<object>
    {
        private readonly Dictionary<Type, object> Inner;

        public int Count => ((ICollection<object>)this.Inner).Count;


        public DragDropInfo()
        {
            this.Inner = new Dictionary<Type, object>();
        }
        public DragDropInfo(IEnumerable<object> objs)
        {
            this.Inner = new Dictionary<Type, object>();
            foreach (var it in objs)
            {
                this.SetData(it.GetType(), it);
            }
        }
        public DragDropInfo(params object[] objs)
        {
            this.Inner = new Dictionary<Type, object>();
            foreach (var it in objs)
            {
                this.SetData(it.GetType(), it);
            }
        }

        public bool TryGetData<T>(out T t)
        {
            var res = this.GetDataOrDefault(typeof(T));
            if (res == null)
            {
                t = default(T);
                return false;
            }
            else
            {
                t = (T)res;
                return true;
            }
        }
        public T GetData<T>() => (T)(this.GetDataOrDefault(typeof(T)) ?? throw new KeyNotFoundException());
        public object GetData(Type t) => this.GetDataOrDefault(t) ?? throw new KeyNotFoundException();
        public T GetDataOrDefault<T>() => (T)(this.GetDataOrDefault(typeof(T)) ?? default(T));
        public object GetDataOrDefault(Type t)
        {
            if (this.Inner.ContainsKey(t))
            {
                return this.Inner[t];
            }
            else
            {
                var tmpKey = this.Inner.Keys.FirstOrDefault((key) => t.IsAssignableFrom(key));
                if (tmpKey == null)
                {
                    return default(object);
                }
                return this.Inner[tmpKey];
            }
        }
        public bool HasData<T>() => this.HasData(typeof(T));
        public bool HasData(Type t) => this.Inner.ContainsKey(t) || this.Inner.Keys.FirstOrDefault((key) => t.IsAssignableFrom(key)) != null;

        public void SetData<T>(T data) => this.SetData(typeof(T), data);
        public void SetData(Type t, object data) => this.Inner.Add(t, data);

        #region ICollection<object>
        int ICollection<object>.Count => this.Inner.Count;
        bool ICollection<object>.IsReadOnly => false;
        public void Add(object item) => this.SetData(item.GetType(), item);
        void ICollection<object>.Clear() => this.Inner.Clear();
        bool ICollection<object>.Contains(object item) => this.Inner.ContainsKey(item.GetType());
        void ICollection<object>.CopyTo(object[] array, int arrayIndex) => new NotSupportedException();
        bool ICollection<object>.Remove(object item) => this.Inner.Remove(item.GetType());

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            foreach(var it in this.Inner)
            {
                yield return it.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var it in this.Inner)
            {
                yield return it.Value;
            }
        }
        #endregion
    }
}

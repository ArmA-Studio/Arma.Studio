using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Data.Configuration
{
    public class Category : ConfigurationBaseClass, IEnumerable<SubCategory>, IList<SubCategory>
    { 
        private readonly IList<SubCategory> InnerList;

        public IEnumerator<SubCategory> GetEnumerator() => this.InnerList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.InnerList.GetEnumerator();

        public int Count => this.InnerList.Count;
        public bool IsReadOnly => this.InnerList.IsReadOnly;
        public SubCategory this[int index] { get { return this.InnerList[index]; } set { this.InnerList[index] = value; } }
        public int IndexOf(SubCategory item) => this.InnerList.IndexOf(item);
        public void Insert(int index, SubCategory item) => this.InnerList.Insert(index, item);
        public void RemoveAt(int index) => this.InnerList.RemoveAt(index);
        public void Add(SubCategory item) => this.InnerList.Add(item);
        public void Clear() => this.InnerList.Clear();
        public bool Contains(SubCategory item) => this.InnerList.Contains(item);
        public void CopyTo(SubCategory[] array, int arrayIndex) => this.InnerList.CopyTo(array, arrayIndex);
        public bool Remove(SubCategory item) => this.InnerList.Remove(item);
        public Category(IEnumerable<SubCategory> items) : this(items.ToList()) { }
        public Category(IList<SubCategory> items)
        {
            this.InnerList = items;
        }

        public void AddRange(IEnumerable<SubCategory> range)
        {
            foreach (var it in range)
                this.InnerList.Add(it);
        }

        public static IEnumerable<Category> Merge(IEnumerable<Category> cats)
        {
            var list = cats.ToList();
            for (var i = 0; i < list.Count; i++)
            {
                var lCat = list[i];
                for (var j = i + 1; j < list.Count; j++)
                {
                    var rCat = list[j];

                    if (lCat.Name.Equals(rCat.Name))
                    {
                        lCat.AddRange(rCat);
                        list.RemoveAt(j);
                        j--;
                    }
                }
            }
            return list;
        }
    }
}

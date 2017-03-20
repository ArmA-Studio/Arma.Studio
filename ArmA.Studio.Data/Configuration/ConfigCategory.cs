using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Data.Configuration
{
    public sealed class ConfigCategory : IList<ConfigEntry>
    {
        #region IList<ConfigEntry>
        public ConfigEntry this[int index] { get { return this.InnerList[index]; } set { this.InnerList[index] = value; } }
        public int Count { get { return this.Count; } }
        public bool IsReadOnly { get { return this.IsReadOnly; } }

        public string Name { get; private set; }
        public string ImageSource { get; private set; }

        public void Add(ConfigEntry item) => this.InnerList.Add(item);
        public void Clear() => this.InnerList.Clear();
        public bool Contains(ConfigEntry item) => this.InnerList.Contains(item);
        public void CopyTo(ConfigEntry[] array, int arrayIndex) => this.InnerList.CopyTo(array, arrayIndex);
        public IEnumerator<ConfigEntry> GetEnumerator() => this.InnerList.GetEnumerator();
        public int IndexOf(ConfigEntry item) => this.InnerList.IndexOf(item);
        public void Insert(int index, ConfigEntry item) => this.InnerList.Insert(index, item);
        public bool Remove(ConfigEntry item) => this.InnerList.Remove(item);
        public void RemoveAt(int index) => this.InnerList.RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => this.InnerList.GetEnumerator();
        #endregion

        private readonly List<ConfigEntry> InnerList;

        public ConfigCategory(string name)
        {
            this.Name = name;
            this.ImageSource = string.Empty;
            this.InnerList = new List<ConfigEntry>();
        }
        public ConfigCategory(string name, string imageSource)
        {
            this.Name = name;
            this.ImageSource = imageSource;
            this.InnerList = new List<ConfigEntry>();
        }
        public ConfigCategory(string name, string imageSource, IEnumerable<ConfigEntry> collection)
        {
            this.Name = name;
            this.ImageSource = imageSource;
            this.InnerList = new List<ConfigEntry>(collection);
        }

        public void Add(params ConfigCategory[] cats)
        {
            foreach(var cat in cats)
            {
                foreach(var entry in cat)
                {
                    if (!this.Contains(entry))
                    {
                        this.Add(entry);
                    }
                }
            }
        }
    }
}

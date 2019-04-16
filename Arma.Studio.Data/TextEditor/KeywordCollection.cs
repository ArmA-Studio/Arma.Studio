using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.Data.TextEditor
{
    public class KeywordCollection : ICollection<string>
    {
        private readonly List<string> Inner;

        public Color Color { get; }
        public bool IsBold { get; }
        public KeywordCollection(Color color, bool isBold)
        {
            this.Inner = new List<string>();
            this.Color = color;
            this.IsBold = isBold;
        }

        public KeywordCollection AddRange(IEnumerable<string> item)
        {
            this.Inner.AddRange(item);
            return this;
        }

        #region ICollection<string>
        public int Count => this.Inner.Count;

        public bool IsReadOnly => ((ICollection<string>)this.Inner).IsReadOnly;

        public void Add(string item)
        {
            this.Inner.Add(item);
        }

        public void Clear()
        {
            this.Inner.Clear();
        }

        public bool Contains(string item)
        {
            return this.Inner.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            this.Inner.CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((ICollection<string>)this.Inner).GetEnumerator();
        }

        public bool Remove(string item)
        {
            return this.Inner.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<string>)this.Inner).GetEnumerator();
        }
        #endregion
    }
}

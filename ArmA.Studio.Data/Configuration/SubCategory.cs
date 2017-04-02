using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Data.Configuration
{
    public class SubCategory : ConfigurationBaseClass, IEnumerable<Item>
    {
        private readonly IEnumerable<Item> InnerList;

        public bool IsExpanded { get { return this._IsExpanded; } set { if (this._IsExpanded == value) return; this._IsExpanded = value; this.NotifyPropertyChanged(); } }
        private bool _IsExpanded;


        public IEnumerator<Item> GetEnumerator() => InnerList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

        public SubCategory(IEnumerable<Item> items) : this (items, true) { }
        public SubCategory(IEnumerable<Item> items, bool isExpanded)
        {
            this.InnerList = items;
            this.IsExpanded = isExpanded;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Data.Configuration
{
    public class Category : ConfigurationBaseClass, IEnumerable<SubCategory>
    {
        private readonly IEnumerable<SubCategory> InnerList;

        public IEnumerator<SubCategory> GetEnumerator() => InnerList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

        public Category(IEnumerable<SubCategory> items)
        {
            this.InnerList = items;
        }
    }
}

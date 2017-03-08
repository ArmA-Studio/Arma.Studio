using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections;

namespace Utility
{
    public static class ICollectionExtensionMethods
    {
        #region Cast
        public static ICollection<T> CastCollection<T>(this ICollection iterator)
        {
            var collection = new List<T>();
            foreach(var it in iterator)
            {
                collection.Add((T)it);
            }
            return collection;
        }
        #endregion
    }
}

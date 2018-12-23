using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio
{
    public static class Extensions
    {
        public static void AddRange<T>(this ICollection<T> col, IEnumerable<T> ts)
        {
            foreach (var it in ts)
            {
                col.Add(it);
            }
        }
    }
}

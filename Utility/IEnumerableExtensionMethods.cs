using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections;

namespace Utility
{
    public static class IEnumerableExtensionMethods
    {
        #region FirstOrNull(able)
        public static T FirstOrNull<T>(this IEnumerable<T> enumerable, Func<T, bool> func) where T : class
        {
            foreach (var it in enumerable)
            {
                if (func(it))
                {
                    return it;
                }
            }
            return null;
        }
        public static T? FirstOrNullable<T>(this IEnumerable<T> enumerable, Func<T, bool> func) where T : struct
        {
            foreach (var it in enumerable)
            {
                if (func(it))
                {
                    return it;
                }
            }
            return null;
        }
        #endregion
        #region Pick
        public static IEnumerable<T> Pick<T>(this IEnumerable<T> enumerable, Func<T, bool> func)
        {
            var list = new List<T>();
            foreach (var it in enumerable)
            {
                if (func(it))
                {
                    list.Add(it);
                }
            }
            return list;
        }
        #endregion
        #region Cast
        public static IEnumerator<T> Cast<T>(this IEnumerator iterator)
        {
            while (iterator.MoveNext())
            {
                yield return (T)iterator.Current;
            }
        }
        public static IEnumerator Cast<T>(this IEnumerator<T> iterator)
        {
            while (iterator.MoveNext())
            {
                yield return iterator.Current;
            }
        }
        #endregion
        #region Count
        public static int Count(this IEnumerable source)
        {
            var collection = source as ICollection;
            if (collection != null)
                return collection.Count;

            var count = 0;
            var enumerator = source.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                    count++;
            }
            catch
            {
                throw;
            }
            finally
            {
                var disposable = enumerator as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
            return count;
        }
        #endregion
        #region AllNested
        public static bool AllNested<T>(this IEnumerable<T> enumerable, Func<T, bool> condition)
        {
            foreach (var it in enumerable)
            {
                if (!condition(it) || (it is IEnumerable<T> && !AllNested(it as IEnumerable<T>, condition)))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
        #region ForEachNested
        public static void ForEachNested<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var it in enumerable)
            {
                action(it);
                if (it is IEnumerable<T>)
                {
                    ForEachNested(it as IEnumerable<T>, action);
                }
            }
        }
        #endregion
    }
}

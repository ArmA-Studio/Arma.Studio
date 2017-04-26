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
        public static T? FirstOrNullable<T>(this IEnumerable<T> enumerable) where T : struct => FirstOrNullable(enumerable, (t) => true);
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
        #region MoveStart
        public static IEnumerable<T> MoveStart<T>(this IEnumerable<T> enumerable, T start)
        {
            var count = enumerable.Count();
            var index = enumerable.IndexOf(start);
            if (index == -1)
                throw new InvalidOperationException();
            if (index == 0)
            {
                foreach(var it in enumerable)
                {
                    yield return it;
                }
            }
            else
            {
                var enumerator = enumerable.GetEnumerator();
                for (int i = 0; enumerator.MoveNext(); i++)
                {
                    if (i < index)
                        continue;
                    yield return enumerator.Current;
                }
                enumerator = enumerable.GetEnumerator();
                for (int i = 0; enumerator.MoveNext() && i < index; i++)
                {
                    yield return enumerator.Current;
                }
            }
        }
        #endregion
        #region IndexOf
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T item)
        {
            var enumerator = enumerable.GetEnumerator();
            for (int i = 0; enumerator.MoveNext(); i++)
            {
                if (enumerator.Current.Equals(item))
                    return i;
            }
            return -1;
        }
        public static int IndexOf(this IEnumerable enumerable, object item)
        {
            var enumerator = enumerable.GetEnumerator();
            for (int i = 0; enumerator.MoveNext(); i++)
            {
                if (enumerator.Current.Equals(item))
                    return i;
            }
            return -1;
        }
        public static int IndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> cond)
        {
            var enumerator = enumerable.GetEnumerator();
            for (int i = 0; enumerator.MoveNext(); i++)
            {
                if (cond.Invoke(enumerator.Current))
                    return i;
            }
            return -1;
        }
        public static int IndexOf(this IEnumerable enumerable, Func<object, bool> cond)
        {
            var enumerator = enumerable.GetEnumerator();
            for (int i = 0; enumerator.MoveNext(); i++)
            {
                if (cond.Invoke(enumerator.Current))
                    return i;
            }
            return -1;
        }
        #endregion
        #region Second
        public static T Second<T>(this IEnumerable<T> enumerable)
        {
            int i = 0;
            foreach (var it in enumerable)
            {
                i++;
                if (i == 2)
                    return it;
            }
            throw new InvalidOperationException();
        }
        public static object Second(this IEnumerable enumerable)
        {
            int i = 0;
            foreach (var it in enumerable)
            {
                i++;
                if (i == 2)
                    return it;
            }
            throw new InvalidOperationException();
        }
        #endregion
    }
}

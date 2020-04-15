using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data
{
    public class Utility
    {
        /// <summary>
        /// Wraps the provided <paramref name="func"/> inside a 
        /// try catch and returns the value it returned.
        /// On exception, default value of <typeparamref name="T"/> is being returned.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to return.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <returns>Either the value returned by <paramref name="func"/> or default of <typeparamref name="T"/>.</returns>
        public static T CatchYield<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// Wraps the provided <paramref name="func"/> inside a 
        /// try catch and returns the value it returned.
        /// On exception, <paramref name="default"/> is being returned.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to return.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="default">The default value. Will be returned if func throws an exception.</param>
        /// <returns>Either the value returned by <paramref name="func"/> or <paramref name="default"/>.</returns>
        public static T CatchYield<T>(Func<T> func, T @default)
        {
            try
            {
                return func();
            }
            catch
            {
                return @default;
            }
        }
        /// <summary>
        /// Wraps the provided <paramref name="func"/> inside a 
        /// try catch and returns the value it returned.
        /// On exception, value returned by <paramref name="funcEx"/> is being returned.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to return.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="funcEx">Exceptionhandler. Returns the value this method should return instead.</param>
        /// <returns>Either the value returned by <paramref name="func"/> or by <paramref name="funcEx"/>.</returns>
        public static T CatchYield<T>(Func<T> func, Func<Exception, T> funcEx)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                return funcEx(ex);
            }
        }
    }
}

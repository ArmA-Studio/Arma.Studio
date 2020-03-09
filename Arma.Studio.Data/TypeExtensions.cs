
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Generates a valid C#-Code name from any type, including generics.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string FullName(this Type t)
        {
            if (t.IsGenericType)
            {
                var builder = new StringBuilder();
                builder.Append(t.Namespace);
                builder.Append('.');
                builder.Append(t.Name.Substring(0, t.Name.IndexOf('`')));
                builder.Append('<');
                builder.Append(FullName(t));
                builder.Append('>');
                return builder.ToString();
            }
            else
            {
                return t.FullName;
            }
        }
		
        /// <summary>
        /// Creates a default instance of this <see cref="Type"/>.
        /// Requires parameterless constructor for Value-Types
        /// </summary>
        /// <param name="t">The type to create a default value for.</param>
        /// <returns>A default value, matching the passed type</returns>
        public static object Default(this Type t)
        {
            if (t.IsValueType)
            {
                return t.CreateInstance();
            }
            return null;
        }

        public static T CreateInstance<T>(this Type t) => (T)Expression.Lambda(Expression.New(t)).Compile().DynamicInvoke();
        public static object CreateInstance(this Type t) => Expression.Lambda(Expression.New(t)).Compile().DynamicInvoke();
        public static object CreateInstance<TArg1>(
            this Type t,
            TArg1 arg1)
        {
            var constructor = t.GetConstructor(new Type[] {
				typeof(TArg1)
            });
            if (constructor == null)
            {
                var ex = new InvalidOperationException("No matching constructor existing.");
                ex.Data.Add("Target", t);
                ex.Data.Add(typeof(TArg1), arg1);
                throw ex;
            }
            var exp_parameters = new Expression[] {
                Expression.Constant(arg1)
            };
            var exp_new = Expression.New(constructor, exp_parameters);
            var result = Expression.Lambda(exp_new).Compile().DynamicInvoke();
            return result;
        }
        public static object CreateInstance<TArg1, TArg2>(
            this Type t,
            TArg1 arg1,
            TArg2 arg2)
        {
            var constructor = t.GetConstructor(new Type[] {
				typeof(TArg1),
                typeof(TArg2)
            });
            if (constructor == null)
            {
                var ex = new InvalidOperationException("No matching constructor existing.");
                ex.Data.Add("Target", t);
                ex.Data.Add(typeof(TArg1), arg1);
                ex.Data.Add(typeof(TArg2), arg1);
                throw ex;
            }
            var exp_parameters = new Expression[] {
                Expression.Constant(arg1),
                Expression.Constant(arg2)
            };
            var exp_new = Expression.New(constructor, exp_parameters);
            var result = Expression.Lambda(exp_new).Compile().DynamicInvoke();
            return result;
        }
        public static object CreateInstance<TArg1, TArg2, TArg3>(
            this Type t,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3)
        {
            var constructor = t.GetConstructor(new Type[] {
				typeof(TArg1),
                typeof(TArg2),
                typeof(TArg3)
            });
            if (constructor == null)
            {
                var ex = new InvalidOperationException("No matching constructor existing.");
                ex.Data.Add("Target", t);
                ex.Data.Add(typeof(TArg1), arg1);
                ex.Data.Add(typeof(TArg2), arg1);
                ex.Data.Add(typeof(TArg3), arg1);
                throw ex;
            }
            var exp_parameters = new Expression[] {
                Expression.Constant(arg1),
                Expression.Constant(arg2),
                Expression.Constant(arg3)
            };
            var exp_new = Expression.New(constructor, exp_parameters);
            var result = Expression.Lambda(exp_new).Compile().DynamicInvoke();
            return result;
        }
        public static object CreateInstance<TArg1, TArg2, TArg3, TArg4>(
            this Type t,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4)
        {
            var constructor = t.GetConstructor(new Type[] {
				typeof(TArg1),
                typeof(TArg2),
                typeof(TArg3),
                typeof(TArg4)
            });
            if (constructor == null)
            {
                var ex = new InvalidOperationException("No matching constructor existing.");
                ex.Data.Add("Target", t);
                ex.Data.Add(typeof(TArg1), arg1);
                ex.Data.Add(typeof(TArg2), arg1);
                ex.Data.Add(typeof(TArg3), arg1);
                ex.Data.Add(typeof(TArg4), arg1);
                throw ex;
            }
            var exp_parameters = new Expression[] {
                Expression.Constant(arg1),
                Expression.Constant(arg2),
                Expression.Constant(arg3),
                Expression.Constant(arg4)
            };
            var exp_new = Expression.New(constructor, exp_parameters);
            var result = Expression.Lambda(exp_new).Compile().DynamicInvoke();
            return result;
        }
        public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5>(
            this Type t,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5)
        {
            var constructor = t.GetConstructor(new Type[] {
				typeof(TArg1),
                typeof(TArg2),
                typeof(TArg3),
                typeof(TArg4),
                typeof(TArg5)
            });
            if (constructor == null)
            {
                var ex = new InvalidOperationException("No matching constructor existing.");
                ex.Data.Add("Target", t);
                ex.Data.Add(typeof(TArg1), arg1);
                ex.Data.Add(typeof(TArg2), arg1);
                ex.Data.Add(typeof(TArg3), arg1);
                ex.Data.Add(typeof(TArg4), arg1);
                ex.Data.Add(typeof(TArg5), arg1);
                throw ex;
            }
            var exp_parameters = new Expression[] {
                Expression.Constant(arg1),
                Expression.Constant(arg2),
                Expression.Constant(arg3),
                Expression.Constant(arg4),
                Expression.Constant(arg5)
            };
            var exp_new = Expression.New(constructor, exp_parameters);
            var result = Expression.Lambda(exp_new).Compile().DynamicInvoke();
            return result;
        }
        public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
            this Type t,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            TArg6 arg6)
        {
            var constructor = t.GetConstructor(new Type[] {
				typeof(TArg1),
                typeof(TArg2),
                typeof(TArg3),
                typeof(TArg4),
                typeof(TArg5),
                typeof(TArg6)
            });
            if (constructor == null)
            {
                var ex = new InvalidOperationException("No matching constructor existing.");
                ex.Data.Add("Target", t);
                ex.Data.Add(typeof(TArg1), arg1);
                ex.Data.Add(typeof(TArg2), arg1);
                ex.Data.Add(typeof(TArg3), arg1);
                ex.Data.Add(typeof(TArg4), arg1);
                ex.Data.Add(typeof(TArg5), arg1);
                ex.Data.Add(typeof(TArg6), arg1);
                throw ex;
            }
            var exp_parameters = new Expression[] {
                Expression.Constant(arg1),
                Expression.Constant(arg2),
                Expression.Constant(arg3),
                Expression.Constant(arg4),
                Expression.Constant(arg5),
                Expression.Constant(arg6)
            };
            var exp_new = Expression.New(constructor, exp_parameters);
            var result = Expression.Lambda(exp_new).Compile().DynamicInvoke();
            return result;
        }
        public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(
            this Type t,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            TArg6 arg6,
            TArg7 arg7)
        {
            var constructor = t.GetConstructor(new Type[] {
				typeof(TArg1),
                typeof(TArg2),
                typeof(TArg3),
                typeof(TArg4),
                typeof(TArg5),
                typeof(TArg6),
                typeof(TArg7)
            });
            if (constructor == null)
            {
                var ex = new InvalidOperationException("No matching constructor existing.");
                ex.Data.Add("Target", t);
                ex.Data.Add(typeof(TArg1), arg1);
                ex.Data.Add(typeof(TArg2), arg1);
                ex.Data.Add(typeof(TArg3), arg1);
                ex.Data.Add(typeof(TArg4), arg1);
                ex.Data.Add(typeof(TArg5), arg1);
                ex.Data.Add(typeof(TArg6), arg1);
                ex.Data.Add(typeof(TArg7), arg1);
                throw ex;
            }
            var exp_parameters = new Expression[] {
                Expression.Constant(arg1),
                Expression.Constant(arg2),
                Expression.Constant(arg3),
                Expression.Constant(arg4),
                Expression.Constant(arg5),
                Expression.Constant(arg6),
                Expression.Constant(arg7)
            };
            var exp_new = Expression.New(constructor, exp_parameters);
            var result = Expression.Lambda(exp_new).Compile().DynamicInvoke();
            return result;
        }
        public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(
            this Type t,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            TArg6 arg6,
            TArg7 arg7,
            TArg8 arg8)
        {
            var constructor = t.GetConstructor(new Type[] {
				typeof(TArg1),
                typeof(TArg2),
                typeof(TArg3),
                typeof(TArg4),
                typeof(TArg5),
                typeof(TArg6),
                typeof(TArg7),
                typeof(TArg8)
            });
            if (constructor == null)
            {
                var ex = new InvalidOperationException("No matching constructor existing.");
                ex.Data.Add("Target", t);
                ex.Data.Add(typeof(TArg1), arg1);
                ex.Data.Add(typeof(TArg2), arg1);
                ex.Data.Add(typeof(TArg3), arg1);
                ex.Data.Add(typeof(TArg4), arg1);
                ex.Data.Add(typeof(TArg5), arg1);
                ex.Data.Add(typeof(TArg6), arg1);
                ex.Data.Add(typeof(TArg7), arg1);
                ex.Data.Add(typeof(TArg8), arg1);
                throw ex;
            }
            var exp_parameters = new Expression[] {
                Expression.Constant(arg1),
                Expression.Constant(arg2),
                Expression.Constant(arg3),
                Expression.Constant(arg4),
                Expression.Constant(arg5),
                Expression.Constant(arg6),
                Expression.Constant(arg7),
                Expression.Constant(arg8)
            };
            var exp_new = Expression.New(constructor, exp_parameters);
            var result = Expression.Lambda(exp_new).Compile().DynamicInvoke();
            return result;
        }
        public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(
            this Type t,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            TArg6 arg6,
            TArg7 arg7,
            TArg8 arg8,
            TArg9 arg9)
        {
            var constructor = t.GetConstructor(new Type[] {
				typeof(TArg1),
                typeof(TArg2),
                typeof(TArg3),
                typeof(TArg4),
                typeof(TArg5),
                typeof(TArg6),
                typeof(TArg7),
                typeof(TArg8),
                typeof(TArg9)
            });
            if (constructor == null)
            {
                var ex = new InvalidOperationException("No matching constructor existing.");
                ex.Data.Add("Target", t);
                ex.Data.Add(typeof(TArg1), arg1);
                ex.Data.Add(typeof(TArg2), arg1);
                ex.Data.Add(typeof(TArg3), arg1);
                ex.Data.Add(typeof(TArg4), arg1);
                ex.Data.Add(typeof(TArg5), arg1);
                ex.Data.Add(typeof(TArg6), arg1);
                ex.Data.Add(typeof(TArg7), arg1);
                ex.Data.Add(typeof(TArg8), arg1);
                ex.Data.Add(typeof(TArg9), arg1);
                throw ex;
            }
            var exp_parameters = new Expression[] {
                Expression.Constant(arg1),
                Expression.Constant(arg2),
                Expression.Constant(arg3),
                Expression.Constant(arg4),
                Expression.Constant(arg5),
                Expression.Constant(arg6),
                Expression.Constant(arg7),
                Expression.Constant(arg8),
                Expression.Constant(arg9)
            };
            var exp_new = Expression.New(constructor, exp_parameters);
            var result = Expression.Lambda(exp_new).Compile().DynamicInvoke();
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections;

namespace Utility
{
    public static class StructExtensionMethods
    {
        public static bool IsDefault<T>(this T t) where T : struct
        {
            return t.Equals(default(T));
        }
    }
}

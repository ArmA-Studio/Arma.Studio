using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealVirtuality.SQF.Parser.v2
{
    public static class Extensions
    {
        internal static bool ContainsName(this IEnumerable<SqfDefinition> enumerable, string s)
        {
            if (s == null)
                return false;
            foreach(var it in enumerable)
            {
                if(it.Name.Equals(s, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        public static string GetTextTillWhitespace(this Antlr4.Runtime.ICharStream stream)
        {
            var builder = new StringBuilder();
            int la;
            for (int i = 1; (la = stream.La(i)) >= 0 && !char.IsWhiteSpace((char)la); i++)
            {
                builder.Append((char)la);
            }
            return builder.ToString();
        }
    }
}

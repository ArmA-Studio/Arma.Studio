using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealVirtuality.SQF
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
        public static SqfNode GetParent(this SqfNode node)
        {
            SqfNode weak;
            if (node.ParentWeak.TryGetTarget(out weak))
                return weak;
            return default(SqfNode);
        }
        public static void SetParent(this SqfNode node, SqfNode parent)
        {
            node.ParentWeak.SetTarget(parent);
        }
        public static void AddChild(this SqfNode node, SqfNode newChild)
        {
            node.Children.Add(newChild);
        }
        public static int ParentCount(this SqfNode node)
        {
            int i = 0;
            var tmp = node;
            while((tmp = tmp.GetParent()) != null)
            {
                i++;
            }
            return i;
        }
        public static void RemoveFromTree(this SqfNode node)
        {
            var parent = node.GetParent();
            var nodeIndex = parent.Children.IndexOf(node);
            parent.Children.Remove(node);
            foreach (var it in node.Children)
            {
                parent.Children.Insert(nodeIndex, it);
            }
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

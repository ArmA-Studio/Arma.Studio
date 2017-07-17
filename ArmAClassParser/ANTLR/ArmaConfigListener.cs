using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace RealVirtuality.Config.Parser.ANTLR
{
    public class ArmaConfigListener : armaconfigBaseListener
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public override void EnterEveryRule([NotNull] ParserRuleContext context)
        {
            var name = context.GetType().Name;
            if(name.EndsWith("Context", StringComparison.InvariantCultureIgnoreCase))
            {
                name = name.Remove(name.IndexOf("Context", StringComparison.InvariantCultureIgnoreCase));
            }
            Logger.Info(string.Format("ENTER:{0}{1}", new string('\t', context.Depth()), name));
        }
        public override void ExitEveryRule([NotNull] ParserRuleContext context)
        {
            var name = context.GetType().Name;
            if (name.EndsWith("Context", StringComparison.InvariantCultureIgnoreCase))
            {
                name = name.Remove(name.IndexOf("Context", StringComparison.InvariantCultureIgnoreCase));
            }
            Logger.Info(string.Format("EXIT :{0:#####}{1}{2}: {3}", context.start.Line, new string('\t', context.Depth()), name, context.GetText()));
        }
    }
}

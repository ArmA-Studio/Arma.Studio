using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data;
using ArmA.Studio.Data.Lint;
using RealVirtuality.Config.Parser;

namespace ArmA.Studio.DefaultPlugin
{
    internal sealed class ConfigLintHelper : ILinterHost
    {
        public IEnumerable<LintInfo> LinterInfo { get; set; }

        public void LintWriteCache(Stream stream, ProjectFile file) => this.LinterInfo = Lint(stream, file);

        public IEnumerable<LintInfo> Lint(Stream stream, ProjectFile file)
        {
            var parser = new Parser(new Scanner(stream));
            parser.Root = null;
            parser.doc = null;
            parser.Parse();
            return parser.errors.ErrorList.Select((it) => new LintInfo(file) { StartOffset = it.Item1, Length = it.Item2, Message = it.Item3, Severity = ESeverity.Error });

        }
    }
}

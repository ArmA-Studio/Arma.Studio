using Arma.Studio.Data.TextEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.SqfEditor
{
    public class SqfEditor : ITextEditor, ILintable, IFoldable
    {
        #region ITextEditor
        private IEnumerable<KeywordCollection> Helper_KeywordCollection()
        {
            yield return new KeywordCollection(Colors.Blue, false).AddRange(PluginMain.SqfDefinitionsFile.Binaries.Where((it) => !it.GroupSpecified).Select((it) => it.Name));
            yield return new KeywordCollection(Colors.Blue, false).AddRange(PluginMain.SqfDefinitionsFile.Unaries.Where((it) => !it.GroupSpecified).Select((it) => it.Name));
            yield return new KeywordCollection(Colors.Blue, false).AddRange(PluginMain.SqfDefinitionsFile.Nulars.Where((it) => !it.GroupSpecified).Select((it) => it.Name));

            var grouped = PluginMain.SqfDefinitionsFile.ConcatAll().Where((it) => it.GroupSpecified).ToArray();
            foreach (var group in PluginMain.SqfDefinitionsFile.Groups)
            {
                var items = grouped.Where((it) => it.Group.Equals(it.Name, StringComparison.InvariantCultureIgnoreCase));
                yield return new KeywordCollection(Color.FromRgb(group.Red, group.Green, group.Blue), group.IsBold).AddRange(items.Select((it) => it.Name));
            }
        }

        public SyntaxFile SyntaxFile
        {
            get
            {
                var sf = new SyntaxFile(true, @"&><~!@$%^*()-+=|\#/{}[]:;""' , .?")
                {
                    DigitsColor = Color.FromRgb(0xB5, 0xCE, 0xA8),
                    Enclosures =
                    {
                        new Enclosure(Color.FromRgb(0xD6, 0x9D, 0x85), "\"", "\""),
                        new Enclosure(Color.FromRgb(0xD6, 0x9D, 0x85), "'", "'"),
                        new Enclosure(Color.FromRgb(0x9B, 0x9B, 0x9B), "#"),
                        new Enclosure(Color.FromRgb(0x57, 0xA6, 0x3A), "//"),
                        new Enclosure(Color.FromRgb(0x57, 0xA6, 0x3A), "/*", "*/")
                    }
                };
                sf.Keywords.AddRange(Helper_KeywordCollection());
                return sf;
            }
        }
        public bool ShowLineNumbers => true;
        #endregion
        #region ILintable
        public async Task<IEnumerable<LintInfo>> GetLintInfos(string text, CancellationToken cancellationToken)
        {
            return await Task.Run(() => {
                var vm = new sqfvm.ClrVirtualmachine();
                var cst = vm.CreateSqfCst(text, "");
                var errors = vm.ErrorContents();
                var warnings = vm.WarningContents();
                var output = vm.InfoContents();
                var lintInfos = new List<LintInfo>();
                if (errors.Length > 0)
                {
                    using (var reader = new System.IO.StringReader(errors))
                    {
                        while (!String.IsNullOrWhiteSpace(reader.ReadLine()))
                        {
                            var l2_markings = reader.ReadLine();
                            var l3_message = reader.ReadLine();

                            var matches = Regex.Matches(l3_message, @"\[[a-zA-Z0-9]+\]\[L([0-9]+)\|C([0-9]+)(\]|\|(.*?)\])(.*)");
                            foreach (Match match in matches)
                            {
                                if (match.Groups.Count != 6)
                                {
                                    continue;
                                }
                                else
                                {
                                    var line = Convert.ToInt32(match.Groups[1].Value);
                                    var column = Convert.ToInt32(match.Groups[2].Value);
                                    var file = Convert.ToString(match.Groups[4].Value);
                                    var message = Convert.ToString(match.Groups[5].Value);
                                    var length = l2_markings.Count((c) => c == '^');
                                    lintInfos.Add(new LintInfo
                                    {
                                        Length = length,
                                        Line = line,
                                        Column = column,
                                        Severity = ESeverity.Error
                                    });
                                }
                            }
                        }
                    }
                }
                return lintInfos;
            });
        }
        #endregion
        #region IFoldable
        public async Task<IEnumerable<FoldingInfo>> GetFoldings(string text, CancellationToken cancellationToken)
        {
            return await Task.Run(() => {
                var vm = new sqfvm.ClrVirtualmachine();
                var cst = vm.CreateSqfCst(text, "");
                var errors = vm.ErrorContents();
                var warnings = vm.WarningContents();
                var output = vm.InfoContents();
                return FindAllCodeNodes(cst).Select((node) => new FoldingInfo
                {
                    StartOffset = (int)node.GetOffset(),
                    Length = (int)node.GetLength()
                });
            });
        }
        private static IEnumerable<sqfvm.SqfNode> FindAllCodeNodes(sqfvm.SqfNode node)
        {
            if (node.GetNodeType() == sqfvm.SqfNodeType.CODE)
            {
                yield return node;
            }
            foreach (var it in node.GetChildren())
            {
                var res = FindAllCodeNodes(it);
                foreach (var subnode in res)
                {
                    yield return subnode;
                }
            }
        }
        #endregion
    }
}

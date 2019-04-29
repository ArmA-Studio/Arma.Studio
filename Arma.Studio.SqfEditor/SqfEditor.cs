using Arma.Studio.Data.IO;
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

        private sqfvm.ClrVirtualmachine Virtualmachine { get; }
        public SqfEditor()
        {
            this.Virtualmachine = new sqfvm.ClrVirtualmachine();
        }
        private class UsageContainer
        {
            sqfvm.SqfNode Node { get; set; }
            int Usage { get; set; }
            public UsageContainer(sqfvm.SqfNode node)
            {
                this.Node = node;
                this.Usage = 0;
            }
        }

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
        public File File { get; set; }
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
                var lintInfos = new List<LintInfo>();
                string errors, warnings, output;
                sqfvm.SqfNode cst;
                lock (this.Virtualmachine)
                {
                    var preproc = this.Virtualmachine.PreProcess(text, this.File?.FullPath ?? "");
                    cst = this.Virtualmachine.CreateSqfCst(preproc, this.File?.FullPath ?? "");
                    errors = this.Virtualmachine.ErrorContents();
                    warnings = this.Virtualmachine.WarningContents();
                    output = this.Virtualmachine.InfoContents();
                }
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
                else
                {
                    var usage = new Dictionary<string, UsageContainer>();
                    DetermineUsage(cst, usage);
                }
                return lintInfos;
            });
        }
        private static void DetermineUsage(sqfvm.SqfNode node, Dictionary<string, UsageContainer> usage)
        {
            switch (node.GetNodeType())
            {
                case sqfvm.SqfNodeType.ASSIGNMENT:
                    break;
                case sqfvm.SqfNodeType.VARIABLE:

                    break;
                default:
                    foreach (var it in node.GetChildren())
                    {
                        DetermineUsage(it, usage);
                    }
                    break;
            }
        }
        #endregion
        #region IFoldable
        public async Task<IEnumerable<FoldingInfo>> GetFoldings(string text, CancellationToken cancellationToken)
        {
            return await Task.Run(() => {
                var resList = new List<FoldingInfo>();
                int offset = 0;
                void recursive()
                {
                    char string_char = '\0';
                    for (; offset < text.Length; offset++)
                    {
                        char c = text[offset];
                        if (string_char != '\0')
                        {
                            if (c == string_char)
                            {
                                string_char = '\0';
                            }
                        }
                        else if (c == '"' || c == '\'')
                        {
                            string_char = c;
                        }
                        else if (c == '{')
                        {
                            var finfo = new FoldingInfo
                            {
                                StartOffset = offset
                            };
                            offset++;
                            recursive();
                            finfo.Length = offset - finfo.StartOffset.Value + 1;
                            resList.Add(finfo);
                        }
                        else if (c == '}')
                        {
                            return;
                        }
                    }
                }
                recursive();
                return resList;
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

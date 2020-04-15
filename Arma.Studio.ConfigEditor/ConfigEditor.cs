using Arma.Studio.Data;
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

namespace Arma.Studio.ConfigEditor
{
    public class ConfigEditor : ITextEditor, ILintable, IFoldable
    {
        private SqfVm.ClrVirtualmachine Virtualmachine { get; }
        public ConfigEditor()
        {
            this.Virtualmachine = new SqfVm.ClrVirtualmachine();
        }
        private class UsageContainer
        {
            SqfVm.Astnode Node { get; set; }
            int Usage { get; set; }
            public UsageContainer(SqfVm.Astnode node)
            {
                this.Node = node;
                this.Usage = 0;
            }
        }

        #region ITextEditor
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
                sf.Keywords.Add(new KeywordCollection(Colors.Blue, false).AddRange(new string[] { "class", "delete" }));
                sf.Keywords.Add(new KeywordCollection(Colors.Blue, false).AddRange(new string[] { "__EXEC", "__EVAL" }));
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
                SqfVm.Astnode cst;
                lock (this.Virtualmachine)
                {
                    var errorsBuilder = new StringBuilder();
                    var warningsBuilder = new StringBuilder();
                    var outputBuilder = new StringBuilder();
                    void Virtualmachine_OnLog(object sender, SqfVm.LogEventArgs eventArgs)
                    {
                        switch (eventArgs.Severity)
                        {
                            case SqfVm.ESeverity.Fatal:
                                errorsBuilder.AppendLine(eventArgs.Message);
                                break;
                            case SqfVm.ESeverity.Error:
                                errorsBuilder.AppendLine(eventArgs.Message);
                                break;
                            case SqfVm.ESeverity.Warning:
                                warningsBuilder.AppendLine(eventArgs.Message);
                                break;
                            case SqfVm.ESeverity.Info:
                                outputBuilder.AppendLine(eventArgs.Message);
                                break;    
                            case SqfVm.ESeverity.Verbose:
                                outputBuilder.AppendLine(eventArgs.Message);
                                break;
                            case SqfVm.ESeverity.Trace:
                                outputBuilder.AppendLine(eventArgs.Message);
                                break;
                            default:
                                break;
                        }
                    }
                    if (this.File.PBO != null)
                    {
                        if (this.File.PBO.Prefix is null && !this.Virtualmachine.PhysicalBoundaries.Any((it) => it.Equals(this.File.PBO.FullPath)))
                        {
                            this.Virtualmachine.AddPhysicalBoundary(this.File.PBO.FullPath);
                        }
                        else if (this.File.PBO.Prefix != null && !this.Virtualmachine.VirtualPaths.Any((it) => it.Equals(this.File.PBO.Prefix)))
                        {
                            this.Virtualmachine.AddVirtualMapping(this.File.PBO.Prefix, this.File.PBO.FullPath);
                        }
                    }
                    try
                    {
                        this.Virtualmachine.OnLog += Virtualmachine_OnLog;
                        var preproc = this.Virtualmachine.PreProcess(text, this.File?.FullPath ?? "");
                        cst = this.Virtualmachine.CreateConfigCst(preproc, this.File?.FullPath ?? "");
                    }
                    catch { cst = default; }
                    finally
                    {
                        this.Virtualmachine.OnLog -= Virtualmachine_OnLog;
                    }
                    errors = errorsBuilder.ToString();
                    warnings = warningsBuilder.ToString();
                    output = outputBuilder.ToString();
                }
                if (errors.Length > 0)
                {
                    using (var reader = new System.IO.StringReader(errors))
                    {
                        string logline;
                        while (!String.IsNullOrWhiteSpace(logline = reader.ReadLine()))
                        {
                            var matches = Regex.Matches(logline, @"\[L([0-9]+)\|C([0-9]+)\|(.+?)\](.+)");
                            foreach (Match match in matches)
                            {
                                if (match.Groups.Count != 5)
                                {
                                    continue;
                                }
                                else
                                {
                                    var line = Convert.ToInt32(match.Groups[1].Value);
                                    var column = Convert.ToInt32(match.Groups[2].Value);
                                    var file = Convert.ToString(match.Groups[3].Value);
                                    var message = Convert.ToString(match.Groups[4].Value);
                                    //var length = l2_markings.Count((c) => c == '^');
                                    lintInfos.Add(new LintInfo
                                    {
                                        Length = 1,
                                        Line = line,
                                        Column = column,
                                        Severity = ESeverity.Error,
                                        File = file.Trim(),
                                        Description = message.Trim()
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
        private static IEnumerable<SqfVm.Astnode> FindAllCodeNodes(SqfVm.Astnode node)
        {
            if (node.GetSqfNodeType() == SqfVm.SqfAstnodeType.CODE)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RealVirtuality.Lang.Preprocessing
{
    public class PreProcessingDirective
    {
        public bool HasParameters => this._Parameters.Length > 0;
        public int ParameterCount => this._Parameters.Length;
        public IEnumerable<string> Parameters => this._Parameters;
        private string[] _Parameters;
        public string Name { get; internal set; }
        public string UnparsedText { get; private set; }

        private PreProcessingDirective()
        {

        }
        /// <summary>
        /// Applies the marco using the provided <paramref name="args"/>
        /// </summary>
        /// <param name="args">Parameters for the Macro. If less the actual param count, empty string will be used. If too many params are provided, they will be ignored.</param>
        /// <returns>The text the macro is supposed to have in the end.</returns>
        public string GetText(params string[] args)
        {
            var text = this.UnparsedText;
            text = text.Replace("##", string.Empty);
            text = Regex.Replace(text, @"#(.+?\b)", "\"$1\"");
            for (var i = 0; i < this._Parameters.Length; i++)
            {
                string arg = args[i];
                if (args.Length > i)
                {
                    arg = args[i];
                }
                text = Regex.Replace(text, $@"\b{this._Parameters[i]}\b", arg);
            }
            return text;
        }

        //ToDo: Add a way to output the errors during parsing
        /// <summary>
        /// Creates a new instance of a <see cref="PreProcessingDirective"/>.
        /// It might throw an error if something is not well formed.
        /// </summary>
        /// <param name="existing">The already existing <see cref="PreProcessingDirective"/>s.</param>
        /// <param name="line">The line to parse. May start with <code>define</code>.</param>
        /// <example><code>CreateNew(new PreProcessingDirective[0], "define stringify(TEXT) #TEXT")</code></example>
        /// <returns>Initialized <see cref="PreProcessingDirective"/> instance.</returns>
        public static PreProcessingDirective CreateNew(IEnumerable<PreProcessingDirective> existing, string line)
        {
            if(line.StartsWith("define"))
            {
                line = line.Remove(0, "define".Length).TrimStart();
            }
            string name = string.Empty;
            var arglist = new List<string>();
            string content = string.Empty;

            var nameend = line.IndexOfAny(new char[] { '(', ' ', '\t' });
            name = line.Substring(0, nameend);
            if (nameend != -1 && line[nameend] == '(')
            {
                var endofargs = line.IndexOf(')');
                if (endofargs == -1)
                {
                    throw new Exception();
                }
                var argstring = line.Substring(nameend + 1, endofargs - nameend - 1);
                arglist.AddRange(argstring.Split(',').Select((s) => s.Trim()));
                content = line.Substring(endofargs + 2);
            }
            else
            {
                content = line.Substring(nameend + 1);
            }

            return new PreProcessingDirective() { Name = name, UnparsedText = PrepareLine(existing, content).TrimStart(), _Parameters = arglist.ToArray() };
        }

        private static string PrepareLine(IEnumerable<PreProcessingDirective> existing, string line)
        {
            foreach(var it in existing)
            {
                if (it.HasParameters)
                {
                    var matches = Regex.Matches(line, $@"\b{it.Name}\b");
                    if (matches.Count == 0)
                        continue;
                    foreach(Match match in matches)
                    {
                        var argstart = match.Index + match.Length;
                        var argend = line.IndexOf(')', match.Index);
                        var argstring = line.Substring(argstart + 1, argend - argstart - 1);
                        var strbldr = new StringBuilder(line);
                        strbldr.Remove(match.Index, argend - match.Index + 1);
                        strbldr.Insert(match.Index, it.GetText(argstring.Split(',')));
                        line = strbldr.ToString();
                    }
                }
                else
                {
                    line = Regex.Replace(line, $@"\b{it.Name}\b", it.GetText());
                }
            }
            return line;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RealVirtuality.Lang.Preprocessing
{
    public class PreProcessingDirective
    {
        public bool HasParameters { get; internal set; }
        public IEnumerable<string> Parameters => this._Parameters;
        private string[] _Parameters;
        public string Name { get; internal set; }
        public string UnparsedText { get; private set; }

        private PreProcessingDirective() { }
        public string GetText(params string[] args)
        {
            var text = this.UnparsedText;
            text = Regex.Replace(text, @"\b#(.*)\b", "\"$1\"");
            for (var i = 0; i < this._Parameters.Length; i++)
            {
                string arg = args[i];
                if (args.Length > i)
                {
                    arg = args[i];
                }
                text = Regex.Replace(text, $@"\b{this._Parameters[i]}\b", arg);
            }
            text = text.Replace("##", string.Empty);
            return text;
        }

        public static PreProcessingDirective CreateNew(IEnumerable<PreProcessingDirective> existing, string line)
        {
            //ToDo: Implement
        }
    }
}
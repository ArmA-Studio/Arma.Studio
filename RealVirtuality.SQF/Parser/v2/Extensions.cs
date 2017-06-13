using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

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
        public static int IndexOfAny(this string s, params char[] arr) => s.IndexOfAny(arr);
        public static int IndexOfAny(this string s, int start, params char[] arr) => s.IndexOfAny(arr, start);
    }

    public class PreProcessingStreamHelper : Stream
    {
        private readonly Stream BaseStream;
        internal PPDirective CurrentDirective;
        private string CurrentDirectiveSource;
        private int CurrentDirectiveSourceIndex;

        public override bool CanRead => this.BaseStream.CanRead;
        public override bool CanSeek => this.BaseStream.CanSeek;
        public override bool CanWrite => this.BaseStream.CanWrite;
        public override long Length => this.BaseStream.Length;
        public override long Position { get { return this.BaseStream.Position; } set { this.BaseStream.Position = value; } }


        public PreProcessingStreamHelper(System.IO.Stream input)
        {
            this.BaseStream = input;
            if (!this.BaseStream.CanSeek || !this.BaseStream.CanRead)
                throw new ArgumentException("provided stream is not supporting seek or read operation.", nameof(input));

            this.CurrentDirectiveSourceIndex = 0;
            this.CurrentDirectiveSource = string.Empty;
            this.CurrentDirective = null;
        }

        public override void Flush() => this.BaseStream.Flush();
        public override long Seek(long offset, SeekOrigin origin) => this.BaseStream.Seek(offset, origin);
        public override void SetLength(long value) => this.BaseStream.SetLength(value);
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.CurrentDirective != null)
            {
                var remLen = this.CurrentDirectiveSource.Length - this.CurrentDirectiveSourceIndex;
                int i;
                int j = 0;
                for (i = this.CurrentDirectiveSourceIndex; remLen > 0; remLen--, i++)
                {
                    char c = this.CurrentDirectiveSource[i];
                    buffer[offset++] = (byte)c;
                    count--;
                    j++;
                }
                this.CurrentDirectiveSourceIndex = i;
                if (count == 0)
                {
                    return j;
                }
                else
                {
                    return this.BaseStream.Read(buffer, offset, count) + j;
                }
            }
            return this.BaseStream.Read(buffer, offset, count);
        }
        public override void Write(byte[] buffer, int offset, int count) => this.BaseStream.Write(buffer, offset, count);

        internal void SetDefine(PPDirective ppd, string[] v, IEnumerable<PPDirective> existing)
        {
            this.CurrentDirectiveSourceIndex = 0;
            this.CurrentDirectiveSource = ppd.Expand(v, existing);
            this.CurrentDirective = ppd;
        }
    }
    public class PPDirective
    {
        public string Name { get; set; }
        public List<string> Args { get; set; }
        public string Content;

        public PPDirective()
        {
            this.Args = new List<string>();
        }
        
        internal string ExpandFromFull(string s, IEnumerable<PPDirective> existing)
        {
            var index = s.IndexOf('(');
            string name;
            if (index == -1)
            {
                name = s;
            }
            else
            {
                name = s.Substring(0, index);
            }
            var args = s.Substring(index + 1, s.IndexOf(')')).Split(',').Select((S) => S.Trim());
            if (name.Equals(this.Name))
            {
                return Expand(args.ToArray(), existing);
            }
            else
            {
                throw new ArgumentException("provided full string is not matching to this PPDirective's name.", nameof(s));
            }
        }

        internal string Expand(string[] v, IEnumerable<PPDirective> existing)
        {
            if (v.Length != this.Args.Count)
                throw new ArgumentOutOfRangeException(nameof(v), "Provided ammount of argument replacement is not matching macro arg count");
            //# <-- stringify
            //## <-- concat
            //ARG
            //DIRECTIVE(foo, bar)
            var text = this.Content;
            for (int i = 0; i < this.Args.Count; i++)
            {
                var s = v[i];
                text = Regex.Replace(text, this.Args[i], $@"\b{s}\b");
            }
            foreach (var ppd in existing.OrderByDescending((d) => d.Args.Count))
            {
                if (ppd == this)
                    continue;
                Match match;
                int loopbreaker = 0;
                const int MAXLOOP = 100000;
                while ((match = Regex.Match(text, $@"\b({ppd.Name})\b")).Success)
                {
                    var fullMatch = text.Substring(match.Index, ppd.Args.Any() ? text.Length - text.IndexOf(')') : match.Length);
                    text = text.Remove(match.Index, fullMatch.Length);
                    text = text.Insert(match.Index, ppd.ExpandFromFull(fullMatch, existing));
                    if (loopbreaker++ > MAXLOOP)
                        throw new Exception($"ENDLESS DIRECTIVE LOOP OR > {MAXLOOP} OCCURANCES FOR {ppd.Name}");
                }
            }
            return text;
        }

        public static PPDirective Parse(string text)
        {
            if (text.StartsWith("#define"))
            {
                text = text.Substring("#define".Length).Trim();
            }
            var index = text.IndexOfAny(' ', '\t', '(', '\\');
            var ppd = new PPDirective();
            ppd.Name = text.Substring(0, index);
            text = text.Substring(0, index).TrimStart();

            if (!text.StartsWith("("))
            {
                ppd.Content = text.Replace('\\', ' ').Replace("\r", string.Empty).Replace("\n", string.Empty);
                return ppd;
            }
            index = text.IndexOf(')');
            var argText = text.Substring(0, index);
            ppd.Args.AddRange(argText.Split(',').Select((s) => s.Trim()));
            ppd.Content = text.Substring(index + 1).TrimStart().Replace('\\', ' ').Replace("\r", string.Empty).Replace("\n", string.Empty);
            return ppd;
        }
    }
    public class PreProcessingTokenStream : CommonTokenStream
    {
        public bool InPreprocessingMode => this.StreamHelper.CurrentDirective != null;
        public List<PPDirective> Defines;
        private readonly PreProcessingStreamHelper StreamHelper;
        private Lexer CurLexer => this.TokenSource as Lexer;
        public PreProcessingTokenStream(Lexer tokenSource, PreProcessingStreamHelper streamhelper) : base(tokenSource)
        {
            this.Defines = new List<PPDirective>();
            this.StreamHelper = streamhelper;
        }
        //only this override is required as ANTLR4 is first checking the type to be matching
        public override int La(int i)
        {
            var tmp = this.Lt(i);
            if (tmp != null)
            {
                if (tmp.Text.StartsWith("#"))
                {
                    if (tmp.Text.StartsWith("#define"))
                    {
                        var other = tmp.Text.Substring("#define".Length).Trim();
                        var ppd = PPDirective.Parse(tmp.Text);
                        var ppdExists = this.Defines.FirstOrDefault((d) => d.Name.Equals(other));
                        if (ppdExists != null)
                        {
                            this.Defines.Remove(ppdExists);
                        }
                        this.Defines.Add(ppd);
                    }
                    else if(tmp.Text.StartsWith("#undef"))
                    {
                        var name = tmp.Text.Substring("#undef".Length).Trim();
                        this.Defines.RemoveAll((d) => d.Name.Equals(name));
                    }
                    //ToDo: Implement the other instructions
                    else if(tmp.Text.StartsWith("#include"))
                    {
                        throw new NotImplementedException();
                    }
                    else if(tmp.Text.StartsWith("#ifdef"))
                    {
                        throw new NotImplementedException();
                    }
                    else if (tmp.Text.StartsWith("#ifndef"))
                    {
                        throw new NotImplementedException();
                    }
                    else if (tmp.Text.StartsWith("#else"))
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    var ppd = Defines.First((d) => d.Name == tmp.Text);
                    if (ppd != null)
                    {
                        var builder = new StringBuilder();
                        builder.Append(tmp.Text);
                        var t = tmp;
                        for(int j = i + 1; t == null && !t.Text.Equals(")"); j++)
                        {
                            t = base.Lt(j);
                            builder.Append(t);
                        }

                        var s = builder.ToString();
                        var index = s.IndexOf('(');
                        string name;
                        if (index == -1)
                        {
                            name = s;
                        }
                        else
                        {
                            name = s.Substring(0, index);
                        }
                        var args = s.Substring(index + 1, s.IndexOf(')')).Split(',').Select((S) => S.Trim());

                        this.StreamHelper.SetDefine(ppd, args.ToArray(), this.Defines);
                    }
                }
            }
            return base.La(i);
        }
    }
}

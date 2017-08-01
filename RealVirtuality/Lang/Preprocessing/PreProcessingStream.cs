using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace RealVirtuality.Lang.Preprocessing
{
    public class PreProcessingStream : Stream
    {
        public event EventHandler<EventArgs> OnIncludeLookup;
        protected enum EIfDefMode
        {
            NA,
            IF,
            ELSE
        }
        private readonly Stream InnerStream;
        private StringBuilder PPBuffer;
        public IEnumerable<PreProcessingDirective> PreProcessingDirectives => this._PreProcessingDirectives;
        private List<PreProcessingDirective> _PreProcessingDirectives;
        public IEnumerable<PreProcessedInfo> PreProcessedInfos => this._PreProcessedInfos;
        private List<PreProcessedInfo> _PreProcessedInfos;
        public IEnumerable<PreProcessingError> Errors => this._Errors;
        private List<PreProcessingError> _Errors;
        private bool IsInPreprocessingDefinition;
        private bool IsWhitespaceCleaning;
        private bool IsEscapedNewLine;
        private bool IsInPreProcessingMode;
        private int CurrentLineOffset;
        private int PPModeOffset;

        protected List<EIfDefMode> IfDefStack;
        private List<StreamReader> IncludeFiles;

        public int LineCount { get; private set; }

        public override bool CanRead => this.InnerStream.CanRead;

        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => this.InnerStream.Length;
        public override long Position { get; set; }
        private int PeekByte;

        /// <summary>
        /// Creates a wrapper that pre-processes the input before passing it out.
        /// Regardless of the input stream, this stream will have disabled seek support.
        /// </summary>
        /// <param name="s">Stream to wrap</param>
        /// <exception cref="ArgumentException">Will be thrown if provided stream is not supporting read operations (indicated via the CanRead property)</exception>
        public PreProcessingStream(Stream s)
        {
            var x = 12;
            this.InnerStream = s;
            if (!s.CanRead)
            {
                throw new ArgumentException("Provided stream is not supporting read operations.", nameof(s));
            }
            this.IsInPreprocessingDefinition = false;
            this.IsWhitespaceCleaning = false;
            this.PPBuffer = new StringBuilder();
            this._PreProcessingDirectives = new List<PreProcessingDirective>();
            this._PreProcessedInfos = new List<PreProcessedInfo>();
            this.Position = s.Position;
            this.PeekByte = '\0';
            this.IfDefStack = new List<EIfDefMode>();
            this._Errors = new List<PreProcessingError>();
            this.CurrentLineOffset = 0;
            this.PPModeOffset = 0;
        }
        public override void Flush() => this.InnerStream.Flush();
        public override long Seek(long offset, SeekOrigin origin) { throw new InvalidOperationException(); }
        public override void SetLength(long value) => this.InnerStream.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) { throw new InvalidOperationException(); }
        

        private int NextByte()
        {
            if (this.IsInPreProcessingMode && this.PPModeOffset >= this.PPBuffer.Length)
            {
                this.IsInPreProcessingMode = false;
                this.PPModeOffset = 0;
                this.PPBuffer.Clear();
            }
            if (this.IsInPreProcessingMode)
            {
                var tmp = this.PPBuffer[this.PPModeOffset++];
                return tmp;
            }
            else
            {
                if (this.PeekByte != '\0')
                {
                    var tmp = this.PeekByte;
                    this.PeekByte = '\0';
                    return tmp;
                }
                return this.InnerStream.ReadByte();
            }
        }
        private int NextBytePeek()
        {
            if (this.PeekByte != '\0')
                return this.PeekByte;
            this.PeekByte = this.InnerStream.ReadByte();
            return this.PeekByte;
        }

        private void ParsePreProcessingDirective(string input, int currentpositionoffset)
        {
            var endofstartindex = input.IndexOf(char.IsWhiteSpace);
            if (input.Length == 0 || endofstartindex == -1 || char.IsWhiteSpace(input[0]))
            {
                this._Errors.Add(new PreProcessingError()
                {
                    Line = this.LineCount,
                    Column = this.CurrentLineOffset,
                    StartOffset = this.Position + currentpositionoffset - input.Length,
                    EndOffset = this.Position + currentpositionoffset,
                    Message = Properties.Localization.PP_PARSE_E0_Unknown,
                    ErrorCode = 0
                });
                return;
            }
            var start = input.Substring(0, endofstartindex);
            var content = input.Substring(endofstartindex).Trim();
            var contentStart = content.Length > 0 ? input.IndexOf(content[0]) : 0;
            switch (start)
            {
                case "define":
                    {
                        try
                        {
                            var ppd = PreProcessingDirective.CreateNew(this._PreProcessingDirectives, input);
                            this._PreProcessingDirectives.Add(ppd);
                        }
                        catch(Exception ex)
                        {

                        }
                    }
                    break;
                case "undef":
                    {
                        var wsindex = content.IndexOf(char.IsWhiteSpace);
                        if (wsindex != -1)
                        {
                            this._Errors.Add(new PreProcessingError()
                            {
                                Line = this.LineCount,
                                Column = this.CurrentLineOffset,
                                StartOffset = this.Position + currentpositionoffset - input.Length + contentStart + wsindex,
                                EndOffset = this.Position + currentpositionoffset,
                                Message = Properties.Localization.PP_PARSE_E2_UnexpectedAdittionalContent,
                                ErrorCode = 2
                            });
                            content = content.Substring(0, wsindex);
                        }
                        var ppdindex = this._PreProcessingDirectives.IndexOf((ppd) => ppd.Name.Equals(content));
                        if (ppdindex == -1)
                        {
                            this._Errors.Add(new PreProcessingError()
                            {
                                Line = this.LineCount,
                                Column = this.CurrentLineOffset,
                                StartOffset = this.Position + currentpositionoffset - input.Length + contentStart,
                                EndOffset = this.Position + currentpositionoffset,
                                Message = Properties.Localization.PP_PARSE_E3_NoMatchingDefine,
                                ErrorCode = 3
                            });
                        }
                        else
                        {
                            this._PreProcessingDirectives.RemoveAt(ppdindex);
                        }
                    }
                    break;
                case "include":
                    break;
                case "ifdef":
                    break;
                case "ifndef":
                    break;
                case "else":
                    break;
                case "endif":
                    break;
                default:
                    this._Errors.Add(new PreProcessingError()
                    {
                        Line = this.LineCount,
                        Column = this.CurrentLineOffset,
                        StartOffset = this.Position + currentpositionoffset - input.Length,
                        EndOffset = this.Position + currentpositionoffset - input.Length + start.Length,
                        Message = Properties.Localization.PP_PARSE_E1_UnknownDirective,
                        ErrorCode = 1
                    });
                    break;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            //this.InnerStream.Read(buffer, offset, count);
            int i = offset;
            for (int counter = 0; counter < count; counter++)
            {
                //var cbyte = buffer[i];
                var cbyte = this.NextByte();
                if (cbyte == -1)
                {
                    buffer[i] = (byte)'\0';
                    break;
                }
                if (this.IsInPreprocessingDefinition)
                {
                    switch ((char)cbyte)
                    {
                        case '\n':
                            if(this.IsEscapedNewLine)
                            {
                                this.IsEscapedNewLine = false;
                                break;
                            }
                            this.IsInPreprocessingDefinition = false;
                            this.ParsePreProcessingDirective(this.PPBuffer.ToString(), i);
                            this.LineCount++;
                            this.CurrentLineOffset = 0;
                            buffer[i++] = (byte)'\n';
                            this.PPBuffer.Clear();
                            break;
                        case '\\':
                            this.IsEscapedNewLine = true;
                            goto case '\r';
                        case '\r':
                        case ' ':
                        case '\t':
                            if (!this.IsWhitespaceCleaning)
                            {
                                this.IsWhitespaceCleaning = true;
                                this.PPBuffer.Append(' ');
                            }
                            break;
                        default:
                            this.PPBuffer.Append((char)cbyte);
                            this.IsWhitespaceCleaning = false;
                            break;
                    }
                }
                else
                {
                    switch ((char)cbyte)
                    {
                        case '#':
                            this.IsInPreprocessingDefinition = true;
                            this.PPBuffer.Clear();
                            break;
                        case '\n':
                            this.LineCount++;
                            this.CurrentLineOffset = 0;
                            goto default;
                        default:
                            if (this.IsInPreProcessingMode)
                            {
                                buffer[i++] = (byte)cbyte;
                                this.CurrentLineOffset++;
                                break;
                            }
                            this.PPBuffer.Append((char)cbyte);
                            var iold = i;
                            this.CheckPP(buffer, ref i, offset);
                            this.CurrentLineOffset += i - iold;
                            break;
                    }
                }
            }
            this.Position += i;
            return i;
        }

        private void CheckPP(byte[] buffer, ref int i, int offset)
        {
            if (!this.PreProcessingDirectives.Any((d) => d.Name[0] == this.PPBuffer[0]))
            {
                buffer[i++] = (byte)this.PPBuffer[0];
                this.PPBuffer.Clear();
            }
            else
            {
                var directive = this.PreProcessingDirectives.FirstOrDefault((d) => d.Name == this.PPBuffer.ToString());
                var peekChar = (char)this.NextBytePeek();
                var startOffset = this.Position - this.PPBuffer.Length;
                if (!char.IsLetterOrDigit(peekChar) && peekChar != '_')
                {
                    if (directive == null)
                    {
                        this.IsInPreProcessingMode = true;
                        return;
                    }
                    this.PPBuffer.Clear();
                    if (directive.HasParameters)
                    {
                        var flag = true;
                        while (flag)
                        {
                            peekChar = (char)this.NextByte();
                            switch (peekChar)
                            {
                                case '(':
                                    flag = false;
                                    break;
                                case ' ':
                                case '\t':
                                case '\r':
                                case '\n':
                                    break;
                                default:
                                    //ToDo: Find a way to log missing parameters as error
                                    return;
                            }
                        }
                        char nextChar;
                        
                        while((nextChar = (char)this.NextByte()) != ')')
                        {
                            this.PPBuffer.Append(nextChar);
                        }
                        var ppparams = this.PPBuffer.ToString().Split(',', ')');
                        this._PreProcessedInfos.Add(new PreProcessedInfo(directive, startOffset, this.Position, this.LineCount, this.CurrentLineOffset, ppparams));
                    }
                    else
                    {
                        this._PreProcessedInfos.Add(new PreProcessedInfo(directive, startOffset, this.Position, this.LineCount, this.CurrentLineOffset, new string[0]));
                    }
                    this.PPBuffer.Clear();
                    this.IsInPreProcessingMode = true;
                    this.PPBuffer.Append(this._PreProcessedInfos.Last().PreProcessedText);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealVirtuality.Lang.Preprocessing
{
    public class PreProcessingStream : Stream
    {
        private readonly Stream InnerStream;
        private StringBuilder PPBuffer;
        public IEnumerable<PreProcessingDirective> PreProcessingDirectives => this._PreProcessingDirectives;
        private List<PreProcessingDirective> _PreProcessingDirectives;
        public IEnumerable<PreProcessedInfo> PreProcessedInfos => this._PreProcessedInfos;
        private List<PreProcessedInfo> _PreProcessedInfos;
        private bool IsInPreprocessingDefinition;
        private bool IsWhitespaceCleaning;
        private bool IsInPreProcessingMode;
        private int PPModeOffset;

        public int LineCount { get; private set; }

        public override bool CanRead => this.InnerStream.CanRead;

        public override bool CanSeek => this.InnerStream.CanRead;
        public override bool CanWrite => this.InnerStream.CanWrite;
        public override long Length => this.InnerStream.Length;
        public override long Position { get; set; }
        private int PeekByte;

        public PreProcessingStream(Stream s)
        {
            this.InnerStream = s;
            this.IsInPreprocessingDefinition = false;
            this.IsWhitespaceCleaning = false;
            this.PPBuffer = new StringBuilder();
            this._PreProcessingDirectives = new List<PreProcessingDirective>();
            this._PreProcessedInfos = new List<PreProcessedInfo>();
            this.Position = s.Position;
            this.PeekByte = '\0';
        }
        public override void Flush() => this.InnerStream.Flush();
        public override long Seek(long offset, SeekOrigin origin) => this.InnerStream.Seek(offset, origin);
        public override void SetLength(long value) => this.InnerStream.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => this.InnerStream.Write(buffer, offset, count);

        private int NextByte()
        {
            this.Position++;
            if (this.IsInPreProcessingMode)
            {
                var tmp = this.PPBuffer[this.PPModeOffset--];
                if (this.PPModeOffset < 0)
                    this.IsInPreProcessingMode = false;
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

        public override int Read(byte[] buffer, int offset, int count)
        {
            this.InnerStream.Read(buffer, offset, count);
            for (var i = offset; i < count; i++)
            {
                //var cbyte = buffer[i];
                var cbyte = this.NextByte();
                
                if (this.IsInPreprocessingDefinition)
                {
                    switch ((char)cbyte)
                    {
                        case '\n':
                            this.IsInPreprocessingDefinition = false;
                            this.LineCount++;
                            //ToDo: Add the PPDirective
                            break;
                        case '\\':
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
                            goto default;
                        default:
                            this.PPBuffer.Append((char)cbyte);
                            this.CheckPP(buffer, i, offset);
                            break;
                    }
                }
            }
        }

        private void CheckPP(byte[] buffer, int i, int offset)
        {
            if (!this.PreProcessingDirectives.Any((d) => d.Name[0] == this.PPBuffer[0]))
            {
                this.PPBuffer.Clear();
            }
            else
            {
                var directive = this.PreProcessingDirectives.FirstOrDefault((d) => d.Name == this.PPBuffer.ToString());
                var peekChar = (char)this.NextBytePeek();
                var startOffset = this.Position - this.PPBuffer.Length;
                if (!char.IsLetterOrDigit(peekChar) && peekChar != '_')
                {
                    this.IsInPreProcessingMode = true;
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
                        
                        do
                        {
                            nextChar = (char)this.NextByte();
                            this.PPBuffer.Append(nextChar);
                        }
                        while (nextChar != ')');
                        var ppparams = this.PPBuffer.ToString().Split(',', ')');
                        this._PreProcessedInfos.Add(new PreProcessedInfo(directive, startOffset, this.Position, ppparams));
                        this.PPBuffer.Clear();
                        this.PPBuffer.Append(this._PreProcessedInfos.Last().PreProcessedText);
                    }
                }
            }
        }
    }
}

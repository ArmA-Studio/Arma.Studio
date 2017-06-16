
using System;
using System.IO;
using System.Collections;


namespace RealVirtuality.SQF.Parser
{
    public class Token {
        public int kind;    // token kind
        public int pos;     // token position in bytes in the source text (starting at 0)
        public int charPos;  // token position in characters in the source text (starting at 0)
        public int col;     // token column (starting at 1)
        public int line;    // token line (starting at 1)
        public string val;  // token value
        public Token next;  // ML 2005-03-11 Tokens are kept in linked list
    }

    //-----------------------------------------------------------------------------------
    // Buffer
    //-----------------------------------------------------------------------------------
    public class Buffer {
        // This Buffer supports the following cases:
        // 1) seekable stream (file)
        //    a) whole stream in buffer
        //    b) part of stream in buffer
        // 2) non seekable stream (network, console)

        public const int EOF = char.MaxValue + 1;
        const int MIN_BUFFER_LENGTH = 1024; // 1KB
        const int MAX_BUFFER_LENGTH = MIN_BUFFER_LENGTH * 64; // 64KB
        byte[] buf;         // input buffer
        int bufStart;       // position of first byte in buffer relative to input stream
        int bufLen;         // length of buffer
        int fileLen;        // length of input stream (may change if the stream is no file)
        int bufPos;         // current position in buffer
        Stream stream;      // input stream (seekable)
        bool isUserStream;  // was the stream opened by the user?
        
        public Buffer (Stream s, bool isUserStream) {
            this.stream = s; this.isUserStream = isUserStream;
            
            if (this.stream.CanSeek) {
                this.fileLen = (int) this.stream.Length;
                this.bufLen = Math.Min(this.fileLen, MAX_BUFFER_LENGTH);
                this.bufStart = Int32.MaxValue; // nothing in the buffer so far
            } else {
                this.fileLen = this.bufLen = this.bufStart = 0;
            }

            this.buf = new byte[(this.bufLen>0) ? this.bufLen : MIN_BUFFER_LENGTH];
            if (this.fileLen > 0) this.Pos = 0; // setup buffer to position 0 (start)
            else this.bufPos = 0; // index 0 is already after the file, thus Pos = 0 is invalid
            if (this.bufLen == this.fileLen && this.stream.CanSeek) this.Close();
        }
        
        protected Buffer(Buffer b) { // called in UTF8Buffer constructor
            this.buf = b.buf;
            this.bufStart = b.bufStart;
            this.bufLen = b.bufLen;
            this.fileLen = b.fileLen;
            this.bufPos = b.bufPos;
            this.stream = b.stream;
            // keep destructor from closing the stream
            b.stream = null;
            this.isUserStream = b.isUserStream;
        }

        ~Buffer() {
            this.Close(); }
        
        protected void Close() {
            if (!this.isUserStream && this.stream != null) {
                this.stream.Close();
                this.stream = null;
            }
        }
        
        public virtual int Read () {
            if (this.bufPos < this.bufLen) {
                return this.buf[this.bufPos++];
            } else if (this.Pos < this.fileLen) {
                this.Pos = this.Pos; // shift buffer start to Pos
                return this.buf[this.bufPos++];
            } else if (this.stream != null && !this.stream.CanSeek && this.ReadNextStreamChunk() > 0) {
                return this.buf[this.bufPos++];
            } else {
                return EOF;
            }
        }

        public int Peek () {
            int curPos = this.Pos;
            int ch = this.Read();
            this.Pos = curPos;
            return ch;
        }
        
        // beg .. begin, zero-based, inclusive, in byte
        // end .. end, zero-based, exclusive, in byte
        public string GetString (int beg, int end) {
            int len = 0;
            char[] buf = new char[end - beg];
            int oldPos = this.Pos;
            this.Pos = beg;
            while (this.Pos < end) buf[len++] = (char) this.Read();
            this.Pos = oldPos;
            return new String(buf, 0, len);
        }

        public int Pos {
            get { return this.bufPos + this.bufStart; }
            set {
                if (value >= this.fileLen && this.stream != null && !this.stream.CanSeek) {
                    // Wanted position is after buffer and the stream
                    // is not seek-able e.g. network or console,
                    // thus we have to read the stream manually till
                    // the wanted position is in sight.
                    while (value >= this.fileLen && this.ReadNextStreamChunk() > 0);
                }

                if (value < 0 || value > this.fileLen) {
                    throw new FatalError("buffer out of bounds access, position: " + value);
                }

                if (value >= this.bufStart && value < this.bufStart + this.bufLen) { // already in buffer
                    this.bufPos = value - this.bufStart;
                } else if (this.stream != null) { // must be swapped in
                    this.stream.Seek(value, SeekOrigin.Begin);
                    this.bufLen = this.stream.Read(this.buf, 0, this.buf.Length);
                    this.bufStart = value;
                    this.bufPos = 0;
                } else {
                    // set the position to the end of the file, Pos will return fileLen.
                    this.bufPos = this.fileLen - this.bufStart;
                }
            }
        }
        
        // Read the next chunk of bytes from the stream, increases the buffer
        // if needed and updates the fields fileLen and bufLen.
        // Returns the number of bytes read.
        private int ReadNextStreamChunk() {
            int free = this.buf.Length - this.bufLen;
            if (free == 0) {
                // in the case of a growing input stream
                // we can neither seek in the stream, nor can we
                // foresee the maximum length, thus we must adapt
                // the buffer size on demand.
                byte[] newBuf = new byte[this.bufLen * 2];
                Array.Copy(this.buf, newBuf, this.bufLen);
                this.buf = newBuf;
                free = this.bufLen;
            }
            int read = this.stream.Read(this.buf, this.bufLen, free);
            if (read > 0) {
                this.fileLen = this.bufLen = (this.bufLen + read);
                return read;
            }
            // end of stream reached
            return 0;
        }
    }

    //-----------------------------------------------------------------------------------
    // UTF8Buffer
    //-----------------------------------------------------------------------------------
    public class UTF8Buffer: Buffer {
        public UTF8Buffer(Buffer b): base(b) {}

        public override int Read() {
            int ch;
            do {
                ch = base.Read();
                // until we find a utf8 start (0xxxxxxx or 11xxxxxx)
            } while ((ch >= 128) && ((ch & 0xC0) != 0xC0) && (ch != EOF));
            if (ch < 128 || ch == EOF) {
                // nothing to do, first 127 chars are the same in ascii and utf8
                // 0xxxxxxx or end of file character
            } else if ((ch & 0xF0) == 0xF0) {
                // 11110xxx 10xxxxxx 10xxxxxx 10xxxxxx
                int c1 = ch & 0x07; ch = base.Read();
                int c2 = ch & 0x3F; ch = base.Read();
                int c3 = ch & 0x3F; ch = base.Read();
                int c4 = ch & 0x3F;
                ch = (((((c1 << 6) | c2) << 6) | c3) << 6) | c4;
            } else if ((ch & 0xE0) == 0xE0) {
                // 1110xxxx 10xxxxxx 10xxxxxx
                int c1 = ch & 0x0F; ch = base.Read();
                int c2 = ch & 0x3F; ch = base.Read();
                int c3 = ch & 0x3F;
                ch = (((c1 << 6) | c2) << 6) | c3;
            } else if ((ch & 0xC0) == 0xC0) {
                // 110xxxxx 10xxxxxx
                int c1 = ch & 0x1F; ch = base.Read();
                int c2 = ch & 0x3F;
                ch = (c1 << 6) | c2;
            }
            return ch;
        }
    }

    //-----------------------------------------------------------------------------------
    // Scanner
    //-----------------------------------------------------------------------------------
    public class Scanner {
        const char EOL = '\n';
        const int eofSym = 0; /* pdt */
    	const int maxT = 39;
	const int noSym = 39;


        public Buffer buffer; // scanner buffer
        
        Token t;          // current token
        int ch;           // current input character
        int pos;          // byte position of current character
        int charPos;      // position by unicode characters starting with 0
        int col;          // column number of current character
        int line;         // line number of current character
        int oldEols;      // EOLs that appeared in a comment;
        static readonly Hashtable start; // maps first token character to start state

        Token tokens;     // list of tokens already peeked (first token is a dummy)
        Token pt;         // current peek token
        
        char[] tval = new char[128]; // text of current token
        int tlen;         // length of current token
        
        static Scanner() {
            start = new Hashtable(128);
    		for (int i = 49; i <= 57; ++i) start[i] = 1;
		for (int i = 65; i <= 90; ++i) start[i] = 8;
		for (int i = 95; i <= 95; ++i) start[i] = 8;
		for (int i = 97; i <= 122; ++i) start[i] = 8;
		start[45] = 33; 
		start[48] = 9; 
		start[34] = 6; 
		start[36] = 7; 
		start[123] = 11; 
		start[125] = 12; 
		start[59] = 13; 
		start[61] = 34; 
		start[124] = 14; 
		start[38] = 16; 
		start[33] = 35; 
		start[62] = 36; 
		start[60] = 37; 
		start[43] = 23; 
		start[42] = 24; 
		start[47] = 25; 
		start[37] = 26; 
		start[94] = 27; 
		start[40] = 28; 
		start[41] = 29; 
		start[91] = 30; 
		start[44] = 31; 
		start[93] = 32; 
		start[Buffer.EOF] = -1;

        }
        public bool FollowedBy(string s)
        {
            Token next = this.Peek();
            return next.val == s;
        }
        public bool FollowedBy(string[] sArr)
        {
            foreach(var s in sArr)
            {
                Token next = this.Peek();
                if(next.val == s)
                    return true;
            }
            return false;
        }
        public bool FollowedByWO(string s)
        {
            Token next = this.Peek();
            return this.t.val == s || next.val == s;
        }
        public bool FollowedByWO(string[] sArr)
        {
            foreach(var s in sArr)
            {
                Token next = this.Peek();
                if(this.t.val == s || next.val == s)
                    return true;
            }
            return false;
        }
        public Scanner (string fileName) {
            try {
                Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                this.buffer = new Buffer(stream, false);
                this.Init();
            } catch (IOException) {
                throw new FatalError("Cannot open file " + fileName);
            }
        }
        
        public Scanner (Stream s) {
            this.buffer = new Buffer(s, true);
            this.Init();
        }
        
        void Init() {
            this.pos = -1;
            this.line = 1;
            this.col = 0;
            this.charPos = -1;
            this.oldEols = 0;
            this.NextCh();
            if (this.ch == 0xEF) { // check optional byte order mark for UTF-8
                this.NextCh(); int ch1 = this.ch;
                this.NextCh(); int ch2 = this.ch;
                if (ch1 != 0xBB || ch2 != 0xBF) {
                    throw new FatalError(String.Format("illegal byte order mark: EF {0,2:X} {1,2:X}", ch1, ch2));
                }
                this.buffer = new UTF8Buffer(this.buffer);
                this.col = 0;
                this.charPos = -1;
                this.NextCh();
            }
            this.pt = this.tokens = new Token();  // first token is a dummy
        }
        
        void NextCh() {
            if (this.oldEols > 0) {
                this.ch = EOL;
                this.oldEols--; } 
            else {
                this.pos = this.buffer.Pos;
                // buffer reads unicode chars, if UTF8 has been detected
                this.ch = this.buffer.Read();
                this.col++;
                this.charPos++;
                // replace isolated '\r' by '\n' in order to make
                // eol handling uniform across Windows, Unix and Mac
                if (this.ch == '\r' && this.buffer.Peek() != '\n') this.ch = EOL;
                if (this.ch == EOL) {
                    this.line++;
                    this.col = 0; }
            }
    
        }

        void AddCh() {
            if (this.tlen >= this.tval.Length) {
                char[] newBuf = new char[2 * this.tval.Length];
                Array.Copy(this.tval, 0, newBuf, 0, this.tval.Length);
                this.tval = newBuf;
            }
            if (this.ch != Buffer.EOF) {
                this.tval[this.tlen++] = (char) this.ch;
                this.NextCh();
            }
        }


    
	bool Comment0() {
		int level = 1, pos0 = this.pos, line0 = this.line, col0 = this.col, charPos0 = this.charPos;
	    this.NextCh();
			for(;;) {
				if (this.ch == 10) {
					level--;
					if (level == 0) {
					    this.oldEols = this.line - line0;
					    this.NextCh(); return true; }
				    this.NextCh();
				} else if (this.ch == Buffer.EOF) return false;
				else this.NextCh();
			}
	}

	bool Comment1() {
		int level = 1, pos0 = this.pos, line0 = this.line, col0 = this.col, charPos0 = this.charPos;
	    this.NextCh();
		if (this.ch == '/') {
		    this.NextCh();
			for(;;) {
				if (this.ch == 10) {
					level--;
					if (level == 0) {
					    this.oldEols = this.line - line0;
					    this.NextCh(); return true; }
				    this.NextCh();
				} else if (this.ch == Buffer.EOF) return false;
				else this.NextCh();
			}
		} else {
		    this.buffer.Pos = pos0;
		    this.NextCh();
		    this.line = line0;
		    this.col = col0;
		    this.charPos = charPos0;
		}
		return false;
	}

	bool Comment2() {
		int level = 1, pos0 = this.pos, line0 = this.line, col0 = this.col, charPos0 = this.charPos;
	    this.NextCh();
		if (this.ch == '*') {
		    this.NextCh();
			for(;;) {
				if (this.ch == '*') {
				    this.NextCh();
					if (this.ch == '/') {
						level--;
						if (level == 0) {
						    this.oldEols = this.line - line0;
						    this.NextCh(); return true; }
					    this.NextCh();
					}
				} else if (this.ch == '/') {
				    this.NextCh();
					if (this.ch == '*') {
						level++;
					    this.NextCh();
					}
				} else if (this.ch == Buffer.EOF) return false;
				else this.NextCh();
			}
		} else {
		    this.buffer.Pos = pos0;
		    this.NextCh();
		    this.line = line0;
		    this.col = col0;
		    this.charPos = charPos0;
		}
		return false;
	}


        void CheckLiteral() {
    		switch (this.t.val) {
			case "private":
			    this.t.kind = 9; break;
			case "or":
			    this.t.kind = 12; break;
			case "and":
			    this.t.kind = 14; break;
			case "else":
			    this.t.kind = 22; break;
			case "max":
			    this.t.kind = 25; break;
			case "min":
			    this.t.kind = 26; break;
			case "mod":
			    this.t.kind = 30; break;
			case "atan2":
			    this.t.kind = 31; break;
			default: break;
		}
        }

        Token NextToken() {
            while (this.ch == ' ' || this.ch >= 9 && this.ch <= 10 || this.ch == 13
            ) this.NextCh();
    		if (this.ch == '#' && this.Comment0() || this.ch == '/' && this.Comment1() || this.ch == '/' && this.Comment2()) return this.NextToken();
            int recKind = noSym;
            int recEnd = this.pos;
            this.t = new Token();
            this.t.pos = this.pos;
            this.t.col = this.col;
            this.t.line = this.line;
            this.t.charPos = this.charPos;
            int state;
            if (start.ContainsKey(this.ch)) { state = (int) start[this.ch]; }
            else { state = 0; }
            this.tlen = 0;
            this.AddCh();
            
            switch (state) {
                case -1: {
                    this.t.kind = eofSym; break; } // NextCh already done
                case 0: {
                    if (recKind != noSym) {
                        this.tlen = recEnd - this.t.pos;
                        this.SetScannerBehindT();
                    }
                    this.t.kind = recKind; break;
                } // NextCh already done
    			case 1:
				recEnd = this.pos; recKind = 1;
				if (this.ch >= '0' && this.ch <= '9') {
				    this.AddCh(); goto case 1;}
				else if (this.ch == '.') {
				    this.AddCh(); goto case 2;}
				else {
				    this.t.kind = 1; break;}
			case 2:
				if (this.ch >= '0' && this.ch <= '9') {
				    this.AddCh(); goto case 3;}
				else {goto case 0;}
			case 3:
				recEnd = this.pos; recKind = 1;
				if (this.ch >= '0' && this.ch <= '9') {
				    this.AddCh(); goto case 3;}
				else {
				    this.t.kind = 1; break;}
			case 4:
				if (this.ch >= '0' && this.ch <= '9' || this.ch >= 'A' && this.ch <= 'F' || this.ch >= 'a' && this.ch <= 'f') {
				    this.AddCh(); goto case 5;}
				else {goto case 0;}
			case 5:
				recEnd = this.pos; recKind = 2;
				if (this.ch >= '0' && this.ch <= '9' || this.ch >= 'A' && this.ch <= 'F' || this.ch >= 'a' && this.ch <= 'f') {
				    this.AddCh(); goto case 5;}
				else {
				    this.t.kind = 2; break;}
			case 6:
				if (this.ch <= 9 || this.ch >= 11 && this.ch <= 12 || this.ch >= 14 && this.ch <= '!' || this.ch >= '#' && this.ch <= 65535) {
				    this.AddCh(); goto case 6;}
				else if (this.ch == '"') {
				    this.AddCh(); goto case 10;}
				else {goto case 0;}
			case 7:
				recEnd = this.pos; recKind = 4;
				if (this.ch >= '0' && this.ch <= '9' || this.ch >= 'A' && this.ch <= 'Z' || this.ch == '_' || this.ch >= 'a' && this.ch <= 'z') {
				    this.AddCh(); goto case 7;}
				else {
				    this.t.kind = 4; break;}
			case 8:
				recEnd = this.pos; recKind = 5;
				if (this.ch >= '0' && this.ch <= '9' || this.ch >= 'A' && this.ch <= 'Z' || this.ch == '_' || this.ch >= 'a' && this.ch <= 'z') {
				    this.AddCh(); goto case 8;}
				else {
				    this.t.kind = 5;
				    this.t.val = new String(this.tval, 0, this.tlen);
				    this.CheckLiteral(); return this.t;}
			case 9:
				recEnd = this.pos; recKind = 1;
				if (this.ch >= '0' && this.ch <= '9') {
				    this.AddCh(); goto case 1;}
				else if (this.ch == '.') {
				    this.AddCh(); goto case 2;}
				else if (this.ch == 'X' || this.ch == 'x') {
				    this.AddCh(); goto case 4;}
				else {
				    this.t.kind = 1; break;}
			case 10:
				recEnd = this.pos; recKind = 3;
				if (this.ch == '"') {
				    this.AddCh(); goto case 6;}
				else {
				    this.t.kind = 3; break;}
			case 11:
				{
				    this.t.kind = 6; break;}
			case 12:
				{
				    this.t.kind = 7; break;}
			case 13:
				{
				    this.t.kind = 8; break;}
			case 14:
				if (this.ch == '|') {
				    this.AddCh(); goto case 15;}
				else {goto case 0;}
			case 15:
				{
				    this.t.kind = 11; break;}
			case 16:
				if (this.ch == '&') {
				    this.AddCh(); goto case 17;}
				else {goto case 0;}
			case 17:
				{
				    this.t.kind = 13; break;}
			case 18:
				{
				    this.t.kind = 15; break;}
			case 19:
				{
				    this.t.kind = 16; break;}
			case 20:
				{
				    this.t.kind = 19; break;}
			case 21:
				{
				    this.t.kind = 20; break;}
			case 22:
				{
				    this.t.kind = 21; break;}
			case 23:
				{
				    this.t.kind = 23; break;}
			case 24:
				{
				    this.t.kind = 27; break;}
			case 25:
				{
				    this.t.kind = 28; break;}
			case 26:
				{
				    this.t.kind = 29; break;}
			case 27:
				{
				    this.t.kind = 32; break;}
			case 28:
				{
				    this.t.kind = 33; break;}
			case 29:
				{
				    this.t.kind = 34; break;}
			case 30:
				{
				    this.t.kind = 36; break;}
			case 31:
				{
				    this.t.kind = 37; break;}
			case 32:
				{
				    this.t.kind = 38; break;}
			case 33:
				recEnd = this.pos; recKind = 24;
				if (this.ch >= '0' && this.ch <= '9') {
				    this.AddCh(); goto case 1;}
				else {
				    this.t.kind = 24; break;}
			case 34:
				recEnd = this.pos; recKind = 10;
				if (this.ch == '=') {
				    this.AddCh(); goto case 18;}
				else {
				    this.t.kind = 10; break;}
			case 35:
				recEnd = this.pos; recKind = 35;
				if (this.ch == '=') {
				    this.AddCh(); goto case 19;}
				else {
				    this.t.kind = 35; break;}
			case 36:
				recEnd = this.pos; recKind = 17;
				if (this.ch == '=') {
				    this.AddCh(); goto case 20;}
				else if (this.ch == '>') {
				    this.AddCh(); goto case 22;}
				else {
				    this.t.kind = 17; break;}
			case 37:
				recEnd = this.pos; recKind = 18;
				if (this.ch == '=') {
				    this.AddCh(); goto case 21;}
				else {
				    this.t.kind = 18; break;}

            }
            this.t.val = new String(this.tval, 0, this.tlen);
            return this.t;
        }
        
        private void SetScannerBehindT() {
            this.buffer.Pos = this.t.pos;
            this.NextCh();
            this.line = this.t.line;
            this.col = this.t.col;
            this.charPos = this.t.charPos;
            for (int i = 0; i < this.tlen; i++) this.NextCh();
        }
        
        // get the next token (possibly a token already seen during peeking)
        public Token Scan () {
            if (this.tokens.next == null) {
                return this.NextToken();
            } else {
                this.pt = this.tokens = this.tokens.next;
                return this.tokens;
            }
        }

        // peek for the next token, ignore pragmas
        public Token Peek () {
            do {
                if (this.pt.next == null) {
                    this.pt.next = this.NextToken();
                }
                this.pt = this.pt.next;
            } while (this.pt.kind > maxT); // skip pragmas
        
            return this.pt;
        }

        // make sure that peeking starts at the current scan position
        public void ResetPeek () {
            this.pt = this.tokens; }

    } // end Scanner
}
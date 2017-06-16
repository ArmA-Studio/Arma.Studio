using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;



using System;
using NLog;


namespace RealVirtuality.Config.Parser
{
    public class Parser {
    	public const int _EOF = 0;
	public const int _T_SCALAR = 1;
	public const int _T_HEX = 2;
	public const int _T_STRING = 3;
	public const int _T_STRINGTABLESTRING = 4;
	public const int _T_IDENT = 5;
	public const int maxT = 17;

        const bool _T = true;
        const bool _x = false;
        const int minErrDist = 2;

        public Scanner scanner;
        public Errors  errors;

        public Token t;    // last recognized token
        public Token la;   // lookahead token
        int errDist = minErrDist;
    public FlowDocument doc;
	public ConfigEntry Root;

    

        public Parser(Scanner scanner) {
            this.scanner = scanner;
            this.errors = new Errors();
        }
        
        bool peekCompare(params int[] values)
        {
            Token t = this.la;
            foreach(int i in values)
            {
                if(i != -1 && t.kind != i)
                {
                    this.scanner.ResetPeek();
                    return false;
                }
                if (t.next == null)
                    t = this.scanner.Peek();
                else
                    t = t.next;
            }
            this.scanner.ResetPeek();
            return true;
        }
        bool peekString(int count, params string[] values)
        {
            Token t = this.la;
            for(; count > 0; count --)
                t = this.scanner.Peek();
            foreach(var it in values)
            {
                if(t.val == it)
                {
                    this.scanner.ResetPeek();
                    return true;
                }
            }
            this.scanner.ResetPeek();
            return false;
        }
        
        
        void SynErr (int n) {
            if (this.errDist >= minErrDist) this.errors.SynErr(this.la.line, this.la.col, n, this.t.charPos, this.t == null ? 0 : this.t.val.Length);
            this.errDist = 0;
        }
        void Warning (string msg) {
            this.errors.Warning(this.la.line, this.la.col, msg);
        }

        public void SemErr (string msg) {
            if (this.errDist >= minErrDist) this.errors.SemErr(this.t.line, this.t.col, msg, this.t.charPos, this.t == null ? 0 : this.t.val.Length);
            this.errDist = 0;
        }
        
        void Get () {
            for (;;) {
                this.t = this.la;
                this.la = this.scanner.Scan();
                if (this.la.kind <= maxT) { ++this.errDist; break; }

                this.la = this.t;
            }
        }
        
        void Expect (int n) {
            if (this.la.kind==n) this.Get(); else {
                this.SynErr(n); }
        }
        
        bool StartOf (int s) {
            return set[s, this.la.kind];
        }
        
        void ExpectWeak (int n, int follow) {
            if (this.la.kind == n) this.Get();
            else {
                this.SynErr(n);
                while (!this.StartOf(follow)) this.Get();
            }
        }


        bool WeakSeparator(int n, int syFol, int repFol) {
            int kind = this.la.kind;
            if (kind == n) {
                this.Get(); return true;}
            else if (this.StartOf(repFol)) {return false;}
            else {
                this.SynErr(n);
                while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
                    this.Get();
                    kind = this.la.kind;
                }
                return this.StartOf(syFol);
            }
        }

        
    	void CONFIGFILE() {
	        this.CONFIG(this.Root);
		while (this.la.kind == 6) {
		    this.CONFIG(this.Root);
		}
	}

	void CONFIG(ConfigEntry parent) {
		ConfigEntry cur = this.Root == null ? null : new ConfigEntry(parent); if(this.Root != null) cur.IsField = true;
	    this.Expect(6);
		if(this.Root != null) cur.FullStart = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos);
	    this.Expect(5);
		if(this.Root != null) {cur.NameStart = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos);
		cur.NameEnd = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos + this.t.val.Length);}
		
		if (this.la.kind == 7) {
		    this.Get();
		    this.Expect(5);
			if(this.Root != null) {cur.ParentStart = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos);
			cur.ParentEnd = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos + this.t.val.Length);}
			
		}
		if (this.la.kind == 8) {
		    this.Get();
			if(this.Root != null) cur.ContentStart = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos + this.t.val.Length); 
			while (this.la.kind == 5 || this.la.kind == 6) {
				if (this.la.kind == 5) {
				    this.FIELD(cur);
				} else {
				    this.CONFIG(cur);
				}
			}
		    this.Expect(9);
			if(this.Root != null) cur.ContentEnd = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos); 
		}
	    this.Expect(10);
		if(this.Root != null) cur.FullEnd = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos + this.t.val.Length); 
	}

	void FIELD(ConfigEntry parent) {
		ConfigEntry cur = this.Root == null ? null : new ConfigEntry(parent); if(this.Root != null) cur.IsField = true;
	    this.Expect(5);
		if(this.Root != null) {cur.FullStart = cur.NameStart = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos);
		cur.NameEnd = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos + this.t.val.Length);}
		
		if (this.la.kind == 11) {
		    this.Get();
		    this.Expect(12);
		}
	    this.Expect(13);
		if (this.la.kind == 8) {
		    this.ARRAY();
			if(this.Root != null) {cur.ContentStart = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos);
			cur.ContentEnd = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos + this.t.val.Length);}
			
		} else if (this.la.kind == 1 || this.la.kind == 2) {
		    this.SCALAR();
			if(this.Root != null) {cur.ContentStart = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos);
			cur.ContentEnd = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos + this.t.val.Length);}
			
		} else if (this.la.kind == 3 || this.la.kind == 4) {
		    this.STRING();
			if(this.Root != null) {cur.ContentStart = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos);
			cur.ContentEnd = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos + this.t.val.Length);}
			
		} else if (this.la.kind == 14 || this.la.kind == 15) {
		    this.BOOLEAN();
			if(this.Root != null) {cur.ContentStart = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos);
			cur.ContentEnd = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos + this.t.val.Length);}
			
		} else if (this.StartOf(1)) {
		    this.Get();
			if(this.Root != null) cur.ContentStart = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos); 
			while (this.StartOf(2)) {
			    this.Get();
			}
			if(this.Root != null) cur.ContentEnd = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos + this.t.val.Length); 
		} else this.SynErr(18);
	    this.Expect(10);
		if(this.Root != null) cur.FullEnd = this.doc.ContentStart.GetPointerFromCharOffset(this.t.charPos + this.t.val.Length); 
	}

	void ARRAY() {
	    this.Expect(8);
		if (this.StartOf(3)) {
			if (this.la.kind == 1 || this.la.kind == 2) {
			    this.SCALAR();
			} else if (this.la.kind == 3 || this.la.kind == 4) {
			    this.STRING();
			} else {
			    this.BOOLEAN();
			}
			while (this.la.kind == 16) {
			    this.Get();
				if (this.la.kind == 1 || this.la.kind == 2) {
				    this.SCALAR();
				} else if (this.la.kind == 3 || this.la.kind == 4) {
				    this.STRING();
				} else if (this.la.kind == 14 || this.la.kind == 15) {
				    this.BOOLEAN();
				} else this.SynErr(19);
			}
		}
	    this.Expect(9);
	}

	void SCALAR() {
		if (this.la.kind == 1) {
		    this.Get();
		} else if (this.la.kind == 2) {
		    this.Get();
		} else this.SynErr(20);
	}

	void STRING() {
		if (this.la.kind == 3) {
		    this.Get();
		} else if (this.la.kind == 4) {
		    this.Get();
		} else this.SynErr(21);
	}

	void BOOLEAN() {
		if (this.la.kind == 14) {
		    this.Get();
		} else if (this.la.kind == 15) {
		    this.Get();
		} else this.SynErr(22);
	}


    
        public void Parse() {
            this.la = new Token();
            this.la.val = "";
            this.Get();
            this.CONFIGFILE();
            this.Expect(0);

        }
        
        static readonly bool[,] set = {
    		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_T,_T,_T, _x,_T,_T,_T, _T,_T,_x,_x, _T,_T,_x},
		{_x,_T,_T,_T, _T,_T,_T,_T, _T,_T,_x,_T, _T,_T,_T,_T, _T,_T,_x},
		{_x,_T,_T,_T, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _x,_x,_x}

        };
    } // end Parser


    public class Errors {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public int Count => this.ErrorList.Count;
        public List<Tuple<int, int, string>> ErrorList;
        public string errMsgFormat = "line {0} col {1}: {2}"; // 0=line, 1=column, 2=text
        public Errors()
        {
            this.ErrorList = new List<Tuple<int, int, string>>();
        }

        public virtual void SynErr (int line, int col, int n, int offset, int length) {
            string s;
            switch (n) {
    			case 0: s = "EOF expected"; break;
			case 1: s = "T_SCALAR expected"; break;
			case 2: s = "T_HEX expected"; break;
			case 3: s = "T_STRING expected"; break;
			case 4: s = "T_STRINGTABLESTRING expected"; break;
			case 5: s = "T_IDENT expected"; break;
			case 6: s = "\"class\" expected"; break;
			case 7: s = "\":\" expected"; break;
			case 8: s = "\"{\" expected"; break;
			case 9: s = "\"}\" expected"; break;
			case 10: s = "\";\" expected"; break;
			case 11: s = "\"[\" expected"; break;
			case 12: s = "\"]\" expected"; break;
			case 13: s = "\"=\" expected"; break;
			case 14: s = "\"true\" expected"; break;
			case 15: s = "\"false\" expected"; break;
			case 16: s = "\",\" expected"; break;
			case 17: s = "??? expected"; break;
			case 18: s = "invalid FIELD"; break;
			case 19: s = "invalid ARRAY"; break;
			case 20: s = "invalid SCALAR"; break;
			case 21: s = "invalid STRING"; break;
			case 22: s = "invalid BOOLEAN"; break;

                default: s = "error " + n; break;
            }
            logger.Error(string.Format(this.errMsgFormat, line, col, s));
            this.ErrorList.Add(new Tuple<int, int, string>(offset, length, s));
}

        public virtual void SemErr (int line, int col, string s, int offset, int length) {
            logger.Error(string.Format(this.errMsgFormat, line, col, s));
            this.ErrorList.Add(new Tuple<int, int, string>(offset, length, s));
}
        
        public virtual void SemErr (string s) {
            logger.Error(s);
        }
        
        public virtual void Warning (int line, int col, string s) {
            logger.Warn(string.Format(this.errMsgFormat, line, col, s));
        }
        
        public virtual void Warning(string s) {
            logger.Warn(s);
        }
    }


    public class FatalError: Exception {
        public FatalError(string m): base(m) {}
    }
}

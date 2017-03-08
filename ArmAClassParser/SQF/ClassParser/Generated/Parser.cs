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
            errors = new Errors();
        }
        
        bool peekCompare(params int[] values)
        {
            Token t = la;
            foreach(int i in values)
            {
                if(i != -1 && t.kind != i)
                {
                    scanner.ResetPeek();
                    return false;
                }
                if (t.next == null)
                    t = scanner.Peek();
                else
                    t = t.next;
            }
            scanner.ResetPeek();
            return true;
        }
        bool peekString(int count, params string[] values)
        {
            Token t = la;
            for(; count > 0; count --)
                t = scanner.Peek();
            foreach(var it in values)
            {
                if(t.val == it)
                {
                    scanner.ResetPeek();
                    return true;
                }
            }
            scanner.ResetPeek();
            return false;
        }
        
        
        void SynErr (int n) {
            if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n, t.charPos, t == null ? 0 : t.val.Length);
            errDist = 0;
        }
        void Warning (string msg) {
            errors.Warning(la.line, la.col, msg);
        }

        public void SemErr (string msg) {
            if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg, t.charPos, t == null ? 0 : t.val.Length);
            errDist = 0;
        }
        
        void Get () {
            for (;;) {
                t = la;
                la = scanner.Scan();
                if (la.kind <= maxT) { ++errDist; break; }
    
                la = t;
            }
        }
        
        void Expect (int n) {
            if (la.kind==n) Get(); else { SynErr(n); }
        }
        
        bool StartOf (int s) {
            return set[s, la.kind];
        }
        
        void ExpectWeak (int n, int follow) {
            if (la.kind == n) Get();
            else {
                SynErr(n);
                while (!StartOf(follow)) Get();
            }
        }


        bool WeakSeparator(int n, int syFol, int repFol) {
            int kind = la.kind;
            if (kind == n) {Get(); return true;}
            else if (StartOf(repFol)) {return false;}
            else {
                SynErr(n);
                while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
                    Get();
                    kind = la.kind;
                }
                return StartOf(syFol);
            }
        }

        
    	void CONFIGFILE() {
		CONFIG(Root);
		while (la.kind == 6) {
			CONFIG(Root);
		}
	}

	void CONFIG(ConfigEntry parent) {
		ConfigEntry cur = Root == null ? null : new ConfigEntry(parent); if(Root != null) cur.IsField = true; 
		Expect(6);
		if(Root != null) cur.FullStart = doc.ContentStart.GetPointerFromCharOffset(t.charPos); 
		Expect(5);
		if(Root != null) {cur.NameStart = doc.ContentStart.GetPointerFromCharOffset(t.charPos);
		cur.NameEnd = doc.ContentStart.GetPointerFromCharOffset(t.charPos + t.val.Length);}
		
		if (la.kind == 7) {
			Get();
			Expect(5);
			if(Root != null) {cur.ParentStart = doc.ContentStart.GetPointerFromCharOffset(t.charPos);
			cur.ParentEnd = doc.ContentStart.GetPointerFromCharOffset(t.charPos + t.val.Length);}
			
		}
		if (la.kind == 8) {
			Get();
			if(Root != null) cur.ContentStart = doc.ContentStart.GetPointerFromCharOffset(t.charPos + t.val.Length); 
			while (la.kind == 5 || la.kind == 6) {
				if (la.kind == 5) {
					FIELD(cur);
				} else {
					CONFIG(cur);
				}
			}
			Expect(9);
			if(Root != null) cur.ContentEnd = doc.ContentStart.GetPointerFromCharOffset(t.charPos); 
		}
		Expect(10);
		if(Root != null) cur.FullEnd = doc.ContentStart.GetPointerFromCharOffset(t.charPos + t.val.Length); 
	}

	void FIELD(ConfigEntry parent) {
		ConfigEntry cur = Root == null ? null : new ConfigEntry(parent); if(Root != null) cur.IsField = true; 
		Expect(5);
		if(Root != null) {cur.FullStart = cur.NameStart = doc.ContentStart.GetPointerFromCharOffset(t.charPos);
		cur.NameEnd = doc.ContentStart.GetPointerFromCharOffset(t.charPos + t.val.Length);}
		
		if (la.kind == 11) {
			Get();
			Expect(12);
		}
		Expect(13);
		if (la.kind == 8) {
			ARRAY();
			if(Root != null) {cur.ContentStart = doc.ContentStart.GetPointerFromCharOffset(t.charPos);
			cur.ContentEnd = doc.ContentStart.GetPointerFromCharOffset(t.charPos + t.val.Length);}
			
		} else if (la.kind == 1 || la.kind == 2) {
			SCALAR();
			if(Root != null) {cur.ContentStart = doc.ContentStart.GetPointerFromCharOffset(t.charPos);
			cur.ContentEnd = doc.ContentStart.GetPointerFromCharOffset(t.charPos + t.val.Length);}
			
		} else if (la.kind == 3 || la.kind == 4) {
			STRING();
			if(Root != null) {cur.ContentStart = doc.ContentStart.GetPointerFromCharOffset(t.charPos);
			cur.ContentEnd = doc.ContentStart.GetPointerFromCharOffset(t.charPos + t.val.Length);}
			
		} else if (la.kind == 14 || la.kind == 15) {
			BOOLEAN();
			if(Root != null) {cur.ContentStart = doc.ContentStart.GetPointerFromCharOffset(t.charPos);
			cur.ContentEnd = doc.ContentStart.GetPointerFromCharOffset(t.charPos + t.val.Length);}
			
		} else if (StartOf(1)) {
			Get();
			if(Root != null) cur.ContentStart = doc.ContentStart.GetPointerFromCharOffset(t.charPos); 
			while (StartOf(2)) {
				Get();
			}
			if(Root != null) cur.ContentEnd = doc.ContentStart.GetPointerFromCharOffset(t.charPos + t.val.Length); 
		} else SynErr(18);
		Expect(10);
		if(Root != null) cur.FullEnd = doc.ContentStart.GetPointerFromCharOffset(t.charPos + t.val.Length); 
	}

	void ARRAY() {
		Expect(8);
		if (StartOf(3)) {
			if (la.kind == 1 || la.kind == 2) {
				SCALAR();
			} else if (la.kind == 3 || la.kind == 4) {
				STRING();
			} else {
				BOOLEAN();
			}
			while (la.kind == 16) {
				Get();
				if (la.kind == 1 || la.kind == 2) {
					SCALAR();
				} else if (la.kind == 3 || la.kind == 4) {
					STRING();
				} else if (la.kind == 14 || la.kind == 15) {
					BOOLEAN();
				} else SynErr(19);
			}
		}
		Expect(9);
	}

	void SCALAR() {
		if (la.kind == 1) {
			Get();
		} else if (la.kind == 2) {
			Get();
		} else SynErr(20);
	}

	void STRING() {
		if (la.kind == 3) {
			Get();
		} else if (la.kind == 4) {
			Get();
		} else SynErr(21);
	}

	void BOOLEAN() {
		if (la.kind == 14) {
			Get();
		} else if (la.kind == 15) {
			Get();
		} else SynErr(22);
	}


    
        public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
    		CONFIGFILE();
		Expect(0);

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
        public int Count { get { return this.ErrorList.Count; } }
        public List<Tuple<int, int, string>> ErrorList;
        public string errMsgFormat = "line {0} col {1}: {2}"; // 0=line, 1=column, 2=text
        public Errors()
        {
            ErrorList = new List<Tuple<int, int, string>>();
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
            logger.Error(string.Format(errMsgFormat, line, col, s));
                    ErrorList.Add(new Tuple<int, int, string>(offset, length, s));
}

        public virtual void SemErr (int line, int col, string s, int offset, int length) {
            logger.Error(string.Format(errMsgFormat, line, col, s));
                    ErrorList.Add(new Tuple<int, int, string>(offset, length, s));
}
        
        public virtual void SemErr (string s) {
            logger.Error(s);
        }
        
        public virtual void Warning (int line, int col, string s) {
            logger.Warn(string.Format(errMsgFormat, line, col, s));
        }
        
        public virtual void Warning(string s) {
            logger.Warn(s);
        }
    }


    public class FatalError: Exception {
        public FatalError(string m): base(m) {}
    }
}

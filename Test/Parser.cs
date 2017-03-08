using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;



using System;


namespace VirtualRealityEngine.Config.Parser
{
    public class Parser {
    	public const int _EOF = 0;
	public const int _T_SCALAR = 1;
	public const int _T_HEX = 2;
	public const int _T_STRING = 3;
	public const int _T_STRINGTABLESTRING = 4;
	public const int _T_IDENT = 5;
	public const int maxT = 39;

        const bool _T = true;
        const bool _x = false;
        const int minErrDist = 2;

        public Scanner scanner;
        public Errors  errors;

        public Token t;    // last recognized token
        public Token la;   // lookahead token
        int errDist = minErrDist;
    

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

        
    	void SQFDOCUMENT() {
		STATEMENT();
		while (la.kind == 8) {
			SEMICOLON();
			if (StartOf(1)) {
				STATEMENT();
			}
		}
	}

	void STATEMENT() {
		if (_T_IDENT == la.kind && peekString(1, "=") || peekString(1, "private") ) {
			ASSIGNMENT();
		} else if (peekString(0, "private") && peekString(1, "[") ) {
			EXPRESSION();
		} else if (StartOf(1)) {
			EXPRESSION();
		} else SynErr(40);
	}

	void SEMICOLON() {
		Expect(8);
		while (la.kind == 8) {
			Get();
		}
	}

	void EXP_CODE() {
		Expect(6);
		if (StartOf(1)) {
			STATEMENT();
			while (la.kind == 8) {
				SEMICOLON();
				if (StartOf(1)) {
					STATEMENT();
				}
			}
		}
		Expect(7);
	}

	void ASSIGNMENT() {
		if (la.kind == 9) {
			Get();
		}
		Expect(5);
		Expect(10);
		EXPRESSION();
	}

	void EXPRESSION() {
		EXP_OR();
	}

	void EXP_OR() {
		EXP_AND();
		if (la.kind == 11 || la.kind == 12) {
			if (la.kind == 11) {
				Get();
			} else {
				Get();
			}
			EXP_OR();
		}
	}

	void EXP_AND() {
		EXP_COMPARISON();
		if (la.kind == 13 || la.kind == 14) {
			if (la.kind == 13) {
				Get();
			} else {
				Get();
			}
			EXP_AND();
		}
	}

	void EXP_COMPARISON() {
		EXP_BINARY();
		if (StartOf(2)) {
			switch (la.kind) {
			case 15: {
				Get();
				break;
			}
			case 16: {
				Get();
				break;
			}
			case 17: {
				Get();
				break;
			}
			case 18: {
				Get();
				break;
			}
			case 19: {
				Get();
				break;
			}
			case 20: {
				Get();
				break;
			}
			case 21: {
				Get();
				break;
			}
			}
			EXP_COMPARISON();
		}
	}

	void EXP_BINARY() {
		EXP_ELSE();
		if (la.kind == 5) {
			Get();
			EXP_BINARY();
		}
	}

	void EXP_ELSE() {
		EXP_ADDITION();
		if (la.kind == 22) {
			Get();
			EXP_ELSE();
		}
	}

	void EXP_ADDITION() {
		EXP_MULTIPLICATION();
		if (StartOf(3)) {
			if (la.kind == 23) {
				Get();
			} else if (la.kind == 24) {
				Get();
			} else if (la.kind == 25) {
				Get();
			} else {
				Get();
			}
			EXP_ADDITION();
		}
	}

	void EXP_MULTIPLICATION() {
		EXP_POWER();
		if (StartOf(4)) {
			if (la.kind == 27) {
				Get();
			} else if (la.kind == 28) {
				Get();
			} else if (la.kind == 29) {
				Get();
			} else if (la.kind == 30) {
				Get();
			} else {
				Get();
			}
			EXP_MULTIPLICATION();
		}
	}

	void EXP_POWER() {
		EXP_HIGHEST();
		if (la.kind == 32) {
			Get();
			EXP_POWER();
		}
	}

	void EXP_HIGHEST() {
		if (la.kind == 5 || la.kind == 9 || la.kind == 35) {
			EXP_UNARYNULL();
		} else if (la.kind == 1 || la.kind == 2 || la.kind == 3) {
			EXP_VALUES();
		} else if (la.kind == 33) {
			Get();
			EXPRESSION();
			Expect(34);
		} else if (la.kind == 6) {
			EXP_CODE();
		} else if (la.kind == 36) {
			EXP_ARRAY();
		} else SynErr(41);
	}

	void EXP_UNARYNULL() {
		if (la.kind == 5) {
			Get();
		} else if (la.kind == 9) {
			Get();
		} else if (la.kind == 35) {
			Get();
		} else SynErr(42);
		if (StartOf(1)) {
			EXP_HIGHEST();
		}
	}

	void EXP_VALUES() {
		if (la.kind == 1) {
			Get();
		} else if (la.kind == 2) {
			Get();
		} else if (la.kind == 3) {
			Get();
		} else SynErr(43);
	}

	void EXP_ARRAY() {
		Expect(36);
		if (StartOf(1)) {
			EXPRESSION();
			while (la.kind == 37) {
				Get();
				EXPRESSION();
			}
		}
		Expect(38);
	}


    
        public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
    		SQFDOCUMENT();
		Expect(0);

        }
        
        static readonly bool[,] set = {
    		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x},
		{_x,_T,_T,_T, _x,_T,_T,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_T, _T,_x,_x,_x, _x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x}

        };
    } // end Parser


    public class Errors {
        //private static Logger logger = LogManager.GetCurrentClassLogger();
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
			case 6: s = "\"{\" expected"; break;
			case 7: s = "\"}\" expected"; break;
			case 8: s = "\";\" expected"; break;
			case 9: s = "\"private\" expected"; break;
			case 10: s = "\"=\" expected"; break;
			case 11: s = "\"||\" expected"; break;
			case 12: s = "\"or\" expected"; break;
			case 13: s = "\"&&\" expected"; break;
			case 14: s = "\"and\" expected"; break;
			case 15: s = "\"==\" expected"; break;
			case 16: s = "\"!=\" expected"; break;
			case 17: s = "\">\" expected"; break;
			case 18: s = "\"<\" expected"; break;
			case 19: s = "\">=\" expected"; break;
			case 20: s = "\"<=\" expected"; break;
			case 21: s = "\">>\" expected"; break;
			case 22: s = "\"else\" expected"; break;
			case 23: s = "\"+\" expected"; break;
			case 24: s = "\"-\" expected"; break;
			case 25: s = "\"max\" expected"; break;
			case 26: s = "\"min\" expected"; break;
			case 27: s = "\"*\" expected"; break;
			case 28: s = "\"/\" expected"; break;
			case 29: s = "\"%\" expected"; break;
			case 30: s = "\"mod\" expected"; break;
			case 31: s = "\"atan2\" expected"; break;
			case 32: s = "\"^\" expected"; break;
			case 33: s = "\"(\" expected"; break;
			case 34: s = "\")\" expected"; break;
			case 35: s = "\"!\" expected"; break;
			case 36: s = "\"[\" expected"; break;
			case 37: s = "\",\" expected"; break;
			case 38: s = "\"]\" expected"; break;
			case 39: s = "??? expected"; break;
			case 40: s = "invalid STATEMENT"; break;
			case 41: s = "invalid EXP_HIGHEST"; break;
			case 42: s = "invalid EXP_UNARYNULL"; break;
			case 43: s = "invalid EXP_VALUES"; break;

                default: s = "error " + n; break;
            }
            //logger.Error(string.Format(errMsgFormat, line, col, s));
            ErrorList.Add(new Tuple<int, int, string>(offset, length, s));
			System.Diagnostics.Debugger.Break();
		}

        public virtual void SemErr (int line, int col, string s, int offset, int length) {
            //logger.Error(string.Format(errMsgFormat, line, col, s));
            ErrorList.Add(new Tuple<int, int, string>(offset, length, s));
		}
        
        public virtual void SemErr (string s) {
            //logger.Error(s);
        }
        
        public virtual void Warning (int line, int col, string s) {
            //logger.Warn(string.Format(errMsgFormat, line, col, s));
        }
        
        public virtual void Warning(string s) {
            //logger.Warn(s);
        }
    }


    public class FatalError: Exception {
        public FatalError(string m): base(m) {}
    }
}

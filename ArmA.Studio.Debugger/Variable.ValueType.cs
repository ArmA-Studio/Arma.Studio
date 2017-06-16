using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Debugger
{
    public partial struct Variable
    {
        public sealed class ValueType
        {
            public static readonly ValueType UNKNOWN = new ValueType("UNKNOWN");

            //Do not forget to add new ValueType to Parse function if it ever occurs
            public static readonly ValueType SCALAR = new ValueType("FLOAT");
            public static readonly ValueType BOOL = new ValueType("BOOL");
            public static readonly ValueType ARRAY = new ValueType("ARRAY");
            public static readonly ValueType STRING = new ValueType("STRING");
            public static readonly ValueType NOTHING = new ValueType("NOTHING");
            public static readonly ValueType ANY = new ValueType("VOID");
            public static readonly ValueType NAMESPACE = new ValueType("NAMESPACE");
            public static readonly ValueType EXPRESSION = new ValueType("EXPRESSION");
            public static readonly ValueType IF = new ValueType("IF");
            public static readonly ValueType WHILE = new ValueType("WHILE");
            public static readonly ValueType FOR = new ValueType("FOR");
            public static readonly ValueType SWITCH = new ValueType("SWITCH <INVISIBLE>");
            public static readonly ValueType EXCEPTION = new ValueType("GAMEEXCEPTIONHELPTYPE");
            public static readonly ValueType WITH = new ValueType("WITH");
            public static readonly ValueType CODE = new ValueType("CODE");
            public static readonly ValueType OBJECT = new ValueType("OBJECT");
            public static readonly ValueType SIDE = new ValueType("SIDE");
            public static readonly ValueType GROUP = new ValueType("GROUP");
            public static readonly ValueType TEXT = new ValueType("TEXT");
            public static readonly ValueType SCRIPT = new ValueType("SCRIPT");
            public static readonly ValueType TARGET = new ValueType("TARGET");
            public static readonly ValueType JCLASS = new ValueType("JCLASS");
            public static readonly ValueType CONFIG = new ValueType("CONFIG");
            public static readonly ValueType DISPLAY = new ValueType("DISPLAY");
            public static readonly ValueType CONTROL = new ValueType("CONTROL");
            public static readonly ValueType NetObject = new ValueType("NETWORK OBJECT");
            public static readonly ValueType TEAM_MEMBER = new ValueType("TEAMMEMBER");
            public static readonly ValueType TASK = new ValueType("TASK");
            public static readonly ValueType DIARY_RECORD = new ValueType("DIARY RECORD");
            public static readonly ValueType LOCATION = new ValueType("LOCATION");

            private readonly string Value;
            private ValueType(string Type)
            {
                this.Value = Type;
            }

            public override bool Equals(object obj)
            {
                if(obj is ValueType)
                {
                    return this.Value.Equals(((ValueType)obj).Value, StringComparison.InvariantCultureIgnoreCase);
                }
                return this.Value.Equals(obj);
            }

            public static ValueType Parse(string vt)
            {
                switch (vt.ToUpper())
                {
                    case "IF":
                        return IF;
                    case "WHILE":
                        return WHILE;
                    case "WITH":
                        return WITH;
                    case "FORTYPE":
                        return FOR;
                    case "GAMEEXCEPTIONHELPTYPE":
                        return EXCEPTION;
                    case "VOID":
                        return ANY;
                    case "FLOAT":
                        return SCALAR;
                    case "NOTHING":
                        return NOTHING;
                    case "STRING":
                        return STRING;
                    case "BOOL":
                        return BOOL;
                    case "ARRAY":
                        return ARRAY;
                    case "SSWITCH <INVISIBLE>":
                        return SWITCH;
                    case "NAMESPACE":
                        return NAMESPACE;
                    case "CODE":
                        return CODE;
                    case "EXPRESSION":
                        return EXPRESSION;
                    case "OBJECT":
                        return OBJECT;
                    case "NETWORK OBJECT":
                        return NetObject;
                    case "TARGET":
                        return TARGET;
                    case "GROUP":
                        return GROUP;
                    case "SCRIPT":
                        return SCRIPT;
                    case "JCLASS":
                        return JCLASS;
                    case "SIDE":      
                        return SIDE;
                    case "TEXT":
                        return TEXT;
                    case "CONFIG":
                        return CONFIG;
                    case "DISPLAY":
                        return DISPLAY;
                    case "CONTROL":
                        return CONTROL;
                    case "TEAMMEMBER":
                        return TEAM_MEMBER;
                    case "LOCATION":
                        return LOCATION;
                    case "TASK":
                        return TASK;
                    case "DIARY_RECORD":
                        return DIARY_RECORD;
                    default:
                        return UNKNOWN;
                }
            }

            public override int GetHashCode()
            {
                return this.Value.GetHashCode();
            }

            public override string ToString()
            {
                return this.Value;
            }
        }
    }

}
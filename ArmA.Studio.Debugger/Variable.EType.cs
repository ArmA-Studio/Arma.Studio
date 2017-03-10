using System;
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
            public static readonly ValueType SCALAR = new ValueType("SCALAR");
            public static readonly ValueType BOOL = new ValueType("BOOL");
            public static readonly ValueType ARRAY = new ValueType("ARRAY");
            public static readonly ValueType STRING = new ValueType("STRING");
            public static readonly ValueType NOTHING = new ValueType("NOTHING");
            public static readonly ValueType ANY = new ValueType("ANY");
            public static readonly ValueType NAMESPACE = new ValueType("NAMESPACE");
            public static readonly ValueType NaN = new ValueType("NaN");
            public static readonly ValueType IF = new ValueType("IF");
            public static readonly ValueType WHILE = new ValueType("WHILE");
            public static readonly ValueType FOR = new ValueType("FOR");
            public static readonly ValueType SWITCH = new ValueType("SWITCH");
            public static readonly ValueType EXCEPTION = new ValueType("EXCEPTION");
            public static readonly ValueType WITH = new ValueType("WITH");
            public static readonly ValueType CODE = new ValueType("CODE");
            public static readonly ValueType OBJECT = new ValueType("OBJECT");
            public static readonly ValueType VECTOR = new ValueType("VECTOR");
            public static readonly ValueType TRANS = new ValueType("TRANS");
            public static readonly ValueType ORIENT = new ValueType("ORIENT");
            public static readonly ValueType SIDE = new ValueType("SIDE");
            public static readonly ValueType GROUP = new ValueType("GROUP");
            public static readonly ValueType TEXT = new ValueType("TEXT");
            public static readonly ValueType SCRIPT = new ValueType("SCRIPT");
            public static readonly ValueType TARGET = new ValueType("TARGET");
            public static readonly ValueType JCLASS = new ValueType("JCLASS");
            public static readonly ValueType CONFIG = new ValueType("CONFIG");
            public static readonly ValueType DISPLAY = new ValueType("DISPLAY");
            public static readonly ValueType CONTROL = new ValueType("CONTROL");
            public static readonly ValueType NetObject = new ValueType("NetObject");
            public static readonly ValueType SUBGROUP = new ValueType("SUBGROUP");
            public static readonly ValueType TEAM_MEMBER = new ValueType("TEAM_MEMBER");
            public static readonly ValueType TASK = new ValueType("TASK");
            public static readonly ValueType DIARY_RECORD = new ValueType("DIARY_RECORD");
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
                    case "SCALAR":
                        return SCALAR;
                    case "BOOL":
                        return BOOL;
                    case "ARRAY":
                        return ARRAY;
                    case "STRING":
                        return STRING;
                    case "NOTHING":
                        return NOTHING;
                    case "ANY":
                        return ANY;
                    case "NAMESPACE":
                        return NAMESPACE;
                    case "NaN":
                        return NaN;
                    case "IF":
                        return IF;
                    case "WHILE":
                        return WHILE;
                    case "FOR":
                        return FOR;
                    case "SWITCH":
                        return SWITCH;
                    case "EXCEPTION":
                        return EXCEPTION;
                    case "WITH":
                        return WITH;
                    case "CODE":
                        return CODE;
                    case "OBJECT":
                        return OBJECT;
                    case "VECTOR":
                        return VECTOR;
                    case "TRANS":
                        return TRANS;
                    case "ORIENT":
                        return ORIENT;
                    case "SIDE":
                        return SIDE;
                    case "GROUP":
                        return GROUP;
                    case "TEXT":
                        return TEXT;
                    case "SCRIPT":
                        return SCRIPT;
                    case "TARGET":
                        return TARGET;
                    case "JCLASS":
                        return JCLASS;
                    case "CONFIG":
                        return CONFIG;
                    case "DISPLAY":
                        return DISPLAY;
                    case "CONTROL":
                        return CONTROL;
                    case "NetObject":
                        return NetObject;
                    case "SUBGROUP":
                        return SUBGROUP;
                    case "TEAM_MEMBER":
                        return TEAM_MEMBER;
                    case "TASK":
                        return TASK;
                    case "DIARY_RECORD":
                        return DIARY_RECORD;
                    case "LOCATION":
                        return LOCATION;
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
                return Value;
            }
        }
    }

}
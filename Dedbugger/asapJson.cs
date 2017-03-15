/*
The MIT License (MIT)

Copyright (c) 2016 Marco Silipo (X39)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asapJson
{
    public class JsonNode
    {
        public enum EJType
        {
            Array,
            String,
            Number,
            Object,
            Boolean
        }
        public EJType Type { get; internal set; }
        private object value;
        public JsonNode()
        {
            Type = EJType.Object;
            value = null;
        }
        public JsonNode(Dictionary<string, JsonNode> input)
        {
            Type = EJType.Object;
            value = (object)input;
        }
        public JsonNode(IEnumerable<JsonNode> input)
        {
            Type = EJType.Array;
            value = input.ToList();
        }
        public JsonNode(string input, bool isJson = false)
        {
            if (isJson)
            {
                Type = EJType.Object;
                value = null;
                using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(memStream);
                    sw.Write(input);
                    sw.Flush();
                    memStream.Seek(0, System.IO.SeekOrigin.Begin);
                    this.Read(new System.IO.StreamReader(memStream));
                }
            }
            else
            {
                Type = EJType.String;
                value = (object)input;
            }
        }
        public JsonNode(double input)
        {
            Type = EJType.Number;
            value = (object)input;
        }
        public JsonNode(bool input)
        {
            Type = EJType.Boolean;
            value = (object)input;
        }
        public JsonNode(System.IO.StreamReader input)
        {
            Type = EJType.Object;
            value = null;
            this.Read(input);
        }

        public JsonNode GetByPath(string path, char delimeter = '/', bool createIfNonExisting = false)
        {
            var pathArgs = path.Split(delimeter);
            JsonNode curNode = this;
            foreach (var next in pathArgs)
            {
                if (string.IsNullOrEmpty(next))
                    continue;
                if (curNode.Type == EJType.Object)
                {
                    Dictionary<string, JsonNode> val;
                    curNode.GetValue(out val);
                    if (val == null)
                    {
                        val = new Dictionary<string, JsonNode>();
                        curNode.SetValue(val);
                    }
                    if (!val.TryGetValue(next, out curNode))
                    {
                        if (createIfNonExisting)
                        {
                            curNode = new JsonNode(new Dictionary<string, JsonNode>());
                            val.Add(next, curNode);
                        }
                        else
                        {
                            throw new Exception("Cannot resolve path to end. No next node at '" + next + '\'');
                        }
                    }
                }
                else
                {
                    throw new Exception("Cannot resolve path to end. No next node at '" + next + '\'');
                }
            }
            return curNode;
        }
        public void SetAtPath(JsonNode node, string path, char delimeter = '/')
        {
            int index = path.LastIndexOf(delimeter);
            JsonNode lastNode;
            if (index > 0)
            {
                lastNode = this.GetByPath(path.Substring(0, index), delimeter, true);
                ;
            }
            else
            {
                lastNode = this;
            }

            if (lastNode.Type == EJType.Object)
            {
                Dictionary<string, JsonNode> val;
                lastNode.GetValue(out val);
                if (val == null)
                {
                    val = new Dictionary<string, JsonNode>();
                    lastNode.SetValue(val);
                }
                if (index > 0)
                {
                    val.Add(path.Substring(index + 1), node);
                }
                else
                {
                    val.Add(path, node);
                }
            }
            else
            {
                throw new Exception("Cannot set path to end. Parent node is not an object");
            }
        }

        public bool GetValue(out List<JsonNode> val)
        {
            if (this.Type == EJType.Array)
            {
                val = (List<JsonNode>)this.value;
                return true;
            }
            else
            {
                val = null;
                return false;
            }
        }
        public bool GetValue(out string val)
        {
            if (this.Type == EJType.String)
            {
                val = (string)this.value;
                return true;
            }
            else
            {
                val = null;
                return false;
            }
        }
        public bool GetValue(out double val)
        {
            if (this.Type == EJType.Number)
            {
                val = (double)this.value;
                return true;
            }
            else
            {
                val = 0;
                return false;
            }
        }
        public bool GetValue(out Dictionary<string, JsonNode> val)
        {
            if (this.Type == EJType.Object)
            {
                val = this.value == null ? null : (Dictionary<string, JsonNode>)this.value;
                return true;
            }
            else
            {
                val = null;
                return false;
            }
        }
        public bool GetValue(out bool val)
        {
            if (this.Type == EJType.Boolean)
            {
                val = (bool)this.value;
                return true;
            }
            else
            {
                val = false;
                return false;
            }
        }

        public EJType GetValueType()
        {
            return this.Type;
        }

        public List<JsonNode> GetValue_Array()
        {
            if (this.Type == EJType.Array)
                return (List<JsonNode>)this.value;
            else
                throw new TypeAccessException("JsonNode type != Type.Array");
        }
        public string GetValue_String()
        {
            if (this.Type == EJType.String)
                return (string)this.value;
            else
                throw new TypeAccessException("JsonNode type != Type.String");
        }
        public double GetValue_Number()
        {
            if (this.Type == EJType.Number)
                return (double)this.value;
            else
                throw new TypeAccessException("JsonNode type != Type.Number");
        }
        public Dictionary<string, JsonNode> GetValue_Object()
        {
            if (this.Type == EJType.Object)
                return this.value == null ? null : (Dictionary<string, JsonNode>)this.value;
            else
                throw new TypeAccessException("JsonNode type != Type.Object");
        }
        public bool GetValue_Boolean()
        {
            if (this.Type == EJType.Boolean)
                return (bool)this.value;
            else
                throw new TypeAccessException("JsonNode type != Type.Boolean");
        }

        public void SetValue(List<JsonNode> val)
        {
            this.value = (object)val;
            this.Type = EJType.Array;
        }
        public void SetValue(string val)
        {
            this.value = (object)val;
            this.Type = EJType.String;
        }
        public void SetValue(double val)
        {
            this.value = (object)val;
            this.Type = EJType.Number;
        }
        public void SetValue(Dictionary<string, JsonNode> val)
        {
            this.value = (object)val;
            this.Type = EJType.Object;
        }
        public void SetValue(bool val)
        {
            this.value = (object)val;
            this.Type = EJType.Boolean;
        }
        public void SetValue()
        {
            this.value = null;
            this.Type = EJType.Object;
        }

        #region Reading
        /*
         * Note to the curious reader:
         * I tried something new in here in regards of control flow ...
         * it kind of messed up everything due to the fact that i got
         * like 1k of ifElse shit + specific onePurpose flags all over
         * the place in theese functions ...
         * It did not made things more simple ... but well ...
         * was an experiment :)
         */
        public void Read(System.IO.StreamReader sr)
        {
            switch (sr.Peek())
            {
                default:
                    throw new Exception("Unexpected character '" + sr.Peek() + '\'');
                case 'n':
                case 'N':
                    sr.Read();
                    int next = sr.Read();
                    if (next != 'u' && next != 'U')
                        throw new Exception("Parsing Object failed: Expected 'u' or 'U', got '" + (char)next + '\'');
                    next = sr.Read();
                    if (next != 'l' && next != 'L')
                        throw new Exception("Parsing Object failed: Expected 'l' or 'L', got '" + (char)next + '\'');
                    next = sr.Read();
                    if (next != 'l' && next != 'L')
                        throw new Exception("Parsing Object failed: Expected 'l' or 'L', got '" + (char)next + '\'');
                    this.SetValue();
                    break;
                case '{':
                    this.ReadObject(sr);
                    break;
                case '[':
                    this.ReadArray(sr);
                    break;
                case '"':
                    this.ReadString(sr);
                    break;
                case '-':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    this.ReadNumber(sr);
                    break;
                case 't':
                case 'f':
                case 'T':
                case 'F':
                    this.ReadBoolean(sr);
                    break;
            }
        }
        private void ReadObject(System.IO.StreamReader sr)
        {
            int i;
            bool isOpen = false;
            bool hasLabel = false;
            bool getNextValue = false;
            bool getNextKeySet = false;
            string label = "";
            char lastChar = '\0';
            Dictionary<string, JsonNode> dict = new Dictionary<string, JsonNode>();
            this.SetValue(dict);
            while ((i = sr.Peek()) >= 0)
            {
                char c = (char)i;
                if (!(c == ' ' || c == '\t' || c == '\n' || c == '\r'))
                {
                    if (isOpen)
                    {//node is opened ('{' already got read) and ready to be parsed
                        if (hasLabel)
                        {//Label for current object item was already started and thus can be
                            if (getNextKeySet)
                            {//Next key set needs to be received (value already parsed) or the current object has to be closed
                                if (c == ',')
                                {
                                    hasLabel = false;
                                    getNextKeySet = false;
                                    getNextValue = false;
                                    hasLabel = false;
                                }
                                else if (c == '}')
                                {
                                    c = (char)sr.Read();
                                    lastChar = c;
                                    break;
                                }
                                else
                                {
                                    throw new Exception("Parsing Object failed: Expected ',' or '}', got '" + c + '\'');
                                }
                            }
                            else if (getNextValue)
                            {//Next value of current label needs to get parsed
                                JsonNode child = new JsonNode();
                                child.Read(sr);
                                dict.Add(label, child);
                                getNextKeySet = true;
                                lastChar = '\0';
                                continue;
                            }
                            else
                            {//Separator for label and string expected
                                if (c != ':')
                                    throw new Exception("Parsing Object failed: Expected '\"', got '" + c + '\'');
                                getNextValue = true;
                            }
                        }
                        else
                        {//No label yet found --> get label of current item
                            if (c != '"')
                                throw new Exception("Parsing Object failed: Expected '\"', got '" + c + '\'');
                            label = ParseStringFromEncoded(sr);
                            hasLabel = true;
                            lastChar = '"';
                            continue;
                        }
                    }
                    else
                    {//node yet has to be opened
                        if (c != '{')
                            throw new Exception("Parsing Object failed: Expected '{', got '" + c + '\'');
                        isOpen = true;
                    }
                }
                c = (i = sr.Read()) > 0 ? (char)c : '\0';
                lastChar = c;
            }
            if (lastChar != '}')
                throw new Exception("Parsing Object failed: Expected '}', got '" + lastChar + '\'');
        }
        private void ReadArray(System.IO.StreamReader sr)
        {
            int i;
            List<JsonNode> array = new List<JsonNode>();
            this.SetValue(array);
            bool isOpen = false;
            bool allowNext = true;
            char lastChar = '\0';
            while ((i = sr.Peek()) >= 0)
            {
                char c = (char)i;
                if (!(c == ' ' || c == '\t' || c == '\n' || c == '\r'))
                {
                    if (isOpen)
                    {
                        if (c == ']')
                        {
                            c = (i = sr.Read()) > 0 ? (char)c : '\0';
                            lastChar = c;
                            break;
                        }
                        else if (allowNext)
                        {
                            JsonNode node = new JsonNode();
                            node.Read(sr);
                            array.Add(node);
                            allowNext = false;
                            lastChar = '\0';
                            continue;
                        }
                        else if (c == ',')
                        {
                            allowNext = true;
                        }
                        else
                        {
                            throw new Exception("Parsing Object failed: Expected ',' or ']', got '" + c + '\'');
                        }
                    }
                    else
                    {
                        if (c != '[')
                            throw new Exception("Parsing Object failed: Expected '{', got '" + c + '\'');
                        isOpen = true;
                        allowNext = true;
                    }
                }
                c = (i = sr.Read()) > 0 ? (char)c : '\0';
                lastChar = c;
            }
        }
        private void ReadString(System.IO.StreamReader sr)
        {
            this.SetValue(ParseStringFromEncoded(sr));
        }
        private static string ParseStringFromEncoded(System.IO.StreamReader sr)
        {
            StringBuilder sb = new StringBuilder();
            int i;
            if (sr.Peek() != '"')
                throw new Exception("Parsing Object failed: Expected '\"', got '" + (char)sr.Peek() + '\'');
            sr.Read();
            bool escape = false;
            char lastChar = '\0';
            while ((i = sr.Peek()) >= 0)
            {
                char c = (char)i;
                if (escape)
                {
                    switch (c)
                    {
                        default:
                            throw new Exception("Parsing Object failed: Invalid escape: '" + c + '\'');
                        case '"':
                            sb.Append('"');
                            break;
                        case '\'':
                            sb.Append('\'');
                            break;
                        case '\\':
                            sb.Append('\\');
                            break;
                        case '/':
                            sb.Append('/');
                            break;
                        case 'b':
                            sb.Append("\b");
                            break;
                        case 'f':
                            sb.Append("\f");
                            break;
                        case 'n':
                            sb.Append("\n");
                            break;
                        case 'r':
                            sb.Append("\r");
                            break;
                        case 't':
                            sb.Append("\t");
                            break;
                        case 'u':
                            string encoded = "";
                            sr.Read();
                            encoded += (char)sr.Read();
                            encoded += (char)sr.Read();
                            encoded += (char)sr.Read();
                            encoded += (char)sr.Peek();
                            sb.Append((char)int.Parse(encoded, System.Globalization.NumberStyles.HexNumber));
                            break;
                    }
                    escape = false;
                }
                else
                {
                    if (c == '\\')
                    {
                        escape = true;
                    }
                    else if (c == '"')
                    {
                        c = (char)sr.Read();
                        lastChar = c;
                        break;
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                c = (char)sr.Read();
                lastChar = c;
            }
            if (lastChar != '"')
                throw new Exception("Parsing Object failed: Expected '\"', got '" + lastChar + '\'');
            return sb.ToString();
        }
        private void ReadNumber(System.IO.StreamReader sr)
        {
            StringBuilder sb = new StringBuilder();
            int i;
            if (sr.Peek() == '-')
            {
                sb.Append('-');
                sr.Read();
            }

            bool isFrontNumber = true;
            while ((i = sr.Peek()) >= 0)
            {
                char c = (char)i;
                if (isFrontNumber)
                {
                    if (c >= '0' && c <= '9')
                    {
                        if (sb.Length > 0 && sb[0] == '0')
                            throw new Exception("Parsing Object failed: Expected '.', got '" + c + "', Note: ASAP-JSON is not capable to parse eg. 0.1234e10");
                        sb.Append(c);
                    }
                    else if (c == '.')
                    {
                        sb.Append(c);
                        isFrontNumber = false;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (c >= '0' && c <= '9')
                    {
                        sb.Append(c);
                    }
                    else if (c == 'e' || c == 'E' || c == '+' || c == '-')
                    {
                        throw new Exception("Parsing Object failed: Expected DIGIT, got '" + c + "', Note: ASAP-JSON is not capable to parse eg. 0.1234e10");
                    }
                    else
                    {
                        break;
                    }
                }
                sr.Read();
            }
            this.SetValue(double.Parse(sb.ToString(), System.Globalization.CultureInfo.InvariantCulture));
        }
        private void ReadBoolean(System.IO.StreamReader sr)
        {
            int i = sr.Peek();
            if (i == 't' || i == 'T')
            {
                sr.Read();
                i = sr.Read();
                if (i != 'r' && i != 'R')
                    throw new Exception("Parsing Object failed: Expected 'r' or 'R', got '" + (char)i + '\'');
                i = sr.Read();
                if (i != 'u' && i != 'U')
                    throw new Exception("Parsing Object failed: Expected 'u' or 'U', got '" + (char)i + '\'');
                i = sr.Read();
                if (i != 'e' && i != 'E')
                    throw new Exception("Parsing Object failed: Expected 'e' or 'E', got '" + (char)i + '\'');
                this.SetValue(true);
            }
            else if (i == 'f' | i == 'F')
            {
                sr.Read();
                i = sr.Read();
                if (i != 'a' && i != 'A')
                    throw new Exception("Parsing Object failed: Expected 'a' or 'A', got '" + (char)i + '\'');
                i = sr.Read();
                if (i != 'l' && i != 'L')
                    throw new Exception("Parsing Object failed: Expected 'l' or 'L', got '" + (char)i + '\'');
                i = sr.Read();
                if (i != 's' && i != 'S')
                    throw new Exception("Parsing Object failed: Expected 's' or 'S', got '" + (char)i + '\'');
                i = sr.Read();
                if (i != 'e' && i != 'E')
                    throw new Exception("Parsing Object failed: Expected 'e' or 'E', got '" + (char)i + '\'');
                this.SetValue(false);
            }
        }
        #endregion

        public void Print(System.IO.StreamWriter sw)
        {
            Print_handle(sw, false, 0);
        }
        public void PrintBeauty(System.IO.StreamWriter sw)
        {
            Print_handle(sw, true, 0);
        }
        private void Print_handle(System.IO.StreamWriter sw, bool beautify, int tablength)
        {
            switch (this.Type)
            {
                case EJType.Array:
                    {
                        List<JsonNode> obj;
                        bool flag = false;
                        this.GetValue(out obj);
                        if (beautify)
                        {
                            sw.Write('\r');
                            sw.Write('\n');
                            sw.Write(new string('\t', tablength));
                        }
                        sw.Write('[');
                        foreach (var it in obj)
                        {
                            if (flag)
                            {
                                sw.Write(',');
                                if (beautify)
                                {
                                    sw.Write('\r');
                                    sw.Write('\n');
                                    sw.Write(new string('\t', tablength + 1));
                                }
                            }
                            else
                                flag = true;
                            it.Print_handle(sw, beautify, tablength + 1);
                        }
                        sw.Write(']');
                    }
                    break;
                case EJType.Boolean:
                    {
                        bool obj;
                        this.GetValue(out obj);
                        sw.Write(obj ? "true" : "false");
                    }
                    break;
                case EJType.Number:
                    {
                        double obj;
                        this.GetValue(out obj);
                        sw.Write(obj.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    }
                    break;
                case EJType.Object:
                    {
                        Dictionary<string, JsonNode> obj;
                        bool flag = false;
                        this.GetValue(out obj);
                        if (obj == null)
                        {
                            sw.Write("null");
                        }
                        else
                        {
                            if (beautify)
                            {
                                sw.Write('\r');
                                sw.Write('\n');
                                sw.Write(new string('\t', tablength));
                            }
                            sw.Write('{');
                            foreach (var it in obj)
                            {
                                if (flag)
                                {
                                    sw.Write(',');
                                    if (beautify)
                                    {
                                        sw.Write('\r');
                                        sw.Write('\n');
                                        sw.Write(new string('\t', tablength + 1));
                                    }
                                }
                                else
                                    flag = true;
                                sw.Write('"');
                                sw.Write(it.Key);
                                sw.Write('"');
                                sw.Write(':');
                                it.Value.Print_handle(sw, beautify, tablength + 1);
                            }
                            sw.Write('}');
                        }
                    }
                    break;
                case EJType.String:
                    {
                        string obj;
                        this.GetValue(out obj);
                        sw.Write('"');
                        var sb = new StringBuilder();
                        foreach (var it in obj)
                        {
                            if (it < 128)
                            {
                                switch (it)
                                {
                                    default:
                                        sb.Append(it);
                                        break;
                                    case '"':
                                        sb.Append("\\\"");
                                        break;
                                    case '\'':
                                        sb.Append("\\'");
                                        break;
                                    case '/':
                                        sb.Append("\\/");
                                        break;
                                    case '\b':
                                        sb.Append("\\b");
                                        break;
                                    case '\f':
                                        sb.Append("\\f");
                                        break;
                                    case '\n':
                                        sb.Append("\\n");
                                        break;
                                    case '\r':
                                        sb.Append("\\r");
                                        break;
                                    case '\t':
                                        sb.Append("\\t");
                                        break;
                                    case '\\':
                                        sb.Append("\\\\");
                                        break;
                                }
                            }
                            else
                            {
                                sb.Append("\\u" + ((int)it).ToString("x4"));
                            }
                        }
                        sw.Write(sb.ToString());
                        sw.Write('"');
                    }
                    break;
                default:
                    throw new Exception("Not Implemented type");
            }
        }

        public override string ToString()
        {
            using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(memStream);
                this.Print(sw);
                sw.Flush();
                memStream.Seek(0, System.IO.SeekOrigin.Begin);
                string s = new System.IO.StreamReader(memStream).ReadToEnd();
                return s;
            }
        }
    }
}
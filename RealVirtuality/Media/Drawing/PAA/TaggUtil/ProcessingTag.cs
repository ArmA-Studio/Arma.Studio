using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RealVirtuality.Media.Drawing.PAA.TaggUtil
{
    public class ProcessingTag : TagBase
    {
        /*
            {
                char  "GGATCORP";
                ulong len;   //  strlen(text)
                char  text   // NOT ASCIIz
            }
         */
        internal const string NAME_REVERSED = "GGATCORP";
        internal const string NAME = "PROCTAGG";

        public string ShaderText
        {
            get
            {
                return new string(Encoding.ASCII.GetChars(this.Data.GetData()));
            }
            set
            {
                var bytes = Encoding.ASCII.GetBytes(value);
                this.Data.SetData(bytes.Take(bytes.Length - 1).ToArray());
            }
        }

        internal ProcessingTag(Tagg t) : base(t, NAME, -1)
        {
            if (t.Name != this.Name.Substring(0, NAMELENGTH))
            {
                throw new ArgumentException("Invalid Tagg provided", "t");
            }
        }
        public static ProcessingTag Create(string shaderText)
        {
            var t = new Tagg();
            t.Name = NAME;
            t.Signature = Tagg.DEFAULT_SIGNATURE;
            var val = new ProcessingTag(t);
            val.ShaderText = shaderText;
            return val;
        }
    }
}

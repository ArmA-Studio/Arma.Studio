using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RealVirtuality.Media.Drawing.PAA.TaggUtil
{
    public class SwizzleTag : TagBase
    {
        internal const string NAME_REVERSED = "GGATZIWS";
        internal const string NAME = "SWIZTAGG";
        internal const int DATALENGTH = 4;
        /*
            {
                char  "GGATZIWS";
                ulong len;   // 4 bytes
                ulong  data; // 0x05040203
            }
            Exactl format unknown
         */

        internal SwizzleTag(Tagg t) : base(t, NAME, DATALENGTH)
        {
            if (t.Name != this.Name.Substring(0, NAMELENGTH) || t.DataLength != this.Length)
            {
                throw new ArgumentException("Invalid Tagg provided", "t");
            }
        }
        public static SwizzleTag Create()
        {
            var t = new Tagg();
            t.Name = NAME;
            t.Signature = Tagg.DEFAULT_SIGNATURE;
            t.SetData(new byte[DATALENGTH]);
            var val = new SwizzleTag(t);
            return val;
        }
    }
}

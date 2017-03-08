using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RealVirtuality.Media.Drawing.PAA.TaggUtil
{
    public class MaxColorTag : TagBase
    {
        /*
            {
                char  "GGATCXAM";
                ulong len;   // 4 bytes
                ulong  data; // FFFFFFFFF no other value seen so far
            }
         */
        internal const string NAME_REVERSED = "GGATCXAM";
        internal const string NAME = "MAXCTAGG";
        internal const int DATALENGTH = 4;

        public Color Color
        {
            get { return Color.FromArgb(this.Data.DataRaw[0], this.Data.DataRaw[3], this.Data.DataRaw[2], this.Data.DataRaw[1]); }
            set
            {
                this.Data.DataRaw[0] = value.A;
                this.Data.DataRaw[1] = value.B;
                this.Data.DataRaw[2] = value.G;
                this.Data.DataRaw[3] = value.R;
            }
        }

        internal MaxColorTag(Tagg t) : base(t, NAME, DATALENGTH)
        {
            if (t.Name != this.Name.Substring(0, NAMELENGTH) || t.DataLength != this.Length)
            {
                throw new ArgumentException("Invalid Tagg provided", "t");
            }
        }
        public static MaxColorTag Create(Color c)
        {
            var t = new Tagg();
            t.Name = NAME;
            t.Signature = Tagg.DEFAULT_SIGNATURE;
            t.SetData(new byte[DATALENGTH]);
            var val = new MaxColorTag(t);
            val.Color = c;
            return val;
        }
    }
}

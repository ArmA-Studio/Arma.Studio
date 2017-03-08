using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RealVirtuality.Media.Drawing.PAA.TaggUtil
{
    public class FlagTag : TagBase
    {
        /*
            {
                  char  "GGATGALF";
                  ulong len;   // 4 bytes
                  ulong range; // 0 to 2
            }
        */
        public enum ETransparencyKind
        {
            NoTransparency,
            BasicTransparency,
            InterpolatedTransparency
        }
        internal const string NAME_REVERSED = "GGATGALF";
        internal const string NAME = "FLAGTAGG";
        internal const int DATALENGTH = 4;

        public ETransparencyKind TransparencyKind { get { return (ETransparencyKind)this.Data.DataRaw[0]; } set { this.Data.DataRaw[0] = (byte)value; } }

        internal FlagTag(Tagg t) : base(t, NAME, DATALENGTH)
        {
            if (t.Name != this.Name.Substring(0, NAMELENGTH) || t.DataLength != this.Length || t.DataRaw[0] > 2)
            {
                throw new ArgumentException("Invalid Tagg provided", "t");
            }
        }
        public static FlagTag Create()
        {
            var t = new Tagg();
            t.Name = NAME;
            t.Signature = Tagg.DEFAULT_SIGNATURE;
            t.SetData(new byte[DATALENGTH]);
            var val = new FlagTag(t);
            val.TransparencyKind = ETransparencyKind.NoTransparency;
            return val;
        }
    }
}

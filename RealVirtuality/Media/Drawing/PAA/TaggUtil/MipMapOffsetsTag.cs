using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RealVirtuality.Media.Drawing.PAA.TaggUtil
{
    public class MipMapOffsetsTag : TagBase
    {
        /*
            {
                char  "GGATSFFO";
                ulong len;         // 16 * sizeof(ulong)
                ulong offsets[16];
            }
            Example:
            = 6 entries. last 10 unused
            256 x 128 Size=16384
            128 x 64 Size=4096
            64 x 32 Size=1024
            32 x 16 Size=256
            16 x 8 Size=64
            8 x 4 Size=16
         */

        internal const string NAME_REVERSED = "GGATCORP";
        internal const string NAME = "PROCTAGG";
        internal const int DATALENGTH = 16 * sizeof(ulong);

        private ulong GetUlongFromOffset(int off)
        {
            ulong val = (ulong)this.Data.DataRaw[off + 0] << (sizeof(byte) * 0) |
                        (ulong)this.Data.DataRaw[off + 1] << (sizeof(byte) * 1) |
                        (ulong)this.Data.DataRaw[off + 2] << (sizeof(byte) * 2) |
                        (ulong)this.Data.DataRaw[off + 3] << (sizeof(byte) * 3) |
                        (ulong)this.Data.DataRaw[off + 4] << (sizeof(byte) * 4) |
                        (ulong)this.Data.DataRaw[off + 5] << (sizeof(byte) * 5) |
                        (ulong)this.Data.DataRaw[off + 6] << (sizeof(byte) * 6) |
                        (ulong)this.Data.DataRaw[off + 7] << (sizeof(byte) * 7);
            return val;
        }
        private void SetUlongFromOffset(int off, ulong value)
        {
            this.Data.DataRaw[off + 0] = (byte)(value >> (sizeof(byte) * 7));
            this.Data.DataRaw[off + 1] = (byte)(value >> (sizeof(byte) * 6));
            this.Data.DataRaw[off + 2] = (byte)(value >> (sizeof(byte) * 5));
            this.Data.DataRaw[off + 3] = (byte)(value >> (sizeof(byte) * 4));
            this.Data.DataRaw[off + 4] = (byte)(value >> (sizeof(byte) * 3));
            this.Data.DataRaw[off + 5] = (byte)(value >> (sizeof(byte) * 2));
            this.Data.DataRaw[off + 6] = (byte)(value >> (sizeof(byte) * 1));
            this.Data.DataRaw[off + 7] = (byte)(value >> (sizeof(byte) * 0));
        }

        public ulong Offset_1048576 { get { return GetUlongFromOffset(sizeof(ulong) * 16); } set { SetUlongFromOffset(sizeof(ulong) * 16, value); } }
        public ulong Offset_524288  { get { return GetUlongFromOffset(sizeof(ulong) * 15); } set { SetUlongFromOffset(sizeof(ulong) * 15, value); } }
        public ulong Offset_262144  { get { return GetUlongFromOffset(sizeof(ulong) * 14); } set { SetUlongFromOffset(sizeof(ulong) * 14, value); } }
        public ulong Offset_131072  { get { return GetUlongFromOffset(sizeof(ulong) * 13); } set { SetUlongFromOffset(sizeof(ulong) * 13, value); } }
        public ulong Offset_32768   { get { return GetUlongFromOffset(sizeof(ulong) * 12); } set { SetUlongFromOffset(sizeof(ulong) * 12, value); } }
        public ulong Offset_16384   { get { return GetUlongFromOffset(sizeof(ulong) * 11); } set { SetUlongFromOffset(sizeof(ulong) * 11, value); } }
        public ulong Offset_8192    { get { return GetUlongFromOffset(sizeof(ulong) * 10); } set { SetUlongFromOffset(sizeof(ulong) * 10, value); } }
        public ulong Offset_4096    { get { return GetUlongFromOffset(sizeof(ulong) *  9); } set { SetUlongFromOffset(sizeof(ulong) *  9, value); } }
        public ulong Offset_2048    { get { return GetUlongFromOffset(sizeof(ulong) *  8); } set { SetUlongFromOffset(sizeof(ulong) *  8, value); } }
        public ulong Offset_1024    { get { return GetUlongFromOffset(sizeof(ulong) *  7); } set { SetUlongFromOffset(sizeof(ulong) *  7, value); } }
        public ulong Offset_512     { get { return GetUlongFromOffset(sizeof(ulong) *  6); } set { SetUlongFromOffset(sizeof(ulong) *  6, value); } }
        public ulong Offset_256     { get { return GetUlongFromOffset(sizeof(ulong) *  5); } set { SetUlongFromOffset(sizeof(ulong) *  5, value); } }
        public ulong Offset_128     { get { return GetUlongFromOffset(sizeof(ulong) *  4); } set { SetUlongFromOffset(sizeof(ulong) *  4, value); } }
        public ulong Offset_32      { get { return GetUlongFromOffset(sizeof(ulong) *  3); } set { SetUlongFromOffset(sizeof(ulong) *  3, value); } }
        public ulong Offset_16      { get { return GetUlongFromOffset(sizeof(ulong) *  2); } set { SetUlongFromOffset(sizeof(ulong) *  2, value); } }
        public ulong Offset_8       { get { return GetUlongFromOffset(sizeof(ulong) *  1); } set { SetUlongFromOffset(sizeof(ulong) *  1, value); } }
        public ulong Offset_4       { get { return GetUlongFromOffset(sizeof(ulong) *  0); } set { SetUlongFromOffset(sizeof(ulong) *  0, value); } }

        internal MipMapOffsetsTag(Tagg t) : base(t, NAME, DATALENGTH)
        {
            if (t.Name != this.Name.Substring(0, NAMELENGTH) || t.DataLength != this.Length)
            {
                throw new ArgumentException("Invalid Tagg provided", "t");
            }
        }
        public static MipMapOffsetsTag Create()
        {
            var t = new Tagg();
            t.Name = NAME;
            t.Signature = Tagg.DEFAULT_SIGNATURE;
            t.SetData(new byte[DATALENGTH]);
            var val = new MipMapOffsetsTag(t);
            return val;
        }
    }
}

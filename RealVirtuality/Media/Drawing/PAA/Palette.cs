using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace RealVirtuality.Media.Drawing.PAA
{
    public struct Palette
    {
        public ushort TripletCount { get { return (ushort)(this.BGRTriplets.Length / 3); } }
        public byte[] BGRTriplets { get; private set; }
        public Color[] Triplets
        {
            get
            {
                var list = new List<Color>();

                for (int i = 2; i < TripletCount; i++)
                {
                    if (i % 3 == 2)
                    {
                        list.Add(Color.FromArgb(BGRTriplets[i], BGRTriplets[i - 1], BGRTriplets[i - 2]));
                    }
                }

                return list.ToArray();
            }
            set
            {
                var bArr = new byte[value.Length * 3];
                for (int i = 0; i < value.Length; i++)
                {
                    var c = value[i];
                    bArr[i + 0] = c.B;
                    bArr[i + 1] = c.G;
                    bArr[i + 2] = c.R;
                }
                this.BGRTriplets = bArr;
            }
        }

        public Palette(Color[] triplets)
        {
            this.BGRTriplets = null;
            this.Triplets = triplets;
        }

        internal static async Task<Palette> ParseFromIoStream(Stream s)
        {
            var nBuffer = new byte[sizeof(ushort)];
            await s.ReadAsync(nBuffer, 0, nBuffer.Length);
            var n = BitConverter.ToUInt16(nBuffer, 0);
            var pBuffer = new byte[3 * n];
            await s.ReadAsync(pBuffer, 0, pBuffer.Length);
            var p = new Palette();
            p.BGRTriplets = pBuffer;
            return p;
        }

        internal async Task WriteIntoIoStream(Stream s)
        {
            var nBuffer = BitConverter.GetBytes(this.TripletCount);
            await s.WriteAsync(nBuffer, 0, nBuffer.Length);
            await s.WriteAsync(this.BGRTriplets, 0, this.BGRTriplets.Length);
        }
    }
}

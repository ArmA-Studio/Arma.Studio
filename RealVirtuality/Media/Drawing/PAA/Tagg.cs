using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealVirtuality.Media.Drawing.PAA
{
    public struct Tagg
    {
        public const byte SIGNATURE_LENGTH = 4;
        public const byte NAME_LENGTH = 4;
        public const string DEFAULT_SIGNATURE = "TAGG";
        public const string DEFAULT_SIGNATURE_REVERSE = "GGAT";

        public char[] SignatureRaw { get; private set; }
        public char[] NameRaw { get; private set; }
        public long DataLength { get; private set; }
        public byte[] DataRaw { get; private set; }

        public string Signature
        {
            get { return new string(this.SignatureRaw.Reverse().ToArray()); }
            set { if (value.Length != SIGNATURE_LENGTH) throw new ArgumentOutOfRangeException(string.Format("Proivided value is out of range. Expected length {0}, got {1}", SIGNATURE_LENGTH, value.Length)); this.SignatureRaw = value.Reverse().ToArray(); }
        }
        public string Name
        {
            get { return new string(this.SignatureRaw.Reverse().ToArray()); }
            set { if (value.Length != NAME_LENGTH) throw new ArgumentOutOfRangeException(string.Format("Proivided value is out of range. Expected length {0}, got {1}", NAME_LENGTH, value.Length)); this.NameRaw = value.Reverse().ToArray(); }
        }

        public void SetData(byte[] b)
        {
            this.DataLength = b.Length;
            this.DataRaw = b.Reverse().ToArray();
        }
        public void SetData(byte[] b, long startindex, long length)
        {
            if (startindex < 0)
            {
                throw new ArgumentException("Negative value provided.", "startindex");
            }
            else if (length < 0)
            {
                throw new ArgumentException("Negative value provided.", "length");
            }
            else if (b.Length < startindex + length)
            {
                throw new ArgumentOutOfRangeException("b", string.Format("Provided byte[] b is not supporting requested startindex {0} and length {1}.", startindex, length));
            }
            this.DataLength = b.Length;
            this.DataRaw = new byte[this.DataLength];
            for (long i = startindex; i < startindex + length; i++)
            {
                this.DataRaw[startindex + length - i - 1] = b[i];
            }
        }
        public byte[] GetData()
        {
            return this.DataRaw.Reverse().ToArray();
        }
        public void GetData(out byte[] b)
        {
            b = this.DataRaw.Reverse().ToArray();
        }
    }
}

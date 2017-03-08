using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealVirtuality.Media.Drawing.PAA.TaggUtil
{
    public abstract class TagBase
    {
        internal const byte NAMELENGTH = 4;
        internal TagBase(Tagg t, string name, long length)
        {
            if (this.Name.Length != 8)
                throw new ArgumentException("Provided name is not of length 8", "name");
            this.Data = t;
            this.Name = name;
            this.Length = length;
        }
        public Tagg Data { get; internal set; }
        public readonly string Name;
        public readonly long Length;
    }
}

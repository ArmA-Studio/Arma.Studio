using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealVirtuality.Lang.Preprocessing
{
    public struct PreProcessingError
    {
        public int Line;
        public int Column;
        public long StartOffset;
        public long EndOffset;
        public string Message;
        public int ErrorCode;

        public long Length => this.EndOffset - this.StartOffset;

    }
}

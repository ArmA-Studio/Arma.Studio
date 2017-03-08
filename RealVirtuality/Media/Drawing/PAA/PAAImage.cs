using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealVirtuality.Media.Drawing.PAA.TaggUtil;


namespace RealVirtuality.Media.Drawing.PAA
{
    public class PAAImage
    {
        public List<TagBase> Tags { get; private set; }
        
        public PAAImage()
        {
            this.Tags = new List<TagBase>();
        }
    }
}

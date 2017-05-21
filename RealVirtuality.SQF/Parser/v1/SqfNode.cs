using System;
using System.Collections.Generic;
namespace RealVirtuality.SQF.Parser.v1
{
    public abstract class SqfNode
    {
        public WeakReference<SqfNode> ParentWeak { get; set; }
        public IList<SqfNode> Children { get; set; }

        public int StartOffset { get; set; }
        public int Length { get; set; }
        public int Line { get; set; }
        public int Col { get; set; }

        public SqfNode(SqfNode parent)
        {
            this.ParentWeak = new WeakReference<SqfNode>(parent);
            this.Children = new List<SqfNode>();
        }
    }
}

namespace RealVirtuality.SQF
{
    public class SqfVariable : SqfNode
    {
        public SqfVariable(SqfNode parent) : base(parent)
        {
        }

        public string Identifier { get; internal set; }
    }
}
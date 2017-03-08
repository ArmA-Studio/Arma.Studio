namespace RealVirtuality.SQF
{
    public class SqfNularExpression : SqfNode
    {
        public SqfNularExpression(SqfNode parent) : base(parent)
        {
        }

        public string Identifier { get; internal set; }
    }
}
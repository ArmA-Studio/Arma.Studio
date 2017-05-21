namespace RealVirtuality.SQF.Parser.v1
{
    public class SqfNularExpression : SqfNode
    {
        public SqfNularExpression(SqfNode parent) : base(parent)
        {
        }

        public string Identifier { get; internal set; }
    }
}
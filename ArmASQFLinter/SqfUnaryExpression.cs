namespace RealVirtuality.SQF
{
    public class SqfUnaryExpression : SqfNode
    {
        public SqfUnaryExpression(SqfNode parent) : base(parent)
        {
        }

        public SqfNode Expression { get; internal set; }
        public string Operator { get; internal set; }
    }
}
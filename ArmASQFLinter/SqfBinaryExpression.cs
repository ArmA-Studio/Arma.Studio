namespace RealVirtuality.SQF
{
    public class SqfBinaryExpression : SqfNode
    {
        public SqfBinaryExpression(SqfNode parent) : base(parent)
        {
            
        }

        public SqfNode LValue { get; internal set; }
        public string Operation { get; internal set; }
        public SqfNode RValue { get; internal set; }
    }
}
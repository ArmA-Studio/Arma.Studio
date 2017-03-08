namespace RealVirtuality.SQF
{
    public class SqfOperator : SqfNode
    {
        public SqfOperator(SqfNode parent) : base(parent)
        {
        }

        public string Operator { get; internal set; }
    }
}
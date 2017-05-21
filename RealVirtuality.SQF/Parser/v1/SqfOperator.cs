namespace RealVirtuality.SQF.Parser.v1
{
    public class SqfOperator : SqfNode
    {
        public SqfOperator(SqfNode parent) : base(parent)
        {
        }

        public string Operator { get; internal set; }
    }
}
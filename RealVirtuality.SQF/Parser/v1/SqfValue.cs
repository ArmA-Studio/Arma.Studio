namespace RealVirtuality.SQF.Parser.v1
{
    public abstract class SqfValue : SqfNode
    {
        public SqfValue(SqfNode parent) : base(parent)
        {
        }

        public string Value { get; internal set; }
    }
}
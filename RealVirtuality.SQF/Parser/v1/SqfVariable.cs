namespace RealVirtuality.SQF.Parser.v1
{
    public class SqfVariable : SqfNode
    {
        public SqfVariable(SqfNode parent) : base(parent)
        {
        }

        public string Identifier { get; internal set; }
    }
}
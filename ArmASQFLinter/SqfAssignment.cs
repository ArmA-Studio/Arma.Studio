using RealVirtuality.SQF.ANTLR.Parser;

namespace RealVirtuality.SQF
{
    public class SqfAssignment : SqfNode
    {
        public SqfAssignment(SqfNode parent) : base(parent)
        {
        }

        public SqfNode AssignedExpression { get; set; }
        public bool HasPrivateKeyword { get; set; }
        public string VariableName { get; set; }
    }
}
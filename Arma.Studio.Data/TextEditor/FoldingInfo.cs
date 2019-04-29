namespace Arma.Studio.Data.TextEditor
{
    public class FoldingInfo
    {
        public int? StartOffset { get; set; }
        public int? LineStart { get; set; }
        public int? ColumnStart { get; set; }
        public int Length { get; set; }
    }
}
namespace Arma.Studio.Data.TextEditor
{
    public class FoldingInfo
    {
        public int StartOffset { get; set; }
        public int Length { get; set; }
        public int EndOffset => this.StartOffset + this.Length;

    }
}
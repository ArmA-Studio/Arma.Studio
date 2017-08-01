namespace RealVirtuality.Lang.Preprocessing
{
    public struct PreProcessedInfo
    {
        public PreProcessingDirective Directive { get; set; }
        public long EndOffset { get; set; }
        public string[] InfoParams { get; set; }
        public long StartOffset { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public PreProcessedInfo(PreProcessingDirective directive, long startOffset, long endOffset, int line, int col, string[] infoParams)
        {
            this.Directive = directive;
            this.StartOffset = startOffset;
            this.EndOffset = endOffset;
            this.InfoParams = infoParams;
            this.Line = line;
            this.Column = col;
        }

        public string PreProcessedText => this.Directive.GetText(this.InfoParams);
    }
}
namespace RealVirtuality.Lang.Preprocessing
{
    public class PreProcessedInfo
    {
        private PreProcessingDirective Directive;
        private long EndOffset;
        private string[] InfoParams;
        private long StartOffset;

        public PreProcessedInfo(PreProcessingDirective directive, long startOffset, long endOffset, string[] infoParams)
        {
            this.Directive = directive;
            this.StartOffset = startOffset;
            this.EndOffset = endOffset;
            this.InfoParams = infoParams;
        }

        public object PreProcessedText => this.Directive.GetText(this.InfoParams);
    }
}
using System;
using ICSharpCode.AvalonEdit.Document;

namespace ArmA.Studio.DataContext.TextEditorUtil
{
    public sealed partial class LinterInfo : ISegment
    {
        public int Line { get; set; }
        public int LineOffset { get; set; }
        public ESeverity Severity { get; set; }
        public string FileName { get; set; }
        public int EndOffset { get { return this.StartOffset + Length; } }
        public int Offset { get { return this.StartOffset; } }

        public int Length { get; set; }
        public int StartOffset { get; set; }
        public string Message { get; set; }

        public static implicit operator TextSegment(LinterInfo sErr)
        {
            return new TextSegment() { StartOffset = sErr.StartOffset, Length = sErr.Length, EndOffset = sErr.EndOffset };
        }
    }
}
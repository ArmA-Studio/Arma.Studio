using Arma.Studio.Data.TextEditor;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.UI
{
    public class CompletionData : ICompletionData
    {
        private readonly ICodeCompletionInfo InnerCompletionInfo;
        public ImageSource Image => this.InnerCompletionInfo.ImageSource;

        public string Text => this.InnerCompletionInfo.Text;

        public object Content => this.InnerCompletionInfo.Content;

        public object Description => this.InnerCompletionInfo.Description;

        public double Priority => this.InnerCompletionInfo.Priority;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            var textSegment = textArea.Document.GetText(completionSegment);
            var text = this.InnerCompletionInfo.Complete(textSegment);
            textArea.Document.Replace(completionSegment, text);
        }

        public CompletionData(ICodeCompletionInfo codeCompletionInfo)
        {
            this.InnerCompletionInfo = codeCompletionInfo;
        }
    }
}

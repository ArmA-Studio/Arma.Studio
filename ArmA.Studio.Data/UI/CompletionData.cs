using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Data.UI
{
    public class CompletionData : ICompletionData
    {
        public CompletionData(string text)
        {
            this.Text = text;
            this.Description = text;
            this.Priority = 0;
        }

        public System.Windows.Media.ImageSource Image { get; set; }

        public string Text { get; set; }

        /// Use this property if you want to show a fancy UIElement in the list.
        public object Content { get { return this._Content ?? this.Text; } set { this._Content = value; } }
        private object _Content;

        public object Description { get; set; }

        public double Priority { get; set; }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }
    }
}

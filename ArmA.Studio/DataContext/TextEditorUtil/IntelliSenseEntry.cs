using System;
using ICSharpCode.AvalonEdit.Document;

namespace ArmA.Studio.DataContext.TextEditorUtil
{
    public class IntelliSenseEntry
    {
        public string Text { get; set; }
        public string IconPath { get; set; }
        public Action Callback { get; set; }
        public string ContentToFinish { get; set; }
    }
}
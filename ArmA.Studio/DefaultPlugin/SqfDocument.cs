using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArmA.Studio.Data;
using ArmA.Studio.Data.IntelliSense;
using ArmA.Studio.Data.Lint;
using ArmA.Studio.Data.UI;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace ArmA.Studio.DefaultPlugin
{
    public class SqfDocument : CodeEditorBaseDataContext, ILinterHost, IIntelliSenseHost
    {
        public SqfDocument() : this(null) { }
        public SqfDocument(ProjectFile fileRef) : base(fileRef)
        {
            using (var memstream = new MemoryStream())
            using (var reader = HighlightingHelper.GetSqfXml(memstream))
                this.SyntaxHighlightingDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
        }

        public bool IsDisplayed { get; set; }

        public IEnumerable<LintInfo> LinterInfo { get; set; }

        public void Display(TextEditor editorInstance, Point pos)
        {
            
        }

        public void DoLinting(Stream reader)
        {
            
        }

        public void Hide()
        {
            
        }

        public void Update(TextDocument document, Caret caret)
        {
            
        }
    }
}

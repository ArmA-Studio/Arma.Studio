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
using ArmA.Studio.UI;
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


        public void LintWriteCache(Stream stream, ProjectFile f)
        {
            var lintHelper = new SqfLintHelper();
            this.LinterInfo = lintHelper.Lint(stream, f);
        }

        public void Display(TextEditor editorInstance, Point pos)
        {
            
        }

        public void Hide()
        {
            
        }

        public void Update(TextDocument document, Caret caret)
        {
            
        }

        protected override void OnEditorInitialized(TextEditor editor)
        {
            base.OnEditorInitialized(editor);
            var bpm = new BreakPointMargin(this.FileReference);
            this.EditorInstance.TextArea.TextView.BackgroundRenderers.Add(new LineHighlighterBackgroundRenderer(this.EditorInstance));
            this.EditorInstance.TextArea.TextView.BackgroundRenderers.Add(new UnderlineBackgroundRenderer(this));
            this.EditorInstance.TextArea.TextView.BackgroundRenderers.Add(bpm);
            this.EditorInstance.TextArea.LeftMargins.Insert(0, bpm);
            this.EditorInstance.TextArea.LeftMargins.Insert(1, new RuntimeExecutionMargin());
        }

        public IEnumerable<LintInfo> Lint(Stream stream, ProjectFile f)
        {
            var lintHelper = new SqfLintHelper();
            return lintHelper.Lint(stream, f);
        }
    }
}

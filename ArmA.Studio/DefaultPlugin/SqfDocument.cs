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
using Utility;

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

        protected override void OnEditorInitialized(TextEditor editor)
        {
            base.OnEditorInitialized(editor);
            var bpm = new BreakPointMargin(this.FileReference);
            this.EditorInstance.TextArea.TextView.BackgroundRenderers.Add(new UnderlineBackgroundRenderer(this));
            this.EditorInstance.TextArea.TextView.BackgroundRenderers.Add(bpm);
            this.EditorInstance.TextArea.LeftMargins.Insert(0, bpm);
            this.EditorInstance.TextArea.LeftMargins.Insert(1, new RuntimeExecutionMargin(this));
        }

        public IEnumerable<LintInfo> Lint(Stream stream, ProjectFile f)
        {
            var lintHelper = new SqfLintHelper();
            return lintHelper.Lint(stream, f);
        }

        public IEnumerable<IntelliSenseItem> Generate(TextDocument document, Caret caret)
        {
            IEnumerable<string> docIdents = null;
            string curWord = null;
            int caretOffset = 0;
            App.Current.Dispatcher.Invoke(() =>
            {
                caretOffset = caret.Offset;
                curWord = document.GetWordAround(caret.Offset - 1);
                docIdents = document.GetAllSqfIdents().ToList();
            });
            var sqfDefs = from def in ConfigHost.Instance.SqfDefinitions where def.Kind != RealVirtuality.SQF.SqfDefinition.EKind.Type && def.Name.StartsWith(curWord) orderby def.Name select def;
            docIdents = (from str in docIdents where str.StartsWith(curWord) && !sqfDefs.Any((def) => def.Name.Equals(str, StringComparison.InvariantCultureIgnoreCase)) orderby str select str).Distinct();


            foreach (var it in docIdents)
            {
                yield return new IntelliSenseItem((td) => td.Insert(caretOffset, it.Remove(0, curWord.Length)), it) { ImageSource = @"/ArmA.Studio;component/Resources/Pictograms/Field/Field_16x.png" };
            }
            var l = new List<string>(sqfDefs.Count());
            foreach (var it in sqfDefs)
            {
                if (l.Contains(it.Name))
                    continue;
                yield return new IntelliSenseItem((td) => td.Insert(caretOffset, it.Name.Remove(0, curWord.Length)), it.Name) { Description = it.Description };
                l.Add(it.Name);
            }
        }

        public override string OnHoverInformations(int offset)
        {
            if (!Workspace.Instance.DebugContext.IsPaused)
                return base.OnHoverInformations(offset);
            var word = this.Document.GetWordAround(offset);
            if (string.IsNullOrWhiteSpace(word))
                return base.OnHoverInformations(offset);
            var task = Workspace.Instance.DebugContext.GetVariablesAsync(Debugger.EVariableNamespace.All, word);
            var variable = task.Result.FirstOrNullable();
            if (!variable.HasValue)
                return base.OnHoverInformations(offset);
            return variable.Value.Value;
        }
    }
}

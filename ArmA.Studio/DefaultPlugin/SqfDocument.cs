using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArmA.Studio.Data;
using ArmA.Studio.Data.Lint;
using ArmA.Studio.Data.UI;
using ArmA.Studio.UI;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Utility;
using System.Windows.Media.Imaging;

namespace ArmA.Studio.DefaultPlugin
{
    public class SqfDocument : CodeEditorBaseDataContext, ILinterHost
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

        public override IEnumerable<ICompletionData> GetAutoCompleteData()
        {
            IEnumerable<string> docIdents = null;
            int caretOffset = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                caretOffset = this.EditorInstance.CaretOffset;
                docIdents = this.EditorInstance.Document.GetAllSqfIdents().ToList();
            });
            var sqfDefs = from def in ConfigHost.Instance.SqfDefinitions where def.Kind != RealVirtuality.SQF.SqfDefinition.EKind.Type select def;
            docIdents = (from str in docIdents where !sqfDefs.Any((def) => def.Name.Equals(str, StringComparison.InvariantCultureIgnoreCase)) select str).Distinct();

            var fieldbitmap = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/Pictograms/Field/Field_16x.png", UriKind.Absolute));
            fieldbitmap.Freeze();
            foreach (var it in docIdents)
            {
                yield return new CompletionData(it)
                {
                    Image = fieldbitmap,
                    Priority = 1
                };
            }
            var l = new List<string>(sqfDefs.Count());
            foreach (var it in sqfDefs)
            {
                if (l.Contains(it.Name))
                    continue;
                yield return new CompletionData(it.Name)
                {
                    Description = it.Description
                };
                l.Add(it.Name);
            }
        }

        public override async Task<string> OnHoverInformationsAsync(int offset)
        {
            if (!Workspace.Instance.DebugContext.IsPaused)
                return await base.OnHoverInformationsAsync(offset);
            var word = this.Document.GetWordAround(offset);
            if (string.IsNullOrWhiteSpace(word))
                return await base.OnHoverInformationsAsync(offset);
            var variable = (await Workspace.Instance.DebugContext.GetVariablesAsync(Debugger.EVariableNamespace.All, word)).FirstOrNullable();
            if (!variable.HasValue)
                return await base.OnHoverInformationsAsync(offset);
            return variable.Value.Value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ArmA.Studio.Data.Lint;
using ArmA.Studio.Data.UI.Commands;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using Utility;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace ArmA.Studio.Data.UI
{
    public class CodeEditorBaseDataContext : TextEditorBaseDataContext
    {
        protected const int CONST_LINTER_UPDATE_TIMEOUT_MS = 200;
        public static readonly Popup HighlightPopup = LoadFromEmbeddedResource<Popup>(typeof(CodeEditorBaseDataContext).Assembly, @"ArmA.Studio.Data.UI.CodeEditorBase_HighlightingPopup.xaml");

        public event EventHandler OnLintingInfoUpdated;
        protected virtual bool IsAutoCompleteEnabled => true;


        public ILinterHost Linter => this as ILinterHost;

        public Task WaitTimeoutTask { get; private set; }
        public Task LinterTask { get; private set; }
        public DateTime LastTextChanged { get; private set; }

        private CompletionWindow AutoCompletionWindow;

        public CodeEditorBaseDataContext(ProjectFile fileRef) : base(fileRef)
        {
            this.ShowLineNumbers = true;
        }

        protected override void OnEditorInitialized(TextEditor editor)
        {
            base.OnEditorInitialized(editor);
            editor.Document.TextChanged += this.OnTextChanged;
            editor.MouseHover += this.Editor_MouseHover;
            editor.MouseHoverStopped += this.Editor_MouseHoverStopped;
            this.ExecuteLinter();
        }

        private void Editor_MouseHoverStopped(object sender, MouseEventArgs e)
        {
           HighlightPopup.IsOpen = false;
        }

        private async void Editor_MouseHover(object sender, MouseEventArgs e)
        {
            if (this.Linter == null)
                return;
            var pos = e.GetPosition(this.EditorInstance);
            var textViewPosQ = this.EditorInstance.GetPositionFromPoint(pos);
            if (!textViewPosQ.HasValue)
                return;
            var textViewPos = textViewPosQ.Value;
            var offset = this.Document.GetOffset(textViewPos.Location);
            var info = await this.OnHoverInformationsAsync(offset);
            if (string.IsNullOrWhiteSpace(info))
            {
                var linterInfo = this.Linter.LinterInfo?.FirstOrDefault((li) => li.StartOffset <= offset && li.EndOffset >= offset);
                if (linterInfo == null)
                    return;
                HighlightPopup.DataContext = linterInfo;
            }
            else
            {
                HighlightPopup.DataContext = new LintInfo(null) { Message = info };
            }
            HighlightPopup.PlacementTarget = this.EditorInstance;
            HighlightPopup.Placement = PlacementMode.Relative;
            HighlightPopup.HorizontalOffset = pos.X + 16;
            HighlightPopup.VerticalOffset = pos.Y - 8;
            HighlightPopup.IsOpen = true;
        }

#pragma warning disable CS1998
        public virtual async Task<string> OnHoverInformationsAsync(int offset) { return string.Empty; }
#pragma warning restore CS1998


        //ToDo: Fix error offset moving
        private void ExecuteLinter()
        {
            if (this.LinterTask != null && !this.LinterTask.IsCompleted || this.Linter == null)
                return;
            var memstream = new MemoryStream();
            try
            {
                using (var reader = this.EditorInstance.Document.CreateReader())
                {
                    //Load content into MemoryStream
                    var writer = new StreamWriter(memstream);
                    writer.Write(reader.ReadToEnd());
                    writer.Flush();
                    memstream.Seek(0, SeekOrigin.Begin);
                }
            }
            catch
            {
                memstream.Close();
                memstream.Dispose();
            }
            this.LinterTask = Task.Run(() =>
            {
                using (memstream)
                {
                    this.Linter.LintWriteCache(memstream, this.FileReference);
                    this.OnLintingInfoUpdated?.Invoke(this, new EventArgs());
                    this.RefreshVisuals();
                    Application.Current.Dispatcher.Invoke(() => this.EditorInstance.TextArea.InvalidateVisual());
                }
            });
        }
        protected virtual void OnTextChanged(object sender, EventArgs e)
        {
            //Linting
            this.LastTextChanged = DateTime.Now;
            if (this.Linter != null)
            {
                if (this.WaitTimeoutTask == null || this.WaitTimeoutTask.IsCompleted)
                {
                    this.WaitTimeoutTask = Task.Run(() =>
                    {
                        SpinWait.SpinUntil(() => (DateTime.Now - this.LastTextChanged).TotalMilliseconds > CONST_LINTER_UPDATE_TIMEOUT_MS && (this.LinterTask == null || this.LinterTask.IsCompleted));
                        Application.Current.Dispatcher.Invoke(this.ExecuteLinter);
                    });
                }
            }
        }
        public virtual IEnumerable<ICompletionData> GetAutoCompleteData() { yield break; }
        public void DisplayAutoCompletion()
        {
            if (!this.IsAutoCompleteEnabled || this.AutoCompletionWindow != null)
                return;
            this.AutoCompletionWindow = new CompletionWindow(this.EditorInstance.TextArea);
            this.AutoCompletionWindow.Closed += delegate
            {
                this.AutoCompletionWindow = null;
            };
            foreach (var cd in this.GetAutoCompleteData())
            {
                this.AutoCompletionWindow.CompletionList.CompletionData.Add(cd);
            }
            this.AutoCompletionWindow.StartOffset = this.EditorInstance.GetStartOffset();
            this.AutoCompletionWindow.EndOffset = this.EditorInstance.CaretOffset;
            this.AutoCompletionWindow.CloseAutomatically = true;
            this.AutoCompletionWindow.CompletionList.SelectItem(this.EditorInstance.Document.GetText(this.AutoCompletionWindow.StartOffset, this.AutoCompletionWindow.EndOffset - this.AutoCompletionWindow.StartOffset));
            this.AutoCompletionWindow.Show();
        }
        protected override void OnPreviewKeyDown(out bool handled)
        {
            handled = false;
            if (Keyboard.IsKeyDown(Key.Space) && (Keyboard.Modifiers & ModifierKeys.Control) > 0)
            {
                this.DisplayAutoCompletion();
                handled = true;
            }
        }
        protected override void OnTextEntering(TextCompositionEventArgs e, out bool handled)
        {
            base.OnTextEntering(e, out handled);
            if (e.Text.Length > 0 && this.AutoCompletionWindow != null && e.Text[0] == ' ')
            {
                // Whenever a space is typed while the completion window is open,
                // insert the currently selected element.
                this.AutoCompletionWindow.CompletionList.RequestInsertion(e);
            }
        }
        protected override void OnTextEntered(TextCompositionEventArgs e, out bool handled)
        {
            base.OnTextEntered(e, out handled);
            //IntelliSense
            int startoff = this.EditorInstance.GetStartOffset();
            var txt = this.EditorInstance.Document.GetText(startoff, this.EditorInstance.CaretOffset - startoff);
            if (txt.Length >= 3 && Char.IsLetterOrDigit(e.Text.Last()))
            {
                this.DisplayAutoCompletion();
            }
        }
    }
}
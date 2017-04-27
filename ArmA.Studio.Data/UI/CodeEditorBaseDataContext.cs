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
using ArmA.Studio.Data.IntelliSense;
using ArmA.Studio.Data.Lint;
using ArmA.Studio.Data.UI.Commands;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using Utility;

namespace ArmA.Studio.Data.UI
{
    public class CodeEditorBaseDataContext : TextEditorBaseDataContext
    {
        private sealed class IntelliSenseContainer : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

            public int SelectedIndex { get { return this._SelectedIndex; } set { if (this._SelectedIndex == value) return; this._SelectedIndex = value; this.RaisePropertyChanged(); } }
            private int _SelectedIndex;

            public object SelectedItem { get { return this._SelectedItem; } set { if (this._SelectedItem == value) return; this._SelectedItem = value; this.RaisePropertyChanged(); } }
            private object _SelectedItem;
            private readonly CodeEditorBaseDataContext Owner;

            public IEnumerable<IntelliSenseItem> Items { get; private set; }

            public ICommand CmdMouseDoubleClick => new RelayCommand((p) =>
            {
                if (this.SelectedItem is IntelliSenseItem)
                {
                    ((IntelliSenseItem)this.SelectedItem).Apply(this.Owner.Document);
                    IntelliSensePopup.IsOpen = false;
                }
            });

            public IntelliSenseContainer(CodeEditorBaseDataContext owner, IEnumerable<IntelliSenseItem> items)
            {
                this.Items = items;
                this.Owner = owner;
            }
        }
        protected const int CONST_LINTER_UPDATE_TIMEOUT_MS = 200;
        public static readonly Popup HighlightPopup = LoadFromEmbeddedResource<Popup>(typeof(CodeEditorBaseDataContext).Assembly, @"ArmA.Studio.Data.UI.CodeEditorBase_HighlightingPopup.xaml");
        public static readonly Popup IntelliSensePopup = LoadFromEmbeddedResource<Popup>(typeof(CodeEditorBaseDataContext).Assembly, @"ArmA.Studio.Data.UI.CodeEditorBase_IntelliSensePopup.xaml");

        public event EventHandler OnLintingInfoUpdated;


        public ILinterHost Linter => this as ILinterHost;
        public IIntelliSenseHost IntelliSenseHost => this as IIntelliSenseHost;

        public Task WaitTimeoutTask { get; private set; }
        public Task LinterTask { get; private set; }
        public DateTime LastTextChanged { get; private set; }

        public CodeEditorBaseDataContext(ProjectFile fileRef) : base(fileRef)
        {
            this.ShowLineNumbers = true;
        }

        protected override void OnEditorInitialized(TextEditor editor)
        {
            base.OnEditorInitialized(editor);
            editor.Document.TextChanged += OnTextChanged;
            editor.MouseHover += Editor_MouseHover;
            editor.MouseHoverStopped += Editor_MouseHoverStopped;
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
                var linterInfo = this.Linter.LinterInfo.FirstOrDefault((li) => li.StartOffset <= offset && li.EndOffset >= offset);
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
        private bool IntelliSenseCaretOperation;
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
                        Application.Current.Dispatcher.Invoke(() => this.ExecuteLinter());
                    });
                }
            }

            //IntelliSense
            this.DisplayIntelliSensePopup();
            this.IntelliSenseCaretOperation = true;
        }

        private void DisplayIntelliSensePopup()
        {
            if (this.EditorInstance != null && this.IntelliSenseHost != null)
            {
                this.IntelliSenseHost.GenerateAsync(this.EditorInstance.Document, this.EditorInstance.TextArea.Caret).ContinueWith((t) =>
                {
                    var result = (t as Task<IEnumerable<IntelliSenseItem>>).Result;
                    if (!result.Any())
                        return;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var pos = this.EditorInstance.TextArea.TextView.GetVisualPosition(this.EditorInstance.TextArea.Caret.Position, ICSharpCode.AvalonEdit.Rendering.VisualYPosition.LineBottom) - this.EditorInstance.TextArea.TextView.ScrollOffset;
                        IntelliSensePopup.DataContext = new IntelliSenseContainer(this, result);
                        IntelliSensePopup.PlacementTarget = this.EditorInstance;
                        IntelliSensePopup.Placement = PlacementMode.Relative;
                        IntelliSensePopup.HorizontalOffset = pos.X;
                        IntelliSensePopup.VerticalOffset = pos.Y + 1;
                        IntelliSensePopup.IsOpen = true;
                    });
                });
            }
        }
        protected override void OnCaretPositionChanged(int line, int column, int offset)
        {
            base.OnCaretPositionChanged(line, column, offset);
            if(this.IntelliSenseCaretOperation)
            {
                this.IntelliSenseCaretOperation = false;
            }
            else
            {
                IntelliSensePopup.IsOpen = false;
            }
        }
        protected override void OnPreviewKeyDown(out bool handled)
        {
            handled = false;
            if (Keyboard.IsKeyDown(Key.Space) && (Keyboard.Modifiers & ModifierKeys.Control) > 0)
            {
                this.DisplayIntelliSensePopup();
                handled = true;
            }
            else if (IntelliSensePopup.IsOpen)
            {
                var container = IntelliSensePopup.DataContext as IntelliSenseContainer;
                if (container == null)
                    return;
                if (Keyboard.IsKeyDown(Key.Escape))
                {
                    IntelliSensePopup.IsOpen = false;
                    handled = true;
                }
                else if (Keyboard.IsKeyDown(Key.Down))
                {
                    var cur = container.SelectedItem;
                    container.SelectedIndex++;
                    if (cur == container.SelectedItem)
                        container.SelectedIndex--;
                    handled = true;
                }
                else if (Keyboard.IsKeyDown(Key.Up))
                {
                    container.SelectedIndex--;
                    if (container.SelectedIndex < 0)
                        container.SelectedIndex++;
                    handled = true;
                }
                else if(Keyboard.IsKeyDown(Key.Tab) || Keyboard.IsKeyDown(Key.Enter))
                {
                    if(container.SelectedItem is IntelliSenseItem)
                    {
                        ((IntelliSenseItem)container.SelectedItem).Apply(this.Document);
                        IntelliSensePopup.IsOpen = false;
                        handled = true;
                    }
                }
            }
        }
        protected override void OnLostFocus()
        {
            base.OnLostFocus();
            //ToDo: Improve so that clicking the popup is possible
            IntelliSensePopup.IsOpen = false;
        }
    }
}
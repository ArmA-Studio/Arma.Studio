using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ArmA.Studio.Data.UI.Commands;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;

namespace ArmA.Studio.Data.UI
{
    public class TextEditorBaseDataContext : DocumentBase
    {
        public static readonly DataTemplate TextEditorBaseDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(TextEditorBaseDataContext).Assembly, @"ArmA.Studio.Data.UI.TextEditorBaseDataTemplate.xaml");
        #region `TextEditor` 'EditorInstance' surroundings
        /// <summary>
        /// The actual Editor Instance.
        /// Requires some time to get initialized.
        /// To get an awaiter, use the function.
        /// </summary>
        public TextEditor EditorInstance { get; private set; }

        public TextDocument Document { get { return this._Document; } set { if (this._Document == value) return; this._Document = value; this.RaisePropertyChanged(); } }
        private TextDocument _Document;
        /// <summary>
        /// Async function to receive the initialized instance of the Editor.
        /// </summary>
        /// <returns>Initialized <see cref="TextEditor"/> of this <see cref="TextEditorBaseDataContext"/>.</returns>
        public async Task<TextEditor> GetEditorInstanceAsync() => await Task.Run(() =>
        {
            SpinWait.SpinUntil(() => this.EditorInstance != null);
            return this.EditorInstance;
        });
        #endregion

        #region Notifying properties
        #region ICommand's
        public ICommand CmdTextEditorInitialized => new RelayCommand((p) => { this.EditorInstance = p as TextEditor; this.OnEditorInitialized(this.EditorInstance); });
        public ICommand CmdLostFocus => new RelayCommand((p) => { this.OnLostFocus(); });
       #endregion

        /// <summary>
        /// The <see cref="FontFamily"/> used by the TextEditor.
        /// Notifies when it gets changed.
        /// </summary>
        public FontFamily UsedFontFamily { get { return this._UsedFontFamily; } set { if (this._UsedFontFamily == value) return; this._UsedFontFamily = value; this.RaisePropertyChanged(); } }
        private FontFamily _UsedFontFamily;

        /// <summary>
        /// Wether the TextEditor should display LineNumbers or not.
        /// Notifies when it gets changed.
        /// </summary>
        public bool ShowLineNumbers { get { return this._ShowLineNumbers; } set { if (this._ShowLineNumbers == value) return; this._ShowLineNumbers = value; this.RaisePropertyChanged(); } }
        private bool _ShowLineNumbers;

        /// <summary>
        /// Syntax Highlighting to use by the editor.
        /// Notifies when it gets changed.
        /// </summary>
        public IHighlightingDefinition SyntaxHighlightingDefinition { get { return this._SyntaxHighlightingDefinition; } set { if (this._SyntaxHighlightingDefinition == value) return; this._SyntaxHighlightingDefinition = value; this.RaisePropertyChanged(); } }
        private IHighlightingDefinition _SyntaxHighlightingDefinition;
        #endregion

        public override bool HasChanges { get { if (this.EditorInstance == null) return false; return !this.EditorInstance.Document.UndoStack.IsOriginalFile; } }
        public override string Title => this.HasChanges ? string.Concat(this.IsTemporary ? "tmp" : this.FileReference.FileName, '*') : this.IsTemporary ? "tmp" : this.FileReference.FileName;

        public TextEditorBaseDataContext(ProjectFile fileRef) : base(fileRef)
        {
            this.UsedFontFamily = new FontFamily("Consolas");
            this.ShowLineNumbers = false;
            this.SyntaxHighlightingDefinition = null;
            this.Document = new TextDocument();
        }


        #region Event Callbacks
        private int LastCaretOffset;
        private void Caret_PositionChanged(object sender, EventArgs e)
        {
            if (this.LastCaretOffset == this.EditorInstance.TextArea.Caret.Offset)
                return;
            this.LastCaretOffset = this.EditorInstance.TextArea.Caret.Offset;
            var line = this.EditorInstance.TextArea.Caret.Line;
            var column = this.EditorInstance.TextArea.Caret.Column;
            this.OnCaretPositionChanged(line, column, this.LastCaretOffset);
        }
        private void UndoStack_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsOriginalFile")
            {
                this.RaisePropertyChanged(nameof(this.HasChanges));
                this.RaisePropertyChanged(nameof(this.Title));
            }
        }
        private void Editor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool handled;
            this.OnPreviewKeyDown(out handled);
            if (handled)
            {
                e.Handled = true;
            }
            else
            {
                var checkResult = this.KeyManager?.CheckKeys();
                e.Handled = checkResult.HasValue && checkResult.Value;
            }
        }
        #endregion

        
        /// <summary>
        /// Sets the text displayed and clears the UndoStack.
        /// Use to create "new" documents/insert the content.
        /// Will be executed async.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public void SetText(string text)
        {
            int limit = this.Document.UndoStack.SizeLimit;
            this.Document.UndoStack.SizeLimit = 0;


            this.Document.Text = text;

            limit = this.Document.UndoStack.SizeLimit = limit;
        }

        public override void SaveDocument() => this.SaveDocument(this.FileReference.FilePath);
        public override void SaveDocument(string path)
        {
            this.EditorInstance.Document.UndoStack.MarkAsOriginalFile();

            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (var writer = new StreamWriter(File.Open(path, FileMode.Create)))
            {
                this.EditorInstance.Document.WriteTextTo(writer);
                writer.Flush();
            }
        }
        public override void LoadDocument()
        {
            var path = this.FileReference.FilePath;
            if (!File.Exists(path))
                return;
            using (var reader = new StreamReader(File.OpenRead(path)))
            {
                this.SetText(reader.ReadToEnd());
            }
        }

        #region virtual functions
        /// <summary>
        /// Callen when the <see cref="TextEditor"/> is initialized.
        /// </summary>
        /// <param name="editor">Initialized <see cref="TextEditor"/> instance.</param>
        protected virtual void OnEditorInitialized(TextEditor editor)
        {
            editor.TextArea.Caret.PositionChanged += Caret_PositionChanged;
            editor.Document.UndoStack.PropertyChanged += UndoStack_PropertyChanged;
            editor.PreviewKeyDown += Editor_PreviewKeyDown;
        }



        /// <summary>
        /// Callen whenever the caret position has changed.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <param name="offset"></param>
        protected virtual void OnCaretPositionChanged(int line, int column, int offset) { }
        protected virtual void OnLostFocus() { }

        protected virtual void OnPreviewKeyDown(out bool handled) { handled = false; }

        public override void RefreshVisuals()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.EditorInstance?.InvalidateVisual();
                this.EditorInstance?.TextArea.InvalidateVisual();
                this.EditorInstance?.TextArea.TextView.InvalidateLayer(ICSharpCode.AvalonEdit.Rendering.KnownLayer.Background);
                this.EditorInstance?.TextArea.TextView.InvalidateLayer(ICSharpCode.AvalonEdit.Rendering.KnownLayer.Caret);
                this.EditorInstance?.TextArea.TextView.InvalidateLayer(ICSharpCode.AvalonEdit.Rendering.KnownLayer.Selection);
                this.EditorInstance?.TextArea.TextView.InvalidateLayer(ICSharpCode.AvalonEdit.Rendering.KnownLayer.Text);
            });
        }

        #endregion
    }
}

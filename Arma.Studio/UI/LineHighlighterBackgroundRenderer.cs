using System;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace Arma.Studio.UI
{
    public class LineHighlighterBackgroundRenderer : IBackgroundRenderer
    {
        protected static readonly SolidColorBrush BackgroundFill;
        protected static readonly Pen BorderPen;
        static LineHighlighterBackgroundRenderer()
        {
            BackgroundFill = new SolidColorBrush(Color.FromArgb(0x10, 0x00, 0x00, 0x00));
            BackgroundFill.Freeze();
            var tmpBrush = new SolidColorBrush(Color.FromArgb(0x20, 0x00, 0x00, 0x00));
            tmpBrush.Freeze();
            BorderPen = new Pen(tmpBrush, 1);
            BorderPen.Freeze();
        }

        public TextEditor Editor => this.EditorWeak.TryGetTarget(out var target) ? target : null;
        private readonly WeakReference<TextEditor> EditorWeak;
        public LineHighlighterBackgroundRenderer(TextEditor editor)
        {
            this.EditorWeak = new WeakReference<TextEditor>(editor);
            this.Editor.TextArea.Caret.PositionChanged += this.Caret_PositionChanged;
        }

        private void Caret_PositionChanged(object sender, EventArgs e)
        {
            this.Editor.TextArea.TextView.InvalidateLayer(KnownLayer.Background);
        }

        public KnownLayer Layer => KnownLayer.Background;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (this.Editor.Document == null)
            {
                return;
            }

            textView.EnsureVisualLines();
            
            var line = this.Editor.Document.GetLineByOffset(this.Editor.CaretOffset);
            var segment = new TextSegment { StartOffset = line.Offset, EndOffset = line.EndOffset };
            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, segment))
            {
                drawingContext.DrawRectangle(BackgroundFill, BorderPen, new Rect(rect.Location, new Size(textView.ActualWidth, rect.Height)));
            }
        }
    }
}
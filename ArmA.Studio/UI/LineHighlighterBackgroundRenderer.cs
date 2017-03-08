using System;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace ArmA.Studio.UI
{
    internal class LineHighlighterBackgroundRenderer : IBackgroundRenderer
    {
        private TextEditor Editor;

        public LineHighlighterBackgroundRenderer(TextEditor editor)
        {
            this.Editor = editor;
            this.Editor.TextArea.Caret.PositionChanged += Caret_PositionChanged;
        }

        private void Caret_PositionChanged(object sender, EventArgs e)
        {
            this.Editor.TextArea.TextView.InvalidateLayer(KnownLayer.Background);
        }

        public KnownLayer Layer { get { return KnownLayer.Background; } }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (this.Editor.Document == null)
                return;

            textView.EnsureVisualLines();
            var line = this.Editor.Document.GetLineByOffset(this.Editor.CaretOffset);
            var segment = new TextSegment { StartOffset = line.Offset, EndOffset = line.EndOffset };
            var color = new SolidColorBrush(ConfigHost.Coloring.SelectedLine.Background);
            color.Freeze();
            var pen = new Pen(new SolidColorBrush(ConfigHost.Coloring.SelectedLine.Border), 1);
            pen.Freeze();
            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, segment))
            {
                drawingContext.DrawRectangle(color, pen, new Rect(rect.Location, new Size(textView.ActualWidth, rect.Height)));
            }
        }
    }
}
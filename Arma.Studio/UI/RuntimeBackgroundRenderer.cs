using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace Arma.Studio.UI
{
    public class RuntimeBackgroundRenderer : IBackgroundRenderer
    {
        protected static readonly SolidColorBrush BackgroundFill_Spot;
        protected static readonly Pen BorderPen_Spot;

        protected static readonly SolidColorBrush BackgroundFill_Highlight;
        protected static readonly Pen BorderPen_Highlight;
        static RuntimeBackgroundRenderer()
        {
            BackgroundFill_Spot = new SolidColorBrush(Color.FromArgb(0x60, 0xFF, 0xFF, 0x00));
            BackgroundFill_Spot.Freeze();
            var tmpBrush = new SolidColorBrush(Color.FromArgb(0x80, 0xFF, 0xA5, 0x00));
            tmpBrush.Freeze();
            BorderPen_Spot = new Pen(tmpBrush, 1);
            BorderPen_Spot.Freeze();

            BackgroundFill_Highlight = new SolidColorBrush(Color.FromArgb(0x20, 0xFF, 0xFF, 0x00));
            BackgroundFill_Highlight.Freeze();
            tmpBrush = new SolidColorBrush(Color.FromArgb(0x30, 0xFF, 0xA5, 0x00));
            tmpBrush.Freeze();
            BorderPen_Highlight = new Pen(tmpBrush, 1);
            BorderPen_Highlight.Freeze();
        }

        public TextEditor Editor => this.EditorWeak.TryGetTarget(out var target) ? target : null;
        private readonly WeakReference<TextEditor> EditorWeak;
        public TextEditorDataContext Owner => this.OwnerWeak.TryGetTarget(out var target) ? target : null;
        private readonly WeakReference<TextEditorDataContext> OwnerWeak;
        public RuntimeBackgroundRenderer(TextEditorDataContext owner, TextEditor editor)
        {
            this.EditorWeak = new WeakReference<TextEditor>(editor);
            this.OwnerWeak = new WeakReference<TextEditorDataContext>(owner);
        }

        public KnownLayer Layer => KnownLayer.Background;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (this.Editor.Document == null ||
                App.MWContext.Debugger is null ||
                App.MWContext.Debugger.State == Data.Debugging.EDebugState.Running)
            {
                return;
            }

            // First draw the actual runtime position
            var haltInfo = App.MWContext.Debugger.GetHaltInfos().FirstOrDefault((hi) => hi.File == this.Owner.File.FullPath);
            if (haltInfo == null)
            {
                return;
            }
            textView.EnsureVisualLines();
            var line = this.Editor.Document.GetLineByNumber(haltInfo.Line);
            var restLine = this.Editor.Document.GetText(line.Offset, line.Length - haltInfo.Column);
            var spaceIndex = restLine.IndexOfAny(new char[] { ' ', '\t' });
            var segment = new TextSegment
            {
                StartOffset = line.Offset + haltInfo.Column,
                EndOffset = spaceIndex == -1 ? line.EndOffset : (line.Offset + spaceIndex)
            };
            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, segment))
            {
                drawingContext.DrawRectangle(BackgroundFill_Spot, BorderPen_Spot, rect);
            }
            // Now highlight the line

            line = this.Editor.Document.GetLineByNumber(haltInfo.Line);
            segment = new TextSegment { StartOffset = line.Offset, EndOffset = line.EndOffset };
            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, segment))
            {
                drawingContext.DrawRectangle(BackgroundFill_Highlight, BorderPen_Highlight, new Rect(rect.Location, new Size(textView.ActualWidth, rect.Height)));
            }
        }
    }
}
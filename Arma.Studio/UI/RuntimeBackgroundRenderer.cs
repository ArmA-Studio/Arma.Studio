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
        protected static readonly SolidColorBrush BackgroundFill;
        protected static readonly Pen BorderPen;
        static RuntimeBackgroundRenderer()
        {
            BackgroundFill = new SolidColorBrush(Color.FromArgb(0x60, 0xFF, 0xFF, 0x00));
            BackgroundFill.Freeze();
            var tmpBrush = new SolidColorBrush(Color.FromArgb(0x80, 0xFF, 0xA5, 0x00));
            tmpBrush.Freeze();
            BorderPen = new Pen(tmpBrush, 1);
            BorderPen.Freeze();
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

            // textView.EnsureVisualLines();
            // var line = this.Editor.Document.GetLineByOffset(this.Editor.CaretOffset);
            // var segment = new TextSegment { StartOffset = line.Offset, EndOffset = line.EndOffset };
            // foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, segment))
            // {
            //     drawingContext.DrawRectangle(BackgroundFill, BorderPen, new Rect(rect.Location, new Size(textView.ActualWidth, rect.Height)));
            // }

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
                drawingContext.DrawRectangle(BackgroundFill, BorderPen, rect);
            }
        }
    }
}
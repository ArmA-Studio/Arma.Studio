using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using Arma.Studio.Data;
using Arma.Studio.Data.UI;
using Arma.Studio.Data.Debugging;
using Arma.Studio.Data.TextEditor;

namespace Arma.Studio.UI
{
    public class UnderlineBackgroundRenderer : IBackgroundRenderer
    {
        public TextEditorDataContext Owner => this.OwnerWeak.TryGetTarget(out var target) ? target : null;
        private readonly WeakReference<TextEditorDataContext> OwnerWeak;

        protected static readonly Pen PenError;
        protected static readonly Pen PenWarning;
        protected static readonly Pen PenInfo;

        static UnderlineBackgroundRenderer()
        {
            PenError = new Pen(Brushes.Red, 1);
            PenError.Freeze();
            PenWarning = new Pen(Brushes.Orange, 1);
            PenWarning.Freeze();
            PenInfo = new Pen(Brushes.Green, 1);
            PenInfo.Freeze();
        }

        /// <summary>
        /// Creates a new <see cref="UnderlineBackgroundRenderer"/> instance
        /// with the provided <see cref="TextEditorDataContext"/> as owner;
        /// </summary>
        /// <param name="owner"></param>
        public UnderlineBackgroundRenderer(TextEditorDataContext owner)
        {
            this.OwnerWeak = new WeakReference<TextEditorDataContext>(owner);
        }

        public KnownLayer Layer => KnownLayer.Selection;

        /// <summary>
        /// Helper method to get the points that represent a curly line.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private IEnumerable<Point> GetPoints(Rect rect, double offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Point(rect.BottomLeft.X + (i * offset), rect.BottomLeft.Y - ((i + 1) % 2 == 0 ? offset : 0));
            }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            textView.EnsureVisualLines();
            foreach (var lintInfo in this.Owner.GetLintInfos().Where((it) => textView.Document.LineCount >= it.Line))
            {
                foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, lintInfo.GetSegment(textView.Document)))
                {
                    var geometry = new StreamGeometry();
                    const double vOffset = 2.5;
                    var count = (int)((rect.BottomRight.X - rect.BottomLeft.X) / vOffset) + 1;

                    using (var streamGeo = geometry.Open())
                    {
                        streamGeo.BeginFigure(rect.BottomLeft, false, false);
                        streamGeo.PolyLineTo(this.GetPoints(rect, vOffset, count).ToArray(), true, false);
                    }
                    geometry.Freeze();
                    Pen pen;
                    switch (lintInfo.Severity)
                    {
                        case ESeverity.Error:
                            pen = PenError;
                            break;
                        case ESeverity.Warning:
                            pen = PenWarning;
                            break;
                        case ESeverity.Info:
                            pen = PenInfo;
                            break;
                        default:
#if DEBUG
                            System.Diagnostics.Debugger.Break();
#endif
                            throw new NotImplementedException();
                    }
                    if (geometry != null)
                    {
                        drawingContext.DrawGeometry(null, pen, geometry);
                    }
                }
            }
        }
    }
}
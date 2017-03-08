using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using ArmA.Studio.DataContext;
using ArmA.Studio.DataContext.TextEditorUtil;

namespace ArmA.Studio.UI
{
    internal class UnderlineBackgroundRenderer : IBackgroundRenderer
    {

        public UnderlineBackgroundRenderer()
        {
        }
        public KnownLayer Layer { get { return KnownLayer.Selection; } }

        public IEnumerable<LinterInfo> SyntaxErrors { get; internal set; }
        public int SyntaxErrors_TextLength { get; internal set; }

        private IEnumerable<Point> GetPoints(Rect rect, double offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Point(rect.BottomLeft.X + (i * offset), rect.BottomLeft.Y - ((i + 1) % 2 == 0 ? offset : 0));
            }
        }


        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (this.SyntaxErrors == null)
                return;
            if (this.SyntaxErrors_TextLength != textView.Document.TextLength)
            {
                //ToDo: Update TextLength
                return; //for now just hide the existing syntax linting
            }
            var colorError = new SolidColorBrush(ConfigHost.Coloring.EditorUnderlining.ErrorColor);
            colorError.Freeze();
            var colorWarning = new SolidColorBrush(ConfigHost.Coloring.EditorUnderlining.WarningColor);
            colorWarning.Freeze();
            var colorInfo = new SolidColorBrush(ConfigHost.Coloring.EditorUnderlining.InfoColor);
            colorInfo.Freeze();

            var penError = new Pen(colorError, 1);
            penError.Freeze();
            var penWarning = new Pen(colorWarning, 1);
            penWarning.Freeze();
            var penInfo = new Pen(colorInfo, 1);
            penInfo.Freeze();
            textView.EnsureVisualLines();
            foreach(var segment in this.SyntaxErrors)
            {
                foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, segment))
                {
                    var geometry = new StreamGeometry();
                    const double vOffset = 2.5;
                    var count = (int)((rect.BottomRight.X - rect.BottomLeft.X) / vOffset) + 1;

                    using (var streamGeo = geometry.Open())
                    {
                        streamGeo.BeginFigure(rect.BottomLeft, false, false);
                        streamGeo.PolyLineTo(GetPoints(rect, vOffset, count).ToArray(), true, false);
                    }
                    geometry.Freeze();
                    Pen pen;
                    switch (segment.Severity)
                    {
                        case ESeverity.Error:
                            pen = penError;
                            break;
                        case ESeverity.Warning:
                            pen = penWarning;
                            break;
                        case ESeverity.Info:
                            pen = penInfo;
                            break;
                        default:
                            System.Diagnostics.Debugger.Break();
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
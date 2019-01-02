using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using Arma.Studio.DataContext;
using Arma.Studio.Data.Lint;
using Arma.Studio.Data;
using Arma.Studio.Data.UI;

namespace Arma.Studio.UI
{
    internal class UnderlineBackgroundRenderer : IBackgroundRenderer
    {
        private TextView ThisView;
        private CodeEditorBaseDataContext EditorDataContext;

        public UnderlineBackgroundRenderer(CodeEditorBaseDataContext cebdc)
        {
            this.EditorDataContext = cebdc;
            cebdc.OnLintingInfoUpdated += this.Cebdc_OnLintingInfoUpdated;
        }

        private void Cebdc_OnLintingInfoUpdated(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => this.ThisView?.InvalidateLayer(KnownLayer.Selection));
        }

        public KnownLayer Layer => KnownLayer.Selection;

        public IEnumerable<LintInfo> SyntaxErrors => this.EditorDataContext.Linter.LinterInfo;

        private IEnumerable<Point> GetPoints(Rect rect, double offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Point(rect.BottomLeft.X + (i * offset), rect.BottomLeft.Y - ((i + 1) % 2 == 0 ? offset : 0));
            }
        }


        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            this.ThisView = textView;
            if (this.SyntaxErrors == null)
                return;
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
                        streamGeo.PolyLineTo(this.GetPoints(rect, vOffset, count).ToArray(), true, false);
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
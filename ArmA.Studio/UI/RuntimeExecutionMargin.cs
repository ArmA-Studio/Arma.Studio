using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace ArmA.Studio.UI
{
    public class RuntimeExecutionMargin : AbstractMargin
    {
        public RuntimeExecutionMargin()
        {
            this.IsHitTestVisible = false;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var view = this.TextView;
            if (view == null || !view.VisualLinesValid)
                return;
            if (!Workspace.CurrentWorkspace.DebugContext.IsDebuggerAttached)
                return;
            if (!Workspace.CurrentWorkspace.DebugContext.IsPaused)
                return;
            if (Workspace.CurrentWorkspace.CurrentDocument != Workspace.CurrentWorkspace.DebugContext.CurrentDocument)
                return;
            var color = new SolidColorBrush(ConfigHost.Coloring.ExecutionMarker.MainColor);
            color.Freeze();
            var pen = new Pen(new SolidColorBrush(ConfigHost.Coloring.ExecutionMarker.BorderColor), 1);
            pen.Freeze();

            var line = view.GetVisualLine(Workspace.CurrentWorkspace.DebugContext.CurrentLine);
            if (line == null)
                return;
            var lineTop = line.GetTextLineVisualYPosition(line.TextLines[0], VisualYPosition.TextTop) - view.VerticalOffset;
            var lineBot = line.GetTextLineVisualYPosition(line.TextLines[0], VisualYPosition.TextBottom) - view.VerticalOffset;
            var geo = new StreamGeometry();
            using (var context = geo.Open())
            {
                context.BeginFigure(new Point(2, 5), true, true);
                context.LineTo(new Point(6, 5), true, false);
                context.LineTo(new Point(6, 2), true, false);
                context.LineTo(new Point(10, 6.5), true, false);
                context.LineTo(new Point(6, 11), true, false);
                context.LineTo(new Point(6, 8), true, false);
                context.LineTo(new Point(2, 8), true, false);
            }
            var transgroup = new TransformGroup();
            Transform t;

            t = new TranslateTransform(-14, lineTop - 1);
            t.Freeze();
            transgroup.Children.Add(t);

            t = new ScaleTransform((lineBot - lineTop) / 14, (lineBot - lineTop) / 14);
            t.Freeze();
            transgroup.Children.Add(t);

            transgroup.Freeze();
            geo.Transform = transgroup;
            geo.Freeze();
            
            drawingContext.DrawGeometry(color, pen, geo);
        }
    }
}
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
using ArmA.Studio.Data;
using Utility;

namespace ArmA.Studio.UI
{
    public class BreakPointMargin : AbstractMargin, IBackgroundRenderer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly ProjectFile FileFolderRef;
        public BreakPointMargin(ProjectFile pff)
        {
            this.FileFolderRef = pff;
        }



        protected override void OnTextViewChanged(TextView oldTextView, TextView newTextView)
        {
            if (oldTextView != null)
            {
                newTextView.BackgroundRenderers.Remove(this);
                oldTextView.VisualLinesChanged -= this.TextView_VisualLinesChanged;
            }
            base.OnTextViewChanged(oldTextView, newTextView);
            if (newTextView != null)
            {
                newTextView.BackgroundRenderers.Add(this);
                newTextView.VisualLinesChanged += this.TextView_VisualLinesChanged;
            }
            this.InvalidateVisual();
        }

        private void TextView_VisualLinesChanged(object sender, EventArgs e)
        {
            this.InvalidateVisual();
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            //accept clicks even when clicking on the background
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(18, 0);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (this.FileFolderRef == null)
                return;
            var view = this.TextView;
            if (view == null || !view.VisualLinesValid)
                return;
            drawingContext.DrawRectangle(Brushes.LightGray, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            var colorActive = new SolidColorBrush(ConfigHost.Coloring.BreakPoint.MainColor);
            colorActive.Freeze();
            var colorInactive = new SolidColorBrush(ConfigHost.Coloring.BreakPoint.MainColorInactive);
            colorInactive.Freeze();
            var pen = new Pen(new SolidColorBrush(ConfigHost.Coloring.BreakPoint.BorderColor), 1);
            pen.Freeze();
            foreach (var line in view.VisualLines)
            {
                var lineNumber = this.GetLineNumber(line);

                var bp = Workspace.Instance.BreakpointManager.GetBreakpoint(this.FileFolderRef, lineNumber);
                if (bp.IsDefault())
                {
                    continue;
                }
                var lineTop = line.GetTextLineVisualYPosition(line.TextLines[0], VisualYPosition.TextTop) - view.VerticalOffset;
                var lineBot = line.GetTextLineVisualYPosition(line.TextLines[0], VisualYPosition.TextBottom) - view.VerticalOffset;
                //drawingContext.DrawRoundedRectangle(color, pen, new Rect((18 - 12) / 2, lineTop, 12, 12), 5, 5);
                const double rectSize = 12;

                drawingContext.DrawRectangle(bp.IsEnabled ? colorActive : colorInactive, pen, new Rect((18 - rectSize) / 2, lineTop + (18 - rectSize) / 4, rectSize, rectSize));
                //ToDo: Enable Actions for breakpoints
                //drawingContext.DrawEllipse(bp.IsEnabled ? colorActive : colorInactive, pen, new Point(18 / 2, lineTop + 15 / 2), rectSize, rectSize);
                if (!string.IsNullOrWhiteSpace(bp.SqfCondition))
                {
                    drawingContext.DrawEllipse(null, pen, new Point(18 / 2, lineTop + 15 / 2), rectSize / 4, rectSize / 4);
                }
            }

        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (this.FileFolderRef == null)
                return;
            var view = this.TextView;
            if (view == null || !view.VisualLinesValid)
                return;
            var pos = e.MouseDevice.GetPosition(view);
            var line = this.GetLineFromPoint(view, pos);
            if (line == null)
                return;
            var lineNumber = this.GetLineNumber(this.GetLineFromPoint(view, e.GetPosition(this)));
            var bp = Workspace.Instance.BreakpointManager.GetBreakpoint(this.FileFolderRef, lineNumber);

            if (bp.IsDefault())
            {
                if (e.LeftButton != MouseButtonState.Pressed)
                    return;
                Workspace.Instance.BreakpointManager.SetBreakpoint(this.FileFolderRef, new BreakpointInfo() { Line = lineNumber, IsEnabled = true, FileRef = this.FileFolderRef, SqfCondition = string.Empty });
            }
            else
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Workspace.Instance.BreakpointManager.RemoveBreakpoint(bp);
                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    //ToDo: Create context menu containing basic breakpoint-related operations. Examples: Delete, Disable, Edit

                    var dlgdc = new Dialogs.EditBreakpointDialogDataContext(bp);
                    var dlg = new Dialogs.EditBreakpointDialog(dlgdc);
                    dlg.ShowDialog();
                    var bp2 = dlgdc.GetUpdatedBPI();
                    if (!bp2.Equals(bp))
                    {
                        if (bp.Line != bp2.Line)
                        {
                            Workspace.Instance.BreakpointManager.RemoveBreakpoint(bp);
                        }
                        Workspace.Instance.BreakpointManager.SetBreakpoint(bp2);
                    }

                }

            }
            this.InvalidateVisual();
            this.TextView.InvalidateVisual();
            e.Handled = true;
        }

        public int GetLineNumber(VisualLine line)
        {
            return line.FirstDocumentLine.LineNumber;
        }
        public VisualLine GetLineFromPoint(TextView view, Point p)
        {
            return view.GetVisualLineFromVisualTop(p.Y + view.ScrollOffset.Y);
        }
        public KnownLayer Layer => KnownLayer.Background;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (this.FileFolderRef == null)
                return;
            textView.EnsureVisualLines();
            var color = new SolidColorBrush(ConfigHost.Coloring.BreakPoint.TextHighlightColor);
            color.Freeze();
            foreach (var bp in Workspace.Instance.BreakpointManager[this.FileFolderRef])
            {
                if (!bp.IsEnabled)
                    continue;
                var line = this.Document.GetLineByNumber(bp.Line);
                var segment = new TextSegment { StartOffset = line.Offset, EndOffset = line.EndOffset };
                foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, segment))
                {
                    drawingContext.DrawRectangle(color, null, new Rect(rect.Location, new Size(textView.ActualWidth, rect.Height)));
                }
            }
        }

    }
}
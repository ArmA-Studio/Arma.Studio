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
using Arma.Studio.Data;
using Arma.Studio.Data.Debugging;

namespace Arma.Studio.UI
{
    public class BreakPointMargin : AbstractMargin, IBackgroundRenderer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        protected static readonly SolidColorBrush LineBackgroundFillBrush;
        protected static readonly Pen LineBackgroundBorderPen;
        protected static readonly SolidColorBrush BreakpointFillActiveBrush;
        protected static readonly SolidColorBrush BreakpointFillInactiveBrush;
        protected static readonly Pen BreakpointBorderPen;
        static BreakPointMargin()
        {
            LineBackgroundFillBrush = new SolidColorBrush(Color.FromArgb(0x40, 0x76, 0x2C, 0x2C));
            LineBackgroundFillBrush.Freeze();
            LineBackgroundBorderPen = new Pen(Brushes.White, 1);
            LineBackgroundBorderPen.Freeze();

            BreakpointFillActiveBrush = Brushes.Red;
            BreakpointFillInactiveBrush = new SolidColorBrush(Color.FromArgb(0x40, 0x80, 0x00, 0x00));
            BreakpointFillInactiveBrush.Freeze();
            BreakpointBorderPen = new Pen(Brushes.White, 1);
            BreakpointBorderPen.Freeze();
        }

        public TextEditorDataContext Owner => this.OwnerWeak.TryGetTarget(out var target) ? target : null;
        private readonly WeakReference<TextEditorDataContext> OwnerWeak;

        public BreakPointMargin(TextEditorDataContext owner)
        {
            this.OwnerWeak = new WeakReference<TextEditorDataContext>(owner);
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
            // accept clicks even when clicking on the background
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }

        protected override Size MeasureOverride(Size availableSize) => new Size(18, 0);

        /// <summary>
        /// Renders the actual breakpoint margin and the corresponding
        /// breakpoint "dots"
        /// </summary>
        /// <param name="drawingContext"></param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            const double rectSize = 12;


            var view = this.TextView;
            if (view == null || !view.VisualLinesValid)
            {
                return;
            }
            
            // Draw along the margin
            drawingContext.DrawRectangle(Brushes.LightGray, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            if (this.Owner.File == null)
            {
                return;
            }
            // Draw the different breakpoints
            foreach (var line in view.VisualLines)
            {
                var lineNumber = this.GetLineNumber(line);

                var breakpoints = App.MWContext.BreakpointManager.GetBreakpoints(this.Owner.File, (bp) => bp.Line == lineNumber);
                if (!breakpoints.Any())
                {
                    continue;
                }

                var lineTop = line.GetTextLineVisualYPosition(line.TextLines[0], VisualYPosition.TextTop) - view.VerticalOffset;
                var lineBot = line.GetTextLineVisualYPosition(line.TextLines[0], VisualYPosition.TextBottom) - view.VerticalOffset;

                EBreakpointKind? kind = breakpoints.First().Kind;
                kind = breakpoints.All((bp) => bp.Kind == kind) ? kind : null;
                bool? isEnabled = breakpoints.First().IsActive;
                isEnabled = breakpoints.All((bp) => bp.IsActive) ? (bool?)true : null;

                var brush = !isEnabled.HasValue || isEnabled.Value ? BreakpointFillActiveBrush : BreakpointFillInactiveBrush;
                drawingContext.DrawRectangle(brush, BreakpointBorderPen, new Rect((18 - rectSize) / 2, lineTop + (18 - rectSize) / 4, rectSize, rectSize));

                switch (kind)
                {
                    case EBreakpointKind.Normal:
                        // Nothing
                        break;
                    case EBreakpointKind.Hitcount:
                        // ToDo: Create proper display thingy for this
                        drawingContext.DrawEllipse(Brushes.White, BreakpointBorderPen, new Point(18 / 2, lineTop + 15 / 2), rectSize / 4, rectSize / 4);
                        break;
                }
            }

        }
        /// <summary>
        /// Renders the line background for breakpoints.
        /// </summary>
        /// <param name="textView"></param>
        /// <param name="drawingContext"></param>
        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (this.Owner.File == null)
            {
                return;
            }
            textView.EnsureVisualLines();
            var breakpoints = App.MWContext.BreakpointManager.GetBreakpoints(this.Owner.File, (bp) => true);
            foreach (var bp in breakpoints)
            {
                if (!bp.IsActive)
                {
                    continue;
                }
                if (bp.Line > this.Document.LineCount)
                {
                    continue;
                }
                var line = this.Document.GetLineByNumber(bp.Line);
                var segment = new TextSegment { StartOffset = line.Offset, EndOffset = line.EndOffset };
                foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, segment))
                {
                    drawingContext.DrawRectangle(LineBackgroundFillBrush, LineBackgroundBorderPen, new Rect(rect.Location, new Size(textView.ActualWidth, rect.Height)));
                }
            }
        }


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            var view = this.TextView;
            if (view == null || !view.VisualLinesValid)
            {
                return;
            }
            // Get clicked line
            var pos = e.MouseDevice.GetPosition(view);
            var line = this.GetLineFromPoint(view, pos);
            if (line == null)
            {
                return;
            }
            var lineNumber = this.GetLineNumber(this.GetLineFromPoint(view, e.GetPosition(this)));

            // Get the related breakpoints on provided line
            var breakpoints = App.MWContext.BreakpointManager.GetBreakpoints(this.Owner.File, (bp) => bp.Line == lineNumber);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (breakpoints.Any())
                {
                    App.MWContext.BreakpointManager.RemoveBreakpoints(this.Owner.File, breakpoints);
                }
                else
                {
                    var bp = App.MWContext.BreakpointManager.CreateBreakpoint(this.Owner.File);
                    bp.Line = lineNumber;
                }
            }
            else if (e.RightButton == MouseButtonState.Pressed && breakpoints.Any())
            {
                // ToDo: Breakpoint Context Menu
                // ToDo: Breakpoint Dialog
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


    }
}
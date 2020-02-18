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

namespace Arma.Studio.UI
{
    public class RuntimeExecutionMargin : AbstractMargin
    {
        protected override void ParentLayoutInvalidated(UIElement child)
        {
            base.ParentLayoutInvalidated(child);
            this.InvalidateVisual();
        }
        public TextEditorDataContext Owner => this.OwnerWeak.TryGetTarget(out var target) ? target : null;
        private readonly WeakReference<TextEditorDataContext> OwnerWeak;


        /// <summary>
        /// Creates a new <see cref="RuntimeExecutionMargin"/> instance
        /// with the provided <see cref="TextEditorDataContext"/> as owner;
        /// </summary>
        /// <param name="owner"></param>
        public RuntimeExecutionMargin(TextEditorDataContext owner)
        {
            this.IsHitTestVisible = false;
            this.OwnerWeak = new WeakReference<TextEditorDataContext>(owner);
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.TextView.ScrollOffsetChanged += this.TextView_ScrollOffsetChanged;
        }
        private void TextView_ScrollOffsetChanged(object sender, EventArgs e)
        {
            this.InvalidateVisual();
        }

        /// <summary>
        /// Renders the <see cref="RuntimeExecutionMargin"/> onto provided DrawingContext.
        /// </summary>
        /// <param name="drawingContext">The drawing context to work with</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            var view = this.TextView;
            if (view == null ||
                !view.VisualLinesValid ||
                App.MWContext.Debugger is null ||
                App.MWContext.Debugger.State == Data.Debugging.EDebugState.Running)
            {
                return;
            }

            var color = new SolidColorBrush(Color.FromRgb(0xFF, 0xD8, 0x00));
            color.Freeze();
            var pen = new Pen(Brushes.Black, 1);

            var haltInfo = App.MWContext.Debugger.GetHaltInfos().FirstOrDefault((hi) => hi.File == this.Owner.File.FullPath);
            if (haltInfo == null)
            {
                return;
            }
            var line = view.GetVisualLine(haltInfo.Line);
            if (line == null)
            {
                return;
            }


            // Actual Drawing starts here.
            // Unless something is wrong with the actual arrow geometry (not color),
            // Avoid changing stuff here.
            var lineTop = line.GetTextLineVisualYPosition(line.TextLines[0], VisualYPosition.TextTop) - view.VerticalOffset;
            var lineBot = line.GetTextLineVisualYPosition(line.TextLines[0], VisualYPosition.TextBottom) - view.VerticalOffset;
            var geo = new StreamGeometry();
            using (var context = geo.Open())
            {
                // Simple arrow
                context.BeginFigure(new Point(2, 2), true, true);
                context.LineTo(new Point(6, 2), true, false);
                context.LineTo(new Point(6, 0), true, false);
                context.LineTo(new Point(7, 0), true, false);
                context.LineTo(new Point(10, 3.5), true, false);
                context.LineTo(new Point(7, 7), true, false);
                context.LineTo(new Point(6, 7), true, false);
                context.LineTo(new Point(6, 5), true, false);
                context.LineTo(new Point(2, 5), true, false);

                // Other arrow

                // context.BeginFigure(new Point(6, 2), true, true);
                // context.LineTo(new Point(6, 0), true, false);
                // context.LineTo(new Point(7, 0), true, false);
                // context.LineTo(new Point(10, 3.5), true, false);
                // context.LineTo(new Point(7, 7), true, false);
                // context.LineTo(new Point(6, 7), true, false);
                // context.LineTo(new Point(6, 5), true, false);
                // context.LineTo(new Point(2, 5), true, false);
                // context.BezierTo(new Point(2, 5), new Point(0, 3), new Point(0, 0), true, false);
                // context.BezierTo(new Point(0, 0), new Point(2, 2), new Point(3, 2), true, false);

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
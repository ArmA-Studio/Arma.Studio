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


        private readonly Brush BrushRuntimeArrowSimple;
        private readonly Brush BrushRuntimeArrowOther;
        private readonly Pen PenRuntimeArrowSimple;
        private readonly Pen PenRuntimeArrowOther;
        //private readonly Brush BrushRuntimeArrowOtherNumber;
        //private readonly Pen PenRuntimeArrowOtherNumber;

        /// <summary>
        /// Creates a new <see cref="RuntimeExecutionMargin"/> instance
        /// with the provided <see cref="TextEditorDataContext"/> as owner;
        /// </summary>
        /// <param name="owner"></param>
        public RuntimeExecutionMargin(TextEditorDataContext owner)
        {
            this.BrushRuntimeArrowSimple = new SolidColorBrush(Color.FromRgb(0xFF, 0xD8, 0x00));
            this.BrushRuntimeArrowSimple.Freeze();
            this.PenRuntimeArrowSimple = new Pen(Brushes.Black, 1);
            this.PenRuntimeArrowSimple.Freeze();

            this.BrushRuntimeArrowOther = new SolidColorBrush(Color.FromRgb(0x6F, 0xA8, 0xDC));
            this.BrushRuntimeArrowOther.Opacity = 0.5;
            this.BrushRuntimeArrowOther.Freeze();
            var tmpBrush = Brushes.Black.Clone();
            tmpBrush.Opacity = 0.5;
            tmpBrush.Freeze();
            this.PenRuntimeArrowOther = new Pen(tmpBrush, 1);
            this.PenRuntimeArrowOther.Freeze();

            //this.BrushRuntimeArrowOtherNumber = new SolidColorBrush(Color.FromRgb(0xFF, 0xD8, 0x00));
            //this.BrushRuntimeArrowOtherNumber.Freeze();
            //this.PenRuntimeArrowOtherNumber = new Pen(Brushes.Black, 2);
            //this.PenRuntimeArrowOtherNumber.Freeze();


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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
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

            var haltInfos = App.MWContext.Debugger.GetHaltInfos().ToArray();
            for (int i = 0; i < haltInfos.Length; i++)
            {
                var haltInfo = haltInfos[i];
                if (haltInfo.File != this.Owner.File.FullPath)
                {
                    continue;
                }
                var line = view.GetVisualLine(haltInfo.Line);
                if (line == null)
                {
                    continue;
                }


                // Actual Drawing starts here.
                // Unless something is wrong with the actual arrow geometry (not color),
                // Avoid changing stuff here.
                var lineTop = line.GetTextLineVisualYPosition(line.TextLines[0], VisualYPosition.TextTop) - view.VerticalOffset;
                var lineBot = line.GetTextLineVisualYPosition(line.TextLines[0], VisualYPosition.TextBottom) - view.VerticalOffset;
                var geo = new StreamGeometry();
                using (var context = geo.Open())
                {
                    if (i == 0)
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

                    }
                    else
                    {
                        // Other arrow
                        context.BeginFigure(new Point(6, 2), true, true);
                        context.LineTo(new Point(6, 0), true, false);
                        context.LineTo(new Point(7, 0), true, false);
                        context.LineTo(new Point(10, 3.5), true, false);
                        context.LineTo(new Point(7, 7), true, false);
                        context.LineTo(new Point(6, 7), true, false);
                        context.LineTo(new Point(6, 5), true, false);
                        context.LineTo(new Point(2, 5), true, false);
                        context.BezierTo(new Point(2, 5), new Point(0, 3), new Point(0, 0), true, false);
                        context.BezierTo(new Point(0, 0), new Point(2, 2), new Point(3, 2), true, false);
                    }
                }
                var transgroup = new TransformGroup();
                Transform t;

                t = new TranslateTransform(-14, lineTop + ((lineBot - lineTop - 10)));
                t.Freeze();
                transgroup.Children.Add(t);

                //t = new ScaleTransform((lineBot - lineTop) / 14, (lineBot - lineTop) / 14);
                //t.Freeze();
                //transgroup.Children.Add(t);

                transgroup.Freeze();
                geo.Transform = transgroup;
                geo.Freeze();

                if (i == 0)
                {
                    drawingContext.DrawGeometry(this.BrushRuntimeArrowSimple, this.PenRuntimeArrowSimple, geo);
                }
                else
                {
                    drawingContext.DrawGeometry(this.BrushRuntimeArrowOther, this.PenRuntimeArrowOther, geo);
                    //
                    //var formattedText = new FormattedText(
                    //    i.ToString(),
                    //    System.Globalization.CultureInfo.CurrentUICulture,
                    //    FlowDirection.LeftToRight,
                    //    new Typeface("monospace"),
                    //    8,
                    //    this.BrushRuntimeArrowOtherNumber);
                    //var textGeometry = formattedText.BuildGeometry(new Point(-10, lineTop + 2));
                    //drawingContext.DrawGeometry(null, this.PenRuntimeArrowOtherNumber, textGeometry);
                    //drawingContext.DrawText(formattedText, new Point(-10, lineTop + 2));
                }
            }
        }
    }
}
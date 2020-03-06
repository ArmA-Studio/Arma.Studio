using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Arma.Studio.UiEditor
{
    public static class CanvasExtensions
    {
        public static FrameworkElement GetChildBelowCursor(this Canvas canvas) => GetChildrenBelowCursor(canvas).FirstOrDefault();
        public static IEnumerable<FrameworkElement> GetChildrenBelowCursor(this Canvas canvas)
        {
            var position = Mouse.GetPosition(canvas);
            var childrenInRange = canvas.Children.Cast<FrameworkElement>().Where((it) =>
            {
                var left = Canvas.GetLeft(it);
                var top = Canvas.GetTop(it);
                var right = left + it.ActualWidth;
                var bot = top + it.ActualHeight;
                return position.X >= left && position.X <= right &&
                position.Y >= top && position.Y <= bot;
            });
            return childrenInRange;
        }
        /// <summary>
        /// Gets all children, that are fully inside of the provided <paramref name="rect"/> on the
        /// given <paramref name="canvas"/>.
        /// </summary>
        /// <param name="canvas">The <see cref="Canvas"/> to get the elements from.</param>
        /// <param name="rect">A <see cref="Rect"/> defining the area.</param>
        /// <returns>A list of children, that are fully inside the defined area.</returns>
        public static IEnumerable<FrameworkElement> GetChildrenInside(this Canvas canvas, Rect rect)
        {
            var childrenInRange = canvas.Children.Cast<FrameworkElement>().Where((it) =>
            {
                double left = Canvas.GetLeft(it);
                double top = Canvas.GetTop(it);
                double right = left + it.ActualWidth;
                double bot = top + it.ActualHeight;
                return rect.Right >= right && rect.Left <= left &&
                rect.Bottom >= bot && rect.Top <= top;
            });
            return childrenInRange.OrderByDescending((it) => Canvas.GetZIndex(it));
        }
        /// <summary>
        /// Gets all children, that are at least partially inside of the provided <paramref name="rect"/> on the
        /// given <paramref name="canvas"/>.
        /// </summary>
        /// <param name="canvas">The <see cref="Canvas"/> to get the elements from.</param>
        /// <param name="rect">A <see cref="Rect"/> defining the area.</param>
        /// <returns>A list of children, that are at least partially inside the defined area.</returns>
        public static IEnumerable<FrameworkElement> GetChildrenInsideTouching(this Canvas canvas, Rect rect)
        {
            var childrenInRange = canvas.Children.Cast<FrameworkElement>().Where((it) =>
            {
                double left = Canvas.GetLeft(it);
                double top = Canvas.GetTop(it);
                double right = left + it.ActualWidth;
                double bot = top + it.ActualHeight;
                return ((left >= rect.Left && left <= rect.Right) || (right >= rect.Left && right <= rect.Right) || (left <= rect.Left && right >= rect.Right)) &&
                       ((top >= rect.Top && top <= rect.Bottom) || (bot >= rect.Top && bot <= rect.Bottom) || (top <= rect.Top && bot >= rect.Bottom));
            });
            return childrenInRange.OrderByDescending((it) => Canvas.GetZIndex(it));
        }
        /// <summary>
        /// Saves the visual contents inside of the provided Canvas as image to disk.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="fileName"></param>
        /// <param name="targetDpi"></param>
        /// <exception cref="ArgumentNullException">Thrown if any of the provided arguments is null.</exception>
        public static void SaveImage(this Canvas canvas,
            string fileName,
            double scale,
            double targetDpi,
            BitmapEncoder encoder,
            System.Windows.Media.PixelFormat pixelFormat)
        {
            if (canvas is null) { throw new ArgumentNullException(nameof(canvas)); }
            if (fileName is null) { throw new ArgumentNullException(nameof(fileName)); }
            if (encoder is null) { throw new ArgumentNullException(nameof(encoder)); }
            var screenDpi = (PresentationSource.FromVisual(canvas)?.CompositionTarget.TransformToDevice.M22 ?? 1) * 96;

            canvas.InvalidateVisual();
            canvas.UpdateLayout();
            var bounds = System.Windows.Media.VisualTreeHelper.GetDescendantBounds(canvas);
            var rtb = new RenderTargetBitmap((int)((bounds.Width / screenDpi * targetDpi) * scale), (int)((bounds.Height / screenDpi * targetDpi) * scale), targetDpi, targetDpi, pixelFormat);
            var visual = new System.Windows.Media.DrawingVisual();
            using (var context = visual.RenderOpen())
            {
                var vb = new System.Windows.Media.VisualBrush(canvas);
                context.DrawRectangle(vb, null, new Rect(0, 0, bounds.Size.Width * scale, bounds.Size.Height * scale));
            }

            rtb.Render(visual);

            encoder.Frames.Add(BitmapFrame.Create(rtb));
            using (var file = System.IO.File.Open(fileName, System.IO.FileMode.Create))
            {
                encoder.Save(file);
            }
        }
    }
}

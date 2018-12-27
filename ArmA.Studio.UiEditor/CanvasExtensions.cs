using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArmA.Studio.UiEditor
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
    }
}

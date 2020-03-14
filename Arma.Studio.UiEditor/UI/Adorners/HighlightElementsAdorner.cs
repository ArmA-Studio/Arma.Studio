using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Arma.Studio.UiEditor.UI.Adorners
{
    public class HighlightElementsAdorner : Adorner
    {

        private static SolidColorBrush BorderBrush { get; }
        private static Pen BorderPen { get; }
        static HighlightElementsAdorner()
        {
            BorderBrush = Brushes.Black;
            BorderPen = new Pen(BorderBrush, 1);
            BorderPen.Freeze();
        }
        public HighlightElementsAdorner(UIElement adornedElement) : base(adornedElement)
        {
            this.IsHitTestVisible = false;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            System.Windows.Data.BindingOperations.SetBinding(this, Adorner.CursorProperty, new System.Windows.Data.Binding("Cursor")
            {
                Source = this.AdornedElement
            });
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var rect = new Rect(this.AdornedElement.RenderSize);
            drawingContext.DrawRectangle(null, BorderPen, rect);
            drawingContext.DrawLine(BorderPen, rect.TopLeft, rect.BottomRight);
            drawingContext.DrawLine(BorderPen, rect.BottomLeft, rect.TopRight);
        }


        #region DependencyProperty: Attached
        public static readonly DependencyProperty AttachedProperty =
           DependencyProperty.RegisterAttached(
              "Attached",
              typeof(bool),
              typeof(HighlightElementsAdorner),
              new FrameworkPropertyMetadata(false, PropertyChangedCallback));
        public static void SetAttached(DependencyObject element, bool value)
        {
            element.SetValue(AttachedProperty, value);
        }

        public static bool GetAttached(DependencyObject element)
        {
            return (bool)element.GetValue(AttachedProperty);
        }
        private static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = sender as UIElement;
            if (e.OldValue is bool oldFlag && oldFlag)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(uiElement);
                if (adornerLayer != null)
                {
                    var adorners = adornerLayer.GetAdorners(uiElement);
                    var affected = adorners.FirstOrDefault((it) => it is HighlightElementsAdorner);
                    if (affected != null)
                    {
                        adornerLayer.Remove(affected);
                    }
                }
            }
            if (e.NewValue is bool newFlag && newFlag)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(uiElement);
                if (adornerLayer is null)
                {
                    return;
                }
                var affected = new HighlightElementsAdorner(uiElement);
                adornerLayer.Add(affected);
            }
        }
        #endregion


    }
}

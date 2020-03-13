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
    public class SelectedAdorner : Adorner
    {

        private static SolidColorBrush RectangleFill { get; }
        private static SolidColorBrush BorderBrush { get; }
        private static Pen BorderPen { get; }
        static SelectedAdorner()
        {
            RectangleFill = new SolidColorBrush(Color.FromArgb(0x08, 0x01, 0x9C, 0xFF));
            RectangleFill.Freeze();
            BorderBrush = new SolidColorBrush(Color.FromRgb(0x01, 0x9C, 0xFF));
            BorderBrush.Freeze();
            BorderPen = new Pen(BorderBrush, 1);
            BorderPen.Freeze();
        }
        public SelectedAdorner(UIElement adornedElement) : base(adornedElement)
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
            drawingContext.DrawRectangle(RectangleFill, BorderPen, rect);
        }


        #region DependencyProperty: Attached
        public static readonly DependencyProperty AttachedProperty =
           DependencyProperty.RegisterAttached(
              "Attached",
              typeof(bool),
              typeof(SelectedAdorner),
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
                var adorners = adornerLayer.GetAdorners(uiElement);
                var affected = adorners.FirstOrDefault((it) => it is SelectedAdorner);
                if (affected != null)
                {
                    adornerLayer.Remove(affected);
                }
            }
            if (e.NewValue is bool newFlag && newFlag)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(uiElement);
                var affected = new SelectedAdorner(uiElement);
                adornerLayer.Add(affected);
            }
        }
        #endregion


    }
}

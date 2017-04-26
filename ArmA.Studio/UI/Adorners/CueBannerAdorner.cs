using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ArmA.Studio.UI.Adorners
{
    public class CueBannerAdorner : Adorner
    {
        public const int CONST_MARGIN_MIN_OFFSET_LEFT = 2;
        public const int CONST_MARGIN_MIN_OFFSET_TOP = 0;
        public const int CONST_MARGIN_MIN_OFFSET_RIGHT = 0;
        public const int CONST_MARGIN_MIN_OFFSET_BOTTOM = 0;
        public ContentPresenter Presenter { get; private set; }
        public CueBannerAdorner(UIElement adornedElement, object cueBanner) : base(adornedElement)
        {
            this.IsHitTestVisible = false;
            this.Presenter = new ContentPresenter();
            if (cueBanner is string)
            {
                var tb = new TextBlock();
                tb.Text = cueBanner as string;
                this.Presenter.Content = tb;
            }
            else
            {
                this.Presenter.Content = cueBanner;
            }
            this.Opacity = 0.25;

            var aCtrl = this.AdornedElement as Control;
            this.Margin = new Thickness(
                aCtrl.Padding.Left < CONST_MARGIN_MIN_OFFSET_LEFT ? CONST_MARGIN_MIN_OFFSET_LEFT : aCtrl.Padding.Left,
                aCtrl.Padding.Top < CONST_MARGIN_MIN_OFFSET_TOP ? CONST_MARGIN_MIN_OFFSET_TOP : aCtrl.Padding.Top,
                aCtrl.Padding.Right < CONST_MARGIN_MIN_OFFSET_RIGHT ? CONST_MARGIN_MIN_OFFSET_RIGHT : aCtrl.Padding.Right,
                aCtrl.Padding.Bottom < CONST_MARGIN_MIN_OFFSET_BOTTOM ? CONST_MARGIN_MIN_OFFSET_BOTTOM : aCtrl.Padding.Bottom
            );
        }
        protected override Size MeasureOverride(Size constraint)
        {
            this.Presenter.Measure((this.AdornedElement as Control).RenderSize);
            return (this.AdornedElement as Control).RenderSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.Presenter.Arrange(new Rect(finalSize));
            return finalSize;
        }
        protected override Visual GetVisualChild(int index)
        {
            return this.Presenter;
        }
        protected override int VisualChildrenCount { get { return 1; } }

    }
}

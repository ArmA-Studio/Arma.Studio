using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace Arma.Studio.Data.UI
{
    [ContentProperty(nameof(PopupContainer.Content))]
    public class PopupContainer : Control
    {
        static PopupContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupContainer), new FrameworkPropertyMetadata(typeof(PopupContainer)));
        }

        #region DependencyProperty: Content (System.Object)
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
                nameof(Content), typeof(object), typeof(PopupContainer));
        public object Content { get => this.GetValue(ContentProperty); set => this.SetValue(ContentProperty, value); }
        #endregion
        #region DependencyProperty: Placement (System.Windows.Controls.Primitives.PlacementMode)
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
                nameof(Placement), typeof(PlacementMode), typeof(PopupContainer));
        public PlacementMode Placement { get => (PlacementMode)this.GetValue(PlacementProperty); set => this.SetValue(PlacementProperty, value); }
        #endregion
        #region DependencyProperty: PopupContent (System.Object)
        public static readonly DependencyProperty PopupContentProperty = DependencyProperty.Register(
                nameof(PopupContent), typeof(object), typeof(PopupContainer));
        public object PopupContent { get => this.GetValue(PopupContentProperty); set => this.SetValue(PopupContentProperty, value); }
        #endregion
        #region DependencyProperty: PopupContentTemplate (System.Windows.DataTemplate)
        public static readonly DependencyProperty PopupContentTemplateProperty = DependencyProperty.Register(
                nameof(PopupContentTemplate), typeof(DataTemplate), typeof(PopupContainer));
        public DataTemplate PopupContentTemplate { get => (DataTemplate)this.GetValue(PopupContentTemplateProperty); set => this.SetValue(PopupContentTemplateProperty, value); }
        #endregion
        #region DependencyProperty: PopupCanOpen (System.Boolean)
        public static readonly DependencyProperty PopupCanOpenProperty = DependencyProperty.Register(
                nameof(PopupCanOpen), typeof(bool), typeof(PopupContainer), new PropertyMetadata(true));
        public bool PopupCanOpen { get => (bool)this.GetValue(PopupCanOpenProperty); set => this.SetValue(PopupCanOpenProperty, value); }
        #endregion
        #region RoutedEvent: PopupOpened
        public static readonly RoutedEvent PopupOpenedEvent = EventManager.RegisterRoutedEvent(nameof(PopupOpened), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PopupContainer));
        public event RoutedEventHandler PopupOpened { add { this.AddHandler(PopupOpenedEvent, value); } remove { this.RemoveHandler(PopupOpenedEvent, value); } }
        protected void RaisePopupOpened() { this.RaiseEvent(new RoutedEventArgs(PopupOpenedEvent, this)); }
        #endregion

        public override void OnApplyTemplate()
        {
            if (this.GetTemplateChild("PART_Popup") is Popup PART_Popup)
            {
                PART_Popup.Opened += this.PART_Popup_Opened;
                PART_Popup.PreviewMouseDown += this.PART_Popup_PreviewMouseDown;
            }
        }

        private void PART_Popup_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.Activate();
        }

        private void PART_Popup_Opened(object sender, EventArgs e)
        {
            this.RaisePopupOpened();
        }
    }
}

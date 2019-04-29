
/*
	To get this updated properly with local items,
	Compile once in debug, save the TT, compile again in whatever you want.

	Due to how TT work, this is required.
*/

using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
namespace Arma.Studio.Data.UI.AttachedProperties
{
	public partial interface IOnPreviewMouseDoubleClick { void OnPreviewMouseDoubleClick(Control sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnMouseDoubleClick { void OnMouseDoubleClick(Control sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnTargetUpdated { void OnTargetUpdated(FrameworkElement sender, System.Windows.Data.DataTransferEventArgs e); }
	public partial interface IOnSourceUpdated { void OnSourceUpdated(FrameworkElement sender, System.Windows.Data.DataTransferEventArgs e); }
	public partial interface IOnDataContextChanged { void OnDataContextChanged(FrameworkElement sender, System.Windows.DependencyPropertyChangedEventArgs e); }
	public partial interface IOnRequestBringIntoView { void OnRequestBringIntoView(FrameworkElement sender, System.Windows.RequestBringIntoViewEventArgs e); }
	public partial interface IOnSizeChanged { void OnSizeChanged(FrameworkElement sender, System.Windows.SizeChangedEventArgs e); }
	public partial interface IOnInitialized { void OnInitialized(FrameworkElement sender, System.EventArgs e); }
	public partial interface IOnLoaded { void OnLoaded(FrameworkElement sender, System.Windows.RoutedEventArgs e); }
	public partial interface IOnUnloaded { void OnUnloaded(FrameworkElement sender, System.Windows.RoutedEventArgs e); }
	public partial interface IOnToolTipOpening { void OnToolTipOpening(FrameworkElement sender, System.Windows.Controls.ToolTipEventArgs e); }
	public partial interface IOnToolTipClosing { void OnToolTipClosing(FrameworkElement sender, System.Windows.Controls.ToolTipEventArgs e); }
	public partial interface IOnContextMenuOpening { void OnContextMenuOpening(FrameworkElement sender, System.Windows.Controls.ContextMenuEventArgs e); }
	public partial interface IOnContextMenuClosing { void OnContextMenuClosing(FrameworkElement sender, System.Windows.Controls.ContextMenuEventArgs e); }
	public partial interface IOnPreviewMouseDown { void OnPreviewMouseDown(UIElement sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnMouseDown { void OnMouseDown(UIElement sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnPreviewMouseUp { void OnPreviewMouseUp(UIElement sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnMouseUp { void OnMouseUp(UIElement sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnPreviewMouseLeftButtonDown { void OnPreviewMouseLeftButtonDown(UIElement sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnMouseLeftButtonDown { void OnMouseLeftButtonDown(UIElement sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnPreviewMouseLeftButtonUp { void OnPreviewMouseLeftButtonUp(UIElement sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnMouseLeftButtonUp { void OnMouseLeftButtonUp(UIElement sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnPreviewMouseRightButtonDown { void OnPreviewMouseRightButtonDown(UIElement sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnMouseRightButtonDown { void OnMouseRightButtonDown(UIElement sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnPreviewMouseRightButtonUp { void OnPreviewMouseRightButtonUp(UIElement sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnMouseRightButtonUp { void OnMouseRightButtonUp(UIElement sender, System.Windows.Input.MouseButtonEventArgs e); }
	public partial interface IOnPreviewMouseMove { void OnPreviewMouseMove(UIElement sender, System.Windows.Input.MouseEventArgs e); }
	public partial interface IOnMouseMove { void OnMouseMove(UIElement sender, System.Windows.Input.MouseEventArgs e); }
	public partial interface IOnPreviewMouseWheel { void OnPreviewMouseWheel(UIElement sender, System.Windows.Input.MouseWheelEventArgs e); }
	public partial interface IOnMouseWheel { void OnMouseWheel(UIElement sender, System.Windows.Input.MouseWheelEventArgs e); }
	public partial interface IOnMouseEnter { void OnMouseEnter(UIElement sender, System.Windows.Input.MouseEventArgs e); }
	public partial interface IOnMouseLeave { void OnMouseLeave(UIElement sender, System.Windows.Input.MouseEventArgs e); }
	public partial interface IOnGotMouseCapture { void OnGotMouseCapture(UIElement sender, System.Windows.Input.MouseEventArgs e); }
	public partial interface IOnLostMouseCapture { void OnLostMouseCapture(UIElement sender, System.Windows.Input.MouseEventArgs e); }
	public partial interface IOnQueryCursor { void OnQueryCursor(UIElement sender, System.Windows.Input.QueryCursorEventArgs e); }
	public partial interface IOnPreviewStylusDown { void OnPreviewStylusDown(UIElement sender, System.Windows.Input.StylusDownEventArgs e); }
	public partial interface IOnStylusDown { void OnStylusDown(UIElement sender, System.Windows.Input.StylusDownEventArgs e); }
	public partial interface IOnPreviewStylusUp { void OnPreviewStylusUp(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnStylusUp { void OnStylusUp(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnPreviewStylusMove { void OnPreviewStylusMove(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnStylusMove { void OnStylusMove(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnPreviewStylusInAirMove { void OnPreviewStylusInAirMove(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnStylusInAirMove { void OnStylusInAirMove(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnStylusEnter { void OnStylusEnter(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnStylusLeave { void OnStylusLeave(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnPreviewStylusInRange { void OnPreviewStylusInRange(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnStylusInRange { void OnStylusInRange(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnPreviewStylusOutOfRange { void OnPreviewStylusOutOfRange(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnStylusOutOfRange { void OnStylusOutOfRange(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnPreviewStylusSystemGesture { void OnPreviewStylusSystemGesture(UIElement sender, System.Windows.Input.StylusSystemGestureEventArgs e); }
	public partial interface IOnStylusSystemGesture { void OnStylusSystemGesture(UIElement sender, System.Windows.Input.StylusSystemGestureEventArgs e); }
	public partial interface IOnGotStylusCapture { void OnGotStylusCapture(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnLostStylusCapture { void OnLostStylusCapture(UIElement sender, System.Windows.Input.StylusEventArgs e); }
	public partial interface IOnStylusButtonDown { void OnStylusButtonDown(UIElement sender, System.Windows.Input.StylusButtonEventArgs e); }
	public partial interface IOnStylusButtonUp { void OnStylusButtonUp(UIElement sender, System.Windows.Input.StylusButtonEventArgs e); }
	public partial interface IOnPreviewStylusButtonDown { void OnPreviewStylusButtonDown(UIElement sender, System.Windows.Input.StylusButtonEventArgs e); }
	public partial interface IOnPreviewStylusButtonUp { void OnPreviewStylusButtonUp(UIElement sender, System.Windows.Input.StylusButtonEventArgs e); }
	public partial interface IOnPreviewKeyDown { void OnPreviewKeyDown(UIElement sender, System.Windows.Input.KeyEventArgs e); }
	public partial interface IOnKeyDown { void OnKeyDown(UIElement sender, System.Windows.Input.KeyEventArgs e); }
	public partial interface IOnPreviewKeyUp { void OnPreviewKeyUp(UIElement sender, System.Windows.Input.KeyEventArgs e); }
	public partial interface IOnKeyUp { void OnKeyUp(UIElement sender, System.Windows.Input.KeyEventArgs e); }
	public partial interface IOnPreviewGotKeyboardFocus { void OnPreviewGotKeyboardFocus(UIElement sender, System.Windows.Input.KeyboardFocusChangedEventArgs e); }
	public partial interface IOnGotKeyboardFocus { void OnGotKeyboardFocus(UIElement sender, System.Windows.Input.KeyboardFocusChangedEventArgs e); }
	public partial interface IOnPreviewLostKeyboardFocus { void OnPreviewLostKeyboardFocus(UIElement sender, System.Windows.Input.KeyboardFocusChangedEventArgs e); }
	public partial interface IOnLostKeyboardFocus { void OnLostKeyboardFocus(UIElement sender, System.Windows.Input.KeyboardFocusChangedEventArgs e); }
	public partial interface IOnPreviewTextInput { void OnPreviewTextInput(UIElement sender, System.Windows.Input.TextCompositionEventArgs e); }
	public partial interface IOnTextInput { void OnTextInput(UIElement sender, System.Windows.Input.TextCompositionEventArgs e); }
	public partial interface IOnPreviewQueryContinueDrag { void OnPreviewQueryContinueDrag(UIElement sender, System.Windows.QueryContinueDragEventArgs e); }
	public partial interface IOnQueryContinueDrag { void OnQueryContinueDrag(UIElement sender, System.Windows.QueryContinueDragEventArgs e); }
	public partial interface IOnPreviewGiveFeedback { void OnPreviewGiveFeedback(UIElement sender, System.Windows.GiveFeedbackEventArgs e); }
	public partial interface IOnGiveFeedback { void OnGiveFeedback(UIElement sender, System.Windows.GiveFeedbackEventArgs e); }
	public partial interface IOnPreviewDragEnter { void OnPreviewDragEnter(UIElement sender, System.Windows.DragEventArgs e); }
	public partial interface IOnDragEnter { void OnDragEnter(UIElement sender, System.Windows.DragEventArgs e); }
	public partial interface IOnPreviewDragOver { void OnPreviewDragOver(UIElement sender, System.Windows.DragEventArgs e); }
	public partial interface IOnDragOver { void OnDragOver(UIElement sender, System.Windows.DragEventArgs e); }
	public partial interface IOnPreviewDragLeave { void OnPreviewDragLeave(UIElement sender, System.Windows.DragEventArgs e); }
	public partial interface IOnDragLeave { void OnDragLeave(UIElement sender, System.Windows.DragEventArgs e); }
	public partial interface IOnPreviewDrop { void OnPreviewDrop(UIElement sender, System.Windows.DragEventArgs e); }
	public partial interface IOnDrop { void OnDrop(UIElement sender, System.Windows.DragEventArgs e); }
	public partial interface IOnPreviewTouchDown { void OnPreviewTouchDown(UIElement sender, System.Windows.Input.TouchEventArgs e); }
	public partial interface IOnTouchDown { void OnTouchDown(UIElement sender, System.Windows.Input.TouchEventArgs e); }
	public partial interface IOnPreviewTouchMove { void OnPreviewTouchMove(UIElement sender, System.Windows.Input.TouchEventArgs e); }
	public partial interface IOnTouchMove { void OnTouchMove(UIElement sender, System.Windows.Input.TouchEventArgs e); }
	public partial interface IOnPreviewTouchUp { void OnPreviewTouchUp(UIElement sender, System.Windows.Input.TouchEventArgs e); }
	public partial interface IOnTouchUp { void OnTouchUp(UIElement sender, System.Windows.Input.TouchEventArgs e); }
	public partial interface IOnGotTouchCapture { void OnGotTouchCapture(UIElement sender, System.Windows.Input.TouchEventArgs e); }
	public partial interface IOnLostTouchCapture { void OnLostTouchCapture(UIElement sender, System.Windows.Input.TouchEventArgs e); }
	public partial interface IOnTouchEnter { void OnTouchEnter(UIElement sender, System.Windows.Input.TouchEventArgs e); }
	public partial interface IOnTouchLeave { void OnTouchLeave(UIElement sender, System.Windows.Input.TouchEventArgs e); }
	public partial interface IOnIsMouseDirectlyOverChanged { void OnIsMouseDirectlyOverChanged(UIElement sender, System.Windows.DependencyPropertyChangedEventArgs e); }
	public partial interface IOnIsKeyboardFocusWithinChanged { void OnIsKeyboardFocusWithinChanged(UIElement sender, System.Windows.DependencyPropertyChangedEventArgs e); }
	public partial interface IOnIsMouseCapturedChanged { void OnIsMouseCapturedChanged(UIElement sender, System.Windows.DependencyPropertyChangedEventArgs e); }
	public partial interface IOnIsMouseCaptureWithinChanged { void OnIsMouseCaptureWithinChanged(UIElement sender, System.Windows.DependencyPropertyChangedEventArgs e); }
	public partial interface IOnIsStylusDirectlyOverChanged { void OnIsStylusDirectlyOverChanged(UIElement sender, System.Windows.DependencyPropertyChangedEventArgs e); }
	public partial interface IOnIsStylusCapturedChanged { void OnIsStylusCapturedChanged(UIElement sender, System.Windows.DependencyPropertyChangedEventArgs e); }
	public partial interface IOnIsStylusCaptureWithinChanged { void OnIsStylusCaptureWithinChanged(UIElement sender, System.Windows.DependencyPropertyChangedEventArgs e); }
	public partial interface IOnIsKeyboardFocusedChanged { void OnIsKeyboardFocusedChanged(UIElement sender, System.Windows.DependencyPropertyChangedEventArgs e); }
	public partial interface IOnLayoutUpdated { void OnLayoutUpdated(UIElement sender, System.EventArgs e); }
	public partial interface IOnGotFocus { void OnGotFocus(UIElement sender, System.Windows.RoutedEventArgs e); }
	public partial interface IOnLostFocus { void OnLostFocus(UIElement sender, System.Windows.RoutedEventArgs e); }
	public partial interface IOnIsEnabledChanged { void OnIsEnabledChanged(UIElement sender, System.Windows.DependencyPropertyChangedEventArgs e); }
	public partial interface IOnIsHitTestVisibleChanged { void OnIsHitTestVisibleChanged(UIElement sender, System.Windows.DependencyPropertyChangedEventArgs e); }
	public partial interface IOnIsVisibleChanged { void OnIsVisibleChanged(UIElement sender, System.Windows.DependencyPropertyChangedEventArgs e); }
	public partial interface IOnFocusableChanged { void OnFocusableChanged(UIElement sender, System.Windows.DependencyPropertyChangedEventArgs e); }
	public partial interface IOnManipulationStarting { void OnManipulationStarting(UIElement sender, System.Windows.Input.ManipulationStartingEventArgs e); }
	public partial interface IOnManipulationStarted { void OnManipulationStarted(UIElement sender, System.Windows.Input.ManipulationStartedEventArgs e); }
	public partial interface IOnManipulationDelta { void OnManipulationDelta(UIElement sender, System.Windows.Input.ManipulationDeltaEventArgs e); }
	public partial interface IOnManipulationInertiaStarting { void OnManipulationInertiaStarting(UIElement sender, System.Windows.Input.ManipulationInertiaStartingEventArgs e); }
	public partial interface IOnManipulationBoundaryFeedback { void OnManipulationBoundaryFeedback(UIElement sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e); }
	public partial interface IOnManipulationCompleted { void OnManipulationCompleted(UIElement sender, System.Windows.Input.ManipulationCompletedEventArgs e); }
	public partial interface IOnTextChanged { void OnTextChanged(TextBoxBase sender, System.Windows.Controls.TextChangedEventArgs e); }
	public partial interface IOnSelectionChanged { void OnSelectionChanged(TextBoxBase sender, System.Windows.RoutedEventArgs e); }
	public partial interface IOnSourceInitialized { void OnSourceInitialized(Window sender, System.EventArgs e); }
	public partial interface IOnActivated { void OnActivated(Window sender, System.EventArgs e); }
	public partial interface IOnDeactivated { void OnDeactivated(Window sender, System.EventArgs e); }
	public partial interface IOnStateChanged { void OnStateChanged(Window sender, System.EventArgs e); }
	public partial interface IOnLocationChanged { void OnLocationChanged(Window sender, System.EventArgs e); }
	public partial interface IOnClosing { void OnClosing(Window sender, System.ComponentModel.CancelEventArgs e); }
	public partial interface IOnClosed { void OnClosed(Window sender, System.EventArgs e); }
	public partial interface IOnContentRendered { void OnContentRendered(Window sender, System.EventArgs e); }
	public partial interface IOnClick { void OnClick(MenuItem sender, System.Windows.RoutedEventArgs e); }
	public partial interface IOnChecked { void OnChecked(MenuItem sender, System.Windows.RoutedEventArgs e); }
	public partial interface IOnUnchecked { void OnUnchecked(MenuItem sender, System.Windows.RoutedEventArgs e); }
	public partial interface IOnSubmenuOpened { void OnSubmenuOpened(MenuItem sender, System.Windows.RoutedEventArgs e); }
	public partial interface IOnSubmenuClosed { void OnSubmenuClosed(MenuItem sender, System.Windows.RoutedEventArgs e); }
	public partial interface IOnDropDownOpened { void OnDropDownOpened(ComboBox sender, System.EventArgs e); }
	public partial interface IOnDropDownClosed { void OnDropDownClosed(ComboBox sender, System.EventArgs e); }
	public partial interface IOnSelectionChanged { void OnSelectionChanged(Selector sender, System.Windows.Controls.SelectionChangedEventArgs e); }
	public partial interface IOnSelectedItemChanged { void OnSelectedItemChanged(TreeView sender, System.Windows.RoutedPropertyChangedEventArgs<System.Object> e); }
	public partial interface IOnScrollChanged { void OnScrollChanged(ScrollViewer sender, System.Windows.Controls.ScrollChangedEventArgs e); }
	public partial interface IOnChecked { void OnChecked(ToggleButton sender, System.Windows.RoutedEventArgs e); }
	public partial interface IOnUnchecked { void OnUnchecked(ToggleButton sender, System.Windows.RoutedEventArgs e); }
	public partial interface IOnIndeterminate { void OnIndeterminate(ToggleButton sender, System.Windows.RoutedEventArgs e); }
	public partial interface IOnClick { void OnClick(ButtonBase sender, System.Windows.RoutedEventArgs e); }
    public static class AttachedDataContext
    {
        public static DependencyProperty DataContextProperty =
            DependencyProperty.RegisterAttached("DataContext",
            typeof(object),
            typeof(AttachedDataContext),
            new UIPropertyMetadata(DataContextChanged));

        public static object GetDataContext(DependencyObject target) => (object)target.GetValue(DataContextProperty);
        public static void SetDataContext(DependencyObject target, object value) => target.SetValue(DataContextProperty, value);


        static void DataContextChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();

            if ((e.NewValue != null) && (e.OldValue == null) && e.NewValue != e.OldValue)
            {
				SetDataContext(target, e.NewValue);
				OnAdd(type, target, e);
            }
            else if ((e.NewValue == null) && (e.OldValue != null) && e.NewValue != e.OldValue)
            {
				OnRemove(type, target, e);
            }
        }
		
		static void OnAdd(Type type, DependencyObject target, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is IOnPreviewMouseDoubleClick && typeof(Control).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseDoubleClick");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseDoubleClick_Control");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseDoubleClick && typeof(Control).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseDoubleClick");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseDoubleClick_Control");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTargetUpdated && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TargetUpdated");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTargetUpdated_FrameworkElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSourceUpdated && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SourceUpdated");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSourceUpdated_FrameworkElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDataContextChanged && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("DataContextChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDataContextChanged_FrameworkElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnRequestBringIntoView && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("RequestBringIntoView");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnRequestBringIntoView_FrameworkElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSizeChanged && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SizeChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSizeChanged_FrameworkElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnInitialized && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Initialized");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnInitialized_FrameworkElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLoaded && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Loaded");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLoaded_FrameworkElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnUnloaded && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Unloaded");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnUnloaded_FrameworkElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnToolTipOpening && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ToolTipOpening");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnToolTipOpening_FrameworkElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnToolTipClosing && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ToolTipClosing");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnToolTipClosing_FrameworkElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnContextMenuOpening && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ContextMenuOpening");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnContextMenuOpening_FrameworkElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnContextMenuClosing && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ContextMenuClosing");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnContextMenuClosing_FrameworkElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseLeftButtonDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseLeftButtonDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseLeftButtonDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseLeftButtonDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseLeftButtonDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseLeftButtonDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseLeftButtonUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseLeftButtonUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseLeftButtonUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseLeftButtonUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseLeftButtonUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseLeftButtonUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseRightButtonDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseRightButtonDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseRightButtonDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseRightButtonDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseRightButtonDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseRightButtonDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseRightButtonUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseRightButtonUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseRightButtonUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseRightButtonUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseRightButtonUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseRightButtonUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseMove_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseMove_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseWheel && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseWheel");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseWheel_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseWheel && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseWheel");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseWheel_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseEnter && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseEnter");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseEnter_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseLeave && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseLeave");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseLeave_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnGotMouseCapture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("GotMouseCapture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnGotMouseCapture_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLostMouseCapture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LostMouseCapture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLostMouseCapture_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnQueryCursor && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("QueryCursor");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnQueryCursor_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusMove_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusMove_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusInAirMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusInAirMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusInAirMove_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusInAirMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusInAirMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusInAirMove_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusEnter && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusEnter");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusEnter_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusLeave && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusLeave");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusLeave_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusInRange && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusInRange");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusInRange_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusInRange && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusInRange");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusInRange_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusOutOfRange && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusOutOfRange");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusOutOfRange_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusOutOfRange && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusOutOfRange");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusOutOfRange_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusSystemGesture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusSystemGesture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusSystemGesture_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusSystemGesture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusSystemGesture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusSystemGesture_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnGotStylusCapture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("GotStylusCapture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnGotStylusCapture_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLostStylusCapture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LostStylusCapture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLostStylusCapture_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusButtonDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusButtonDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusButtonDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusButtonUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusButtonUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusButtonUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusButtonDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusButtonDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusButtonDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusButtonUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusButtonUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusButtonUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewKeyDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewKeyDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewKeyDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnKeyDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("KeyDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnKeyDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewKeyUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewKeyUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewKeyUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnKeyUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("KeyUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnKeyUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewGotKeyboardFocus && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewGotKeyboardFocus");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewGotKeyboardFocus_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnGotKeyboardFocus && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("GotKeyboardFocus");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnGotKeyboardFocus_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewLostKeyboardFocus && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewLostKeyboardFocus");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewLostKeyboardFocus_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLostKeyboardFocus && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LostKeyboardFocus");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLostKeyboardFocus_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewTextInput && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewTextInput");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewTextInput_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTextInput && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TextInput");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTextInput_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewQueryContinueDrag && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewQueryContinueDrag");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewQueryContinueDrag_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnQueryContinueDrag && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("QueryContinueDrag");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnQueryContinueDrag_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewGiveFeedback && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewGiveFeedback");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewGiveFeedback_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnGiveFeedback && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("GiveFeedback");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnGiveFeedback_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewDragEnter && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewDragEnter");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewDragEnter_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDragEnter && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("DragEnter");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDragEnter_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewDragOver && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewDragOver");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewDragOver_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDragOver && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("DragOver");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDragOver_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewDragLeave && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewDragLeave");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewDragLeave_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDragLeave && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("DragLeave");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDragLeave_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewDrop && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewDrop");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewDrop_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDrop && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Drop");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDrop_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewTouchDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewTouchDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewTouchDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTouchDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TouchDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTouchDown_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewTouchMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewTouchMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewTouchMove_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTouchMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TouchMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTouchMove_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewTouchUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewTouchUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewTouchUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTouchUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TouchUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTouchUp_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnGotTouchCapture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("GotTouchCapture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnGotTouchCapture_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLostTouchCapture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LostTouchCapture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLostTouchCapture_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTouchEnter && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TouchEnter");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTouchEnter_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTouchLeave && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TouchLeave");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTouchLeave_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsMouseDirectlyOverChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsMouseDirectlyOverChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsMouseDirectlyOverChanged_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsKeyboardFocusWithinChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsKeyboardFocusWithinChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsKeyboardFocusWithinChanged_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsMouseCapturedChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsMouseCapturedChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsMouseCapturedChanged_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsMouseCaptureWithinChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsMouseCaptureWithinChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsMouseCaptureWithinChanged_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsStylusDirectlyOverChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsStylusDirectlyOverChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsStylusDirectlyOverChanged_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsStylusCapturedChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsStylusCapturedChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsStylusCapturedChanged_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsStylusCaptureWithinChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsStylusCaptureWithinChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsStylusCaptureWithinChanged_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsKeyboardFocusedChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsKeyboardFocusedChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsKeyboardFocusedChanged_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLayoutUpdated && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LayoutUpdated");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLayoutUpdated_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnGotFocus && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("GotFocus");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnGotFocus_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLostFocus && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LostFocus");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLostFocus_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsEnabledChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsEnabledChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsEnabledChanged_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsHitTestVisibleChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsHitTestVisibleChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsHitTestVisibleChanged_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsVisibleChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsVisibleChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsVisibleChanged_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnFocusableChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("FocusableChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnFocusableChanged_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnManipulationStarting && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ManipulationStarting");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnManipulationStarting_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnManipulationStarted && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ManipulationStarted");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnManipulationStarted_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnManipulationDelta && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ManipulationDelta");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnManipulationDelta_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnManipulationInertiaStarting && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ManipulationInertiaStarting");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnManipulationInertiaStarting_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnManipulationBoundaryFeedback && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ManipulationBoundaryFeedback");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnManipulationBoundaryFeedback_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnManipulationCompleted && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ManipulationCompleted");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnManipulationCompleted_UIElement");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTextChanged && typeof(TextBoxBase).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TextChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTextChanged_TextBoxBase");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSelectionChanged && typeof(TextBoxBase).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SelectionChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSelectionChanged_TextBoxBase");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSourceInitialized && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SourceInitialized");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSourceInitialized_Window");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnActivated && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Activated");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnActivated_Window");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDeactivated && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Deactivated");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDeactivated_Window");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStateChanged && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StateChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStateChanged_Window");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLocationChanged && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LocationChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLocationChanged_Window");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnClosing && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Closing");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnClosing_Window");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnClosed && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Closed");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnClosed_Window");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnContentRendered && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ContentRendered");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnContentRendered_Window");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnClick && typeof(MenuItem).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Click");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnClick_MenuItem");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnChecked && typeof(MenuItem).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Checked");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnChecked_MenuItem");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnUnchecked && typeof(MenuItem).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Unchecked");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnUnchecked_MenuItem");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSubmenuOpened && typeof(MenuItem).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SubmenuOpened");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSubmenuOpened_MenuItem");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSubmenuClosed && typeof(MenuItem).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SubmenuClosed");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSubmenuClosed_MenuItem");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDropDownOpened && typeof(ComboBox).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("DropDownOpened");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDropDownOpened_ComboBox");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDropDownClosed && typeof(ComboBox).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("DropDownClosed");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDropDownClosed_ComboBox");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSelectionChanged && typeof(Selector).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SelectionChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSelectionChanged_Selector");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSelectedItemChanged && typeof(TreeView).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SelectedItemChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSelectedItemChanged_TreeView");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnScrollChanged && typeof(ScrollViewer).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ScrollChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnScrollChanged_ScrollViewer");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnChecked && typeof(ToggleButton).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Checked");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnChecked_ToggleButton");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnUnchecked && typeof(ToggleButton).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Unchecked");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnUnchecked_ToggleButton");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIndeterminate && typeof(ToggleButton).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Indeterminate");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIndeterminate_ToggleButton");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnClick && typeof(ButtonBase).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Click");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnClick_ButtonBase");
					evinfo.AddEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
		}
		static void OnRemove(Type type, DependencyObject target, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is IOnPreviewMouseDoubleClick && typeof(Control).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseDoubleClick");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseDoubleClick_Control");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseDoubleClick && typeof(Control).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseDoubleClick");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseDoubleClick_Control");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTargetUpdated && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TargetUpdated");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTargetUpdated_FrameworkElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSourceUpdated && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SourceUpdated");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSourceUpdated_FrameworkElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDataContextChanged && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("DataContextChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDataContextChanged_FrameworkElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnRequestBringIntoView && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("RequestBringIntoView");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnRequestBringIntoView_FrameworkElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSizeChanged && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SizeChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSizeChanged_FrameworkElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnInitialized && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Initialized");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnInitialized_FrameworkElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLoaded && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Loaded");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLoaded_FrameworkElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnUnloaded && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Unloaded");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnUnloaded_FrameworkElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnToolTipOpening && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ToolTipOpening");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnToolTipOpening_FrameworkElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnToolTipClosing && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ToolTipClosing");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnToolTipClosing_FrameworkElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnContextMenuOpening && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ContextMenuOpening");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnContextMenuOpening_FrameworkElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnContextMenuClosing && typeof(FrameworkElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ContextMenuClosing");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnContextMenuClosing_FrameworkElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseLeftButtonDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseLeftButtonDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseLeftButtonDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseLeftButtonDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseLeftButtonDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseLeftButtonDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseLeftButtonUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseLeftButtonUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseLeftButtonUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseLeftButtonUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseLeftButtonUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseLeftButtonUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseRightButtonDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseRightButtonDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseRightButtonDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseRightButtonDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseRightButtonDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseRightButtonDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseRightButtonUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseRightButtonUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseRightButtonUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseRightButtonUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseRightButtonUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseRightButtonUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseMove_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseMove_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewMouseWheel && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewMouseWheel");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewMouseWheel_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseWheel && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseWheel");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseWheel_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseEnter && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseEnter");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseEnter_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnMouseLeave && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("MouseLeave");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnMouseLeave_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnGotMouseCapture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("GotMouseCapture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnGotMouseCapture_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLostMouseCapture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LostMouseCapture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLostMouseCapture_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnQueryCursor && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("QueryCursor");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnQueryCursor_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusMove_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusMove_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusInAirMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusInAirMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusInAirMove_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusInAirMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusInAirMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusInAirMove_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusEnter && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusEnter");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusEnter_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusLeave && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusLeave");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusLeave_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusInRange && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusInRange");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusInRange_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusInRange && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusInRange");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusInRange_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusOutOfRange && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusOutOfRange");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusOutOfRange_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusOutOfRange && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusOutOfRange");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusOutOfRange_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusSystemGesture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusSystemGesture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusSystemGesture_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusSystemGesture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusSystemGesture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusSystemGesture_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnGotStylusCapture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("GotStylusCapture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnGotStylusCapture_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLostStylusCapture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LostStylusCapture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLostStylusCapture_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusButtonDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusButtonDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusButtonDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStylusButtonUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StylusButtonUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStylusButtonUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusButtonDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusButtonDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusButtonDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewStylusButtonUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewStylusButtonUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewStylusButtonUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewKeyDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewKeyDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewKeyDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnKeyDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("KeyDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnKeyDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewKeyUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewKeyUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewKeyUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnKeyUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("KeyUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnKeyUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewGotKeyboardFocus && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewGotKeyboardFocus");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewGotKeyboardFocus_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnGotKeyboardFocus && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("GotKeyboardFocus");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnGotKeyboardFocus_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewLostKeyboardFocus && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewLostKeyboardFocus");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewLostKeyboardFocus_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLostKeyboardFocus && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LostKeyboardFocus");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLostKeyboardFocus_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewTextInput && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewTextInput");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewTextInput_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTextInput && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TextInput");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTextInput_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewQueryContinueDrag && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewQueryContinueDrag");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewQueryContinueDrag_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnQueryContinueDrag && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("QueryContinueDrag");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnQueryContinueDrag_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewGiveFeedback && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewGiveFeedback");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewGiveFeedback_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnGiveFeedback && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("GiveFeedback");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnGiveFeedback_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewDragEnter && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewDragEnter");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewDragEnter_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDragEnter && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("DragEnter");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDragEnter_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewDragOver && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewDragOver");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewDragOver_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDragOver && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("DragOver");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDragOver_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewDragLeave && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewDragLeave");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewDragLeave_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDragLeave && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("DragLeave");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDragLeave_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewDrop && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewDrop");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewDrop_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDrop && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Drop");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDrop_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewTouchDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewTouchDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewTouchDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTouchDown && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TouchDown");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTouchDown_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewTouchMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewTouchMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewTouchMove_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTouchMove && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TouchMove");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTouchMove_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnPreviewTouchUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("PreviewTouchUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnPreviewTouchUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTouchUp && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TouchUp");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTouchUp_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnGotTouchCapture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("GotTouchCapture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnGotTouchCapture_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLostTouchCapture && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LostTouchCapture");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLostTouchCapture_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTouchEnter && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TouchEnter");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTouchEnter_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTouchLeave && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TouchLeave");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTouchLeave_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsMouseDirectlyOverChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsMouseDirectlyOverChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsMouseDirectlyOverChanged_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsKeyboardFocusWithinChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsKeyboardFocusWithinChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsKeyboardFocusWithinChanged_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsMouseCapturedChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsMouseCapturedChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsMouseCapturedChanged_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsMouseCaptureWithinChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsMouseCaptureWithinChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsMouseCaptureWithinChanged_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsStylusDirectlyOverChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsStylusDirectlyOverChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsStylusDirectlyOverChanged_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsStylusCapturedChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsStylusCapturedChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsStylusCapturedChanged_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsStylusCaptureWithinChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsStylusCaptureWithinChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsStylusCaptureWithinChanged_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsKeyboardFocusedChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsKeyboardFocusedChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsKeyboardFocusedChanged_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLayoutUpdated && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LayoutUpdated");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLayoutUpdated_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnGotFocus && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("GotFocus");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnGotFocus_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLostFocus && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LostFocus");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLostFocus_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsEnabledChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsEnabledChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsEnabledChanged_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsHitTestVisibleChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsHitTestVisibleChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsHitTestVisibleChanged_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIsVisibleChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("IsVisibleChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIsVisibleChanged_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnFocusableChanged && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("FocusableChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnFocusableChanged_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnManipulationStarting && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ManipulationStarting");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnManipulationStarting_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnManipulationStarted && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ManipulationStarted");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnManipulationStarted_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnManipulationDelta && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ManipulationDelta");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnManipulationDelta_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnManipulationInertiaStarting && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ManipulationInertiaStarting");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnManipulationInertiaStarting_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnManipulationBoundaryFeedback && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ManipulationBoundaryFeedback");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnManipulationBoundaryFeedback_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnManipulationCompleted && typeof(UIElement).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ManipulationCompleted");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnManipulationCompleted_UIElement");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnTextChanged && typeof(TextBoxBase).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("TextChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnTextChanged_TextBoxBase");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSelectionChanged && typeof(TextBoxBase).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SelectionChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSelectionChanged_TextBoxBase");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSourceInitialized && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SourceInitialized");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSourceInitialized_Window");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnActivated && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Activated");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnActivated_Window");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDeactivated && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Deactivated");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDeactivated_Window");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnStateChanged && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("StateChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnStateChanged_Window");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnLocationChanged && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("LocationChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnLocationChanged_Window");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnClosing && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Closing");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnClosing_Window");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnClosed && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Closed");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnClosed_Window");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnContentRendered && typeof(Window).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ContentRendered");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnContentRendered_Window");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnClick && typeof(MenuItem).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Click");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnClick_MenuItem");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnChecked && typeof(MenuItem).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Checked");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnChecked_MenuItem");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnUnchecked && typeof(MenuItem).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Unchecked");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnUnchecked_MenuItem");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSubmenuOpened && typeof(MenuItem).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SubmenuOpened");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSubmenuOpened_MenuItem");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSubmenuClosed && typeof(MenuItem).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SubmenuClosed");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSubmenuClosed_MenuItem");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDropDownOpened && typeof(ComboBox).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("DropDownOpened");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDropDownOpened_ComboBox");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnDropDownClosed && typeof(ComboBox).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("DropDownClosed");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnDropDownClosed_ComboBox");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSelectionChanged && typeof(Selector).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SelectionChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSelectionChanged_Selector");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnSelectedItemChanged && typeof(TreeView).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("SelectedItemChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnSelectedItemChanged_TreeView");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnScrollChanged && typeof(ScrollViewer).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("ScrollChanged");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnScrollChanged_ScrollViewer");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnChecked && typeof(ToggleButton).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Checked");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnChecked_ToggleButton");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnUnchecked && typeof(ToggleButton).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Unchecked");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnUnchecked_ToggleButton");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnIndeterminate && typeof(ToggleButton).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Indeterminate");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnIndeterminate_ToggleButton");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
			if (e.NewValue is IOnClick && typeof(ButtonBase).IsAssignableFrom(type))
			{
				var evinfo = type.GetEvent("Click");
				if (evinfo != null)
				{
					var mtinfo = typeof(AttachedDataContext).GetMethod("OnClick_ButtonBase");
					evinfo.RemoveEventHandler(target, Delegate.CreateDelegate(evinfo.EventHandlerType, mtinfo));
				}
			}
		}
		public static void OnPreviewMouseDoubleClick_Control(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is Control cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewMouseDoubleClick;
				dc.OnPreviewMouseDoubleClick(cntrl, e);
			}
		}
		public static void OnMouseDoubleClick_Control(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is Control cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnMouseDoubleClick;
				dc.OnMouseDoubleClick(cntrl, e);
			}
		}
		public static void OnTargetUpdated_FrameworkElement(object sender, System.Windows.Data.DataTransferEventArgs e)
		{
			if (sender is FrameworkElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnTargetUpdated;
				dc.OnTargetUpdated(cntrl, e);
			}
		}
		public static void OnSourceUpdated_FrameworkElement(object sender, System.Windows.Data.DataTransferEventArgs e)
		{
			if (sender is FrameworkElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnSourceUpdated;
				dc.OnSourceUpdated(cntrl, e);
			}
		}
		public static void OnDataContextChanged_FrameworkElement(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (sender is FrameworkElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnDataContextChanged;
				dc.OnDataContextChanged(cntrl, e);
			}
		}
		public static void OnRequestBringIntoView_FrameworkElement(object sender, System.Windows.RequestBringIntoViewEventArgs e)
		{
			if (sender is FrameworkElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnRequestBringIntoView;
				dc.OnRequestBringIntoView(cntrl, e);
			}
		}
		public static void OnSizeChanged_FrameworkElement(object sender, System.Windows.SizeChangedEventArgs e)
		{
			if (sender is FrameworkElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnSizeChanged;
				dc.OnSizeChanged(cntrl, e);
			}
		}
		public static void OnInitialized_FrameworkElement(object sender, System.EventArgs e)
		{
			if (sender is FrameworkElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnInitialized;
				dc.OnInitialized(cntrl, e);
			}
		}
		public static void OnLoaded_FrameworkElement(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is FrameworkElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnLoaded;
				dc.OnLoaded(cntrl, e);
			}
		}
		public static void OnUnloaded_FrameworkElement(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is FrameworkElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnUnloaded;
				dc.OnUnloaded(cntrl, e);
			}
		}
		public static void OnToolTipOpening_FrameworkElement(object sender, System.Windows.Controls.ToolTipEventArgs e)
		{
			if (sender is FrameworkElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnToolTipOpening;
				dc.OnToolTipOpening(cntrl, e);
			}
		}
		public static void OnToolTipClosing_FrameworkElement(object sender, System.Windows.Controls.ToolTipEventArgs e)
		{
			if (sender is FrameworkElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnToolTipClosing;
				dc.OnToolTipClosing(cntrl, e);
			}
		}
		public static void OnContextMenuOpening_FrameworkElement(object sender, System.Windows.Controls.ContextMenuEventArgs e)
		{
			if (sender is FrameworkElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnContextMenuOpening;
				dc.OnContextMenuOpening(cntrl, e);
			}
		}
		public static void OnContextMenuClosing_FrameworkElement(object sender, System.Windows.Controls.ContextMenuEventArgs e)
		{
			if (sender is FrameworkElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnContextMenuClosing;
				dc.OnContextMenuClosing(cntrl, e);
			}
		}
		public static void OnPreviewMouseDown_UIElement(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewMouseDown;
				dc.OnPreviewMouseDown(cntrl, e);
			}
		}
		public static void OnMouseDown_UIElement(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnMouseDown;
				dc.OnMouseDown(cntrl, e);
			}
		}
		public static void OnPreviewMouseUp_UIElement(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewMouseUp;
				dc.OnPreviewMouseUp(cntrl, e);
			}
		}
		public static void OnMouseUp_UIElement(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnMouseUp;
				dc.OnMouseUp(cntrl, e);
			}
		}
		public static void OnPreviewMouseLeftButtonDown_UIElement(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewMouseLeftButtonDown;
				dc.OnPreviewMouseLeftButtonDown(cntrl, e);
			}
		}
		public static void OnMouseLeftButtonDown_UIElement(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnMouseLeftButtonDown;
				dc.OnMouseLeftButtonDown(cntrl, e);
			}
		}
		public static void OnPreviewMouseLeftButtonUp_UIElement(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewMouseLeftButtonUp;
				dc.OnPreviewMouseLeftButtonUp(cntrl, e);
			}
		}
		public static void OnMouseLeftButtonUp_UIElement(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnMouseLeftButtonUp;
				dc.OnMouseLeftButtonUp(cntrl, e);
			}
		}
		public static void OnPreviewMouseRightButtonDown_UIElement(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewMouseRightButtonDown;
				dc.OnPreviewMouseRightButtonDown(cntrl, e);
			}
		}
		public static void OnMouseRightButtonDown_UIElement(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnMouseRightButtonDown;
				dc.OnMouseRightButtonDown(cntrl, e);
			}
		}
		public static void OnPreviewMouseRightButtonUp_UIElement(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewMouseRightButtonUp;
				dc.OnPreviewMouseRightButtonUp(cntrl, e);
			}
		}
		public static void OnMouseRightButtonUp_UIElement(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnMouseRightButtonUp;
				dc.OnMouseRightButtonUp(cntrl, e);
			}
		}
		public static void OnPreviewMouseMove_UIElement(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewMouseMove;
				dc.OnPreviewMouseMove(cntrl, e);
			}
		}
		public static void OnMouseMove_UIElement(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnMouseMove;
				dc.OnMouseMove(cntrl, e);
			}
		}
		public static void OnPreviewMouseWheel_UIElement(object sender, System.Windows.Input.MouseWheelEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewMouseWheel;
				dc.OnPreviewMouseWheel(cntrl, e);
			}
		}
		public static void OnMouseWheel_UIElement(object sender, System.Windows.Input.MouseWheelEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnMouseWheel;
				dc.OnMouseWheel(cntrl, e);
			}
		}
		public static void OnMouseEnter_UIElement(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnMouseEnter;
				dc.OnMouseEnter(cntrl, e);
			}
		}
		public static void OnMouseLeave_UIElement(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnMouseLeave;
				dc.OnMouseLeave(cntrl, e);
			}
		}
		public static void OnGotMouseCapture_UIElement(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnGotMouseCapture;
				dc.OnGotMouseCapture(cntrl, e);
			}
		}
		public static void OnLostMouseCapture_UIElement(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnLostMouseCapture;
				dc.OnLostMouseCapture(cntrl, e);
			}
		}
		public static void OnQueryCursor_UIElement(object sender, System.Windows.Input.QueryCursorEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnQueryCursor;
				dc.OnQueryCursor(cntrl, e);
			}
		}
		public static void OnPreviewStylusDown_UIElement(object sender, System.Windows.Input.StylusDownEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewStylusDown;
				dc.OnPreviewStylusDown(cntrl, e);
			}
		}
		public static void OnStylusDown_UIElement(object sender, System.Windows.Input.StylusDownEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnStylusDown;
				dc.OnStylusDown(cntrl, e);
			}
		}
		public static void OnPreviewStylusUp_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewStylusUp;
				dc.OnPreviewStylusUp(cntrl, e);
			}
		}
		public static void OnStylusUp_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnStylusUp;
				dc.OnStylusUp(cntrl, e);
			}
		}
		public static void OnPreviewStylusMove_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewStylusMove;
				dc.OnPreviewStylusMove(cntrl, e);
			}
		}
		public static void OnStylusMove_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnStylusMove;
				dc.OnStylusMove(cntrl, e);
			}
		}
		public static void OnPreviewStylusInAirMove_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewStylusInAirMove;
				dc.OnPreviewStylusInAirMove(cntrl, e);
			}
		}
		public static void OnStylusInAirMove_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnStylusInAirMove;
				dc.OnStylusInAirMove(cntrl, e);
			}
		}
		public static void OnStylusEnter_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnStylusEnter;
				dc.OnStylusEnter(cntrl, e);
			}
		}
		public static void OnStylusLeave_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnStylusLeave;
				dc.OnStylusLeave(cntrl, e);
			}
		}
		public static void OnPreviewStylusInRange_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewStylusInRange;
				dc.OnPreviewStylusInRange(cntrl, e);
			}
		}
		public static void OnStylusInRange_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnStylusInRange;
				dc.OnStylusInRange(cntrl, e);
			}
		}
		public static void OnPreviewStylusOutOfRange_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewStylusOutOfRange;
				dc.OnPreviewStylusOutOfRange(cntrl, e);
			}
		}
		public static void OnStylusOutOfRange_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnStylusOutOfRange;
				dc.OnStylusOutOfRange(cntrl, e);
			}
		}
		public static void OnPreviewStylusSystemGesture_UIElement(object sender, System.Windows.Input.StylusSystemGestureEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewStylusSystemGesture;
				dc.OnPreviewStylusSystemGesture(cntrl, e);
			}
		}
		public static void OnStylusSystemGesture_UIElement(object sender, System.Windows.Input.StylusSystemGestureEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnStylusSystemGesture;
				dc.OnStylusSystemGesture(cntrl, e);
			}
		}
		public static void OnGotStylusCapture_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnGotStylusCapture;
				dc.OnGotStylusCapture(cntrl, e);
			}
		}
		public static void OnLostStylusCapture_UIElement(object sender, System.Windows.Input.StylusEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnLostStylusCapture;
				dc.OnLostStylusCapture(cntrl, e);
			}
		}
		public static void OnStylusButtonDown_UIElement(object sender, System.Windows.Input.StylusButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnStylusButtonDown;
				dc.OnStylusButtonDown(cntrl, e);
			}
		}
		public static void OnStylusButtonUp_UIElement(object sender, System.Windows.Input.StylusButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnStylusButtonUp;
				dc.OnStylusButtonUp(cntrl, e);
			}
		}
		public static void OnPreviewStylusButtonDown_UIElement(object sender, System.Windows.Input.StylusButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewStylusButtonDown;
				dc.OnPreviewStylusButtonDown(cntrl, e);
			}
		}
		public static void OnPreviewStylusButtonUp_UIElement(object sender, System.Windows.Input.StylusButtonEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewStylusButtonUp;
				dc.OnPreviewStylusButtonUp(cntrl, e);
			}
		}
		public static void OnPreviewKeyDown_UIElement(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewKeyDown;
				dc.OnPreviewKeyDown(cntrl, e);
			}
		}
		public static void OnKeyDown_UIElement(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnKeyDown;
				dc.OnKeyDown(cntrl, e);
			}
		}
		public static void OnPreviewKeyUp_UIElement(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewKeyUp;
				dc.OnPreviewKeyUp(cntrl, e);
			}
		}
		public static void OnKeyUp_UIElement(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnKeyUp;
				dc.OnKeyUp(cntrl, e);
			}
		}
		public static void OnPreviewGotKeyboardFocus_UIElement(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewGotKeyboardFocus;
				dc.OnPreviewGotKeyboardFocus(cntrl, e);
			}
		}
		public static void OnGotKeyboardFocus_UIElement(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnGotKeyboardFocus;
				dc.OnGotKeyboardFocus(cntrl, e);
			}
		}
		public static void OnPreviewLostKeyboardFocus_UIElement(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewLostKeyboardFocus;
				dc.OnPreviewLostKeyboardFocus(cntrl, e);
			}
		}
		public static void OnLostKeyboardFocus_UIElement(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnLostKeyboardFocus;
				dc.OnLostKeyboardFocus(cntrl, e);
			}
		}
		public static void OnPreviewTextInput_UIElement(object sender, System.Windows.Input.TextCompositionEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewTextInput;
				dc.OnPreviewTextInput(cntrl, e);
			}
		}
		public static void OnTextInput_UIElement(object sender, System.Windows.Input.TextCompositionEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnTextInput;
				dc.OnTextInput(cntrl, e);
			}
		}
		public static void OnPreviewQueryContinueDrag_UIElement(object sender, System.Windows.QueryContinueDragEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewQueryContinueDrag;
				dc.OnPreviewQueryContinueDrag(cntrl, e);
			}
		}
		public static void OnQueryContinueDrag_UIElement(object sender, System.Windows.QueryContinueDragEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnQueryContinueDrag;
				dc.OnQueryContinueDrag(cntrl, e);
			}
		}
		public static void OnPreviewGiveFeedback_UIElement(object sender, System.Windows.GiveFeedbackEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewGiveFeedback;
				dc.OnPreviewGiveFeedback(cntrl, e);
			}
		}
		public static void OnGiveFeedback_UIElement(object sender, System.Windows.GiveFeedbackEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnGiveFeedback;
				dc.OnGiveFeedback(cntrl, e);
			}
		}
		public static void OnPreviewDragEnter_UIElement(object sender, System.Windows.DragEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewDragEnter;
				dc.OnPreviewDragEnter(cntrl, e);
			}
		}
		public static void OnDragEnter_UIElement(object sender, System.Windows.DragEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnDragEnter;
				dc.OnDragEnter(cntrl, e);
			}
		}
		public static void OnPreviewDragOver_UIElement(object sender, System.Windows.DragEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewDragOver;
				dc.OnPreviewDragOver(cntrl, e);
			}
		}
		public static void OnDragOver_UIElement(object sender, System.Windows.DragEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnDragOver;
				dc.OnDragOver(cntrl, e);
			}
		}
		public static void OnPreviewDragLeave_UIElement(object sender, System.Windows.DragEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewDragLeave;
				dc.OnPreviewDragLeave(cntrl, e);
			}
		}
		public static void OnDragLeave_UIElement(object sender, System.Windows.DragEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnDragLeave;
				dc.OnDragLeave(cntrl, e);
			}
		}
		public static void OnPreviewDrop_UIElement(object sender, System.Windows.DragEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewDrop;
				dc.OnPreviewDrop(cntrl, e);
			}
		}
		public static void OnDrop_UIElement(object sender, System.Windows.DragEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnDrop;
				dc.OnDrop(cntrl, e);
			}
		}
		public static void OnPreviewTouchDown_UIElement(object sender, System.Windows.Input.TouchEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewTouchDown;
				dc.OnPreviewTouchDown(cntrl, e);
			}
		}
		public static void OnTouchDown_UIElement(object sender, System.Windows.Input.TouchEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnTouchDown;
				dc.OnTouchDown(cntrl, e);
			}
		}
		public static void OnPreviewTouchMove_UIElement(object sender, System.Windows.Input.TouchEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewTouchMove;
				dc.OnPreviewTouchMove(cntrl, e);
			}
		}
		public static void OnTouchMove_UIElement(object sender, System.Windows.Input.TouchEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnTouchMove;
				dc.OnTouchMove(cntrl, e);
			}
		}
		public static void OnPreviewTouchUp_UIElement(object sender, System.Windows.Input.TouchEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnPreviewTouchUp;
				dc.OnPreviewTouchUp(cntrl, e);
			}
		}
		public static void OnTouchUp_UIElement(object sender, System.Windows.Input.TouchEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnTouchUp;
				dc.OnTouchUp(cntrl, e);
			}
		}
		public static void OnGotTouchCapture_UIElement(object sender, System.Windows.Input.TouchEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnGotTouchCapture;
				dc.OnGotTouchCapture(cntrl, e);
			}
		}
		public static void OnLostTouchCapture_UIElement(object sender, System.Windows.Input.TouchEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnLostTouchCapture;
				dc.OnLostTouchCapture(cntrl, e);
			}
		}
		public static void OnTouchEnter_UIElement(object sender, System.Windows.Input.TouchEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnTouchEnter;
				dc.OnTouchEnter(cntrl, e);
			}
		}
		public static void OnTouchLeave_UIElement(object sender, System.Windows.Input.TouchEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnTouchLeave;
				dc.OnTouchLeave(cntrl, e);
			}
		}
		public static void OnIsMouseDirectlyOverChanged_UIElement(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnIsMouseDirectlyOverChanged;
				dc.OnIsMouseDirectlyOverChanged(cntrl, e);
			}
		}
		public static void OnIsKeyboardFocusWithinChanged_UIElement(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnIsKeyboardFocusWithinChanged;
				dc.OnIsKeyboardFocusWithinChanged(cntrl, e);
			}
		}
		public static void OnIsMouseCapturedChanged_UIElement(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnIsMouseCapturedChanged;
				dc.OnIsMouseCapturedChanged(cntrl, e);
			}
		}
		public static void OnIsMouseCaptureWithinChanged_UIElement(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnIsMouseCaptureWithinChanged;
				dc.OnIsMouseCaptureWithinChanged(cntrl, e);
			}
		}
		public static void OnIsStylusDirectlyOverChanged_UIElement(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnIsStylusDirectlyOverChanged;
				dc.OnIsStylusDirectlyOverChanged(cntrl, e);
			}
		}
		public static void OnIsStylusCapturedChanged_UIElement(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnIsStylusCapturedChanged;
				dc.OnIsStylusCapturedChanged(cntrl, e);
			}
		}
		public static void OnIsStylusCaptureWithinChanged_UIElement(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnIsStylusCaptureWithinChanged;
				dc.OnIsStylusCaptureWithinChanged(cntrl, e);
			}
		}
		public static void OnIsKeyboardFocusedChanged_UIElement(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnIsKeyboardFocusedChanged;
				dc.OnIsKeyboardFocusedChanged(cntrl, e);
			}
		}
		public static void OnLayoutUpdated_UIElement(object sender, System.EventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnLayoutUpdated;
				dc.OnLayoutUpdated(cntrl, e);
			}
		}
		public static void OnGotFocus_UIElement(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnGotFocus;
				dc.OnGotFocus(cntrl, e);
			}
		}
		public static void OnLostFocus_UIElement(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnLostFocus;
				dc.OnLostFocus(cntrl, e);
			}
		}
		public static void OnIsEnabledChanged_UIElement(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnIsEnabledChanged;
				dc.OnIsEnabledChanged(cntrl, e);
			}
		}
		public static void OnIsHitTestVisibleChanged_UIElement(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnIsHitTestVisibleChanged;
				dc.OnIsHitTestVisibleChanged(cntrl, e);
			}
		}
		public static void OnIsVisibleChanged_UIElement(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnIsVisibleChanged;
				dc.OnIsVisibleChanged(cntrl, e);
			}
		}
		public static void OnFocusableChanged_UIElement(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnFocusableChanged;
				dc.OnFocusableChanged(cntrl, e);
			}
		}
		public static void OnManipulationStarting_UIElement(object sender, System.Windows.Input.ManipulationStartingEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnManipulationStarting;
				dc.OnManipulationStarting(cntrl, e);
			}
		}
		public static void OnManipulationStarted_UIElement(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnManipulationStarted;
				dc.OnManipulationStarted(cntrl, e);
			}
		}
		public static void OnManipulationDelta_UIElement(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnManipulationDelta;
				dc.OnManipulationDelta(cntrl, e);
			}
		}
		public static void OnManipulationInertiaStarting_UIElement(object sender, System.Windows.Input.ManipulationInertiaStartingEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnManipulationInertiaStarting;
				dc.OnManipulationInertiaStarting(cntrl, e);
			}
		}
		public static void OnManipulationBoundaryFeedback_UIElement(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnManipulationBoundaryFeedback;
				dc.OnManipulationBoundaryFeedback(cntrl, e);
			}
		}
		public static void OnManipulationCompleted_UIElement(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
		{
			if (sender is UIElement cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnManipulationCompleted;
				dc.OnManipulationCompleted(cntrl, e);
			}
		}
		public static void OnTextChanged_TextBoxBase(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			if (sender is TextBoxBase cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnTextChanged;
				dc.OnTextChanged(cntrl, e);
			}
		}
		public static void OnSelectionChanged_TextBoxBase(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is TextBoxBase cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnSelectionChanged;
				dc.OnSelectionChanged(cntrl, e);
			}
		}
		public static void OnSourceInitialized_Window(object sender, System.EventArgs e)
		{
			if (sender is Window cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnSourceInitialized;
				dc.OnSourceInitialized(cntrl, e);
			}
		}
		public static void OnActivated_Window(object sender, System.EventArgs e)
		{
			if (sender is Window cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnActivated;
				dc.OnActivated(cntrl, e);
			}
		}
		public static void OnDeactivated_Window(object sender, System.EventArgs e)
		{
			if (sender is Window cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnDeactivated;
				dc.OnDeactivated(cntrl, e);
			}
		}
		public static void OnStateChanged_Window(object sender, System.EventArgs e)
		{
			if (sender is Window cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnStateChanged;
				dc.OnStateChanged(cntrl, e);
			}
		}
		public static void OnLocationChanged_Window(object sender, System.EventArgs e)
		{
			if (sender is Window cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnLocationChanged;
				dc.OnLocationChanged(cntrl, e);
			}
		}
		public static void OnClosing_Window(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (sender is Window cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnClosing;
				dc.OnClosing(cntrl, e);
			}
		}
		public static void OnClosed_Window(object sender, System.EventArgs e)
		{
			if (sender is Window cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnClosed;
				dc.OnClosed(cntrl, e);
			}
		}
		public static void OnContentRendered_Window(object sender, System.EventArgs e)
		{
			if (sender is Window cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnContentRendered;
				dc.OnContentRendered(cntrl, e);
			}
		}
		public static void OnClick_MenuItem(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is MenuItem cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnClick;
				dc.OnClick(cntrl, e);
			}
		}
		public static void OnChecked_MenuItem(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is MenuItem cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnChecked;
				dc.OnChecked(cntrl, e);
			}
		}
		public static void OnUnchecked_MenuItem(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is MenuItem cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnUnchecked;
				dc.OnUnchecked(cntrl, e);
			}
		}
		public static void OnSubmenuOpened_MenuItem(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is MenuItem cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnSubmenuOpened;
				dc.OnSubmenuOpened(cntrl, e);
			}
		}
		public static void OnSubmenuClosed_MenuItem(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is MenuItem cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnSubmenuClosed;
				dc.OnSubmenuClosed(cntrl, e);
			}
		}
		public static void OnDropDownOpened_ComboBox(object sender, System.EventArgs e)
		{
			if (sender is ComboBox cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnDropDownOpened;
				dc.OnDropDownOpened(cntrl, e);
			}
		}
		public static void OnDropDownClosed_ComboBox(object sender, System.EventArgs e)
		{
			if (sender is ComboBox cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnDropDownClosed;
				dc.OnDropDownClosed(cntrl, e);
			}
		}
		public static void OnSelectionChanged_Selector(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (sender is Selector cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnSelectionChanged;
				dc.OnSelectionChanged(cntrl, e);
			}
		}
		public static void OnSelectedItemChanged_TreeView(object sender, System.Windows.RoutedPropertyChangedEventArgs<System.Object> e)
		{
			if (sender is TreeView cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnSelectedItemChanged;
				dc.OnSelectedItemChanged(cntrl, e);
			}
		}
		public static void OnScrollChanged_ScrollViewer(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
		{
			if (sender is ScrollViewer cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnScrollChanged;
				dc.OnScrollChanged(cntrl, e);
			}
		}
		public static void OnChecked_ToggleButton(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is ToggleButton cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnChecked;
				dc.OnChecked(cntrl, e);
			}
		}
		public static void OnUnchecked_ToggleButton(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is ToggleButton cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnUnchecked;
				dc.OnUnchecked(cntrl, e);
			}
		}
		public static void OnIndeterminate_ToggleButton(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is ToggleButton cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnIndeterminate;
				dc.OnIndeterminate(cntrl, e);
			}
		}
		public static void OnClick_ButtonBase(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is ButtonBase cntrl)
			{
				var dc = GetDataContext(cntrl) as IOnClick;
				dc.OnClick(cntrl, e);
			}
		}
    }
    public class PreviewMouseDoubleClick
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewMouseDoubleClick),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewMouseDoubleClick),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(Control target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(Control target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(Control target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(Control target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewMouseDoubleClick");
            var method = typeof(PreviewMouseDoubleClick).GetMethod("OnPreviewMouseDoubleClick");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewMouseDoubleClick(object sender, EventArgs e)
        {
            var control = sender as Control;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class MouseDoubleClick
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseDoubleClick),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(MouseDoubleClick),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(Control target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(Control target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(Control target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(Control target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("MouseDoubleClick");
            var method = typeof(MouseDoubleClick).GetMethod("OnMouseDoubleClick");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnMouseDoubleClick(object sender, EventArgs e)
        {
            var control = sender as Control;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class TargetUpdated
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(TargetUpdated),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(TargetUpdated),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(FrameworkElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(FrameworkElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(FrameworkElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(FrameworkElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("TargetUpdated");
            var method = typeof(TargetUpdated).GetMethod("OnTargetUpdated");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnTargetUpdated(object sender, EventArgs e)
        {
            var control = sender as FrameworkElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class SourceUpdated
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(SourceUpdated),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(SourceUpdated),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(FrameworkElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(FrameworkElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(FrameworkElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(FrameworkElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("SourceUpdated");
            var method = typeof(SourceUpdated).GetMethod("OnSourceUpdated");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnSourceUpdated(object sender, EventArgs e)
        {
            var control = sender as FrameworkElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class DataContextChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(DataContextChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(DataContextChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(FrameworkElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(FrameworkElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(FrameworkElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(FrameworkElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("DataContextChanged");
            var method = typeof(DataContextChanged).GetMethod("OnDataContextChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnDataContextChanged(object sender, EventArgs e)
        {
            var control = sender as FrameworkElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class RequestBringIntoView
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(RequestBringIntoView),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(RequestBringIntoView),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(FrameworkElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(FrameworkElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(FrameworkElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(FrameworkElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("RequestBringIntoView");
            var method = typeof(RequestBringIntoView).GetMethod("OnRequestBringIntoView");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnRequestBringIntoView(object sender, EventArgs e)
        {
            var control = sender as FrameworkElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class SizeChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(SizeChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(SizeChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(FrameworkElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(FrameworkElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(FrameworkElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(FrameworkElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("SizeChanged");
            var method = typeof(SizeChanged).GetMethod("OnSizeChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnSizeChanged(object sender, EventArgs e)
        {
            var control = sender as FrameworkElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class Initialized
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Initialized),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Initialized),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(FrameworkElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(FrameworkElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(FrameworkElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(FrameworkElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Initialized");
            var method = typeof(Initialized).GetMethod("OnInitialized");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnInitialized(object sender, EventArgs e)
        {
            var control = sender as FrameworkElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class Loaded
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Loaded),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Loaded),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(FrameworkElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(FrameworkElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(FrameworkElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(FrameworkElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Loaded");
            var method = typeof(Loaded).GetMethod("OnLoaded");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnLoaded(object sender, EventArgs e)
        {
            var control = sender as FrameworkElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class Unloaded
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Unloaded),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Unloaded),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(FrameworkElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(FrameworkElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(FrameworkElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(FrameworkElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Unloaded");
            var method = typeof(Unloaded).GetMethod("OnUnloaded");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnUnloaded(object sender, EventArgs e)
        {
            var control = sender as FrameworkElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class ToolTipOpening
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(ToolTipOpening),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ToolTipOpening),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(FrameworkElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(FrameworkElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(FrameworkElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(FrameworkElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("ToolTipOpening");
            var method = typeof(ToolTipOpening).GetMethod("OnToolTipOpening");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnToolTipOpening(object sender, EventArgs e)
        {
            var control = sender as FrameworkElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class ToolTipClosing
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(ToolTipClosing),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ToolTipClosing),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(FrameworkElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(FrameworkElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(FrameworkElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(FrameworkElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("ToolTipClosing");
            var method = typeof(ToolTipClosing).GetMethod("OnToolTipClosing");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnToolTipClosing(object sender, EventArgs e)
        {
            var control = sender as FrameworkElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class ContextMenuOpening
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(ContextMenuOpening),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ContextMenuOpening),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(FrameworkElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(FrameworkElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(FrameworkElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(FrameworkElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("ContextMenuOpening");
            var method = typeof(ContextMenuOpening).GetMethod("OnContextMenuOpening");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnContextMenuOpening(object sender, EventArgs e)
        {
            var control = sender as FrameworkElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class ContextMenuClosing
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(ContextMenuClosing),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ContextMenuClosing),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(FrameworkElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(FrameworkElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(FrameworkElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(FrameworkElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("ContextMenuClosing");
            var method = typeof(ContextMenuClosing).GetMethod("OnContextMenuClosing");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnContextMenuClosing(object sender, EventArgs e)
        {
            var control = sender as FrameworkElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewMouseDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewMouseDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewMouseDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewMouseDown");
            var method = typeof(PreviewMouseDown).GetMethod("OnPreviewMouseDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewMouseDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class MouseDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(MouseDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("MouseDown");
            var method = typeof(MouseDown).GetMethod("OnMouseDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnMouseDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewMouseUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewMouseUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewMouseUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewMouseUp");
            var method = typeof(PreviewMouseUp).GetMethod("OnPreviewMouseUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewMouseUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class MouseUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(MouseUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("MouseUp");
            var method = typeof(MouseUp).GetMethod("OnMouseUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnMouseUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewMouseLeftButtonDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewMouseLeftButtonDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewMouseLeftButtonDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewMouseLeftButtonDown");
            var method = typeof(PreviewMouseLeftButtonDown).GetMethod("OnPreviewMouseLeftButtonDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewMouseLeftButtonDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class MouseLeftButtonDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseLeftButtonDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(MouseLeftButtonDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("MouseLeftButtonDown");
            var method = typeof(MouseLeftButtonDown).GetMethod("OnMouseLeftButtonDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnMouseLeftButtonDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewMouseLeftButtonUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewMouseLeftButtonUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewMouseLeftButtonUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewMouseLeftButtonUp");
            var method = typeof(PreviewMouseLeftButtonUp).GetMethod("OnPreviewMouseLeftButtonUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewMouseLeftButtonUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class MouseLeftButtonUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseLeftButtonUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(MouseLeftButtonUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("MouseLeftButtonUp");
            var method = typeof(MouseLeftButtonUp).GetMethod("OnMouseLeftButtonUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnMouseLeftButtonUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewMouseRightButtonDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewMouseRightButtonDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewMouseRightButtonDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewMouseRightButtonDown");
            var method = typeof(PreviewMouseRightButtonDown).GetMethod("OnPreviewMouseRightButtonDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewMouseRightButtonDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class MouseRightButtonDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseRightButtonDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(MouseRightButtonDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("MouseRightButtonDown");
            var method = typeof(MouseRightButtonDown).GetMethod("OnMouseRightButtonDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnMouseRightButtonDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewMouseRightButtonUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewMouseRightButtonUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewMouseRightButtonUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewMouseRightButtonUp");
            var method = typeof(PreviewMouseRightButtonUp).GetMethod("OnPreviewMouseRightButtonUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewMouseRightButtonUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class MouseRightButtonUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseRightButtonUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(MouseRightButtonUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("MouseRightButtonUp");
            var method = typeof(MouseRightButtonUp).GetMethod("OnMouseRightButtonUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnMouseRightButtonUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewMouseMove
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewMouseMove),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewMouseMove),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewMouseMove");
            var method = typeof(PreviewMouseMove).GetMethod("OnPreviewMouseMove");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewMouseMove(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class MouseMove
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseMove),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(MouseMove),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("MouseMove");
            var method = typeof(MouseMove).GetMethod("OnMouseMove");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnMouseMove(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewMouseWheel
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewMouseWheel),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewMouseWheel),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewMouseWheel");
            var method = typeof(PreviewMouseWheel).GetMethod("OnPreviewMouseWheel");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewMouseWheel(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class MouseWheel
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseWheel),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(MouseWheel),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("MouseWheel");
            var method = typeof(MouseWheel).GetMethod("OnMouseWheel");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnMouseWheel(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class MouseEnter
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseEnter),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(MouseEnter),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("MouseEnter");
            var method = typeof(MouseEnter).GetMethod("OnMouseEnter");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnMouseEnter(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class MouseLeave
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseLeave),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(MouseLeave),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("MouseLeave");
            var method = typeof(MouseLeave).GetMethod("OnMouseLeave");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnMouseLeave(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class GotMouseCapture
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(GotMouseCapture),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(GotMouseCapture),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("GotMouseCapture");
            var method = typeof(GotMouseCapture).GetMethod("OnGotMouseCapture");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnGotMouseCapture(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class LostMouseCapture
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(LostMouseCapture),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(LostMouseCapture),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("LostMouseCapture");
            var method = typeof(LostMouseCapture).GetMethod("OnLostMouseCapture");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnLostMouseCapture(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class QueryCursor
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(QueryCursor),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(QueryCursor),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("QueryCursor");
            var method = typeof(QueryCursor).GetMethod("OnQueryCursor");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnQueryCursor(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewStylusDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewStylusDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewStylusDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewStylusDown");
            var method = typeof(PreviewStylusDown).GetMethod("OnPreviewStylusDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewStylusDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class StylusDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(StylusDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(StylusDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("StylusDown");
            var method = typeof(StylusDown).GetMethod("OnStylusDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnStylusDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewStylusUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewStylusUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewStylusUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewStylusUp");
            var method = typeof(PreviewStylusUp).GetMethod("OnPreviewStylusUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewStylusUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class StylusUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(StylusUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(StylusUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("StylusUp");
            var method = typeof(StylusUp).GetMethod("OnStylusUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnStylusUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewStylusMove
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewStylusMove),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewStylusMove),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewStylusMove");
            var method = typeof(PreviewStylusMove).GetMethod("OnPreviewStylusMove");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewStylusMove(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class StylusMove
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(StylusMove),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(StylusMove),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("StylusMove");
            var method = typeof(StylusMove).GetMethod("OnStylusMove");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnStylusMove(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewStylusInAirMove
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewStylusInAirMove),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewStylusInAirMove),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewStylusInAirMove");
            var method = typeof(PreviewStylusInAirMove).GetMethod("OnPreviewStylusInAirMove");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewStylusInAirMove(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class StylusInAirMove
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(StylusInAirMove),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(StylusInAirMove),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("StylusInAirMove");
            var method = typeof(StylusInAirMove).GetMethod("OnStylusInAirMove");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnStylusInAirMove(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class StylusEnter
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(StylusEnter),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(StylusEnter),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("StylusEnter");
            var method = typeof(StylusEnter).GetMethod("OnStylusEnter");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnStylusEnter(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class StylusLeave
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(StylusLeave),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(StylusLeave),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("StylusLeave");
            var method = typeof(StylusLeave).GetMethod("OnStylusLeave");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnStylusLeave(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewStylusInRange
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewStylusInRange),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewStylusInRange),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewStylusInRange");
            var method = typeof(PreviewStylusInRange).GetMethod("OnPreviewStylusInRange");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewStylusInRange(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class StylusInRange
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(StylusInRange),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(StylusInRange),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("StylusInRange");
            var method = typeof(StylusInRange).GetMethod("OnStylusInRange");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnStylusInRange(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewStylusOutOfRange
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewStylusOutOfRange),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewStylusOutOfRange),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewStylusOutOfRange");
            var method = typeof(PreviewStylusOutOfRange).GetMethod("OnPreviewStylusOutOfRange");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewStylusOutOfRange(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class StylusOutOfRange
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(StylusOutOfRange),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(StylusOutOfRange),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("StylusOutOfRange");
            var method = typeof(StylusOutOfRange).GetMethod("OnStylusOutOfRange");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnStylusOutOfRange(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewStylusSystemGesture
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewStylusSystemGesture),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewStylusSystemGesture),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewStylusSystemGesture");
            var method = typeof(PreviewStylusSystemGesture).GetMethod("OnPreviewStylusSystemGesture");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewStylusSystemGesture(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class StylusSystemGesture
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(StylusSystemGesture),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(StylusSystemGesture),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("StylusSystemGesture");
            var method = typeof(StylusSystemGesture).GetMethod("OnStylusSystemGesture");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnStylusSystemGesture(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class GotStylusCapture
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(GotStylusCapture),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(GotStylusCapture),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("GotStylusCapture");
            var method = typeof(GotStylusCapture).GetMethod("OnGotStylusCapture");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnGotStylusCapture(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class LostStylusCapture
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(LostStylusCapture),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(LostStylusCapture),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("LostStylusCapture");
            var method = typeof(LostStylusCapture).GetMethod("OnLostStylusCapture");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnLostStylusCapture(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class StylusButtonDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(StylusButtonDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(StylusButtonDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("StylusButtonDown");
            var method = typeof(StylusButtonDown).GetMethod("OnStylusButtonDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnStylusButtonDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class StylusButtonUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(StylusButtonUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(StylusButtonUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("StylusButtonUp");
            var method = typeof(StylusButtonUp).GetMethod("OnStylusButtonUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnStylusButtonUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewStylusButtonDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewStylusButtonDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewStylusButtonDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewStylusButtonDown");
            var method = typeof(PreviewStylusButtonDown).GetMethod("OnPreviewStylusButtonDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewStylusButtonDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewStylusButtonUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewStylusButtonUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewStylusButtonUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewStylusButtonUp");
            var method = typeof(PreviewStylusButtonUp).GetMethod("OnPreviewStylusButtonUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewStylusButtonUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewKeyDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewKeyDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewKeyDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewKeyDown");
            var method = typeof(PreviewKeyDown).GetMethod("OnPreviewKeyDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewKeyDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class KeyDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(KeyDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(KeyDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("KeyDown");
            var method = typeof(KeyDown).GetMethod("OnKeyDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnKeyDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewKeyUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewKeyUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewKeyUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewKeyUp");
            var method = typeof(PreviewKeyUp).GetMethod("OnPreviewKeyUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewKeyUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class KeyUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(KeyUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(KeyUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("KeyUp");
            var method = typeof(KeyUp).GetMethod("OnKeyUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnKeyUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewGotKeyboardFocus
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewGotKeyboardFocus),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewGotKeyboardFocus),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewGotKeyboardFocus");
            var method = typeof(PreviewGotKeyboardFocus).GetMethod("OnPreviewGotKeyboardFocus");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewGotKeyboardFocus(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class GotKeyboardFocus
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(GotKeyboardFocus),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(GotKeyboardFocus),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("GotKeyboardFocus");
            var method = typeof(GotKeyboardFocus).GetMethod("OnGotKeyboardFocus");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnGotKeyboardFocus(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewLostKeyboardFocus
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewLostKeyboardFocus),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewLostKeyboardFocus),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewLostKeyboardFocus");
            var method = typeof(PreviewLostKeyboardFocus).GetMethod("OnPreviewLostKeyboardFocus");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewLostKeyboardFocus(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class LostKeyboardFocus
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(LostKeyboardFocus),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(LostKeyboardFocus),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("LostKeyboardFocus");
            var method = typeof(LostKeyboardFocus).GetMethod("OnLostKeyboardFocus");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnLostKeyboardFocus(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewTextInput
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewTextInput),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewTextInput),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewTextInput");
            var method = typeof(PreviewTextInput).GetMethod("OnPreviewTextInput");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewTextInput(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class TextInput
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(TextInput),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(TextInput),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("TextInput");
            var method = typeof(TextInput).GetMethod("OnTextInput");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnTextInput(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewQueryContinueDrag
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewQueryContinueDrag),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewQueryContinueDrag),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewQueryContinueDrag");
            var method = typeof(PreviewQueryContinueDrag).GetMethod("OnPreviewQueryContinueDrag");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewQueryContinueDrag(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class QueryContinueDrag
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(QueryContinueDrag),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(QueryContinueDrag),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("QueryContinueDrag");
            var method = typeof(QueryContinueDrag).GetMethod("OnQueryContinueDrag");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnQueryContinueDrag(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewGiveFeedback
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewGiveFeedback),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewGiveFeedback),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewGiveFeedback");
            var method = typeof(PreviewGiveFeedback).GetMethod("OnPreviewGiveFeedback");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewGiveFeedback(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class GiveFeedback
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(GiveFeedback),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(GiveFeedback),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("GiveFeedback");
            var method = typeof(GiveFeedback).GetMethod("OnGiveFeedback");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnGiveFeedback(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewDragEnter
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewDragEnter),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewDragEnter),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewDragEnter");
            var method = typeof(PreviewDragEnter).GetMethod("OnPreviewDragEnter");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewDragEnter(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class DragEnter
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(DragEnter),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(DragEnter),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("DragEnter");
            var method = typeof(DragEnter).GetMethod("OnDragEnter");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnDragEnter(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewDragOver
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewDragOver),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewDragOver),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewDragOver");
            var method = typeof(PreviewDragOver).GetMethod("OnPreviewDragOver");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewDragOver(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class DragOver
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(DragOver),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(DragOver),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("DragOver");
            var method = typeof(DragOver).GetMethod("OnDragOver");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnDragOver(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewDragLeave
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewDragLeave),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewDragLeave),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewDragLeave");
            var method = typeof(PreviewDragLeave).GetMethod("OnPreviewDragLeave");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewDragLeave(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class DragLeave
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(DragLeave),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(DragLeave),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("DragLeave");
            var method = typeof(DragLeave).GetMethod("OnDragLeave");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnDragLeave(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewDrop
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewDrop),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewDrop),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewDrop");
            var method = typeof(PreviewDrop).GetMethod("OnPreviewDrop");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewDrop(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class Drop
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Drop),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Drop),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Drop");
            var method = typeof(Drop).GetMethod("OnDrop");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnDrop(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewTouchDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewTouchDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewTouchDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewTouchDown");
            var method = typeof(PreviewTouchDown).GetMethod("OnPreviewTouchDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewTouchDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class TouchDown
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(TouchDown),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(TouchDown),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("TouchDown");
            var method = typeof(TouchDown).GetMethod("OnTouchDown");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnTouchDown(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewTouchMove
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewTouchMove),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewTouchMove),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewTouchMove");
            var method = typeof(PreviewTouchMove).GetMethod("OnPreviewTouchMove");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewTouchMove(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class TouchMove
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(TouchMove),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(TouchMove),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("TouchMove");
            var method = typeof(TouchMove).GetMethod("OnTouchMove");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnTouchMove(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class PreviewTouchUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(PreviewTouchUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(PreviewTouchUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("PreviewTouchUp");
            var method = typeof(PreviewTouchUp).GetMethod("OnPreviewTouchUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnPreviewTouchUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class TouchUp
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(TouchUp),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(TouchUp),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("TouchUp");
            var method = typeof(TouchUp).GetMethod("OnTouchUp");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnTouchUp(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class GotTouchCapture
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(GotTouchCapture),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(GotTouchCapture),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("GotTouchCapture");
            var method = typeof(GotTouchCapture).GetMethod("OnGotTouchCapture");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnGotTouchCapture(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class LostTouchCapture
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(LostTouchCapture),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(LostTouchCapture),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("LostTouchCapture");
            var method = typeof(LostTouchCapture).GetMethod("OnLostTouchCapture");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnLostTouchCapture(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class TouchEnter
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(TouchEnter),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(TouchEnter),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("TouchEnter");
            var method = typeof(TouchEnter).GetMethod("OnTouchEnter");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnTouchEnter(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class TouchLeave
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(TouchLeave),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(TouchLeave),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("TouchLeave");
            var method = typeof(TouchLeave).GetMethod("OnTouchLeave");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnTouchLeave(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class IsMouseDirectlyOverChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(IsMouseDirectlyOverChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(IsMouseDirectlyOverChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("IsMouseDirectlyOverChanged");
            var method = typeof(IsMouseDirectlyOverChanged).GetMethod("OnIsMouseDirectlyOverChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnIsMouseDirectlyOverChanged(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class IsKeyboardFocusWithinChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(IsKeyboardFocusWithinChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(IsKeyboardFocusWithinChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("IsKeyboardFocusWithinChanged");
            var method = typeof(IsKeyboardFocusWithinChanged).GetMethod("OnIsKeyboardFocusWithinChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnIsKeyboardFocusWithinChanged(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class IsMouseCapturedChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(IsMouseCapturedChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(IsMouseCapturedChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("IsMouseCapturedChanged");
            var method = typeof(IsMouseCapturedChanged).GetMethod("OnIsMouseCapturedChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnIsMouseCapturedChanged(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class IsMouseCaptureWithinChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(IsMouseCaptureWithinChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(IsMouseCaptureWithinChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("IsMouseCaptureWithinChanged");
            var method = typeof(IsMouseCaptureWithinChanged).GetMethod("OnIsMouseCaptureWithinChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnIsMouseCaptureWithinChanged(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class IsStylusDirectlyOverChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(IsStylusDirectlyOverChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(IsStylusDirectlyOverChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("IsStylusDirectlyOverChanged");
            var method = typeof(IsStylusDirectlyOverChanged).GetMethod("OnIsStylusDirectlyOverChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnIsStylusDirectlyOverChanged(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class IsStylusCapturedChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(IsStylusCapturedChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(IsStylusCapturedChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("IsStylusCapturedChanged");
            var method = typeof(IsStylusCapturedChanged).GetMethod("OnIsStylusCapturedChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnIsStylusCapturedChanged(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class IsStylusCaptureWithinChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(IsStylusCaptureWithinChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(IsStylusCaptureWithinChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("IsStylusCaptureWithinChanged");
            var method = typeof(IsStylusCaptureWithinChanged).GetMethod("OnIsStylusCaptureWithinChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnIsStylusCaptureWithinChanged(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class IsKeyboardFocusedChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(IsKeyboardFocusedChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(IsKeyboardFocusedChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("IsKeyboardFocusedChanged");
            var method = typeof(IsKeyboardFocusedChanged).GetMethod("OnIsKeyboardFocusedChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnIsKeyboardFocusedChanged(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class LayoutUpdated
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(LayoutUpdated),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(LayoutUpdated),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("LayoutUpdated");
            var method = typeof(LayoutUpdated).GetMethod("OnLayoutUpdated");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnLayoutUpdated(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class GotFocus
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(GotFocus),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(GotFocus),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("GotFocus");
            var method = typeof(GotFocus).GetMethod("OnGotFocus");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnGotFocus(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class LostFocus
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(LostFocus),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(LostFocus),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("LostFocus");
            var method = typeof(LostFocus).GetMethod("OnLostFocus");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnLostFocus(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class IsEnabledChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(IsEnabledChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(IsEnabledChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("IsEnabledChanged");
            var method = typeof(IsEnabledChanged).GetMethod("OnIsEnabledChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnIsEnabledChanged(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class IsHitTestVisibleChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(IsHitTestVisibleChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(IsHitTestVisibleChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("IsHitTestVisibleChanged");
            var method = typeof(IsHitTestVisibleChanged).GetMethod("OnIsHitTestVisibleChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnIsHitTestVisibleChanged(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class IsVisibleChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(IsVisibleChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(IsVisibleChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("IsVisibleChanged");
            var method = typeof(IsVisibleChanged).GetMethod("OnIsVisibleChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnIsVisibleChanged(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class FocusableChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(FocusableChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(FocusableChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("FocusableChanged");
            var method = typeof(FocusableChanged).GetMethod("OnFocusableChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnFocusableChanged(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class ManipulationStarting
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(ManipulationStarting),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ManipulationStarting),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("ManipulationStarting");
            var method = typeof(ManipulationStarting).GetMethod("OnManipulationStarting");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnManipulationStarting(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class ManipulationStarted
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(ManipulationStarted),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ManipulationStarted),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("ManipulationStarted");
            var method = typeof(ManipulationStarted).GetMethod("OnManipulationStarted");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnManipulationStarted(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class ManipulationDelta
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(ManipulationDelta),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ManipulationDelta),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("ManipulationDelta");
            var method = typeof(ManipulationDelta).GetMethod("OnManipulationDelta");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnManipulationDelta(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class ManipulationInertiaStarting
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(ManipulationInertiaStarting),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ManipulationInertiaStarting),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("ManipulationInertiaStarting");
            var method = typeof(ManipulationInertiaStarting).GetMethod("OnManipulationInertiaStarting");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnManipulationInertiaStarting(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class ManipulationBoundaryFeedback
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(ManipulationBoundaryFeedback),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ManipulationBoundaryFeedback),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("ManipulationBoundaryFeedback");
            var method = typeof(ManipulationBoundaryFeedback).GetMethod("OnManipulationBoundaryFeedback");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnManipulationBoundaryFeedback(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class ManipulationCompleted
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(ManipulationCompleted),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ManipulationCompleted),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(UIElement target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(UIElement target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(UIElement target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("ManipulationCompleted");
            var method = typeof(ManipulationCompleted).GetMethod("OnManipulationCompleted");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnManipulationCompleted(object sender, EventArgs e)
        {
            var control = sender as UIElement;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class TextChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(TextChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(TextChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(TextBoxBase target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(TextBoxBase target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(TextBoxBase target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(TextBoxBase target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("TextChanged");
            var method = typeof(TextChanged).GetMethod("OnTextChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnTextChanged(object sender, EventArgs e)
        {
            var control = sender as TextBoxBase;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class SelectionChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(SelectionChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(SelectionChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(TextBoxBase target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(TextBoxBase target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(TextBoxBase target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(TextBoxBase target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("SelectionChanged");
            var method = typeof(SelectionChanged).GetMethod("OnSelectionChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnSelectionChanged(object sender, EventArgs e)
        {
            var control = sender as TextBoxBase;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class SourceInitialized
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(SourceInitialized),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(SourceInitialized),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(Window target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(Window target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(Window target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(Window target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("SourceInitialized");
            var method = typeof(SourceInitialized).GetMethod("OnSourceInitialized");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnSourceInitialized(object sender, EventArgs e)
        {
            var control = sender as Window;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class Activated
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Activated),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Activated),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(Window target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(Window target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(Window target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(Window target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Activated");
            var method = typeof(Activated).GetMethod("OnActivated");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnActivated(object sender, EventArgs e)
        {
            var control = sender as Window;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class Deactivated
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Deactivated),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Deactivated),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(Window target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(Window target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(Window target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(Window target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Deactivated");
            var method = typeof(Deactivated).GetMethod("OnDeactivated");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnDeactivated(object sender, EventArgs e)
        {
            var control = sender as Window;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class StateChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(StateChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(StateChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(Window target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(Window target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(Window target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(Window target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("StateChanged");
            var method = typeof(StateChanged).GetMethod("OnStateChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnStateChanged(object sender, EventArgs e)
        {
            var control = sender as Window;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class LocationChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(LocationChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(LocationChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(Window target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(Window target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(Window target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(Window target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("LocationChanged");
            var method = typeof(LocationChanged).GetMethod("OnLocationChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnLocationChanged(object sender, EventArgs e)
        {
            var control = sender as Window;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class Closing
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Closing),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Closing),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(Window target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(Window target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(Window target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(Window target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Closing");
            var method = typeof(Closing).GetMethod("OnClosing");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnClosing(object sender, EventArgs e)
        {
            var control = sender as Window;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class Closed
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Closed),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Closed),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(Window target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(Window target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(Window target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(Window target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Closed");
            var method = typeof(Closed).GetMethod("OnClosed");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnClosed(object sender, EventArgs e)
        {
            var control = sender as Window;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class ContentRendered
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(ContentRendered),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ContentRendered),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(Window target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(Window target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(Window target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(Window target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("ContentRendered");
            var method = typeof(ContentRendered).GetMethod("OnContentRendered");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnContentRendered(object sender, EventArgs e)
        {
            var control = sender as Window;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class Click
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Click),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Click),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(MenuItem target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(MenuItem target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(MenuItem target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(MenuItem target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Click");
            var method = typeof(Click).GetMethod("OnClick");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnClick(object sender, EventArgs e)
        {
            var control = sender as MenuItem;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class Checked
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Checked),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Checked),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(MenuItem target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(MenuItem target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(MenuItem target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(MenuItem target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Checked");
            var method = typeof(Checked).GetMethod("OnChecked");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnChecked(object sender, EventArgs e)
        {
            var control = sender as MenuItem;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class Unchecked
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Unchecked),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Unchecked),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(MenuItem target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(MenuItem target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(MenuItem target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(MenuItem target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Unchecked");
            var method = typeof(Unchecked).GetMethod("OnUnchecked");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnUnchecked(object sender, EventArgs e)
        {
            var control = sender as MenuItem;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class SubmenuOpened
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(SubmenuOpened),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(SubmenuOpened),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(MenuItem target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(MenuItem target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(MenuItem target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(MenuItem target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("SubmenuOpened");
            var method = typeof(SubmenuOpened).GetMethod("OnSubmenuOpened");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnSubmenuOpened(object sender, EventArgs e)
        {
            var control = sender as MenuItem;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class SubmenuClosed
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(SubmenuClosed),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(SubmenuClosed),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(MenuItem target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(MenuItem target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(MenuItem target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(MenuItem target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("SubmenuClosed");
            var method = typeof(SubmenuClosed).GetMethod("OnSubmenuClosed");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnSubmenuClosed(object sender, EventArgs e)
        {
            var control = sender as MenuItem;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class DropDownOpened
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(DropDownOpened),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(DropDownOpened),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(ComboBox target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(ComboBox target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(ComboBox target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(ComboBox target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("DropDownOpened");
            var method = typeof(DropDownOpened).GetMethod("OnDropDownOpened");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnDropDownOpened(object sender, EventArgs e)
        {
            var control = sender as ComboBox;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class DropDownClosed
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(DropDownClosed),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(DropDownClosed),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(ComboBox target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(ComboBox target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(ComboBox target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(ComboBox target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("DropDownClosed");
            var method = typeof(DropDownClosed).GetMethod("OnDropDownClosed");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnDropDownClosed(object sender, EventArgs e)
        {
            var control = sender as ComboBox;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class SelectionChangedSelector
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(SelectionChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(SelectionChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(Selector target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(Selector target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(Selector target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(Selector target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("SelectionChanged");
            var method = typeof(SelectionChanged).GetMethod("OnSelectionChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnSelectionChanged(object sender, EventArgs e)
        {
            var control = sender as Selector;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class SelectedItemChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(SelectedItemChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(SelectedItemChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(TreeView target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(TreeView target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(TreeView target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(TreeView target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("SelectedItemChanged");
            var method = typeof(SelectedItemChanged).GetMethod("OnSelectedItemChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnSelectedItemChanged(object sender, EventArgs e)
        {
            var control = sender as TreeView;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class ScrollChanged
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(ScrollChanged),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ScrollChanged),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(ScrollViewer target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(ScrollViewer target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(ScrollViewer target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(ScrollViewer target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("ScrollChanged");
            var method = typeof(ScrollChanged).GetMethod("OnScrollChanged");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnScrollChanged(object sender, EventArgs e)
        {
            var control = sender as ScrollViewer;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class CheckedToggleButton
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Checked),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Checked),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(ToggleButton target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(ToggleButton target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(ToggleButton target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(ToggleButton target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Checked");
            var method = typeof(Checked).GetMethod("OnChecked");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnChecked(object sender, EventArgs e)
        {
            var control = sender as ToggleButton;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class UncheckedToggleButton
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Unchecked),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Unchecked),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(ToggleButton target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(ToggleButton target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(ToggleButton target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(ToggleButton target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Unchecked");
            var method = typeof(Unchecked).GetMethod("OnUnchecked");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnUnchecked(object sender, EventArgs e)
        {
            var control = sender as ToggleButton;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class Indeterminate
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Indeterminate),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Indeterminate),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(ToggleButton target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(ToggleButton target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(ToggleButton target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(ToggleButton target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Indeterminate");
            var method = typeof(Indeterminate).GetMethod("OnIndeterminate");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnIndeterminate(object sender, EventArgs e)
        {
            var control = sender as ToggleButton;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
    public class ClickButtonBase
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(Click),
            new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(Click),
                                                new UIPropertyMetadata(null));

        public static ICommand GetCommand(ButtonBase target) => (ICommand)target.GetValue(CommandProperty);
        public static void SetCommand(ButtonBase target, ICommand value) => target.SetValue(CommandProperty, value);
        public static object GetCommandParameter(ButtonBase target) => target.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(ButtonBase target, object value) => target.SetValue(CommandParameterProperty, value);


        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var type = target.GetType();
            var ev = type.GetEvent("Click");
            var method = typeof(Click).GetMethod("OnClick");

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                ev.AddEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                ev.RemoveEventHandler(target, Delegate.CreateDelegate(ev.EventHandlerType, method));
            }
        }

        public static void OnClick(object sender, EventArgs e)
        {
            var control = sender as ButtonBase;
            var command = (ICommand)control.GetValue(CommandProperty);
            var commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
}

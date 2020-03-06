using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arma.Studio.Data.UI.AttachedProperties
{
    public class ScrollViewerAttached
    { // https://stackoverflow.com/a/47260643/2684203
        #region Vertical
        /// <summary>
        /// VerticalOffset attached property
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached(
                "VerticalOffset",
                typeof(double),
                typeof(ScrollViewerAttached),
                new FrameworkPropertyMetadata(
                    Double.NaN,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnVerticalOffsetPropertyChanged
                    )
                );

        /// <summary>
        /// Just a flag that the binding has been applied.
        /// </summary>
        private static readonly DependencyProperty VerticalScrollBindingProperty =
            DependencyProperty.RegisterAttached(
                "VerticalScrollBinding",
                typeof(bool?),
                typeof(ScrollViewerAttached)
                );

        public static double GetVerticalOffset(DependencyObject depObj)
        {
            return (double)depObj.GetValue(VerticalOffsetProperty);
        }

        public static void SetVerticalOffset(DependencyObject depObj, double value)
        {
            depObj.SetValue(VerticalOffsetProperty, value);
        }

        private static void OnVerticalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;
            if (scrollViewer == null)
            {
                return;
            }

            BindVerticalOffset(scrollViewer);
            scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
        }

        public static void BindVerticalOffset(ScrollViewer scrollViewer)
        {
            if (scrollViewer.GetValue(VerticalScrollBindingProperty) != null)
            {
                return;
            }

            scrollViewer.SetValue(VerticalScrollBindingProperty, true);
            scrollViewer.ScrollChanged += (s, se) =>
            {
                if (se.VerticalChange == 0)
                {
                    return;
                }

                SetVerticalOffset(scrollViewer, se.VerticalOffset);
            };
        }
        #endregion
        #region Horizontal
        /// <summary>
        /// HorizontalOffset attached property
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached(
                "HorizontalOffset",
                typeof(double),
                typeof(ScrollViewerAttached),
                new FrameworkPropertyMetadata(
                    Double.NaN,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnHorizontalOffsetPropertyChanged
                    )
                );

        /// <summary>
        /// Just a flag that the binding has been applied.
        /// </summary>
        private static readonly DependencyProperty HorizontalScrollBindingProperty =
            DependencyProperty.RegisterAttached(
                "HorizontalScrollBinding",
                typeof(bool?),
                typeof(ScrollViewerAttached)
                );

        public static double GetHorizontalOffset(DependencyObject depObj)
        {
            return (double)depObj.GetValue(HorizontalOffsetProperty);
        }

        public static void SetHorizontalOffset(DependencyObject depObj, double value)
        {
            depObj.SetValue(HorizontalOffsetProperty, value);
        }

        private static void OnHorizontalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;
            if (scrollViewer == null)
            {
                return;
            }

            BindHorizontalOffset(scrollViewer);
            scrollViewer.ScrollToHorizontalOffset((double)e.NewValue);
        }

        public static void BindHorizontalOffset(ScrollViewer scrollViewer)
        {
            if (scrollViewer.GetValue(HorizontalScrollBindingProperty) != null)
            {
                return;
            }

            scrollViewer.SetValue(HorizontalScrollBindingProperty, true);
            scrollViewer.ScrollChanged += (s, se) =>
            {
                if (se.HorizontalChange == 0)
                {
                    return;
                }

                SetHorizontalOffset(scrollViewer, se.HorizontalOffset);
            };
        }
        #endregion
    }
}

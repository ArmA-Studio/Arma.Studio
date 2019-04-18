using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Arma.Studio.Data.UI.Converters
{
    /// <summary>
    /// Adds up the left margin of the upper visual tree up to the point where the <see cref="Type"/> is no longer matching starting at 0.
    /// Expects the <see cref="DependencyObject"/> as value, will simply return value if passed value is not of <see cref="Type"/> <see cref="DependencyObject"/>
    /// <see cref="UnsetOnNullConverter.ConvertBack(Object, Type, Object, CultureInfo)"/> will always return <see cref="System.Windows.DependencyProperty.UnsetValue"/>.
    /// </summary>
    public class StaggeredThicknessConverter : IValueConverter
    {
        /// <summary>
        /// The length that should be added each time.
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Type that will be searched for. Continues to count until null or this.
        /// If not set, will count until different parent is encountered.
        /// </summary>
        public Type EndType { get; set; }

        /// <summary>
        /// Sets wether the top part of <see cref="Thickness"/> should be affected.
        /// </summary>
        public bool Top { get; set; }
        /// <summary>
        /// Sets wether the bottom part of <see cref="Thickness"/> should be affected.
        /// </summary>
        public bool Bot { get; set; }
        /// <summary>
        /// Sets wether the left part of <see cref="Thickness"/> should be affected.
        /// </summary>
        public bool Left { get; set; }
        /// <summary>
        /// Sets wether the right part of <see cref="Thickness"/> should be affected.
        /// </summary>
        public bool Right { get; set; }

        /// <summary>
        /// Converts a value
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DependencyObject obj))
            {
                return value;
            }
            var actingtype = obj.GetType();
            Type objtype;
            double leftmargin = 0;

            while ((objtype = (obj = VisualTreeHelper.GetParent(obj)).GetType()).IsEquivalentTo(actingtype) || (this.EndType != null && !objtype.IsEquivalentTo(this.EndType)))
            {
                if (objtype.IsEquivalentTo(actingtype))
                {
                    leftmargin += this.Length;
                }
            }
            return new Thickness(
                this.Left ? leftmargin : 0,
                this.Top ? leftmargin : 0,
                this.Right ? leftmargin : 0,
                this.Bot ? leftmargin : 0);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

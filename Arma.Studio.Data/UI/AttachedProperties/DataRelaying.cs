using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arma.Studio.Data.UI.AttachedProperties
{

    /// <summary>
    /// Allows to tunnel ReadOnly-DependencyProperties like eg. <see cref="FrameworkElement.ActualHeight"/>
    /// </summary>
    public static class DataRelaying
    {
        #region DependencyProperty: DataRelays (SimplifySoft.WPF.AttachedProperties.DataRelayCollection)
        public static readonly DependencyProperty DataRelaysProperty =
            DependencyProperty.RegisterAttached(
                "DataRelays",
                typeof(DataRelayCollection),
                typeof(DataRelaying),
                new UIPropertyMetadata(null));
        public static void SetDataRelays(DependencyObject dependencyObject, DataRelayCollection value) => dependencyObject.SetValue(DataRelaysProperty, value);
        public static DataRelayCollection GetDataRelays(DependencyObject dependencyObject) => (DataRelayCollection)dependencyObject.GetValue(DataRelaysProperty);
        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Arma.Studio.Data.UI.Converters
{
    public class ToCompositeCollectionMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var compositeCollection = new CompositeCollection(values.Length);
            foreach (IEnumerable it in values.Where((it) => it != DependencyProperty.UnsetValue))
            {
                compositeCollection.Add(new CollectionContainer { Collection = it });
            }
            return compositeCollection;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is CompositeCollection compositeCollection)
            {
                return compositeCollection.Cast<object>().Select((it) => it).ToArray();
            }
            else
            {
                return default;
            }
        }
    }
}

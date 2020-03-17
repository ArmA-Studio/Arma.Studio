using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arma.Studio.Data.UI
{
    public class RelayDataTemplateSelector : DataTemplateSelector
    {
        private readonly Control RelayTo;
        public RelayDataTemplateSelector(Control control)
        {
            this.RelayTo = control;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var baseResult = base.SelectTemplate(item, container);
            if (baseResult != null || item is null)
            {
                return baseResult;
            }
            var objType = item.GetType();
            var matches = this.RelayTo.Resources
                .Cast<DictionaryEntry>()
                .Select((it) => it.Value)
                .Where((it) => it is DataTemplate)
                .Cast<DataTemplate>()
                .Where((it) => it.DataType is Type t && t.IsAssignableFrom(objType)).ToArray();
            if (!matches.Any())
            {
                return null;
            }
            // ToDo: Find the best match
            var template = matches.First();
            return (DataTemplate)template;
        }
    }
}

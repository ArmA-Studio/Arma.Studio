using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.PropertiesWindow.PropertyContainers
{
    public class PropertyContainerGroup
    {
        public bool IsExpanded { get; set; }
        public string Title { get; }
        public IEnumerable<PropertyContainerBase> Properties { get; }
        public PropertyContainerGroup(string title, IEnumerable<PropertyContainerBase> properties)
        {
            this.Title = title;
            this.Properties = properties.ToArray();
        }
    }
}

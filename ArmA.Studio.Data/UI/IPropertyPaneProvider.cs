using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data.Configuration;

namespace ArmA.Studio.Data.UI
{
    public interface IPropertyPaneProvider
    {
        IEnumerable<Item> Items { get; }
    }
}

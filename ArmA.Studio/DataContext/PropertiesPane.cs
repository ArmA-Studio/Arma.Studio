using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data.UI;

namespace ArmA.Studio.DataContext
{
    public sealed class PropertiesPane : PanelBase
    {
        public override string Title => Properties.Localization.PanelDisplayName_Properties;
    }
}

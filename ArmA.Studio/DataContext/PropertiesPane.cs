using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.DataContext
{
    public sealed class PropertiesPane : PanelBase
    {
        public override string Title { get { return Properties.Localization.PanelDisplayName_Properties; } }
    }
}

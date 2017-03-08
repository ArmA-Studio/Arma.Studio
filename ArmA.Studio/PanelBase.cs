using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace ArmA.Studio
{
    public abstract class PanelBase : DockableBase
    {
        public virtual bool AutoAddPanel { get { return true; } }
    }
}

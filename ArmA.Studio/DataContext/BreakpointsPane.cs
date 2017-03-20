using System.Windows.Input;
using ArmA.Studio.DataContext.BreakpointsPaneUtil;
using System.Collections.ObjectModel;

namespace ArmA.Studio.DataContext
{
    public class BreakpointsPane : PanelBase
    {
        public static ObservableCollection<Breakpoint> Breakpoints { get; private set; }
        static BreakpointsPane()
        {
            Breakpoints = new ObservableCollection<Breakpoint>();
        }

        public object XAMLBreakpointsReference { get { return Breakpoints; } }

        public override string Title { get { return Properties.Localization.PanelDisplayName_Breakpoints; } }

        public ICommand CmdEntryOnDoubleClick { get; private set; }

        public BreakpointsPane()
        {
            
        }
    }
}
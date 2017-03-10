using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;
using System.Xml;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ArmA.Studio.UI.Commands;
using Utility.Collections;
using ArmA.Studio.DataContext.BreakpointsPaneUtil;
using System.Collections.ObjectModel;

namespace ArmA.Studio.DataContext
{
    public class BreakpointsPane : PanelBase
    {
        public override string Title { get { return Properties.Localization.PanelDisplayName_Breakpoints; } }

        public ICommand CmdEntryOnDoubleClick { get; private set; }

        public ObservableCollection<Breakpoint> Breakpoints { get { return this._Breakpoints; } set { this._Breakpoints = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<Breakpoint> _Breakpoints;

        public BreakpointsPane()
        {
            this._Breakpoints = new ObservableCollection<Breakpoint>();
        }
    }
}
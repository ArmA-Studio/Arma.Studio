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
using Utility.Collections;
using ArmA.Studio.Data.UI;
using ArmA.Studio.Data.UI.Commands;

namespace ArmA.Studio.DataContext
{
    public class SolutionPane : PanelBase
    {
        public override string Title => Properties.Localization.PanelDisplayName_Solution;
    }
}
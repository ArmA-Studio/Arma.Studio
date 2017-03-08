using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;

namespace ArmA.Studio.SolutionUtil
{
    [XmlRoot("folder")]
    public class SolutionFolder : SolutionFileBase
    {
        public override ICommand CmdContextMenu_OpenInExplorer { get { return new UI.Commands.RelayCommand((o) => System.Diagnostics.Process.Start(string.Format("\"{0}\"", this.FullPath))); } }

        public override DataTemplate GetPropertiesTemplate()
        {
            return null;
        }
        protected override void OnMouseDoubleClick(object param) { }
    }
}

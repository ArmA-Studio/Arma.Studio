using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using Utility.Collections;

namespace ArmA.Studio.SolutionUtil
{
    [XmlRoot("config")]
    public class SolutionFile : SolutionFileBase
    {
        public override ICommand CmdContextMenu_OpenInExplorer { get { return new UI.Commands.RelayCommand((o) => System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", this.FullPath))); } }

        public override ObservableSortedCollection<SolutionFileBase> Children { get { return null; } set { } }

        public override DataTemplate GetPropertiesTemplate()
        {
            return null;
        }

        [XmlArray("breakpoints")]
        [XmlArrayItem("breakpoint")]
        public ObservableSortedCollection<int> BreakPoints { get { return this._BreakPoints; } set { this._BreakPoints = value; this.RaisePropertyChanged(); } }
        private ObservableSortedCollection<int> _BreakPoints;

        public SolutionFile()
        {
            this._BreakPoints = new ObservableSortedCollection<int>();
        }
    }
}

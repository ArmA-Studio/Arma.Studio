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
using ArmA.Studio.DataContext.BreakpointsPaneUtil;
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


        [XmlIgnore]
        public string FileContent
        {
            get
            {
                using (var reader = new System.IO.StreamReader(this.FullPath))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        [XmlArray("breakpoints")]
        [XmlArrayItem("breakpoint")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        //ToDo: Register breakpoint in BreakpointsPane when set is callen
        public List<Breakpoint> BreakPoints { get { return this._BreakPoints; } set { this._BreakPoints = value; foreach (var bp in value) bp.FileReference = this; } }
        private List<Breakpoint> _BreakPoints;

        public SolutionFile()
        {
            this._BreakPoints = new List<Breakpoint>();
        }

        //ToDo: Register breakpoint in BreakpointsPane
        public void AddBreakpoint(Breakpoint b)
        {
            if (!this.BreakPoints.Contains(b))
            {
                this.BreakPoints.Add(b);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        public void RemoveBreakpoint(Breakpoint b)
        {
            if (this.BreakPoints.Contains(b))
            {
                this.BreakPoints.Remove(b);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        public void AddBreakpoint(int line, int col)
        {
            var b = new Breakpoint() { FileReference = this, Line = line, LineOffset = col };
            if (!this.BreakPoints.Contains(b))
            {
                this.BreakPoints.Add(b);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        public void RemoveBreakpoint(int line, int col)
        {
            var b = new Breakpoint() { FileReference = this, Line = line, LineOffset = col };
            if (this.BreakPoints.Contains(b))
            {
                this.BreakPoints.Remove(b);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        public Breakpoint GetBreakpoint(int line, int col)
        {
            return this.BreakPoints.FirstOrDefault((b) => b.Line == line && b.LineOffset == col);
        }
        public Breakpoint GetFirstBreakpoint(int line)
        {
            return this.BreakPoints.FirstOrDefault((b) => b.Line == line);
        }
        public List<Breakpoint> GetBreakpoints(int line)
        {
            return this.BreakPoints.FindAll((b) => b.Line == line);
        }
    }
}

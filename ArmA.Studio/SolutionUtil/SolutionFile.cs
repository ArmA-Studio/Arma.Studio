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

        internal void RedrawEditor()
        {
            var ted = WorkspaceOld.CurrentWorkspace.GetDocumentOfSolutionFileBase(this) as DataContext.TextEditorDocument;
            if (ted != null)
            {
                ted.Editor?.TextArea.LeftMargins.All((it) => { it.InvalidateVisual(); return true; });
            }
        }

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

        #region Breakpoint Handling
        [XmlArray("breakpoints")]
        [XmlArrayItem("breakpoint")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        //ToDo: Register breakpoint in BreakpointsPane when set is callen
        public ActedCollection<Breakpoint> BreakPoints
        {
            get
            {
                return this._BreakPoints;
            }
            set
            {
                this._BreakPoints = value;
                this._BreakPoints.OnAdding += BreakPoints_OnAdding;
                this._BreakPoints.OnRemoving += BreakPoints_OnRemoving;
                this._BreakPoints.OnUpdating += BreakPoints_OnUpdating;
                this.RaisePropertyChanged();
            }
        }
        private ActedCollection<Breakpoint> _BreakPoints;

        private void BreakPoints_OnUpdating(object sender, Tuple<Breakpoint, Breakpoint> e)
        {
            DataContext.BreakpointsPane.Breakpoints.Remove(e.Item1);
            e.Item1.PropertyChanged -= BreakPointsChild_PropertyChanged;
            e.Item1.FileReference = null;
            e.Item2.FileReference = this;
            e.Item2.PropertyChanged += BreakPointsChild_PropertyChanged;
            DataContext.BreakpointsPane.Breakpoints.Add(e.Item2);
        }

        private void BreakPoints_OnRemoving(object sender, Breakpoint e)
        {
            DataContext.BreakpointsPane.Breakpoints.Remove(e);
            e.PropertyChanged -= BreakPointsChild_PropertyChanged;
            e.FileReference = null;
        }

        private void BreakPoints_OnAdding(object sender, Breakpoint e)
        {
            e.FileReference = this;
            e.PropertyChanged += BreakPointsChild_PropertyChanged;
            DataContext.BreakpointsPane.Breakpoints.Add(e);
        }

        private void BreakPointsChild_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            WorkspaceOld.CurrentWorkspace.DebugContext.UpdateBreakpoint(sender as Breakpoint);
        }

        public void AddBreakpoint(int line, int col)
        {
            var b = this.BreakPoints.FirstOrDefault((bp) => bp.Line == line && bp.LineOffset == col);
            if (b == null)
            {
                this.BreakPoints.Add(new Breakpoint() { Line = line, LineOffset = col });
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void RemoveBreakpoint(int line, int col)
        {
            var b = this.BreakPoints.FirstOrDefault((bp) => bp.Line == line && bp.LineOffset == col);
            if (b != null)
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

        public IEnumerable<Breakpoint> GetBreakpoints(int line)
        {
            return this.BreakPoints.FindAll((b) => b.Line == line);
        }
        #endregion

        public SolutionFile()
        {
            this.BreakPoints = new ActedCollection<Breakpoint>();
        }
    }
}

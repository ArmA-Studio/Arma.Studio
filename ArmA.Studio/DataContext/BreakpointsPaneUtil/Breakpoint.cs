using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.DataContext.BreakpointsPaneUtil
{
    public class Breakpoint : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public bool IsEnabled { get { return this._IsEnabled; } set { this._IsEnabled = value; this.FileReference?.RedrawEditor(); this.RaisePropertyChanged(); } }
        private bool _IsEnabled;

        public int Line { get { return this._Line; } set { this._Line = value; this.RaisePropertyChanged(); } }
        private int _Line;

        public int LineOffset { get { return this._LineOffset; } set { this._LineOffset = value; this.RaisePropertyChanged(); } }
        private int _LineOffset;

        [System.Xml.Serialization.XmlIgnore]
        public SolutionUtil.SolutionFile FileReference { get { SolutionUtil.SolutionFile sf; _FileReference.TryGetTarget(out sf); return sf; } set { this._FileReference.SetTarget(value); this.RaisePropertyChanged(); } }
        private WeakReference<SolutionUtil.SolutionFile> _FileReference;

        public string Label { get { return this._Label; } set { this._Label = value; this.RaisePropertyChanged(); } }
        private string _Label;

        public string Condition { get { return this._Condition; } set { this._Condition = value; this.RaisePropertyChanged(); } }
        private string _Condition;

        public Breakpoint()
        {
            this._IsEnabled = true;
            this._Line = -1;
            this._LineOffset = -1;
            this._Condition = string.Empty;
            this._Label = string.Empty;
            this._FileReference = new WeakReference<SolutionUtil.SolutionFile>(null);
        }

        public static implicit operator Debugger.Breakpoint(Breakpoint bp)
        {
            return new Debugger.Breakpoint() { ArmAPath = bp.FileReference.ArmAPath, Line = bp.Line };
        }
    }
}

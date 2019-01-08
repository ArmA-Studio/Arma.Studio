using Arma.Studio.Data.Debugging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.UI
{
    public class Breakpoint : INotifyPropertyChanged, INotifyPropertyChanging, IBreakpoint
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callee = "") => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee));
        public event PropertyChangingEventHandler PropertyChanging;
        protected void RaisePropertyChanging([System.Runtime.CompilerServices.CallerMemberName]string callee = "") => this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(callee));

        public string File { get; }

        public int Line { get => this._Line; set { if (this._Line == value) { return; } this.RaisePropertyChanging(); this._Line = value; this.RaisePropertyChanged(); } }
        private int _Line;

        public int Column { get => this._Column; set { if (this._Column == value) { return; } this.RaisePropertyChanging(); this._Column = value; this.RaisePropertyChanged(); } }
        private int _Column;

        public bool IsActive { get => this._IsActive; set { if (this._IsActive == value) { return; } this.RaisePropertyChanging(); this._IsActive = value; this.RaisePropertyChanged(); } }
        private bool _IsActive;

        public int HitCount { get => this._HitCount; set { if (this._HitCount == value) { return; } this.RaisePropertyChanging(); this._HitCount = value; this.RaisePropertyChanged(); } }
        private int _HitCount;

        public EBreakpointKind Kind { get => this._Kind; set { if (this._Kind == value) { return; } this.RaisePropertyChanging(); this._Kind = value; this.RaisePropertyChanged(); } }
        private EBreakpointKind _Kind;

        public Breakpoint(string file)
        {
            this.File = file;
            this._Line = -1;
            this._Column = -1;
            this._IsActive = true;
            this._HitCount = 0;
        }
    }
}

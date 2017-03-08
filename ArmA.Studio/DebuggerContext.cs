using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ArmA.Studio
{
    public class DebuggerContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public bool IsDebuggerAttached { get { return this._IsDebuggerAttached; } set { this._IsDebuggerAttached = value; this.RaisePropertyChanged(); } }
        private bool _IsDebuggerAttached;

        public bool IsPaused { get { return this._IsPaused; } set { this._IsPaused = value; this.RaisePropertyChanged(); } }
        private bool _IsPaused;

        public ICommand CmdRunDebuggerClick { get; private set; }
        public ICommand CmdPauseDebugger { get; private set; }
        public ICommand CmdStopDebugger { get; private set; }
        public ICommand CmdStepInto { get; private set; }
        public ICommand CmdStepOver { get; private set; }
        public ICommand CmdStepOut { get; private set; }


        public DebuggerContext()
        {
            this._IsDebuggerAttached = false;
            this._IsPaused = false;

            this.CmdRunDebuggerClick = new UI.Commands.RelayCommand((p) => { throw new NotImplementedException(); });
            this.CmdPauseDebugger = new UI.Commands.RelayCommand((p) => { throw new NotImplementedException(); });
            this.CmdStopDebugger = new UI.Commands.RelayCommand((p) => { throw new NotImplementedException(); });
            this.CmdStepInto = new UI.Commands.RelayCommand((p) => { throw new NotImplementedException(); });
            this.CmdStepOver = new UI.Commands.RelayCommand((p) => { throw new NotImplementedException(); });
            this.CmdStepOut = new UI.Commands.RelayCommand((p) => { throw new NotImplementedException(); });
        }
    }
}






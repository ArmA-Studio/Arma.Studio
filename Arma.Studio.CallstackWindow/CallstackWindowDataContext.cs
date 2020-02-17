using Arma.Studio.Data;
using Arma.Studio.Data.Debugging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Arma.Studio.CallstackWindow
{
    public class CallstackWindowDataContext : DockableBase, IDisposable
    {
        #region Collection: Callstack (ObservableCollection<SqfVm.VariableReference>)
        public ObservableCollection<HaltInfo> Callstack
        {
            get => this._Callstack;
            set
            {
                this._Callstack = value;
                this.RaisePropertyChanged();
            }
        }
        private ObservableCollection<HaltInfo> _Callstack;
        #endregion


        private void MainWindow_DebuggerStateChanged(object sender, EventArgs e)
        {
            switch ((Application.Current as IApp).MainWindow.Debugger?.State ?? Data.Debugging.EDebugState.NA)
            {
                case Data.Debugging.EDebugState.Running:
                case Data.Debugging.EDebugState.NA:
                    this.Callstack = new ObservableCollection<HaltInfo>();
                    break;
                case Data.Debugging.EDebugState.Halted:
                    this.Callstack = new ObservableCollection<HaltInfo>((Application.Current as IApp).MainWindow.Debugger.GetHaltInfos());
                    break;
            }
        }

        public CallstackWindowDataContext()
        {
            (Application.Current as IApp).MainWindow.DebuggerStateChanged += this.MainWindow_DebuggerStateChanged;
            switch ((Application.Current as IApp).MainWindow.Debugger?.State ?? Data.Debugging.EDebugState.NA)
            {
                case Data.Debugging.EDebugState.Running:
                case Data.Debugging.EDebugState.NA:
                    this.Callstack = new ObservableCollection<HaltInfo>();
                    break;
                case Data.Debugging.EDebugState.Halted:
                    this.Callstack = new ObservableCollection<HaltInfo>((Application.Current as IApp).MainWindow.Debugger.GetHaltInfos());
                    break;
            }
        }

        public override string Title { get => Properties.Language.CallstackWindow; set => throw new NotSupportedException(); }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    (Application.Current as IApp).MainWindow.DebuggerStateChanged -= this.MainWindow_DebuggerStateChanged;
                }
                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
        #endregion
    }
}

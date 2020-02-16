using Arma.Studio.Data;
using Arma.Studio.Data.Debugging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Arma.Studio.LocalsWindow
{
    public class LocalsWindowDataContext : DockableBase, IDisposable
    {
        #region Collection: LocalVariables (ObservableCollection<SqfVm.VariableReference>)
        public ObservableCollection<VariableInfo> LocalVariables
        {
            get => this._LocalVariables;
            set
            {
                if (this._LocalVariables != null)
                {
                    foreach (var it in this._LocalVariables)
                    {
                        it.PropertyChanging -= this.VariableInfo_PropertyChanging;
                        it.PropertyChanged -= this.VariableInfo_PropertyChanged;
                    }
                }
                this._LocalVariables = value;
                this.RaisePropertyChanged();
                if (this._LocalVariables != null)
                {
                    foreach (var it in this._LocalVariables)
                    {
                        it.PropertyChanging += this.VariableInfo_PropertyChanging;
                        it.PropertyChanged += this.VariableInfo_PropertyChanged;
                    }
                }
            }
        }

        private readonly Dictionary<VariableInfo, string> PreviousValueDictionary;
        private void VariableInfo_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (sender is VariableInfo variableInfo)
            {
                if (e.PropertyName == nameof(variableInfo.Data))
                {
                    this.PreviousValueDictionary[variableInfo] = variableInfo.Data;
                }
            }
        }

        private void VariableInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is VariableInfo variableInfo)
            {
                if (e.PropertyName == nameof(variableInfo.Data))
                {
                    variableInfo.PropertyChanged -= this.VariableInfo_PropertyChanged;
                    variableInfo.PropertyChanged -= this.VariableInfo_PropertyChanged;
                    try
                    {
                        if (!(Application.Current as IApp).MainWindow.Debugger.SetVariable(variableInfo.VariableName, variableInfo.Data, ENamespace.Default))
                        {
                            variableInfo.Data = this.PreviousValueDictionary[variableInfo];
                            System.Media.SystemSounds.Beep.Play();
                        }
                        else
                        {
                            var variableInfoUpdated = (Application.Current as IApp).MainWindow.Debugger.GetVariable(variableInfo.VariableName, ENamespace.Default);
                            variableInfo.Data = variableInfoUpdated.Data;
                            variableInfo.DataType = variableInfoUpdated.DataType;
                        }
                    }
                    finally
                    {
                        this.PreviousValueDictionary.Remove(variableInfo);
                        variableInfo.PropertyChanged += this.VariableInfo_PropertyChanged;
                        variableInfo.PropertyChanged += this.VariableInfo_PropertyChanged;
                    }
                }
            }
        }

        private ObservableCollection<VariableInfo> _LocalVariables;
        #endregion


        private void MainWindow_DebuggerStateChanged(object sender, EventArgs e)
        {
            switch ((Application.Current as IApp).MainWindow.Debugger?.State ?? Data.Debugging.EDebugState.NA)
            {
                case Data.Debugging.EDebugState.Running:
                case Data.Debugging.EDebugState.NA:
                    this.LocalVariables = new ObservableCollection<VariableInfo>();
                    break;
                case Data.Debugging.EDebugState.Halted:
                    this.LocalVariables = new ObservableCollection<VariableInfo>((Application.Current as IApp).MainWindow.Debugger.GetLocalVariables());
                    break;
            }
        }

        public LocalsWindowDataContext()
        {
            this.PreviousValueDictionary = new Dictionary<VariableInfo, string>();
            (Application.Current as IApp).MainWindow.DebuggerStateChanged += this.MainWindow_DebuggerStateChanged;
            switch ((Application.Current as IApp).MainWindow.Debugger?.State ?? Data.Debugging.EDebugState.NA)
            {
                case Data.Debugging.EDebugState.Running:
                case Data.Debugging.EDebugState.NA:
                    this.LocalVariables = new ObservableCollection<VariableInfo>();
                    break;
                case Data.Debugging.EDebugState.Halted:
                    this.LocalVariables = new ObservableCollection<VariableInfo>((Application.Current as IApp).MainWindow.Debugger.GetLocalVariables());
                    break;
            }
        }

        public override string Title { get => Properties.Language.LocalsWindow; set => throw new NotSupportedException(); }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.LocalVariables = new ObservableCollection<VariableInfo>();
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

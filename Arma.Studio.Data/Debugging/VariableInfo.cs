using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Debugging
{
    public class VariableInfo : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        protected void RaisePropertyChanging([System.Runtime.CompilerServices.CallerMemberName]string callee = "")
        { this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(callee)); }
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callee = "")
        { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee)); }


        public int ScopeIndex
        {
            get => this._ScopeIndex;
            set
            {
                this.RaisePropertyChanging();
                this._ScopeIndex = value;
                this.RaisePropertyChanged();
            }
        }
        private int _ScopeIndex;
        public string DataType
        {
            get => this._DataType;
            set
            {
                this.RaisePropertyChanging();
                this._DataType = value;
                this.RaisePropertyChanged();
            }
        }
        private string _DataType;
        public string ScopeName
        {
            get => this._ScopeName;
            set
            {
                this.RaisePropertyChanging();
                this._ScopeName = value;
                this.RaisePropertyChanged();
            }
        }
        private string _ScopeName;
        public string VariableName
        {
            get => this._VariableName;
            set
            {
                this.RaisePropertyChanging();
                this._VariableName = value;
                this.RaisePropertyChanged();
            }
        }
        private string _VariableName;
        public string Data
        {
            get => this._Data;
            set
            {
                this.RaisePropertyChanging();
                this._Data = value;
                this.RaisePropertyChanged();
            }
        }
        private string _Data;
    }
}

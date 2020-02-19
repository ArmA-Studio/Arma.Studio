using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Debugging
{
    public class SqfValue : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        protected void RaisePropertyChanging([System.Runtime.CompilerServices.CallerMemberName]string callee = "")
        { this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(callee)); }
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callee = "")
        { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee)); }

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

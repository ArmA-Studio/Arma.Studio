using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Data.Configuration
{
    public abstract class ConfigurationBaseClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string caller = default(string)) { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller)); }

        public string Name { get { return this._Name; } set { if (this._Name == value) return; this._Name = value; this.NotifyPropertyChanged(); } }
        private string _Name;

        public string ImageSource { get { return this._ImageSource; } set { if (this._ImageSource == value) return; this._ImageSource = value; this.NotifyPropertyChanged(); } }
        private string _ImageSource;
    }
}

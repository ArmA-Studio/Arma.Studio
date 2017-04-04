using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Data.UI.Behaviors
{
    public sealed class PropertyPaneSelectionStatic : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public static PropertyPaneSelectionStatic Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new PropertyPaneSelectionStatic();
                return _Instance;
            }
        }
        private static PropertyPaneSelectionStatic _Instance;
        public IPropertyPaneProvider Provider { get { return this._Provider; } set { this._Provider = value; this.RaisePropertyChanged(); } }
        private IPropertyPaneProvider _Provider;
    }
}
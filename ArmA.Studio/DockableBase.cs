using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ArmA.Studio
{
    public abstract class DockableBase : INotifyPropertyChanged, IComparable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public int CompareTo(object obj)
        {
            if (obj is DockableBase)
            {
                return this.Title.CompareTo((obj as DockableBase).Title);
            }
            else
            {
                return this.Title.CompareTo(obj);
            }
        }

        public abstract string Title { get; }
        public ImageSource IconSource { get; }
        public virtual string ContentId { get; set; }
        public bool IsSelected { get { return this._IsSelected; } set { this._IsSelected = value; this.RaisePropertyChanged(); } }
        private bool _IsSelected;
        public bool IsActive { get { return this._IsActive; } set { this._IsActive = value; this.RaisePropertyChanged(); } }
        private bool _IsActive;
        public Visibility CurrentVisibility
        {
            get { return this._CurrentVisibility; }
            set
            {
                this._CurrentVisibility = value;
                this.RaisePropertyChanged();
            }
        }
        private Visibility _CurrentVisibility;
    }
}

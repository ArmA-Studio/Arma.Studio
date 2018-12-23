using ArmA.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ArmA.Studio.Data
{
    public abstract class DockableBase : INotifyPropertyChanged, IComparable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") => Application.Current.Dispatcher.Invoke(() => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)));

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

        public virtual string Title
        {
            get => this._Title;
            set
            {
                this._Title = value;
                this.RaisePropertyChanged();
            }
        }
        private string _Title;
        public string ContentId
        {
            get => this._ContentId;
            set
            {
                this._ContentId = value;
                this.RaisePropertyChanged();
            }
        }
        private string _ContentId;
        public string IconSource
        {
            get => this._IconSource;
            set
            {
                this._IconSource = value;
                this.RaisePropertyChanged();
            }
        }
        private string _IconSource;
        public bool IsSelected
        {
            get => this._IsSelected;
            set
            {
                this._IsSelected = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _IsSelected;
        public bool IsActive
        {
            get => this._IsActive;
            set
            {
                this._IsActive = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _IsActive;
        public bool IsAnchorable
        {
            get => this._IsAnchorable ?? false;
            set
            {
                if (this._IsAnchorable.HasValue)
                {
                    throw new InvalidOperationException();
                }
                this._IsAnchorable = value;
                this.RaisePropertyChanged();
                this.IsAnchorableSet(value);
            }
        }
        private bool? _IsAnchorable;
        public bool IsEnabled
        {
            get => this._IsEnabled;
            set
            {
                this._IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _IsEnabled;

        protected virtual void IsAnchorableSet(bool value) { }

        public Visibility CurrentVisibility { get { return this._CurrentVisibility; } set { this._CurrentVisibility = value; this.RaisePropertyChanged(); } }
        private Visibility _CurrentVisibility;


        public virtual void LayoutLoadCallback(dynamic section) { }
        public virtual void LayoutSaveCallback(dynamic section) { }

        public event EventHandler<DockableBaseOnDocumentClosingEventArgs> OnDockableClosing;
        public event EventHandler OnDockableClose;
        public ICommand CmdClose => new RelayCommand((p) => this.Close());

        protected bool IsCloseInProgress { get; private set; }
        public virtual bool OnClosing() { return false; }
        public virtual void OnClose() { }
        public void Close()
        {
            this.IsCloseInProgress = true;
            var cancel = this.OnClosing();
            var ea = new DockableBaseOnDocumentClosingEventArgs();
            this.OnDockableClosing?.Invoke(this, ea);
            if (cancel || ea.Cancel)
            {
                this.IsCloseInProgress = false;
                return;
            }
            this.OnDockableClose?.Invoke(this, new EventArgs());
            this.OnClose();
            if (this is IDisposable disposable) { disposable.Dispose(); }
            this.IsCloseInProgress = false;
        }

        public DockableBase(string title = "", string iconsource = "")
        {
            this._IconSource = iconsource;
            this._Title = title;
        }
    }
}

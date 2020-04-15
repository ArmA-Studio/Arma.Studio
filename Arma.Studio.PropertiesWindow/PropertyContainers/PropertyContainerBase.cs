using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Arma.Studio.PropertiesWindow.PropertyContainers
{
    public abstract class PropertyContainerBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string callee = "")
        { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee)); }

        public readonly Func<object, object> GetFunc;
        public readonly Action<object, object> SetFunc;
        public readonly object Data;
        public readonly string PropertyName;


        public object Value
        {
            get => this.GetFunc(this.Data);
            set
            {
                this.SetFunc(this.Data, value);
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// UI-Configuration.
        /// The Minimum value the property is supposed to have.
        /// </summary>
        public double MinValue { get; set; }
        /// <summary>
        /// UI-Configuration.
        /// The Maximum value the property is supposed to have.
        /// </summary>
        public double MaxValue { get; set; }
        /// <summary>
        /// UI-Configuration.
        /// Stepsize for numeric up-down.
        /// </summary>
        public double Stepsize { get; set; }

        public string Title { get; }
        public string ToolTip { get; }
        public PropertyContainerBase(string title, string tooltip, object data, string propertyName, Func<object, object> getFunc, Action<object, object> setFunc)
        {
            this.Title = title;
            this.ToolTip = tooltip;
            this.Data = data;
            this.GetFunc = getFunc;
            this.SetFunc = setFunc;
            if (data is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged += this.NotifyPropertyChanged_PropertyChanged;
            }
            this.PropertyName = propertyName;
        }

        private void NotifyPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.PropertyName)
            {
                this.RaisePropertyChanged(nameof(this.Value));
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (this.Data is INotifyPropertyChanged notifyPropertyChanged)
                {
                    notifyPropertyChanged.PropertyChanged -= this.NotifyPropertyChanged_PropertyChanged;
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
using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Arma.Studio.Data
{

    public class BusyContainer : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The <see cref="System.Threading.CancellationTokenSource"/> this <see cref="BusyContainer"/>
        /// is using to optionally cancel.
        /// May be null.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Wether or not Cancellation is supported.
        /// </summary>
        public bool SupportsCancellation => this.CancellationTokenSource != null;


        /// <summary>
        /// Wether the "nice cancellation" was requested via the <see cref="CancellationTokenSource"/>.
        /// </summary>
        public bool WasCancelled
        {
            get => this._WasCancelled;
            set
            {
                if (this._WasCancelled == value)
                {
                    return;
                }
                this._WasCancelled = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool _WasCancelled;

        public void Cancel()
        {
            this.WasCancelled = true;
            this.CancellationTokenSource.Cancel();
        }
        public void Cancel(TimeSpan timeSpan)
        {
            this.WasCancelled = true;
            this.CancellationTokenSource.CancelAfter(timeSpan);
        }
        public ICommand CmdCancel => new RelayCommand((p) =>
        {
            if (p is TimeSpan timeSpan)
            {
                this.Cancel(timeSpan);
            }
            else
            {
                this.Cancel();
            }
        });

        public void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string caller = default)
        { System.Windows.Application.Current.Dispatcher.Invoke(() => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller))); }

        /// <summary>
        /// The text that represents what is happening right now.
        /// </summary>
        public string Text
        {
            get => this._Text;
            set
            {
                if (this._Text == value)
                {
                    return;
                }
                this._Text = value; this.NotifyPropertyChanged();
            }
        }
        private string _Text;

        /// <summary>
        /// The minimum <see cref="Value"/> possible.
        /// </summary>
        public double MinValue
        {
            get => this._MinValue;
            set
            {
                if (this._MinValue == value)
                {
                    return;
                }
                this._MinValue = value;
                this.NotifyPropertyChanged();
            }
        }
        private double _MinValue;

        /// <summary>
        /// The maximum <see cref="Value"/> possible.
        /// </summary>
        public double MaxValue
        {
            get => this._MaxValue;
            set
            {
                if (this._MaxValue == value)
                {
                    return;
                }
                this._MaxValue = value;
                this.NotifyPropertyChanged();
            }
        }
        private double _MaxValue;

        /// <summary>
        /// Current value this <see cref="BusyContainer"/> instance withholds.
        /// </summary>
        public double Value
        {
            get => this._Value;
            set
            {
                if (this._Value == value)
                {
                    return;
                }
                this._Value = value;
                this.NotifyPropertyChanged();
            }
        }
        private double _Value;

        /// <summary>
        /// Indicates wether or not the current progress of this <see cref="BusyContainer"/> instance is determinable or not.
        /// </summary>
        public bool IsIndeterminate
        {
            get => this._IsIndeterminate;
            set
            {
                if (this._IsIndeterminate == value)
                {
                    return;
                }
                this._IsIndeterminate = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool _IsIndeterminate;

        /// <summary>
        /// Creates a new <see cref="BusyContainer"/> with <see cref="IsIndeterminate"/> set to true.
        /// </summary>
        /// <param name="textToDisplay">The text, this container should display.</param>
        public BusyContainer(string textToDisplay) : this(textToDisplay, 0, 1)
        {
            this._IsIndeterminate = true;
        }
        /// <summary>
        /// Creates a new <see cref="BusyContainer"/> with <see cref="IsIndeterminate"/> set to true.
        /// </summary>
        /// <param name="textToDisplay">The text, this container should display.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to check wether or not to cancell.</param>
        public BusyContainer(string textToDisplay, out CancellationToken cancellationToken) : this(textToDisplay, 0, 1, out cancellationToken)
        {
            this._IsIndeterminate = true;
        }

        /// <summary>
        /// Creates a new <see cref="BusyContainer"/> with <see cref="IsIndeterminate"/> set to false.
        /// 
        /// </summary>
        /// <param name="textToDisplay">The text, this container should display.</param>
        /// <param name="minValue">Minimum <see cref="Value"/> possible. Will be saved into <see cref="MinValue"/>.</param>
        /// <param name="maxValue">Maximum <see cref="Value"/> possible. Will be saved into <see cref="MaxValue"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to check wether or not to cancell.</param>
        public BusyContainer(string textToDisplay, double minValue, double maxValue)
        {
            this._Text = textToDisplay;
            this._MinValue = minValue;
            this._MaxValue = maxValue;
            this._Value = minValue;
            this._IsIndeterminate = false;
            this.CancellationTokenSource = null;
        }

        /// <summary>
        /// Creates a new <see cref="BusyContainer"/> with <see cref="IsIndeterminate"/> set to false.
        /// 
        /// </summary>
        /// <param name="textToDisplay">The text, this container should display.</param>
        /// <param name="minValue">Minimum <see cref="Value"/> possible. Will be saved into <see cref="MinValue"/>.</param>
        /// <param name="maxValue">Maximum <see cref="Value"/> possible. Will be saved into <see cref="MaxValue"/>.</param>
        public BusyContainer(string textToDisplay,
            double minValue,
            double maxValue,
            out CancellationToken cancellationToken)
        {
            this._Text = textToDisplay;
            this._MinValue = minValue;
            this._MaxValue = maxValue;
            this._Value = minValue;
            this._IsIndeterminate = false;
            this.CancellationTokenSource = new CancellationTokenSource();
            cancellationToken = this.CancellationTokenSource.Token;
        }


        public override string ToString()
        {
            if (this.IsIndeterminate)
            {
                return $"BusyContainer (indenterminate): {this.Text}";
            }
            else
            {
                return $"BusyContainer ({this.MinValue} <= {this.Value} <= {this.MaxValue}): {this.Text}";
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                this.CancellationTokenSource?.Dispose();

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

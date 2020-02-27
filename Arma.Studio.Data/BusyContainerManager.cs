using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Arma.Studio.Data
{
    public class BusyContainerManager : INotifyPropertyChanged, ICollection<BusyContainer>
    {
        public sealed class BusyContainerAutoRemove : BusyContainer
        {
            private readonly BusyContainerManager Manager;
            public BusyContainerAutoRemove(BusyContainerManager manager, string textToDisplay) : base(textToDisplay)
            {
                this.Manager = manager;
                this.Manager.SetBusyContainerStatus(this, true);
            }

            public BusyContainerAutoRemove(BusyContainerManager manager, string textToDisplay, double minValue, double maxValue) : base(textToDisplay, minValue, maxValue)
            {
                this.Manager = manager;
                this.Manager.SetBusyContainerStatus(this, true);
            }
            public BusyContainerAutoRemove(BusyContainerManager manager, string textToDisplay, out CancellationToken cancellationToken) : base(textToDisplay, out cancellationToken)
            {
                this.Manager = manager;
                this.Manager.SetBusyContainerStatus(this, true);
            }

            public BusyContainerAutoRemove(BusyContainerManager manager,
                string textToDisplay,
                double minValue,
                double maxValue,
                out CancellationToken cancellationToken) : base(textToDisplay, minValue, maxValue, out cancellationToken)
            {
                this.Manager = manager;
                this.Manager.SetBusyContainerStatus(this, true);
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                this.Manager.SetBusyContainerStatus(this, false);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string caller = default) { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller)); }

        public ObservableCollection<BusyContainer> Items { get { return this._Items; } set { this._Items = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<BusyContainer> _Items;

        int ICollection<BusyContainer>.Count => ((ICollection<BusyContainer>)this._Items).Count;
        bool ICollection<BusyContainer>.IsReadOnly => ((ICollection<BusyContainer>)this._Items).IsReadOnly;

        public BusyContainerManager()
        {
            this._Items = new ObservableCollection<BusyContainer>();
        }
        public void SetBusyContainerStatus(BusyContainer container, bool isActive)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (isActive)
                {
                    if (!this.Contains(container))
                    {
                        this.Items.Add(container);
                    }
                }
                else
                {
                    if (this.Contains(container))
                    {
                        this.Items.Remove(container);
                    }
                }
            });
        }

        /// <summary>
        /// Creates a new <see cref="BusyContainerAutoRemove"/> instance.
        /// Can be used in conjunction with the using syntax.
        /// </summary>
        /// <param name="textToDisplay">The text to display</param>
        /// <returns>A <see cref="BusyContainer"/> that implements the <see cref="IDisposable"/> interface.</returns>
        public BusyContainerAutoRemove Busy(string textToDisplay)
        {
            return new BusyContainerAutoRemove(this, textToDisplay);
        }
        /// <summary>
        /// Creates a new <see cref="BusyContainerAutoRemove"/> instance.
        /// Can be used in conjunction with the using syntax.
        /// </summary>
        /// <param name="textToDisplay">The text to display</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to check wether or not to cancell.</param>
        /// <returns>A <see cref="BusyContainer"/> that implements the <see cref="IDisposable"/> interface.</returns>
        public BusyContainerAutoRemove Busy(string textToDisplay, out CancellationToken cancellationToken)
        {
            return new BusyContainerAutoRemove(this, textToDisplay, out cancellationToken);
        }
        /// <summary>
        /// Creates a new <see cref="BusyContainerAutoRemove"/> instance.
        /// Can be used in conjunction with the using syntax.
        /// </summary>
        /// <param name="textToDisplay">The text to display</param>
        /// <returns>A <see cref="BusyContainer"/> that implements the <see cref="IDisposable"/> interface.</returns>
        public BusyContainerAutoRemove Busy(string textToDisplay, double minValue, double maxValue)
        {
            return new BusyContainerAutoRemove(this, textToDisplay, minValue, maxValue);
        }
        /// <summary>
        /// Creates a new <see cref="BusyContainerAutoRemove"/> instance.
        /// Can be used in conjunction with the using syntax.
        /// </summary>
        /// <param name="textToDisplay">The text to display</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to check wether or not to cancell.</param>
        /// <returns>A <see cref="BusyContainer"/> that implements the <see cref="IDisposable"/> interface.</returns>
        public BusyContainerAutoRemove Busy(string textToDisplay, double minValue, double maxValue, out CancellationToken cancellationToken)
        {
            return new BusyContainerAutoRemove(this, textToDisplay, minValue, maxValue, out cancellationToken);
        }

        public void Add(BusyContainer item) => this.SetBusyContainerStatus(item, true);
        void ICollection<BusyContainer>.Clear() => ((ICollection<BusyContainer>)this._Items).Clear();
        public bool Contains(BusyContainer item) => ((ICollection<BusyContainer>)this._Items).Contains(item);
        void ICollection<BusyContainer>.CopyTo(BusyContainer[] array, int arrayIndex) => ((ICollection<BusyContainer>)this._Items).CopyTo(array, arrayIndex);
        public IEnumerator<BusyContainer> GetEnumerator() => ((ICollection<BusyContainer>)this._Items).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((ICollection<BusyContainer>)this._Items).GetEnumerator();

        public bool Remove(BusyContainer item)
        {
            this.SetBusyContainerStatus(item, false);
            return true;
        }


        public Task Run(Func<Task> func, string text)
        {
            var cancelable = new BusyContainerAutoRemove(this, text);
            var task = Task.Run(async () =>
            {
                try
                {
                    using (cancelable)
                    {
                        await func();
                    }
                }
                catch (Exception ex)
                {
                    (Application.Current as IApp).DisplayOperationFailed(ex);
                    throw new BusyContainerRethrowException(Properties.Language.SeeInnerException, ex);
                }
            });
            task.ContinueWith((t) =>
            {
                cancelable.Dispose();
            }, TaskContinuationOptions.OnlyOnCanceled);
            return task;
        }

        public Task Run(Func<CancellationToken, Task> func, string text)
        {
            var cancelable = new BusyContainerAutoRemove(this, text, out var token);
            var task = Task.Run(async () =>
            {
                try
                {
                    using (cancelable)
                    {
                        await func(token);
                    }
                }
                catch (Exception ex)
                {
                    (Application.Current as IApp).DisplayOperationFailed(ex);
                    throw new BusyContainerRethrowException(Properties.Language.SeeInnerException, ex);
                }
            }, token);
            task.ContinueWith((t) =>
            {
                cancelable.Dispose();
            }, TaskContinuationOptions.OnlyOnCanceled);
            return task;
        }
        public Task Run(Func<BusyContainer, CancellationToken, Task> func, string text)
        {
            var cancelable = new BusyContainerAutoRemove(this, text, out var token);
            var task = Task.Run(async () =>
            {
                try
                {
                    using (cancelable)
                    {
                        await func(cancelable, token);
                    }
                }
                catch (Exception ex)
                {
                    (Application.Current as IApp).DisplayOperationFailed(ex);
                    throw new BusyContainerRethrowException(Properties.Language.SeeInnerException, ex);
                }
            }, token);
            task.ContinueWith((t) =>
            {
                cancelable.Dispose();
            }, TaskContinuationOptions.OnlyOnCanceled);
            return task;
        }
        public Task Run(Func<BusyContainer, CancellationToken, Task> func, string text, double minValue, double maxValue)
        {
            var cancelable = new BusyContainerAutoRemove(this, text, minValue, maxValue, out var token);
            var task = Task.Run(async () =>
            {
                try
                {
                    using (cancelable)
                    {
                        await func(cancelable, token);
                    }
                }
                catch (Exception ex)
                {
                    (Application.Current as IApp).DisplayOperationFailed(ex);
                    throw new BusyContainerRethrowException(Properties.Language.SeeInnerException, ex);
                }
            }, token);
            task.ContinueWith((t) =>
            {
                cancelable.Dispose();
            }, TaskContinuationOptions.OnlyOnCanceled);
            return task;
        }
        public Task<TResult> Run<TResult>(Func<CancellationToken, Task<TResult>> func, string text)
        {
            var cancelable = new BusyContainerAutoRemove(this, text, out var token);
            var task = Task.Run(async () =>
            {
                try
                {
                    using (cancelable)
                    {
                        return await func(token);
                    }
                }
                catch (Exception ex)
                {
                    (Application.Current as IApp).DisplayOperationFailed(ex);
                    throw new BusyContainerRethrowException(Properties.Language.SeeInnerException, ex);
                }
            });
            task.ContinueWith((t) =>
            {
                cancelable.Dispose();
            }, TaskContinuationOptions.OnlyOnCanceled);
            return task;
        }
        public Task<TResult> Run<TResult>(Func<BusyContainer, CancellationToken, Task<TResult>> func, string text)
        {
            var cancelable = new BusyContainerAutoRemove(this, text, out var token);
            var task = Task.Run(async () =>
            {
                try
                {
                    using (cancelable)
                    {
                        return await func(cancelable, token);
                    }
                }
                catch (Exception ex)
                {
                    (Application.Current as IApp).DisplayOperationFailed(ex);
                    throw new BusyContainerRethrowException(Properties.Language.SeeInnerException, ex);
                }
            }, token);
            task.ContinueWith((t) =>
            {
                cancelable.Dispose();
            }, TaskContinuationOptions.OnlyOnCanceled);
            return task;
        }
        public Task<TResult> Run<TResult>(Func<BusyContainer, CancellationToken, Task<TResult>> func, string text, double minValue, double maxValue)
        {
            var cancelable = new BusyContainerAutoRemove(this, text, minValue, maxValue, out var token);
            var task = Task.Run(async () =>
            {
                try
                {
                    using (cancelable)
                    {
                        return await func(cancelable, token);
                    }
                }
                catch (Exception ex)
                {
                    (Application.Current as IApp).DisplayOperationFailed(ex);
                    throw new BusyContainerRethrowException(Properties.Language.SeeInnerException, ex);
                }
            }, token);
            task.ContinueWith((t) =>
            {
                cancelable.Dispose();
            }, TaskContinuationOptions.OnlyOnCanceled);
            return task;
        }
    }
}

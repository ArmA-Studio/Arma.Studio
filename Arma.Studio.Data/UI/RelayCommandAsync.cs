/*
MIT License

Copyright (c) 2016 Marco Silipo (X39)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
namespace Arma.Studio.Data.UI
{
    public class RelayCommandAsync<T> : ICommand, INotifyPropertyChanged
    {
        public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }
        private readonly Func<T, Task> execute;
        private Task awaitable;
        private readonly Predicate<T> canExecute;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callee = "") => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee));

        public bool IsRunning { get { return this._IsRunning; } private set { this._IsRunning = value; this.RaisePropertyChanged(); } }
        private bool _IsRunning;

        public RelayCommandAsync(Func<Task> execute) : this((p) => execute(), DefaultCanExecute)
        {
        }
        public RelayCommandAsync(Func<T, Task> execute) : this(execute, DefaultCanExecute)
        {
        }

        public RelayCommandAsync(Func<T, Task> execute, Predicate<T> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
            this.awaitable = null;
            this._IsRunning = false;
        }
        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute((T)parameter) && (this.awaitable == null || this.awaitable.IsCompleted);
        }
        public void Execute(object parameter)
        {
            this.IsRunning = true;
            this.awaitable = this.execute((T)parameter).ContinueWith((t) => this.IsRunning = false);
        }

        private static bool DefaultCanExecute(T parameter)
        {
            return true;
        }
    }
    public class RelayCommandAsync : ICommand, INotifyPropertyChanged
    {
        public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }
        private readonly Func<object, Task> execute;
        private Task awaitable;
        private readonly Predicate<object> canExecute;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callee = "") => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee));

        public bool IsRunning { get { return this._IsRunning; } private set { this._IsRunning = value; this.RaisePropertyChanged(); } }
        private bool _IsRunning;

        public RelayCommandAsync(Func<Task> execute) : this((p) => execute(), DefaultCanExecute)
        {
        }
        public RelayCommandAsync(Func<object, Task> execute) : this(execute, DefaultCanExecute)
        {
        }

        public RelayCommandAsync(Func<object, Task> execute, Predicate<object> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
            this.awaitable = null;
            this._IsRunning = false;
        }
        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter) && (this.awaitable == null || this.awaitable.IsCompleted);
        }
        public void Execute(object parameter)
        {
            this.IsRunning = true;
            this.awaitable = this.execute(parameter).ContinueWith((t) => this.IsRunning = false);
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
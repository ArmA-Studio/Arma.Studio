using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.IO
{
    public abstract class FileFolderBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callee = "") => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee));


        public FileFolderBase Parent
        {
            get => this.ParentWeak.TryGetTarget(out var target) ? target : null;
            set { this.ParentWeak.SetTarget(value); this.RaisePropertyChanged(); }
        }
        private readonly WeakReference<FileFolderBase> ParentWeak;
        public FileFolderBase()
        {
            this.ParentWeak = new WeakReference<FileFolderBase>(null);
        }
    }
}

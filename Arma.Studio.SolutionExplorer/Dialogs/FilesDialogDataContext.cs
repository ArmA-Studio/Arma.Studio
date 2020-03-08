using Arma.Studio.Data;
using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Arma.Studio.SolutionExplorer.Dialogs
{
    public class FilesDialogDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string callee = "")
        { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee)); }
        public IEnumerable<EditorInfo> EditorInfos { get; }
        public EditorInfo SelectedEditorInfo
        {
            get => this._SelectedEditorInfo;
            set
            {
                this._SelectedEditorInfo = value;
                this.RaisePropertyChanged();
                if (value != null)
                {
                    this.FileName = String.Concat(value.Name, value.Extensions.First());
                }
                this.RaisePropertyChanged(nameof(this.OKButtonEnabled));
            }
        }
        private EditorInfo _SelectedEditorInfo;

        public string FileName
        {
            get => this._FileName;
            set
            {
                this._FileName = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.OKButtonEnabled));
            }
        }
        private string _FileName;

        public bool OKButtonEnabled => !String.IsNullOrWhiteSpace(this.FileName) &&
            this.FileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) == -1 &&
            this.SelectedEditorInfo != null &&
            !System.IO.Directory.GetFiles(this.basePath, "*", System.IO.SearchOption.TopDirectoryOnly).Any((path) => System.IO.Path.GetFileNameWithoutExtension(path) == System.IO.Path.GetFileNameWithoutExtension(this.FileName));

        private readonly string basePath;
        public FilesDialogDataContext(string basePath)
        {
            this.basePath = basePath;
            this.EditorInfos = (Application.Current as IApp).MainWindow.EditorInfos;
        }
    }
}

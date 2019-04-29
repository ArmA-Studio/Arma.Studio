using Arma.Studio.Data;
using Arma.Studio.Data.IO;
using Arma.Studio.Data.TextEditor;
using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Arma.Studio.SolutionExplorer
{
    public class SolutionExplorerDataContext : DockableBase
    {
        public object InRenameMode
        {
            get => this._InRenameMode;
            set
            {
                this._InRenameMode = value;
                this.RaisePropertyChanged();
            }
        }
        private object _InRenameMode;

        public ICommand CmdReleaseRename => new RelayCommand(() => this.InRenameMode = null);
        public ICommand CmdOpen => new RelayCommand<FileFolderBase>((ffb) =>
        {
            if (ffb is File file)
            {
                (Application.Current as IApp).MainWindow.OpenFile(file);
            }
        });
        public override string Title { get => Properties.Language.SolutionExplorer; set { throw new NotSupportedException(); } }

        public IFileManagement FileManagement => (Application.Current as IApp).MainWindow.FileManagement;
    }
}

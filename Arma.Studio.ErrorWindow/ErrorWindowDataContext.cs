using Arma.Studio.Data;
using Arma.Studio.Data.TextEditor;
using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Arma.Studio.ErrorWindow
{
    public class ErrorWindowDataContext : DockableBase
    {
        public override string Title { get => Properties.Language.ErrorWindow; set => throw new NotSupportedException(); }
        public ObservableCollection<LintInfo> LintInfos => Instanceable<PluginMain>.Instance.LintInfos;

        #region Property: CurrentErrorCount (System.Int32)
        public int CurrentErrorCount
        {
            get => this._CurrentErrorCount;
            set
            {
                this._CurrentErrorCount = value;
                this.RaisePropertyChanged();
            }
        }
        private int _CurrentErrorCount;
        #endregion
        #region Property: CurrentWarningCount (System.Int32)
        public int CurrentWarningCount
        {
            get => this._CurrentWarningCount;
            set
            {
                this._CurrentWarningCount = value;
                this.RaisePropertyChanged();
            }
        }
        private int _CurrentWarningCount;
        #endregion
        #region Property: CurrentInfoCount (System.Int32)
        public int CurrentInfoCount
        {
            get => this._CurrentInfoCount;
            set
            {
                this._CurrentInfoCount = value;
                this.RaisePropertyChanged();
            }
        }
        private int _CurrentInfoCount;
        #endregion

        public ICommand CmdEntryOnDoubleClick => new RelayCommand<LintInfo>((lintinfo) =>
        {
            if ((Application.Current as IApp).MainWindow.FileManagement.ContainsKey(lintinfo.File))
            {
                var file = (Application.Current as IApp).MainWindow.FileManagement[lintinfo.File] as Data.IO.File;
                (Application.Current as IApp).MainWindow.OpenFile(file).ContinueWith((textDocument) =>
                {
                    textDocument.Result.ScrollTo((int)lintinfo.Line, (int)lintinfo.Column);
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }
        });
        public ErrorWindowDataContext()
        {
            this.LintInfos.CollectionChanged += this.LintInfos_CollectionChanged;
            this.CurrentErrorCount = this.LintInfos.Count((it) => it.Severity == ESeverity.Error);
            this.CurrentWarningCount = this.LintInfos.Count((it) => it.Severity == ESeverity.Warning);
            this.CurrentInfoCount = this.LintInfos.Count((it) => it.Severity == ESeverity.Info || it.Severity == ESeverity.Diagnostic || it.Severity == ESeverity.Trace);
        }

        private void LintInfos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.CurrentErrorCount = this.LintInfos.Count((it) => it.Severity == ESeverity.Error);
            this.CurrentWarningCount = this.LintInfos.Count((it) => it.Severity == ESeverity.Warning);
            this.CurrentInfoCount = this.LintInfos.Count((it) => it.Severity == ESeverity.Info || it.Severity == ESeverity.Diagnostic || it.Severity == ESeverity.Trace);
        }
    }
}

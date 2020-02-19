using Arma.Studio.Data;
using Arma.Studio.Data.UI;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Windows;
using System.Windows.Input;

namespace Arma.Studio.ImmediateWindow
{
    public class ImmediateWindowDataContext : DockableBase
    {

        public TextDocument TextDocument { get; }
        public ImmediateWindowDataContext()
        {
            this.TextDocument = new TextDocument();
        }

        public ICommand CmdClearOutputWindow => new RelayCommand(() => Application.Current.Dispatcher.Invoke(() => this.TextDocument.Text = string.Empty));
        public override string Title { get => Properties.Language.ImmediateWindow; set { throw new NotSupportedException(); } }
    }
}
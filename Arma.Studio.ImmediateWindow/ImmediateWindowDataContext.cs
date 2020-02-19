using Arma.Studio.Data;
using Arma.Studio.Data.UI;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Windows;
using System.Windows.Input;

namespace Arma.Studio.ImmediateWindow
{
    public class ImmediateWindowDataContext : DockableBase, Data.UI.AttachedProperties.IOnInitialized
    {

        public TextDocument TextDocument { get; }
        public ImmediateWindowDataContext()
        {
            this.TextDocument = new TextDocument();
            this.TextDocument.TextChanged += this.TextDocument_TextChanged; ;
        }
        #region Property: IsReadOnly (System.Boolean)
        public bool IsReadOnly
        {
            get => this._IsReadOnly;
            set
            {
                if (this._IsReadOnly == value)
                {
                    return;
                }
                this._IsReadOnly = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _IsReadOnly;
        #endregion

        private int StartIndex = 0;

        public ICommand CmdClearOutputWindow => new RelayCommand(() =>
        {
            this.StartIndex = 0;
            Application.Current.Dispatcher.Invoke(() => this.TextDocument.Text = String.Empty);
        });
        public override string Title { get => Properties.Language.ImmediateWindow; set => throw new NotSupportedException(); }


        private void TextDocument_TextChanged(object sender, EventArgs e)
        {
            if (this.TextDocument.TextLength < this.StartIndex)
            {
                this.StartIndex = this.TextDocument.TextLength;
            }
            else
            {
                string text = this.TextDocument.GetText(this.StartIndex, this.TextDocument.TextLength - this.StartIndex);
                if (text.TrimEnd(' ', '\t').EndsWith("\n") && !String.IsNullOrWhiteSpace(text))
                {
                    this.IsReadOnly = true;
                    this.TextDocument.TextChanged -= this.TextDocument_TextChanged;
                    try
                    {
                        text = text.Trim();
                        var res = (Application.Current as IApp).MainWindow.Debugger?.Evaluate(text);
                        if (String.IsNullOrWhiteSpace(res?.DataType))
                        {
                            this.TextDocument.Insert(this.TextDocument.TextLength, $"Error\n");
                        }
                        else
                        {
                            this.TextDocument.Insert(this.TextDocument.TextLength, $"{res.DataType} - {res.Data}\n");
                        }

                    }
                    finally
                    {
                        Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            this.StartIndex = this.TextDocument.TextLength;
                            this.IsReadOnly = false;
                            this.TextDocument.TextChanged += this.TextDocument_TextChanged;
                        }, System.Windows.Threading.DispatcherPriority.Input);
                    }
                }
            }
        }
        private TextEditor TextEditor;
        public void OnInitialized(FrameworkElement sender, EventArgs e)
        {
            if (sender is TextEditor textEditor)
            {
                this.TextEditor = textEditor;
                this.TextEditor.TextArea.Caret.PositionChanged += this.Caret_PositionChanged;
            }
        }

        private void Caret_PositionChanged(object sender, EventArgs e)
        {
            this.IsReadOnly = this.TextEditor.CaretOffset < this.StartIndex;
        }
    }
}
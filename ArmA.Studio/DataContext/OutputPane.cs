using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;
using System.Xml;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ArmA.Studio.UI.Commands;
using Utility.Collections;

namespace ArmA.Studio.DataContext
{
    public class OutputPane : PanelBase
    {
        private static OutputPane Instance;
        private static readonly TextDocument NullDocument = new TextDocument();
        private static Dictionary<string, TextDocument> DocumentDictionary { get; set; }
        private static void Logger_OnLog(object sender, LoggerTargets.SubscribableTarget.OnLogEventArgs e)
        {
            App.Current.Dispatcher.InvokeAsync(() =>
            {
                if (!DocumentDictionary.ContainsKey(e.Logger))
                {
                    DocumentDictionary.Add(e.Logger, new TextDocument());
                    Instance?.AvailableTargets.Add(e.Logger);
                    if (Instance != null && Instance.SelectedTarget == null)
                    {
                        Instance.SelectedTarget = e.Logger;
                    }
                }
                var doc = DocumentDictionary[e.Logger];
                doc.Insert(doc.TextLength, string.Concat(e.Severity, ": ", e.Message, "\r\n"));
            });
        }
        static OutputPane()
        {
            DocumentDictionary = new Dictionary<string, TextDocument>();
            App.SubscribableLoggerTarget.OnLog += Logger_OnLog;
        }

        public override string Title { get { return Properties.Localization.PanelDisplayName_Output; } }

        public override string Icon { get { return @"Resources\Pictograms\Output\Output.ico"; } }

        public ICommand CmdClearOutputWindow { get; private set; }


        public TextDocument Document { get { return this.SelectedTarget == null ? NullDocument : DocumentDictionary[this.SelectedTarget as string]; } }

        public object SelectedTarget { get { return this._SelectedTarget; } set { this._SelectedTarget = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(this.Document)); } }
        private object _SelectedTarget;

        public ObservableSortedCollection<string> AvailableTargets { get { return this._AvailableTargets; } set { this._AvailableTargets = value; this.RaisePropertyChanged(); } }
        private ObservableSortedCollection<string> _AvailableTargets;

        public OutputPane()
        {
            this._AvailableTargets = new ObservableSortedCollection<string>(DocumentDictionary.Keys);
            this.CmdClearOutputWindow = new RelayCommand((p) => this.Document.Text = string.Empty);
            Instance = this;
        }

    }
}
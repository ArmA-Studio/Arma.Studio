using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ArmA.Studio.Data.UI;
using ArmA.Studio.Data.UI.Commands;
using ArmA.Studio.LoggerTargets;
using ICSharpCode.AvalonEdit.Document;
using Utility.Collections;
using Localization = ArmA.Studio.Properties.Localization;

namespace ArmA.Studio.DataContext
{
    public class OutputPane : PanelBase
    {
        private static OutputPane Instance;
        private static readonly TextDocument NullDocument = new TextDocument();
        private ObservableSortedCollection<string> _AvailableTargets;
        private object _SelectedTarget;

        public OutputPane()
        {
            this._AvailableTargets = new ObservableSortedCollection<string>(DocumentDictionary.Keys);
            this.CmdClearOutputWindow = new RelayCommand(p => this.Document.Text = string.Empty);
            Instance = this;
        }

        private static Dictionary<string, TextDocument> DocumentDictionary { get; set; }

        public override string Title => Localization.PanelDisplayName_Output;

        public override string Icon => @"Resources\Pictograms\Output\Output.ico";

        public ICommand CmdClearOutputWindow { get; }


        public TextDocument Document => !(this.SelectedTarget is string)
            ? NullDocument
            : DocumentDictionary[(string) this.SelectedTarget];

        public object SelectedTarget
        {
            get { return this._SelectedTarget; }
            set
            {
                this._SelectedTarget = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.Document));
            }
        }

        public ObservableSortedCollection<string> AvailableTargets
        {
            get { return this._AvailableTargets; }
            set
            {
                this._AvailableTargets = value;
                this.RaisePropertyChanged();
            }
        }

        private static void Logger_OnLog(object sender, SubscribableTarget.OnLogEventArgs e)
        {
            if (Application.Current == null)
            {
                return;
            }
            Application.Current.Dispatcher.InvokeAsync(() =>
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
                doc.Insert(doc.TextLength,
                    string.Concat(DateTime.Now.ToString("HH:mm:ss"), " - ", e.Severity, ": ", e.Message, "\r\n"));
            });
        }

        internal static void Initialize()
        {
            DocumentDictionary = new Dictionary<string, TextDocument>();
            App.SubscribableLoggerTarget.OnLog += Logger_OnLog;
        }
    }
}
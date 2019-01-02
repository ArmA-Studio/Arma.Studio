using Arma.Studio.Data.TextEditor;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arma.Studio.UI
{
    public class TextEditorDataContext : INotifyPropertyChanged, Data.UI.AttachedProperties.IOnInitialized
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callee = "") => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee));

        public TextDocument TextDocument { get; }
        public ITextEditor TextEditorInstance
        {
            get => this._TextEditorInstance;
            set { this._TextEditorInstance = value; this.RaisePropertyChanged(); }
        }
        private ITextEditor _TextEditorInstance;
        public TextEditor TextEditorControl
        {
            get => this._TextEditorControl;
            set { this._TextEditorControl = value; this.RaisePropertyChanged(); }
        }
        private TextEditor _TextEditorControl;
        public int CaretOffset
        {
            get => this._CaretOffset;
            set { this._CaretOffset = value; this.RaisePropertyChanged(); }
        }
        private int _CaretOffset;
        //IHighlightingDefinition

        public bool IsReadOnly
        {
            get => this._IsReadOnly;
            set { this._IsReadOnly = value; this.RaisePropertyChanged(); }
        }
        private bool _IsReadOnly;

        public IHighlightingDefinition SyntaxHighlightingDefinition
        {
            get => this._SyntaxHighlightingDefinition;
            set { this._SyntaxHighlightingDefinition = value; this.RaisePropertyChanged(); }
        }
        private IHighlightingDefinition _SyntaxHighlightingDefinition;

        public TextEditorDataContext()
        {
            this.TextDocument = new TextDocument();
            new TextEditor().Encoding = Encoding.UTF8;
        }
        private void CreateHighlightingDefinition(SyntaxFile syntaxFile)
        {
            using (var memstream = new System.IO.MemoryStream())
            {
                using (var writer = System.Xml.XmlWriter.Create(memstream))
                {
                    writer.WriteStartElement("SyntaxDefinition");
                    writer.WriteAttributeString("name", "Config");
                    {
                        writer.WriteStartElement("Digits");
                        writer.WriteAttributeString("name", "Digits");
                        writer.WriteAttributeString("color", syntaxFile.DigitsColor.ToString());
                        writer.WriteEndElement();

                        writer.WriteStartElement("RuleSets");
                        writer.WriteStartElement("RuleSet");
                        writer.WriteAttributeString("ignorecase", syntaxFile.IgnoreCase.ToString().ToLower());
                        {
                            writer.WriteElementString("Delimiters", syntaxFile.Delimeters);

                            int i = 0;
                            foreach (var it in syntaxFile.Enclosures)
                            {
                                writer.WriteStartElement("Span");
                                writer.WriteAttributeString("name", $"OpenClose{i++}");
                                writer.WriteAttributeString("color", it.Color.ToString());
                                if (String.IsNullOrWhiteSpace(it.Close))
                                {
                                    writer.WriteAttributeString("stopateol", "true");
                                }
                                writer.WriteElementString("Begin", it.Open);
                                if (!String.IsNullOrWhiteSpace(it.Close))
                                {
                                    writer.WriteElementString("End", it.Close);
                                }
                                writer.WriteEndElement();
                            }

                            i = 0;
                            foreach(var it in syntaxFile.Keywords)
                            {
                                writer.WriteStartElement("KeyWords");
                                writer.WriteAttributeString("name", $"ControlStructure{i++}");
                                writer.WriteAttributeString("color", it.Color.ToString());
                                writer.WriteAttributeString("bold", it.IsBold.ToString().ToLower());
                                foreach (var key in it)
                                {
                                    writer.WriteStartElement("Key");
                                    writer.WriteAttributeString("word", syntaxFile.IgnoreCase ? key.ToLower() : key);
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                memstream.Seek(0, System.IO.SeekOrigin.Begin);
                using (var reader = System.Xml.XmlReader.Create(memstream))
                {
                    this.SyntaxHighlightingDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
        }

        public void OnInitialized(FrameworkElement sender, EventArgs e)
        {
            if (sender is TextEditor textEditor)
            {
                this.TextEditorControl = textEditor;
                var bpm = new BreakPointMargin();
                textEditor.TextArea.TextView.BackgroundRenderers.Add(new UnderlineBackgroundRenderer(this));
                textEditor.TextArea.TextView.BackgroundRenderers.Add(bpm);
                textEditor.TextArea.LeftMargins.Insert(0, bpm);
                textEditor.TextArea.LeftMargins.Insert(1, new RuntimeExecutionMargin(this));
            }
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using ArmA.Studio.Data.UI;
using ArmA.Studio.Data.UI.Commands;
using ArmA.Studio.DataContext.VariablesViewPaneUtil;

namespace ArmA.Studio.DataContext
{
    public class VariablesViewPane : PanelBase
    {
        public override string Title { get { return Properties.Localization.PanelDisplayName_VariablesView; } }

        public ObservableCollection<ItemContainer> Variables { get { return this._Variables; } set { this._Variables = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<ItemContainer> _Variables;

        public bool IsAvailable { get { return this._IsAvailable; } set { this._IsAvailable = value; this.RaisePropertyChanged(); } }
        private bool _IsAvailable;

        private readonly ICommand RemoveVariableCmd;


        public VariablesViewPane()
        {
            this.RemoveVariableCmd = new RelayCommand((para) => { this.Variables.Remove(para as ItemContainer); });
            Variables = new ObservableCollection<ItemContainer>();
            this.Variables.Add(new ItemContainer());
            this.Variables.Last().PropertyChanged += LastItemContainer_PropertyChanged;
            this._IsAvailable = false;
            Task.Run(() =>
            {
                System.Threading.SpinWait.SpinUntil(() => Workspace.Instance != null);
                System.Threading.SpinWait.SpinUntil(() => Workspace.Instance.DebugContext != null);
                Workspace.Instance.DebugContext.PropertyChanged += DebugContext_PropertyChanged;
            });
        }

        private void LastItemContainer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var ic = sender as ItemContainer;
            if (ic == null)
                throw new InvalidOperationException();
            if (e.PropertyName == nameof(ItemContainer.Name))
            {
                ic.PropertyChanged -= LastItemContainer_PropertyChanged;
                ic.CmdDelete = this.RemoveVariableCmd;
                this.Variables.Add(new ItemContainer());
                this.Variables.Last().PropertyChanged += LastItemContainer_PropertyChanged;
            }
        }

        private void DebugContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(DebuggerContext.IsPaused):
                    if (Workspace.Instance.DebugContext.IsPaused)
                    {
                        this.IsAvailable = true;
                        foreach (var it in this.Variables)
                        {
                            it.UpdateValue();
                        }
                    }
                    else
                    {
                        this.IsAvailable = false;
                    }
                    break;
                case nameof(DebuggerContext.IsDebuggerAttached):
                    if (!Workspace.Instance.DebugContext.IsDebuggerAttached)
                    {
                        this.IsAvailable = false;
                    }
                    break;
            }
        }

        public void LoadVariables(System.IO.Stream stream)
        {
            var reader = XmlReader.Create(stream);
            this.Variables.Clear();
            while (reader.Read())
            {
                try
                {
                    if (reader.Name.Equals("item"))
                    {
                        var str = reader.ReadElementContentAsString();
                        this.Variables.Add(new ItemContainer() { Name = str, CmdDelete = this.RemoveVariableCmd });
                    }
                }
                catch { }
            }
            this.Variables.Add(new ItemContainer());
            this.Variables.Last().PropertyChanged += LastItemContainer_PropertyChanged;
        }
        public void SaveVariables(System.IO.Stream stream)
        {
            var writer = XmlWriter.Create(stream, new XmlWriterSettings()
            {
                Indent = true
            });

            writer.WriteStartDocument();
            writer.WriteStartElement("root");
            foreach (var container in this.Variables)
            {
                if (string.IsNullOrWhiteSpace(container.Name))
                    continue;
                writer.WriteElementString("item", container.Name);
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            stream.Flush();
        }
        
    }
}
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ArmA.Studio.Data.UI;
using ArmA.Studio.Data.UI.Commands;

namespace ArmA.Studio.DataContext
{
    public class VariablesViewPane : PanelBase
    {
        public sealed class VariableViewContainer : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }
            
            public string Name { get { return this._Name; } set { this._Name = value; this.UpdateValue(); this.RaisePropertyChanged(); } }
            private string _Name;

            public string Value { get { return this._Value; } set { this._Value = value; this.RaisePropertyChanged(); } }
            private string _Value;

            public ICommand CmdDelete { get; set; }

            public async void UpdateValue()
            {
                var vars = await Workspace.Instance.DebugContext.GetVariablesAsync(Debugger.EVariableNamespace.All, this.Name);
                if (vars.Any())
                    this.Value = vars.First().Value;
                else
                    this.Value = "NA";
            }
        }
        public override string Title { get { return Properties.Localization.PanelDisplayName_VariablesView; } }

        public ObservableCollection<VariableViewContainer> Variables { get { return this._Variables; } set { this._Variables = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<VariableViewContainer> _Variables;

        public ICommand CmdAdd
        {
            get
            {
                return new RelayCommand((p) =>
                {
                    VariableViewContainer container = null;
                    container = new VariableViewContainer() { Name = string.Empty, Value = "NA", CmdDelete = new RelayCommand((para) => this.Variables.Remove(container)) };
                    this.Variables.Add(container);
                });
            }
        }

        public VariablesViewPane()
        {
            Variables = new ObservableCollection<VariableViewContainer>();
            Task.Run(() =>
            {
                System.Threading.SpinWait.SpinUntil(() => Workspace.Instance != null);
                Workspace.Instance.DebugContext.PropertyChanged += DebugContext_PropertyChanged;
            });
        }

        private void DebugContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(DebuggerContext.IsPaused):
                    if(Workspace.Instance.DebugContext.IsPaused)
                    {
                        foreach(var it in this.Variables)
                        {
                            it.Value = "NA";
                        }
                    }
                    else
                    {
                        foreach (var it in this.Variables)
                        {
                            it.UpdateValue();
                        }
                    }
                    break;
                case nameof(DebuggerContext.IsDebuggerAttached):
                    if (!Workspace.Instance.DebugContext.IsDebuggerAttached)
                    {
                        foreach (var it in this.Variables)
                        {
                            it.Value = "NA";
                        }
                    }
                    break;
            }
        }
    }
}
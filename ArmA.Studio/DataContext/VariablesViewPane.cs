using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
        
        public VariablesViewPane()
        {
            Variables = new ObservableCollection<ItemContainer>();
            Variables.Add(new ItemContainer());
            Task.Run(() =>
            {
                System.Threading.SpinWait.SpinUntil(() => Workspace.Instance != null);
                System.Threading.SpinWait.SpinUntil(() => Workspace.Instance.DebugContext != null);
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
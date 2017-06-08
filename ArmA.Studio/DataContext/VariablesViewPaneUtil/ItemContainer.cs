using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ArmA.Studio.Data.UI.Commands;

namespace ArmA.Studio.DataContext.VariablesViewPaneUtil
{
    public class ItemContainer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }
        
        public string Name { get { return this._Name; } set { if (this._Name == value) return; if (string.IsNullOrWhiteSpace(value) && this.CmdDelete != null) { this.CmdDelete.Execute(this); return; } this._Name = value; this.UpdateValue(); this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(IsExpression)); } }
        private string _Name;
        public bool EditName { get { return this._EditName; } set { this._EditName = value; this.RaisePropertyChanged(); } }
        private bool _EditName;
        public bool IsExpression => !Name.All((c) => char.IsLetterOrDigit(c) || c == '_');
        public bool NeedsUpdate { get { return this._NeedsUpdate; } set { if (this._NeedsUpdate == value) return; this._NeedsUpdate = value; this.RaisePropertyChanged(); } }
        private bool _NeedsUpdate;
        public bool IsUpdating { get { return this._IsUpdating; } set { if (this._IsUpdating == value) return; this._IsUpdating = value; this.RaisePropertyChanged(); } }
        private bool _IsUpdating;


        public string Value { get { return this._Value; } set { this._Value = value; this.RaisePropertyChanged(); } }
        private string _Value;
        public string ValueType { get { return this._ValueType; } set { this._ValueType = value; this.RaisePropertyChanged(); } }
        private string _ValueType;

        private DateTime LastUp;
        private int UpCount;

        public ICommand CmdDelete { get { return this._CmdDelete; } set { this._CmdDelete = value; this.RaisePropertyChanged(); } }
        private ICommand _CmdDelete;
        public ICommand CmdTextBoxLostKeyboardFocus => new RelayCommand((p) => { this.EditName = false; });
        public ICommand CmdNameClick => new RelayCommand((p) =>
        {
            if (DateTime.Now - this.LastUp <= new TimeSpan(0, 0, 0, 0, SystemInformation.DoubleClickTime))
            {
                UpCount++;
            }
            else
            {
                UpCount = 1;
            }
            if (UpCount == 2)
            {
                this.EditName = true;
            }
            this.LastUp = DateTime.Now;
        });

        public ItemContainer()
        {
            this._Name = string.Empty;
            this._ValueType = string.Empty;
            this.LastUp = DateTime.MinValue;
            this.UpCount = 0;
        }


        public async void UpdateValue()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                return;
            if (!Workspace.Instance.DebugContext.IsDebuggerAttached || !Workspace.Instance.DebugContext.IsPaused)
            {
                this.NeedsUpdate = true;
                this.Value = null;
                return;
            }
            if (this.IsExpression)
            {
                this.NeedsUpdate = true;
            }
            else
            {
                this.IsUpdating = true;
                try
                {
                    var vars = await Workspace.Instance.DebugContext.GetVariablesAsync(Debugger.EVariableNamespace.All, this.Name);
                    if (vars.Any())
                    {
                        var v = vars.First();
                        this.Value = v.Value;
                        this.ValueType = v.VariableType.ToString();
                    }
                    else
                    {
                        this.Value = null;
                    }
                }
                catch
                {
                    this.Value = null;
                }
                this.IsUpdating = false;
            }
        }
    }
}
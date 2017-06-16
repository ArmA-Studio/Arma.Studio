using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ArmA.Studio.Data;
using ArmA.Studio.Data.UI.Commands;
using Utility;

namespace ArmA.Studio.Dialogs
{
    public class EditBreakpointDialogDataContext : IDialogContext, INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }


        public ICommand CmdOKButtonPressed => new RelayCommand((p) => this.DialogResult = true);
        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;
        public string WindowHeader => Properties.Localization.EditBreakpointDialog_Header;
        public string OKButtonText => Properties.Localization.OK;
        public bool OKButtonEnabled => true;

        public BreakpointInfo BP { get; private set; }


        public int Line { get { return this._Line; } set { this._Line = value; this.RaisePropertyChanged(); } }
        private int _Line;

        public bool ConditionEnabled { get { return this._ConditionEnabled; } set { this._ConditionEnabled = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(Condition)); } }
        private bool _ConditionEnabled;

        public string Condition { get { return this._Condition; } set { this._Condition = value; this.RaisePropertyChanged(); } }
        private string _Condition;

        public bool IsActive { get { return this._IsActive; } set { this._IsActive = value; this.RaisePropertyChanged(); } }
        private bool _IsActive;

        private readonly IEnumerable<Tuple<string, Func<object, string>>> ErrorsList = new Tuple<string, Func<object, string>>[] {
            new Tuple<string, Func<object, string>>(
                nameof(Condition), 
                (o) => string.IsNullOrWhiteSpace((o as EditBreakpointDialogDataContext).Condition) && (o as EditBreakpointDialogDataContext).ConditionEnabled ? "Condition needs to be set" : null
            )
        };
        public bool HasErrors => ErrorsList.Any((t) => !string.IsNullOrWhiteSpace(t.Item2.Invoke(this)));
        public IEnumerable GetErrors(string propertyName) => ErrorsList.Where((t) => t.Item1.Equals(propertyName)).Select((t) => t.Item2.Invoke(this)).Where((s) => !string.IsNullOrWhiteSpace(s));

        public EditBreakpointDialogDataContext(BreakpointInfo bpi)
        {
            this.BP = bpi;
            this.Line = this.BP.Line;
            this.Condition = this.BP.SqfCondition;
            this.IsActive = this.BP.IsEnabled;
            this.ConditionEnabled = !string.IsNullOrWhiteSpace(this.BP.SqfCondition);
        }

        public BreakpointInfo GetUpdatedBPI() => new BreakpointInfo()
        {
            FileRef = this.BP.FileRef,
            IsEnabled = this.IsActive,
            Line = this.Line,
            SqfCondition = this.ConditionEnabled ? this.Condition : string.Empty
        };
    }
}

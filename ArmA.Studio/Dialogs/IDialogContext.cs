using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ArmA.Studio.Dialogs
{
    interface IDialogContext : INotifyPropertyChanged
    {
        ICommand CmdOKButtonPressed { get; }
        string WindowHeader { get; }
        string OKButtonText { get; }
        bool OKButtonEnabled { get; }
        bool? DialogResult { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ArmA.Studio.UI
{
    public class UserInteractionEventArgs : EventArgs
    {
        public ICommand ExecutedCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }

        public UserInteractionEventArgs(ICommand cmdExecuted, ICommand undoCmd)
        {
            this.ExecutedCommand = cmdExecuted;
            this.UndoCommand = undoCmd;
        }
    }
}

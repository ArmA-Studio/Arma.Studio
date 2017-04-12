using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ArmA.Studio.Data;

namespace ArmA.Studio
{
    public class ASKeyContainer : INotifyPropertyChanged
    {
        public static implicit operator ASKeyContainer(KeyContainer cont) => new ASKeyContainer(cont);
        public static implicit operator KeyContainer(ASKeyContainer cont) => cont.Original;

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public readonly KeyContainer Original;

        public ASKeyContainer(KeyContainer original)
        {
            this.Original = original;
        }

        public bool IsPressed() => this.Original.DefaultKeys.All((k) => Keyboard.IsKeyDown(k));
        public void Call() => this.Original.KeyAction.Invoke(this.Original.KeyActionParameter);
    }
}
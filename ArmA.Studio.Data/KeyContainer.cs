using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ArmA.Studio.Data
{
    public class KeyContainer
    {
        public IEnumerable<Key> DefaultKeys { get; private set; }
        public string Name { get; private set; }
        public Action<object> KeyAction { get; private set; }
        public object KeyActionParameter { get; private set; }

        public KeyContainer(string name, IEnumerable<Key> defaultKeys, Action<object> action) : this(name, defaultKeys, action, null) { }
        public KeyContainer(string name, IEnumerable<Key> defaultKeys, Action<object> action, object parameter)
        {
            this.DefaultKeys = defaultKeys;
            this.Name = name;
            this.KeyAction = action;
            this.KeyActionParameter = parameter;
        }
    }
}
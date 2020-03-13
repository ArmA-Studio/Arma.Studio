using System.Collections.Generic;
using System.Windows.Input;

namespace Arma.Studio.Data.UI
{
    public class HotkeyManager
    {
        private static readonly Key[] ModificatorKeyDefinition = { Key.LeftAlt, Key.LeftCtrl, Key.LeftShift, Key.RightAlt, Key.RightCtrl, Key.RightShift };
        public class Container
        {
            public HotKeyCallback Callback { get; }
            public Key Key { get; set; }
            public bool Ctrl { get; set; }
            public bool Alt { get; set; }
            public bool Shift { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }

            public Container(HotKeyCallback cb) : this(cb, Key.NoName) { }
            public Container(HotKeyCallback cb, Key key, string name = "", string description = "", bool ctrl = false, bool alt = false, bool shift = false)
            {
                this.Callback = cb;
                this.Key = key;
                this.Ctrl = ctrl;
                this.Alt = alt;
                this.Shift = shift;
                this.Name = name;
                this.Description = description;
            }
        }
        public delegate void HotKeyCallback();

        private readonly List<Container> Callbacks;

        public IEnumerable<Container> Containers => this.Callbacks;

        public HotkeyManager()
        {
            this.Callbacks = new List<Container>();
        }

        public void RegisterCallback(Container container)
        {
            this.Callbacks.Add(container);
        }

        public bool KeyDown(KeyEventArgs e)
        {
            foreach (var cb in this.Callbacks)
            {
                if (
                       (cb.Key != e.Key)
                    || (cb.Ctrl && !(e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl)))
                    || (cb.Alt && !(e.KeyboardDevice.IsKeyDown(Key.LeftAlt) || e.KeyboardDevice.IsKeyDown(Key.RightAlt)))
                    || (cb.Shift && !(e.KeyboardDevice.IsKeyDown(Key.LeftShift) || e.KeyboardDevice.IsKeyDown(Key.RightShift)))
                    )
                {
                    continue;
                }
                cb.Callback();
                return true;
            }
            return false;
        }
    }
}
using ArmA.Studio.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.UiEditor
{
    public class EditorDataContext : DockableBase
    {
        public ObservableCollection<Control> Controls { get => this._Controls; set { this._Controls = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<Control> _Controls;

        public ObservableCollection<Control> SelectedControls { get => this._SelectedControls; set { this._SelectedControls = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<Control> _SelectedControls;

        public EditorDataContext()
        {
            this._Controls = new ObservableCollection<Control>
            {
                new Control(this)
                {
                    PositionX = 100,
                    PositionY = 100,
                    Width = 50,
                    Height = 50
                },
                new Control(this)
                {
                    PositionX = 200,
                    PositionY = 200,
                    Width = 50,
                    Height = 50
                }
            };
            this.SelectedControls = new ObservableCollection<Control>();
        }
    }
}

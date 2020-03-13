using Arma.Studio.Data;
using Arma.Studio.Data.UI.AttachedProperties;
using Arma.Studio.UiEditor.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Arma.Studio.UiEditor.UI
{
    public class EditorToolboxDataContext  :
        DockableBase,
        IOnMouseMove,
        IOnMouseLeftButtonDown
    {
        public ObservableCollection<EditorToolboxItem> Items { get => this._Items; set { this._Items = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<EditorToolboxItem> _Items;

        public EditorToolboxItem SelectedItem { get => this._SelectedItem; set { this._SelectedItem = value; this.RaisePropertyChanged(); } }
        private EditorToolboxItem _SelectedItem;


        public override string Title { get => Properties.Language.UIEditor_Toolbox; set { throw new NotSupportedException(); } }

        public EditorToolboxDataContext()
        {
            var controlTypes = Enum.GetValues(typeof(EControlType)).Cast<EControlType>();
            this.Items = new ObservableCollection<EditorToolboxItem>(
                controlTypes
                .Select((controlType) => {
                    string name = Enum.GetName(typeof(EControlType), controlType);
                    var fieldInfo = typeof(EControlType).GetField(name);
                    if (fieldInfo == null)
                    {
                        return default;
                    }
                    var controlTypeAttribute = fieldInfo.GetCustomAttributes(typeof(ControlTypeAttribute), false).Cast<ControlTypeAttribute>().FirstOrDefault();
                    return new
                    {
                        Name = Studio.Data.UI.Converters.EnumNameConverter.Instance.Convert(typeof(EControlType), controlType),
                        Value = controlType,
                        Attribute = controlTypeAttribute
                    };
                })
                .Where((obj) => obj != null && obj.Attribute != null)
                .Select((obj) => new EditorToolboxItem { Name = obj.Name, Type = obj.Value, IconPath = obj.Attribute.IconPath }));
        }

        public Point MouseDownPosition { get; private set; }
        public void OnMouseLeftButtonDown(UIElement sender, MouseButtonEventArgs e) => this.MouseDownPosition = e.GetPosition(sender);
        public void OnMouseMove(UIElement sender, MouseEventArgs e)
        {
            if (this.SelectedItem != null && e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                var vec = Point.Subtract(e.GetPosition(sender), this.MouseDownPosition);
                var distX = vec.X > 0 ? vec.X : -vec.X;
                var distY = vec.Y > 0 ? vec.Y : -vec.Y;
                if (distX >= SystemParameters.MinimumHorizontalDragDistance || distY >= SystemParameters.MinimumVerticalDragDistance)
                {
                    DragDrop.DoDragDrop(sender, new DragDropInfo(this.SelectedItem), DragDropEffects.Move);
                }
            }
        }
    }
}

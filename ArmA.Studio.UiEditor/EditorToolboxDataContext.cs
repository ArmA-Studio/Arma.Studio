using Arma.Studio.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Arma.Studio.UiEditor
{
    public class EditorToolboxDataContext  :
        DockableBase,
        Data.UI.AttachedProperties.IOnMouseMove,
        Data.UI.AttachedProperties.IOnMouseLeftButtonDown
    {
        public ObservableCollection<EditorToolboxItem> Items { get => this._Items; set { this._Items = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<EditorToolboxItem> _Items;

        public EditorToolboxItem SelectedItem { get => this._SelectedItem; set { this._SelectedItem = value; this.RaisePropertyChanged(); } }
        private EditorToolboxItem _SelectedItem;

        public EditorToolboxDataContext()
        {
            this.Items = new ObservableCollection<EditorToolboxItem>()
            {
                new EditorToolboxItem { Name = Properties.Language.CT_STATIC, Type = EControlType.CT_STATIC },
                new EditorToolboxItem { Name = Properties.Language.CT_BUTTON, Type = EControlType.CT_BUTTON },
                new EditorToolboxItem { Name = Properties.Language.CT_EDIT, Type = EControlType.CT_EDIT },
                new EditorToolboxItem { Name = Properties.Language.CT_SLIDER, Type = EControlType.CT_SLIDER },
                new EditorToolboxItem { Name = Properties.Language.CT_COMBO, Type = EControlType.CT_COMBO },
                new EditorToolboxItem { Name = Properties.Language.CT_LISTBOX, Type = EControlType.CT_LISTBOX },
                new EditorToolboxItem { Name = Properties.Language.CT_TOOLBOX, Type = EControlType.CT_TOOLBOX },
                new EditorToolboxItem { Name = Properties.Language.CT_CHECKBOXES, Type = EControlType.CT_CHECKBOXES },
                new EditorToolboxItem { Name = Properties.Language.CT_PROGRESS, Type = EControlType.CT_PROGRESS },
                new EditorToolboxItem { Name = Properties.Language.CT_HTML, Type = EControlType.CT_HTML },
                new EditorToolboxItem { Name = Properties.Language.CT_STATIC_SKEW, Type = EControlType.CT_STATIC_SKEW },
                new EditorToolboxItem { Name = Properties.Language.CT_ACTIVETEXT, Type = EControlType.CT_ACTIVETEXT },
                new EditorToolboxItem { Name = Properties.Language.CT_TREE, Type = EControlType.CT_TREE },
                new EditorToolboxItem { Name = Properties.Language.CT_STRUCTURED_TEXT, Type = EControlType.CT_STRUCTURED_TEXT },
                new EditorToolboxItem { Name = Properties.Language.CT_CONTEXT_MENU, Type = EControlType.CT_CONTEXT_MENU },
                new EditorToolboxItem { Name = Properties.Language.CT_CONTROLS_GROUP, Type = EControlType.CT_CONTROLS_GROUP },
                new EditorToolboxItem { Name = Properties.Language.CT_SHORTCUTBUTTON, Type = EControlType.CT_SHORTCUTBUTTON },
                new EditorToolboxItem { Name = Properties.Language.CT_HITZONES, Type = EControlType.CT_HITZONES },
                new EditorToolboxItem { Name = Properties.Language.CT_VEHICLETOGGLES, Type = EControlType.CT_VEHICLETOGGLES },
                new EditorToolboxItem { Name = Properties.Language.CT_CONTROLS_TABLE, Type = EControlType.CT_CONTROLS_TABLE },
                new EditorToolboxItem { Name = Properties.Language.CT_XKEYDESC, Type = EControlType.CT_XKEYDESC },
                new EditorToolboxItem { Name = Properties.Language.CT_XBUTTON, Type = EControlType.CT_XBUTTON },
                new EditorToolboxItem { Name = Properties.Language.CT_XLISTBOX, Type = EControlType.CT_XLISTBOX },
                new EditorToolboxItem { Name = Properties.Language.CT_XSLIDER, Type = EControlType.CT_XSLIDER },
                new EditorToolboxItem { Name = Properties.Language.CT_XCOMBO, Type = EControlType.CT_XCOMBO },
                new EditorToolboxItem { Name = Properties.Language.CT_ANIMATED_TEXTURE, Type = EControlType.CT_ANIMATED_TEXTURE },
                new EditorToolboxItem { Name = Properties.Language.CT_MENU, Type = EControlType.CT_MENU },
                new EditorToolboxItem { Name = Properties.Language.CT_MENU_STRIP, Type = EControlType.CT_MENU_STRIP },
                new EditorToolboxItem { Name = Properties.Language.CT_CHECKBOX, Type = EControlType.CT_CHECKBOX },
                new EditorToolboxItem { Name = Properties.Language.CT_OBJECT, Type = EControlType.CT_OBJECT },
                new EditorToolboxItem { Name = Properties.Language.CT_OBJECT_ZOOM, Type = EControlType.CT_OBJECT_ZOOM },
                new EditorToolboxItem { Name = Properties.Language.CT_OBJECT_CONTAINER, Type = EControlType.CT_OBJECT_CONTAINER },
                new EditorToolboxItem { Name = Properties.Language.CT_OBJECT_CONT_ANIM, Type = EControlType.CT_OBJECT_CONT_ANIM },
                new EditorToolboxItem { Name = Properties.Language.CT_LINEBREAK, Type = EControlType.CT_LINEBREAK },
                new EditorToolboxItem { Name = Properties.Language.CT_USER, Type = EControlType.CT_USER },
                new EditorToolboxItem { Name = Properties.Language.CT_MAP, Type = EControlType.CT_MAP },
                new EditorToolboxItem { Name = Properties.Language.CT_MAP_MAIN, Type = EControlType.CT_MAP_MAIN },
                new EditorToolboxItem { Name = Properties.Language.CT_LISTNBOX, Type = EControlType.CT_LISTNBOX },
                new EditorToolboxItem { Name = Properties.Language.CT_ITEMSLOT, Type = EControlType.CT_ITEMSLOT },
                new EditorToolboxItem { Name = Properties.Language.CT_LISTNBOX_CHECKABLE, Type = EControlType.CT_LISTNBOX_CHECKABLE },
                new EditorToolboxItem { Name = Properties.Language.CT_VEHICLE_DIRECTION, Type = EControlType.CT_VEHICLE_DIRECTION },
            };
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.UiEditor.Data
{
    public enum EControlType
    {
        [ControlType(typeof(ControlStatic))]
        CT_STATIC = 0x00,
        [ControlType(typeof(ControlButton))]
        CT_BUTTON = 0x01,
        [ControlType(typeof(ControlEdit))]
        CT_EDIT = 0x02,
        [ControlType(typeof(ControlSlider))]
        CT_SLIDER = 0x03,
        [ControlType(typeof(ControlCombo))]
        CT_COMBO = 0x04,
        [ControlType(typeof(ControlListBox))]
        CT_LISTBOX = 0x05,
        [ControlType(typeof(ControlToolBox))]
        CT_TOOLBOX = 0x06,
        [ControlType(typeof(ControlCheckBoxes))]
        CT_CHECKBOXES = 0x07,
        [ControlType(typeof(ControlProgress))]
        CT_PROGRESS = 0x08,
        [ControlType(typeof(ControlHtml))]
        CT_HTML = 0x09,
        // [ControlType(typeof(ControlStaticSkew))]
        CT_STATIC_SKEW = 0x0A,
        [ControlType(typeof(ControlActiveText))]
        CT_ACTIVETEXT = 0x0B,
        [ControlType(typeof(ControlTree))]
        CT_TREE = 0x0C,
        [ControlType(typeof(ControlStructuredText))]
        CT_STRUCTURED_TEXT = 0x0D,
        [ControlType(typeof(ControlContextMenu))]
        CT_CONTEXT_MENU = 0x0E,
        [ControlType(typeof(ControlControlsGroup))]
        CT_CONTROLS_GROUP = 0x0F,
        [ControlType(typeof(ControlShortcutButton))]
        CT_SHORTCUTBUTTON = 0x10,
        // [ControlType(typeof(ControlHitzones))]
        CT_HITZONES = 0x11,
        // [ControlType(typeof(ControlVehicleToggles))]
        CT_VEHICLETOGGLES = 0x12,
        [ControlType(typeof(ControlControlsTable))]
        CT_CONTROLS_TABLE = 0x13,
        [ControlType(typeof(ControlXKeyDescription))]
        CT_XKEYDESC = 0x28,
        // [ControlType(typeof(ControlXButton))]
        CT_XBUTTON = 0x29,
        [ControlType(typeof(ControlXListBox))]
        CT_XLISTBOX = 0x2A,
        [ControlType(typeof(ControlXSlider))]
        CT_XSLIDER = 0x2B,
        [ControlType(typeof(ControlXCombobox))]
        CT_XCOMBO = 0x2C,
        [ControlType(typeof(ControlAnimatedTexture))]
        CT_ANIMATED_TEXTURE = 0x2D,
        [ControlType(typeof(ControlMenu))]
        CT_MENU = 0x2E,
        [ControlType(typeof(ControlMenuStrip))]
        CT_MENU_STRIP = 0x2F,
        // [ControlType(typeof(ControlCheckBox))]
        CT_CHECKBOX = 0x4D,
        [ControlType(typeof(ControlObject))]
        CT_OBJECT = 0x50,
        [ControlType(typeof(ControlObjectZoom))]
        CT_OBJECT_ZOOM = 0x51,
        [ControlType(typeof(ControlObjectContainer))]
        CT_OBJECT_CONTAINER = 0x52,
        // [ControlType(typeof(ControlObjectContainerAnimation))]
        CT_OBJECT_CONT_ANIM = 0x53,
        [ControlType(typeof(ControlLinebreak))]
        CT_LINEBREAK = 0x62,
        [ControlType(typeof(ControlUser))]
        CT_USER = 0x63,
        [ControlType(typeof(ControlMap))]
        CT_MAP = 0x64,
        [ControlType(typeof(ControlMapMain))]
        CT_MAP_MAIN = 0x65,
        [ControlType(typeof(ControlListNBox))]
        CT_LISTNBOX = 0x66,
        // [ControlType(typeof(ControlItemSlot))]
        CT_ITEMSLOT = 0x67,
        // [ControlType(typeof(ControlListNBoxCheckable))]
        CT_LISTNBOX_CHECKABLE = 0x68,
        // [ControlType(typeof(ControlVehicleDirection))]
        CT_VEHICLE_DIRECTION = 0x69
    }
}

using Arma.Studio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.UiEditor.Data
{
    public enum EControlType
    {
        [ControlType(typeof(ControlStatic), IconPath = "pack://application:,,,/Arma.Studio.UiEditor;component/Resources/TextBlock_16x.png"), EnumName("CT_STATIC", ResourceName = "CT_STATIC")]
        CT_STATIC = 0x00,
        // [ControlType(typeof(ControlButton)), EnumName("CT_BUTTON", ResourceName = "CT_BUTTON")]
        CT_BUTTON = 0x01,
        [ControlType(typeof(ControlEdit), IconPath = "pack://application:,,,/Arma.Studio.UiEditor;component/Resources/TextBox_16x.png"), EnumName("CT_EDIT", ResourceName = "CT_EDIT")]
        CT_EDIT = 0x02,
        // [ControlType(typeof(ControlSlider)), EnumName("CT_SLIDER", ResourceName = "CT_SLIDER")]
        CT_SLIDER = 0x03,
        // [ControlType(typeof(ControlCombo)), EnumName("CT_COMBO", ResourceName = "CT_COMBO")]
        CT_COMBO = 0x04,
        // [ControlType(typeof(ControlListBox)), EnumName("CT_LISTBOX", ResourceName = "CT_LISTBOX")]
        CT_LISTBOX = 0x05,
        // [ControlType(typeof(ControlToolBox)), EnumName("CT_TOOLBOX", ResourceName = "CT_TOOLBOX")]
        CT_TOOLBOX = 0x06,
        // [ControlType(typeof(ControlCheckBoxes)), EnumName("CT_CHECKBOXES", ResourceName = "CT_CHECKBOXES")]
        CT_CHECKBOXES = 0x07,
        // [ControlType(typeof(ControlProgress)), EnumName("CT_PROGRESS", ResourceName = "CT_PROGRESS")]
        CT_PROGRESS = 0x08,
        // [ControlType(typeof(ControlHtml)), EnumName("CT_HTML", ResourceName = "CT_HTML")]
        CT_HTML = 0x09,
        // [ControlType(typeof(ControlStaticSkew)), EnumName("CT_STATIC_SKEW", ResourceName = "CT_STATIC_SKEW")]
        CT_STATIC_SKEW = 0x0A,
        // [ControlType(typeof(ControlActiveText)), EnumName("CT_ACTIVETEXT", ResourceName = "CT_ACTIVETEXT")]
        CT_ACTIVETEXT = 0x0B,
        // [ControlType(typeof(ControlTree)), EnumName("CT_TREE", ResourceName = "CT_TREE")]
        CT_TREE = 0x0C,
        // [ControlType(typeof(ControlStructuredText)), EnumName("CT_STRUCTURED_TEXT", ResourceName = "CT_STRUCTURED_TEXT")]
        CT_STRUCTURED_TEXT = 0x0D,
        // [ControlType(typeof(ControlContextMenu)), EnumName("CT_CONTEXT_MENU", ResourceName = "CT_CONTEXT_MENU")]
        CT_CONTEXT_MENU = 0x0E,
        // [ControlType(typeof(ControlControlsGroup)), EnumName("CT_CONTROLS_GROUP", ResourceName = "CT_CONTROLS_GROUP")]
        CT_CONTROLS_GROUP = 0x0F,
        // [ControlType(typeof(ControlShortcutButton)), EnumName("CT_SHORTCUTBUTTON", ResourceName = "CT_SHORTCUTBUTTON")]
        CT_SHORTCUTBUTTON = 0x10,
        // [ControlType(typeof(ControlHitzones)), EnumName("CT_HITZONES", ResourceName = "CT_HITZONES")]
        CT_HITZONES = 0x11,
        // [ControlType(typeof(ControlVehicleToggles)), EnumName("CT_VEHICLETOGGLES", ResourceName = "CT_VEHICLETOGGLES")]
        CT_VEHICLETOGGLES = 0x12,
        // [ControlType(typeof(ControlControlsTable)), EnumName("CT_CONTROLS_TABLE", ResourceName = "CT_CONTROLS_TABLE")]
        CT_CONTROLS_TABLE = 0x13,
        // [ControlType(typeof(ControlXKeyDescription)), EnumName("CT_XKEYDESC", ResourceName = "CT_XKEYDESC")]
        CT_XKEYDESC = 0x28,
        // [ControlType(typeof(ControlXButton)), EnumName("CT_XBUTTON", ResourceName = "CT_XBUTTON")]
        CT_XBUTTON = 0x29,
        // [ControlType(typeof(ControlXListBox)), EnumName("CT_XLISTBOX", ResourceName = "CT_XLISTBOX")]
        CT_XLISTBOX = 0x2A,
        // [ControlType(typeof(ControlXSlider)), EnumName("CT_XSLIDER", ResourceName = "CT_XSLIDER")]
        CT_XSLIDER = 0x2B,
        // [ControlType(typeof(ControlXCombobox)), EnumName("CT_XCOMBO", ResourceName = "CT_XCOMBO")]
        CT_XCOMBO = 0x2C,
        // [ControlType(typeof(ControlAnimatedTexture)), EnumName("CT_ANIMATED_TEXTURE", ResourceName = "CT_ANIMATED_TEXTURE")]
        CT_ANIMATED_TEXTURE = 0x2D,
        // [ControlType(typeof(ControlMenu)), EnumName("CT_MENU", ResourceName = "CT_MENU")]
        CT_MENU = 0x2E,
        // [ControlType(typeof(ControlMenuStrip)), EnumName("CT_MENU_STRIP", ResourceName = "CT_MENU_STRIP")]
        CT_MENU_STRIP = 0x2F,
        // [ControlType(typeof(ControlCheckBox)), EnumName("CT_CHECKBOX", ResourceName = "CT_CHECKBOX")]
        CT_CHECKBOX = 0x4D,
        // [ControlType(typeof(ControlObject)), EnumName("CT_OBJECT", ResourceName = "CT_OBJECT")]
        CT_OBJECT = 0x50,
        // [ControlType(typeof(ControlObjectZoom)), EnumName("CT_OBJECT_ZOOM", ResourceName = "CT_OBJECT_ZOOM")]
        CT_OBJECT_ZOOM = 0x51,
        // [ControlType(typeof(ControlObjectContainer)), EnumName("CT_OBJECT_CONTAINER", ResourceName = "CT_OBJECT_CONTAINER")]
        CT_OBJECT_CONTAINER = 0x52,
        // [ControlType(typeof(ControlObjectContainerAnimation)), EnumName("CT_OBJECT_CONT_ANIM", ResourceName = "CT_OBJECT_CONT_ANIM")]
        CT_OBJECT_CONT_ANIM = 0x53,
        // [ControlType(typeof(ControlLinebreak)), EnumName("CT_LINEBREAK", ResourceName = "CT_LINEBREAK")]
        CT_LINEBREAK = 0x62,
        // [ControlType(typeof(ControlUser)), EnumName("CT_USER", ResourceName = "CT_USER")]
        CT_USER = 0x63,
        // [ControlType(typeof(ControlMap)), EnumName("CT_MAP", ResourceName = "CT_MAP")]
        CT_MAP = 0x64,
        // [ControlType(typeof(ControlMapMain)), EnumName("CT_MAP_MAIN", ResourceName = "CT_MAP_MAIN")]
        CT_MAP_MAIN = 0x65,
        // [ControlType(typeof(ControlListNBox)), EnumName("CT_LISTNBOX", ResourceName = "CT_LISTNBOX")]
        CT_LISTNBOX = 0x66,
        // [ControlType(typeof(ControlItemSlot)), EnumName("CT_ITEMSLOT", ResourceName = "CT_ITEMSLOT")]
        CT_ITEMSLOT = 0x67,
        // [ControlType(typeof(ControlListNBoxCheckable)), EnumName("CT_LISTNBOX_CHECKABLE", ResourceName = "CT_LISTNBOX_CHECKABLE")]
        CT_LISTNBOX_CHECKABLE = 0x68,
        // [ControlType(typeof(ControlVehicleDirection)), EnumName("CT_VEHICLE_DIRECTION", ResourceName = "CT_VEHICLE_DIRECTION")]
        CT_VEHICLE_DIRECTION = 0x69
    }
}

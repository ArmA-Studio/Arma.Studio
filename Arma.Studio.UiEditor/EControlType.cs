using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.UiEditor
{
    public enum EControlType
    {
        CT_STATIC = 0x00,
        CT_BUTTON = 0x01,
        CT_EDIT = 0x02,
        CT_SLIDER = 0x03,
        CT_COMBO = 0x04,
        CT_LISTBOX = 0x05,
        CT_TOOLBOX = 0x06,
        CT_CHECKBOXES = 0x07,
        CT_PROGRESS = 0x08,
        CT_HTML = 0x09,
        CT_STATIC_SKEW = 0x0A,
        CT_ACTIVETEXT = 0x0B,
        CT_TREE = 0x0C,
        CT_STRUCTURED_TEXT = 0x0D,
        CT_CONTEXT_MENU = 0x0E,
        CT_CONTROLS_GROUP = 0x0F,
        CT_SHORTCUTBUTTON = 0x10,
        CT_HITZONES = 0x11,
        CT_VEHICLETOGGLES = 0x12,
        CT_CONTROLS_TABLE = 0x13,
        CT_XKEYDESC = 0x28,
        CT_XBUTTON = 0x29,
        CT_XLISTBOX = 0x2A,
        CT_XSLIDER = 0x2B,
        CT_XCOMBO = 0x2C,
        CT_ANIMATED_TEXTURE = 0x2D,
        CT_MENU = 0x2E,
        CT_MENU_STRIP = 0x2F,
        CT_CHECKBOX = 0x4D,
        CT_OBJECT = 0x50,
        CT_OBJECT_ZOOM = 0x51,
        CT_OBJECT_CONTAINER = 0x52,
        CT_OBJECT_CONT_ANIM = 0x53,
        CT_LINEBREAK = 0x62,
        CT_USER = 0x63,
        CT_MAP = 0x64,
        CT_MAP_MAIN = 0x65,
        CT_LISTNBOX = 0x66,
        CT_ITEMSLOT = 0x67,
        CT_LISTNBOX_CHECKABLE = 0x68,
        CT_VEHICLE_DIRECTION = 0x69
    }
}

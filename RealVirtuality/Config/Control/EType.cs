using RealVirtuality.Config.Control.Attributes;

namespace RealVirtuality.Config.Control
{
    public enum EType
    {
        [ControlInfo(typeof(StaticControl))]
        CT_STATIC = 0,
        [ControlInfo(typeof(Button))]
        CT_BUTTON = 1,
        [ControlInfo(typeof(TextBox))]
        CT_EDIT = 2,
        [ControlInfo(typeof(Slider))]
        CT_SLIDER = 3,
        [ControlInfo(typeof(ComboBox))]
        CT_COMBO = 4,
        [ControlInfo(typeof(ListBox))]
        CT_LISTBOX = 5,
        [ControlInfo(typeof(ToolBox))]
        CT_TOOLBOX = 6,
        [ControlInfo(typeof(CheckBox))]
        CT_CHECKBOXES = 7,
        [ControlInfo(typeof(ProgressBar))]
        CT_PROGRESS = 8,
        [ControlInfo(typeof(HtmlView))]
        CT_HTML = 9,
        CT_STATIC_SKEW = 10,
        [ControlInfo(typeof(ActiveText))]
        CT_ACTIVETEXT = 11,
        [ControlInfo(typeof(TreeView))]
        CT_TREE = 12,
        [ControlInfo(typeof(StructuredText))]
        CT_STRUCTURED_TEXT = 13,
        CT_CONTEXT_MENU = 14,
        CT_CONTROLS_GROUP = 15,
        CT_SHORTCUTBUTTON = 16,
        CT_XKEYDESC = 40,
        CT_XBUTTON = 41,
        CT_XLISTBOX = 42,
        CT_XSLIDER = 43,
        CT_XCOMBO = 44,
        CT_ANIMATED_TEXTURE = 45,
        CT_MENU = 46,
        CT_MENU_STRIP = 47,
        [ControlInfo(typeof(CheckBox))]
        CT_CHECKBOX = 77,
        CT_OBJECT = 80,
        CT_OBJECT_ZOOM = 81,
        CT_OBJECT_CONTAINER = 82,
        CT_OBJECT_CONT_ANIM = 83,
        CT_LINEBREAK = 98,
        CT_ANIMATED_USER = 99,
        [ControlInfo(typeof(MapControl))]
        CT_MAP = 100,
        [ControlInfo(typeof(MapHostControl))]
        CT_MAP_MAIN = 101,
        [ControlInfo(typeof(ListNBox))]
        CT_LISTNBOX = 102,
        CT_ITEMSLOT = 103
    }
}

using System;
using RealVirtuality.Config.Control.Attributes;

namespace RealVirtuality.Config.Control
{
    [Flags]
    public enum EStyle
    {
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_POS = 0x0F,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_HPOS = 0x03,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_VPOS = 0x0C,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_LEFT = 0x00,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_RIGHT = 0x01,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_CENTER = 0x02,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_DOWN = 0x04,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_UP = 0x08,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_VCENTER = 0x0c,
        /// <summary>
        /// Static style
        /// </summary>

        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_TYPE = 0xF0,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_SINGLE = 0,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_MULTI = 16,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_TITLE_BAR = 32,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_PICTURE = 48,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_FRAME = 64,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_BACKGROUND = 80,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_GROUP_BOX = 96,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_GROUP_BOX2 = 112,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_HUD_BACKGROUND = 128,
        /// <summary>
        /// Static style
        /// tileH and tileW params required for tiled image
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_TILE_PICTURE = 144,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_WITH_RECT = 160,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_LINE = 176,

        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_SHADOW = 0x100,
        /// <summary>
        /// Static style
        /// this style works for CT_STATIC in conjunction with ST_MULTI
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_NO_RECT = 0x200,
        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_KEEP_ASPECT_RATIO = 0x800,

        /// <summary>
        /// Static style
        /// </summary>
        [StyleInfo(Type = typeof(ControlBase))]
        ST_TITLE = ST_TITLE_BAR + ST_CENTER,

        /// <summary>
        /// Slider style
        /// </summary>
        [StyleInfo(Type = typeof(Slider))]
        SL_DIR = 0x400,
        /// <summary>
        /// Slider style
        /// </summary>
        [StyleInfo(Type = typeof(Slider))]
        SL_VERT = 0,
        /// <summary>
        /// Slider style
        /// </summary>
        [StyleInfo(Type = typeof(Slider))]
        SL_HORZ = 1024,

        /// <summary>
        /// Slider style
        /// </summary>
        [StyleInfo(Type = typeof(Slider))]
        SL_TEXTURES = 0x10,

        /// <summary>
        /// progress bar
        /// </summary>
        [StyleInfo(Type = typeof(ProgressBar))]
        ST_VERTICAL = 0x01,
        /// <summary>
        /// progress bar
        /// </summary>
        [StyleInfo(Type = typeof(ProgressBar))]
        ST_HORIZONTAL = 0,

        /// <summary>
        /// Listbox style
        /// </summary>
        [StyleInfo(Type = typeof(ListBox))]
        LB_TEXTURES = 0x10,
        /// <summary>
        /// Listbox style
        /// </summary>
        [StyleInfo(Type = typeof(ListBox))]
        LB_MULTI = 0x20,


        /// <summary>
        /// Tree style
        /// </summary>
        [StyleInfo(Type = typeof(TreeView))]
        TR_SHOWROOT = 1,
        /// <summary>
        /// Tree style
        /// </summary>
        [StyleInfo(Type = typeof(TreeView))]
        TR_AUTOCOLLAPSE = 2,

        //ToDo: Reenable
        //Disabled as MessageBox is currently non-existing
        // /// <summary>
        // /// MessageBox style
        // /// </summary>
        // //[StyleInfo(Type = typeof(MessageBox))]
        // MB_BUTTON_OK = 1,
        // /// <summary>
        // /// MessageBox style
        // /// </summary>
        // [StyleInfo(Type = typeof(MessageBox))]
        // MB_BUTTON_CANCEL = 2,
        // /// <summary>
        // /// MessageBox style
        // /// </summary>
        // [StyleInfo(Type = typeof(MessageBox))]
        // MB_BUTTON_USER = 4
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.UiEditor
{
    public enum EControlStyle
    {
        ST_LEFT = 0x00,
        ST_RIGHT = 0x01,
        ST_CENTER = 0x02,
        ST_DOWN = 0x04,
        ST_UP = 0x08,
        ST_VCENTER = 0x0C,
        ST_SINGLE = 0x00,
        ST_MULTI = 0x10,
        ST_TITLE_BAR = 0x20,
        ST_PICTURE = 0x30,
        ST_FRAME = 0x40,
        ST_BACKGROUND = 0x50,
        ST_GROUP_BOX = 0x60,
        ST_GROUP_BOX2 = 0x70,
        ST_HUD_BACKGROUND = 0x80,
        ST_TILE_PICTURE = 0x90,
        ST_WITH_RECT = 0xA0,
        ST_LINE = 0xB0,
        ST_UPPERCASE = 0xC0,
        ST_LOWERCASE = 0xD0,
        ST_ADDITIONAL_INFO = 0x0F00,
        ST_SHADOW = 0x0100,
        ST_NO_RECT = 0x0200,
        ST_KEEP_ASPECT_RATIO = 0x0800,
        ST_TITLE = 0x22
    }
}

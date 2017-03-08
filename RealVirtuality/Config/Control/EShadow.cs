using System;
using RealVirtuality.Config.Control.Attributes;

namespace RealVirtuality.Config.Control
{
    [Flags]
    public enum EShadow
    {
        NoShadow = 0,
        DropShadowSoftEdges = 1,
        Stroke = 2
    }
}

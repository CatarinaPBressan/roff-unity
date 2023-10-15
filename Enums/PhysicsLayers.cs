using System;

namespace Enums
{
    [Flags]
    public enum PhysicsLayers
    {
        None = 0,
        Terrain = 1 << 3,
        Player = 1 << 6,
        Monster = 1 << 7
    }
}

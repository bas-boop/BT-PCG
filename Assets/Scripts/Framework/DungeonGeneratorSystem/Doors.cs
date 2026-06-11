using System;

namespace Framework.DungeonGeneratorSystem
{
    [Flags]
    public enum Doors
    {
        NORTH = 1,
        EAST = 2,
        SOUTH = 4,
        WEST = 8
    }
}
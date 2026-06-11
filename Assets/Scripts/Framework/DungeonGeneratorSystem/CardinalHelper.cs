using System;

namespace Framework.DungeonGeneratorSystem
{
    public static class CardinalHelper
    {
        public static Doors ToDoor(this CardinalDirections dir) => dir switch
        {
            CardinalDirections.NORTH => Doors.NORTH,
            CardinalDirections.EAST  => Doors.EAST,
            CardinalDirections.SOUTH => Doors.SOUTH,
            CardinalDirections.WEST  => Doors.WEST,
            _ => throw new ArgumentOutOfRangeException()
        };

        public static Doors ToOppositeDoor(this CardinalDirections dir) => dir switch
        {
            CardinalDirections.NORTH => Doors.SOUTH,
            CardinalDirections.EAST  => Doors.WEST,
            CardinalDirections.SOUTH => Doors.NORTH,
            CardinalDirections.WEST  => Doors.EAST,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
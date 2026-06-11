using FrameWork.Attributes;

namespace Framework.DungeonGeneratorSystem
{
    public enum CardinalDirections
    {
        [Vector2Value(0, 1)] NORTH,
        [Vector2Value(1, 0)] EAST,
        [Vector2Value(0, -1)] SOUTH,
        [Vector2Value(-1, 0)] WEST
    }
}
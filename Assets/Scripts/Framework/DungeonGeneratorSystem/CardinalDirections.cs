using FrameWork.Attributes;

namespace Framework.DungeonGeneratorSystem
{
    public enum CardinalDirections
    {
        [Vector2Value(1, 0)] NORTH,
        [Vector2Value(0, 1)] EAST,
        [Vector2Value(-1, 0)] SOUTH,
        [Vector2Value(0, -1)] WEST
    }
}
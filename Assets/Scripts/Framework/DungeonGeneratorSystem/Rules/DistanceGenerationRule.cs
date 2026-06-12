using UnityEngine;

namespace Framework.DungeonGeneratorSystem.Rules
{
    [CreateAssetMenu(fileName = "NewDistanceGenerationRule", menuName = "PCG/Distance", order = 0)]
    public class DistanceGenerationRule : GenerationRule
    {
        public CellType otherRoomType;
        public Distances distance;
    }
}
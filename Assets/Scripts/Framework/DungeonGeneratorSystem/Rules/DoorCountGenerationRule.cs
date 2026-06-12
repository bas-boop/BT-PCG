using UnityEngine;

namespace Framework.DungeonGeneratorSystem.Rules
{
    [CreateAssetMenu(fileName = "NewDoorCountGenerationRule", menuName = "PCG/DoorCount", order = 0)]
    public class DoorCountGenerationRule : GenerationRule
    {
        [Min(1)] public int minDoors = 1;
        [Range(1, 4)] public int maxDoors = 4;
    }
}
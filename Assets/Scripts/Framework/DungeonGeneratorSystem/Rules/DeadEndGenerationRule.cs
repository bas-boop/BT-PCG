using UnityEngine;

namespace Framework.DungeonGeneratorSystem.Rules
{
    [CreateAssetMenu(fileName = "NewDeadEndGenerationRule", menuName = "PCG/DeadEnd", order = 0)]
    public class DeadEndGenerationRule : GenerationRule
    {
        public override bool TryPlace(Grid grid) => DoorCountGenerationRule.TryPlaceByDoorCount(grid, roomType, 1, 1);
    }
}
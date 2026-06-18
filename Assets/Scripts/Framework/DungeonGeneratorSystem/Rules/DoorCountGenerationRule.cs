using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Framework.DungeonGeneratorSystem.Seed;
using CollectionExtensions = Framework.Extensions.CollectionExtensions;

namespace Framework.DungeonGeneratorSystem.Rules
{
    [CreateAssetMenu(fileName = "NewDoorCountGenerationRule", menuName = "PCG/DoorCount", order = 0)]
    public class DoorCountGenerationRule : GenerationRule
    {
        [Range(1, 3)] public int minDoors = 1;
        [Range(1, 4)] public int maxDoors = 4;

        public override bool TryPlace(Grid grid) => TryPlaceByDoorCount(grid, roomType, minDoors, maxDoors);

        public static bool TryPlaceByDoorCount(Grid grid, CellType roomType, int minDoors, int maxDoors)
        {
            int low = Mathf.Clamp(minDoors, 1, 4);
            int high = Mathf.Clamp(maxDoors, low, 4);
 
            List<KeyValuePair<Vector2Int, Cell>> candidates = grid.ActiveCells.Where(kvp =>
            {
                if (kvp.Value.Type != CellType.NORMAL)
                    return false;
 
                int count = Grid.GetDoorCount(kvp.Value.Doors);
                return count >= low && count <= high;
            }).ToList();
 
            if (candidates.Count == 0)
                return false;
 
            KeyValuePair<Vector2Int, Cell> picked = CollectionExtensions.GetRandomItem(candidates, RandomSeedSystem.GetRandom());
            grid.SetCellType(picked.Key, roomType);
            return true;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Framework.DungeonGeneratorSystem.Seed;
using CollectionExtensions = Framework.Extensions.CollectionExtensions;

namespace Framework.DungeonGeneratorSystem.Rules
{
    [CreateAssetMenu(fileName = "NewGenerationRule", menuName = "PCG/Random", order = 0)]
    public class GenerationRule : ScriptableObject
    {
        public int amount;
        public CellType roomType;

        public virtual bool TryPlace(Grid grid)
        {
            List<KeyValuePair<Vector2Int, Cell>> candidates = grid.ActiveCells
                .Where(kvp => kvp.Value.Type == CellType.NORMAL)
                .ToList();
 
            if (candidates.Count == 0)
                return false;
 
            while (true)
            {
                KeyValuePair<Vector2Int, Cell> picked = CollectionExtensions.GetRandomItem(candidates, RandomSeedSystem.GetRandom());
 
                if (picked.Value.Type != CellType.NORMAL)
                    continue;
 
                grid.SetCellType(picked.Key, roomType);
                return true;
            }
        }
    }
}
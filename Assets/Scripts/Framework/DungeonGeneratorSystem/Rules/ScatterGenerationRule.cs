using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Framework.DungeonGeneratorSystem.Seed;
using CollectionExtensions = Framework.Extensions.CollectionExtensions;

namespace Framework.DungeonGeneratorSystem.Rules
{
    [CreateAssetMenu(fileName = "NewScatterGenerationRule", menuName = "PCG/Funky/Scatter", order = 0)]
    public class ScatterGenerationRule : GenerationRule
    {
        [Min(1)] public int scatterCount = 5;

        public override bool TryPlace(Grid grid)
        {
            List<KeyValuePair<Vector2Int, Cell>> candidates = grid.ActiveCells
                .Where(kvp => kvp.Value.Type == CellType.NORMAL)
                .ToList();

            if (candidates.Count == 0)
                return false;

            bool placedAny = false;
            int count = Mathf.Min(scatterCount, candidates.Count);

            for (int i = 0; i < count; i++)
            {
                if (candidates.Count == 0)
                    break;

                KeyValuePair<Vector2Int, Cell> picked = CollectionExtensions.GetRandomItem(candidates, RandomSeedSystem.GetRandom());

                grid.SetCellType(picked.Key, roomType);
                candidates.Remove(picked);
                placedAny = true;
            }

            return placedAny;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Framework.DungeonGeneratorSystem.Seed;
using CollectionExtensions = Framework.Extensions.CollectionExtensions;

namespace Framework.DungeonGeneratorSystem.Rules
{
    [CreateAssetMenu(fileName = "NewWildcardGenerationRule", menuName = "PCG/Funky/Wildcard", order = 0)]
    public class WildcardGenerationRule : GenerationRule
    {
        public CellType[] possibleTypes;

        public override bool TryPlace(Grid grid)
        {
            if (possibleTypes == null
                || possibleTypes.Length == 0)
                return false;

            List<KeyValuePair<Vector2Int, Cell>> candidates = grid.ActiveCells
                .Where(kvp => kvp.Value.Type == CellType.NORMAL)
                .ToList();

            if (candidates.Count == 0)
                return false;

            KeyValuePair<Vector2Int, Cell> picked = CollectionExtensions.GetRandomItem(candidates, RandomSeedSystem.GetRandom());
            CellType chosenType = CollectionExtensions.GetRandomItem(possibleTypes.ToList(), RandomSeedSystem.GetRandom());

            grid.SetCellType(picked.Key, chosenType);
            return true;
        }
    }
}
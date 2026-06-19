using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Framework.DungeonGeneratorSystem.Seed;
using CollectionExtensions = Framework.Extensions.CollectionExtensions;

namespace Framework.DungeonGeneratorSystem.Rules
{
    [CreateAssetMenu(fileName = "NewTeleportSwapGenerationRule", menuName = "PCG/Funky/TeleportSwap", order = 0)]
    public class TeleportSwapGenerationRule : GenerationRule
    {
        public CellType otherRoomType;

        public override bool TryPlace(Grid grid)
        {
            List<KeyValuePair<Vector2Int, Cell>> candidates = grid.ActiveCells
                .Where(kvp => kvp.Value.Type == CellType.NORMAL)
                .ToList();

            if (candidates.Count < 2)
                return false;

            KeyValuePair<Vector2Int, Cell> first = CollectionExtensions.GetRandomItem(candidates, RandomSeedSystem.GetRandom());

            List<KeyValuePair<Vector2Int, Cell>> remaining = candidates
                .Where(kvp => kvp.Key != first.Key)
                .ToList();

            if (remaining.Count == 0)
                return false;

            KeyValuePair<Vector2Int, Cell> second = CollectionExtensions.GetRandomItem(remaining, RandomSeedSystem.GetRandom());

            grid.SetCellType(first.Key, roomType);
            grid.SetCellType(second.Key, otherRoomType);

            return true;
        }
    }
}
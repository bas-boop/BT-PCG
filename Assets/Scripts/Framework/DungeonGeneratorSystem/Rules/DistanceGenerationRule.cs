using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.DungeonGeneratorSystem.Rules
{
    [CreateAssetMenu(fileName = "NewDistanceGenerationRule", menuName = "PCG/Distance", order = 0)]
    public class DistanceGenerationRule : GenerationRule
    {
        public CellType otherRoomType;
        public Distances distance;

        public override bool TryPlace(Grid grid)
        {
            float bestDistance = distance == Distances.CLOSE ? float.MaxValue : 0f;
            Vector2Int bestPos = Vector2Int.zero;
            Vector2Int distanceCheck = Vector2Int.zero;
            bool found = false;
 
            foreach (KeyValuePair<Vector2Int, Cell> kvp in grid.AllCells)
            {
                if (kvp.Value.Type != otherRoomType)
                    continue;
                
                distanceCheck = kvp.Key;
                break;
            }
 
            foreach (KeyValuePair<Vector2Int, Cell> kvp in grid.AllCells)
            {
                if (kvp.Value.Type != CellType.NORMAL)
                    continue;
 
                float dist = Vector2Int.Distance(distanceCheck, kvp.Key);
 
                switch (distance)
                {
                    case Distances.CLOSE:
                        if (dist > bestDistance)
                            continue;
                        break;
                    case Distances.FAR:
                        if (dist <= bestDistance)
                            continue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
 
                bestDistance = dist;
                bestPos = kvp.Key;
                found = true;
            }
 
            if (found)
                grid.SetCellType(bestPos, roomType);
 
            return found;
        }
    }
}
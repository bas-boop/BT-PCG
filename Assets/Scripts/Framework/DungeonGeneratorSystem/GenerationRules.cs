using System;
using UnityEngine;

namespace Framework.DungeonGeneratorSystem
{
    [Serializable]
    public struct GenerationRules
    {
        public int amount;
        public CellType roomType;
        public CellType otherRoomType;
        [Range(0, 1)] public int distance;
        public Rule rule;
    }
}
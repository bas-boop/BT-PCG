using UnityEngine;

namespace Framework.DungeonGeneratorSystem.Rules
{
    [CreateAssetMenu(fileName = "NewGenerationRule", menuName = "PCG/Random", order = 0)]
    public class GenerationRule : ScriptableObject
    {
        public int amount;
        public CellType roomType;
    }
}
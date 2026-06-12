using UnityEngine;

namespace Framework.DungeonGeneratorSystem.Rules
{
    [CreateAssetMenu(fileName = "NewDeadEndGenerationRule", menuName = "PCG/DeadEnd", order = 0)]
    public class DeadEndGenerationRule : GenerationRule { }
    // No extra fields needed — dead-end detection is purely door-count == 1
}
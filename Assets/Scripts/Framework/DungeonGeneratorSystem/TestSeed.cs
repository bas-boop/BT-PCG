using UnityEngine;

namespace Framework.DungeonGeneratorSystem
{
    public class TestSeed : MonoBehaviour
    {
        [SerializeField] private string seed;
        [SerializeField] private SpriteRenderer[] sprites;

        private void Start() => SetRandomColors();

        [ContextMenu("Do random colors")]
        private void SetRandomColors()
        {
            RandomSeedSystem.SetSeed(seed);

            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = new (RandomSeedSystem.GetRandomFloat(), RandomSeedSystem.GetRandomFloat(), RandomSeedSystem.GetRandomFloat());
            }
        }
    }
}
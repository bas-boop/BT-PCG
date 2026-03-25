using UnityEngine;

using Framework.Extensions;

namespace Framework.DungeonGeneratorSystem
{
    public class TestSeed : MonoBehaviour
    {
        [SerializeField] private string seed;
        [SerializeField] private SpriteRenderer[] sprites;

        private int _times;

        [ContextMenu("Do random colors")]
        public void SetRandomColors()
        {
            //RandomSeedSystem.SetSeed(seed);

            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = new (RandomSeedSystem.GetRandomFloat(), RandomSeedSystem.GetRandomFloat(), RandomSeedSystem.GetRandomFloat());
            }
        }

        private void Walk()
        {
            CardinalDirections r = EnumExtensions.GetRandomEnumValue<CardinalDirections>(RandomSeedSystem.GetRandom());
            Debug.Log($"{r.GetVector2()} - {_times}");
            _times++;
        }
    }
}
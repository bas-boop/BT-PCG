using UnityEngine;

using Framework.DungeonGeneratorSystem;

namespace UI
{
    public class SeedUI : MonoBehaviour
    {
        private string _seed = "00000";

        private void Start() => RandomSeedSystem.SetSeed(_seed);

        public void SetCurrentSeed(string seed)
        {
            _seed = seed;
            SetCurrentSeed();
        }

        public void SetCurrentSeed()
        {
            RandomSeedSystem.SetSeed(_seed);
        }
    }
}
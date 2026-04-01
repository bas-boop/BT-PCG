using UnityEngine;
using TMPro;

using Framework.DungeonGeneratorSystem;

namespace UI
{
    public class SeedUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        
        private string _seed = "00000";

        private void Start() => RandomSeedSystem.SetSeed(_seed);

        public void SetCurrentSeed(string seed)
        {
            _seed = seed;
            SetCurrentSeed();
        }

        public void SetRandomSeed()
        {
            _seed = DiceRoller.Roll(5, 10).ToString();
            inputField.SetTextWithoutNotify(_seed);
            RandomSeedSystem.SetSeed(_seed);
        }

        public void SetCurrentSeed()
        {
            RandomSeedSystem.SetSeed(_seed);
        }
    }
}
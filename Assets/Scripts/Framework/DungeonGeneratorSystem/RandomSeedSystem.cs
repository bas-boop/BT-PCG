using System;

namespace Framework.DungeonGeneratorSystem
{
    public static class RandomSeedSystem
    {
        private static int _seed;
        private static Random _random;

        public static void SetSeed(string seed)
        {
            _seed = seed.GetHashCode();
            _random = new (_seed);
        }

        public static double GetRandomDouble() => _random.NextDouble();
        
        public static int GetRandomInt() => _random.Next();

        public static float GetRandomFloat() => (float) GetRandomDouble();

        public static Random GetRandom() => _random;
    }
}
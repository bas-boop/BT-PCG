using Random = System.Random;

namespace Framework.DungeonGeneratorSystem
{
    public static class DiceRoller
    {
        private static Random _random = new ();
 
        public static int Roll(int count = 1, int sides = 1)
        {
            int results = 1;

            for (int i = 0; i < count; i++)
                results *= _random.Next(1, sides);
            
            return results;
        }
    }
}
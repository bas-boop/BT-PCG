using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace Framework.Extensions
{
    public static class CollectionExtensions
    {
        public static T GetRandomItem<T>(IList<T> list, Random r = null)
        {
            r ??= new ();
            int index = r.Next(list.Count);
            return list[index];
        }
        
        public static T GetRandomItem<T>(ICollection<T> collection)
        {
            int r = UnityEngine.Random.Range(0, collection.Count);
            return collection.ElementAt(r);
        }
    }
}
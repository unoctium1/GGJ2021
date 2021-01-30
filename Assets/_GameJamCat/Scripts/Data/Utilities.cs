using System.Collections.Generic;
using UnityEngine;

namespace GameJamCat
{
    public static class Utilities
    {
        public static T GetRandom<T>(IList<T> array)
        {
            return array[Random.Range(0, array.Count)];
        }

        public static float GetRandom(Vector2 range)
        {
            return Random.Range(range.x, range.y);
        }
    }
}

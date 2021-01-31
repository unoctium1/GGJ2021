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

        public static Texture2D ToTexture2D(this RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }
    }
}

using UnityEngine;

namespace EasyAudioSystem.Utility
{
    /* This class provides an extention method to get a random value between the
     * x and y values of a Vector2.
     * This will be helpful when you are using the Bonus MinMaxRange Attribute in the Uitlity 
     * namespace of this EasyAudio package.
     * For more information in how to use the MinMaxRange Attribute in your scripts check out
     * the corresponding EasyAudio Documentation:
     * 
     */
    public static class Vector2Utility
    {
        /// <summary>
        /// Get a random float value that's bewteen Vector2.x and Vector2.y
        /// </summary>
        /// <param name="v">The range limitations for the random value</param>
        /// <returns>Random float bewteen x and y</returns>
        public static float GetRandomRangeValue(this Vector2 v) => UnityEngine.Random.Range(v.x, v.y);
    }
}

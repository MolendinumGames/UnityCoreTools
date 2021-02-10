using UnityEngine;
using EasyAudioSystem.Core;

namespace EasyAudioSystem
{
    /* This Utility class provides an extension method for the Unity AudioSource.
    * The method has the same functionality as EasyAudio.Stop and simply provides another way
    * of stopping the target AudioSource from playing as well as stopping the
    * corresponding coroutines.
    * For more information on the EasyAudio workflow check out the Documentation:
    * 
    */
    /// <summary>
    /// Provides another way of stopping AudioSource and all corresponding coroutines.
    /// </summary>
    public static class AudioSourceUtility
    {
        /// <summary>
        /// Stops the AudioSource from playing.
        /// Use this over AudioSource.Stop if you are using EasyAudio in your project.
        /// </summary>
        /// <param name="source">Target AudioSource to stop</param>
        public static void EasyAudioStop(this AudioSource source)
        {
            EasyAudioManager.Instance.StopBySource(source);
            source.Stop();
        }
    }
}


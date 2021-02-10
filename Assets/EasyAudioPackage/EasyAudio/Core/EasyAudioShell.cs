using UnityEngine;

namespace EasyAudioSystem.Core
{
    /* For mroe information on the EasyAudio workflow check out the Documentation:
     * 
     */
    /// <summary>
    /// Holds a reference to an AudioClip and stores corresponding clip specific settings.
    /// </summary>
    [System.Serializable]
    public class EasyAudioShell
    {
        public AudioClip clip;

        public float volume = 1f;
        public float pitch = 1f;

#if UNITY_EDITOR
        public bool isExtended = false;
#endif

        public float GetClipLength => clip == null ? 0 : clip.length;
    }
}

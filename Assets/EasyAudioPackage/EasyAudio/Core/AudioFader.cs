using UnityEngine;

namespace EasyAudioSystem.Core
{
    /// <summary>
    /// Handles the volume level of an AudioSource based on an AnimationCurve.
    /// </summary>
    public class AudioFader
    {
        private AudioSource source;
        private AnimationCurve curve;
        private float startVolume = 1f;
        private float length = 1f;
        private float timer = 0f;
        private bool isLooping = false;

        public AudioFader(AudioSource source, AnimationCurve curve, float length, bool isLooping)
        {
            this.source = source;
            this.curve = curve;
            this.length = length;
            this.startVolume = source.volume;
            this.isLooping = isLooping;
        }

        public bool Process(float time)
        {
            if (source == null || !source.enabled)
            {
                return true;
            }
            else if (!source.isPlaying)
            {
                OnEndFade();
                return false;
            }

            timer += time;
            source.volume = curve.Evaluate(timer / length) * startVolume;
            if (timer >= length)
            {
                if (isLooping)
                {
                    timer = 0;
                }
                else
                {
                    OnEndFade();
                    return true;
                }
            }
            return false;
        }
        public void OnEndFade()
        {
            source.volume = startVolume;
            timer = 0;
        }
        public AudioSource GetAudioSource() => source;
    }
}

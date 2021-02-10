using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAudioSystem.Utility;
using UnityEngine.Audio;

namespace EasyAudioSystem.Core
{
    [CreateAssetMenu(fileName = "NewAudioObj", menuName = "EasyAudio/SoundObject", order = 0)]
    public class EasyAudioObj : ScriptableObject, ISerializationCallbackReceiver
    {
        /// <summary>
        /// Holds all the clip and their specific settings for this sound effect
        /// </summary>
        public List<EasyAudioShell> audioClips = new List<EasyAudioShell>();

        public AudioMixerGroup mixerGroup = null;

        public bool isLooping = false;
        public bool enableSource = true;

        public PlayState playState = PlayState.Normal;
        public float delay = 0f;

        public SelectState selectState = SelectState.Random;
        public int specificId = 0;
        public AnimationCurve fadeCurve = new AnimationCurve(new Keyframe[] { new Keyframe( 0f,    1 ),
                                                                              new Keyframe( 0.25f, 1 ),
                                                                              new Keyframe( 0.5f,  1 ),
                                                                              new Keyframe( 0.75f, 1 ),
                                                                              new Keyframe( 1,     1 ) });

        [MinMaxSlider(0f, 1f, 0.5f, 0.5f)] public Vector2 volumeRange = new Vector2(0.5f, 0.5f);
        [MinMaxSlider(0f, 2f,   1f,   1f)] public Vector2 pitchRange  = new Vector2(  1f,   1f);

#if UNITY_EDITOR
        public bool clearConsoleOnLog = false;
#endif

        #region SerializationCallbacks
        public void OnBeforeSerialize()
        {
            TryCreateFirstClip();
        }
        public void OnAfterDeserialize()
        {
            // Not needed
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetRandomId()
        {
            IEnumerable<int> possibleIds = from clip in audioClips
                                           where clip.clip != null
                                           select audioClips.IndexOf(clip);
            if (possibleIds.Count() > 0)
            {
                int rndId = UnityEngine.Random.Range(0, possibleIds.Count());
                return possibleIds.ElementAt(rndId);
            }
            else { return -1; }
        }
        public float GetPitch => UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
        public float GetVolume => UnityEngine.Random.Range(volumeRange.x, volumeRange.y);
        private void TryCreateFirstClip()
        {
            if (audioClips.Count < 1)
                CreateNewClipShell();
        }
        public void CreateNewClipShell()
        {
            EasyAudioShell newShell = new EasyAudioShell();
            audioClips.Add(newShell);
        }
        public bool AClipExists()
        {
            bool val = false;
            foreach (var clip in audioClips)
            {
                if (clip.clip != null)
                {
                    val = true;
                    break;
                }
            }
            return val;
        }
        public float GetCombinedClipLenght()
        {
            float returnVal = 0f;
            for (int i = 0; i < audioClips.Count; i++)
            {
                if (audioClips[i].clip != null)
                    returnVal += audioClips[i].clip.length;
            }
            return returnVal;
        }
        public float GetLongestClipLength()
        {
            float longest = 0f;
            for (int i = 0; i < audioClips.Count; i++)
            {
                if (audioClips[i].clip != null
                    && audioClips[i].clip.length > longest)
                    longest = audioClips[i].clip.length;
            }
            return longest;
        }
        public bool ClipCheck(int id)
        {
            if (audioClips.Count < 1 || !AClipExists())
            {
                Debug.LogWarning($"{this.name} is trying to play an audio clip but no clips were attached!");
                return false;
            }
            else if (id < 0 || id >= audioClips.Count)
            {
                Debug.LogWarning($"{this.name} is trying to play a clip with ID: {id} that is out of bounds.");
                return false;
            }
            else if (audioClips[id] == null || audioClips[id].clip == null)
            {
                Debug.LogWarning(
                    $"{this.name} is trying to play audioclip with ID: {id} which is null!");
                return false;
            }
            return true;
        }
    }
}

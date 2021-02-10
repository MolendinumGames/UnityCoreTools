using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAudioSystem;
using EasyAudioSystem.Utility;

namespace EasyAudioSystem.Core
{
    /* The EasyAudioManager class is used to provide functionality features of MonoBehaviours
     * to all EasyAudioObjects that cannot be achieved purely inside a ScriptableObject.
     * It runs/keeps track of Coroutines that handle playing multiple clips, delays and loops.
     * It also holds all Fader instances and processes them in the Update loop.
     * You do not need to access the EasyAudioManager to use the EasyAudio workflow.
     * For more information you can check out the EasyAudio Documentation:
     * 
     */
    /// <summary>
    /// This class handles fading and coroutines for playing multiple clips.
    /// </summary>
    [ExecuteInEditMode]
    public class EasyAudioManager : Singleton<EasyAudioManager>
    {
        #region Properties

        // This dictionary will keep track of all coroutines and what AudioSource they correspond to.
        private protected Dictionary<Coroutine, AudioSource> activeCorountines = new Dictionary<Coroutine, AudioSource>();

        // A list of all active Instances of Faders used to handle the in/out fading of the volume on target AudioSOurces
        private protected List<AudioFader> faders = new List<AudioFader>();

        #endregion

        #region EventFunctions
        private void Update()
        {
            HandleFaders();
        }
        private void OnDisable()
        {
            this.StopAllCoroutines();
        }
        #endregion

        #region Handle Faders

        /// <summary>
        /// Creates a new Instance of the Fade class and registers it so it can process the fading of audio volume each Update loop.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="curve"></param>
        /// <param name="length"></param>
        /// <param name="isLooping"></param>
        public void RegisterNewFade(AudioSource source, AnimationCurve curve, float length, bool isLooping)
        {
            List<int> toRemove = new List<int>();
            for (int i = 0; i < faders.Count; i++)
            {
                if (source == faders[i].GetAudioSource() || faders[i].GetAudioSource() == null)
                {
                    toRemove.Add(i);
                }
            }
            for (int i = toRemove.Count - 1; i >= 0; i--)
            {
                faders.RemoveAt(toRemove[i]);
            }
            faders.Add(new AudioFader(source, curve, length, isLooping));
        }

        /// <summary>
        /// Feed the delta time into the faders and remove them if they are done.
        /// </summary>
        private void HandleFaders()
        {
            List<int> toRemove = new List<int>();
            for (int i = 0; i < faders.Count; i++)
            {
                if (faders[i].Process(Time.deltaTime))
                {
                    toRemove.Add(i);
                }
            }
            for (int i = toRemove.Count - 1; i >= 0; i--)
            {
                faders.RemoveAt(toRemove[i]);
            }
        }
        #endregion

        #region Play Coroutines
        /// <summary>
        /// Play all clips after another.
        /// </summary>
        /// <param name="obj">EasyAudio source object.</param>
        /// <param name="source">AudioSource to play clips.</param>
        /// <param name="shuffle">Uses a shuffled list if true.</param>
        /// <returns>Coroutine that handles the timing of the clips.</returns>
        public IEnumerator PlayAll(EasyAudio player, AudioSource source, bool shuffle)
        {
            do
            {
                List<int> idsToPlay = new List<int>();

                for (int i = 0; i < player.easyAudio.audioClips.Count; i++)
                {
                    if (player.easyAudio.audioClips[i].clip == null) continue;
                    idsToPlay.Add(i);
                }

                if (shuffle)
                    idsToPlay = idsToPlay.OrderBy(X => UnityEngine.Random.value).ToList();

                foreach (var id in idsToPlay)
                {
                    if (source == null || !source.enabled)
                        break;
                    player.Play(source, id);
                    yield return new WaitForSeconds(player.easyAudio.audioClips[id].clip.length);
                }
                if (source == null || !source.enabled)
                    break;
            } while (source.loop);
            ClearDeadRoutines();
        }
        /// <summary>
        /// Play all clips combined as OneShot. Repeat if the AudioSource is set to loop.
        /// </summary>
        /// <param name="obj">Target EasyAudioObject</param>
        /// <param name="source">Target AudioSource</param>
        /// <param name="length">Highest length of all clips in seconds</param>
        /// <param name="delay">Delay before each playthrough in seconds</param>
        /// <returns></returns>
        public IEnumerator PlayAllTogether(EasyAudio player, AudioSource source, float length ,float delay)
        {
            WaitForSeconds maxLength = new WaitForSeconds(length);
            do
            {
                if (delay > 0)
                    yield return new WaitForSeconds(delay);
                if (source == null || !source.enabled)
                    break;
                for (int i = 0; i < player.easyAudio.audioClips.Count; i++)
                    if (player.easyAudio.audioClips[i].clip != null)
                        player.PlayOneShot(source, i);
                if (source.loop)
                    yield return maxLength;
                if (source == null || !source.enabled)
                    break;
            } while (source.loop);
            ClearDeadRoutines();
        }
        #endregion

        /// <summary>
        /// Stop all Coroutines and Faders that target the passed in AudioSource.
        /// </summary>
        /// <param name="source">Target AudioSource to stop.</param>
        public void StopBySource(AudioSource source)
        {
            // Stop Coroutine
            List<Coroutine> keysToRemove = new List<Coroutine>();
            foreach (var pair in activeCorountines)
            {
                if (pair.Value == null || pair.Value == source)
                {
                    keysToRemove.Add(pair.Key);
                }
            }
            foreach (var key in keysToRemove)
            {
                StopCoroutine(key);
                activeCorountines.Remove(key);
            }
            // Stop Faders
            for (int i = faders.Count - 1; i >= 0; i--)
            {
                if (SourceActiveCheck(faders[i].GetAudioSource()))
                {
                    faders[i].OnEndFade();
                    faders.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// Check activeCoroutines for Coroutines that reference a null or disabled source
        /// then disables and removes them from the dictionary.
        /// </summary>
        private void ClearDeadRoutines()
        {
            List<Coroutine> deadSourceKeys = new List<Coroutine>();
            foreach (var routine in activeCorountines)
            {
                if (SourceActiveCheck(routine.Value))
                {
                    deadSourceKeys.Add(routine.Key);
                }
            }
            foreach (var key in deadSourceKeys)
            {
                StopCoroutine(key);
                activeCorountines.Remove(key);
            }
        }
        private bool SourceActiveCheck(AudioSource source)
        {
            return source != null
                   && source.enabled
                   && source.isPlaying;
        }
    }
}

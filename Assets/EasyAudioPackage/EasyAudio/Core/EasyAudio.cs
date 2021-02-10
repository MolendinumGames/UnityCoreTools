using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAudioSystem.Core;

namespace EasyAudioSystem
{
    /* This class is the final EasyAudio property you will use in your scripts.
     * It contains functionality to apply settings and holds a reference to 
     * an EasyAudioObj and optioanlly to an AudioSource.
     * You can also specify the target as a passed in parameter:
     *  MyEasyAudio.Play(MyAudioSource);
     * For more information on the EasyAudio workflow please refer to the documentation:
     * 
     * For additional questions, feedback and/or feature suggestions please contact via eMail:
     * 
     */

    /// <summary>
    /// Plays your soundeffect from a plugged in EasyAudioObj on the targeted AudioSource.
    /// </summary>
    [System.Serializable]
    public class EasyAudio
    {
        /////////////// STATE DATA ////////////////////////////////

        /// <summary>
        /// The EasyAudioObj that holds the settings and clips for this soundeffect.
        /// </summary>
        public EasyAudioObj easyAudio;

        /// <summary>
        /// The target AudioSource.
        /// </summary>
        public AudioSource audioSource;

        private int counter = 0;

        ////////////////////////////////////////////////////////////

        #region Public Play Functions
        public void Play()
        {
            if (StateNullCheck(audioSource))
                Play(audioSource);
        }
        public void Play(AudioSource source)
        {
            if (!StateNullCheck(source)) return;

            if (easyAudio.playState != PlayState.AsOneshot)
                Stop(source);
            // Handle clip selection
            switch (easyAudio.selectState)
            {
                case SelectState.Random:
                    EasyAudioPlay(source, easyAudio.GetRandomId(), easyAudio.playState);
                    break;
                case SelectState.Specific:
                    EasyAudioPlay(source, easyAudio.specificId, easyAudio.playState);
                    break;
                case SelectState.Next:
                    EasyAudioPlay(source, counter, easyAudio.playState);
                    IncreaseCounter();
                    break;
                case SelectState.AllInRow:
                    EasyAudioManager.Instance.StartCoroutine(EasyAudioManager.Instance.PlayAll(this, source, false));
                    if (easyAudio.playState == PlayState.FadeInOut)
                        EasyAudioManager.Instance.RegisterNewFade(source, easyAudio.fadeCurve, easyAudio.GetCombinedClipLenght(), easyAudio.isLooping);
                    break;
                case SelectState.AllInRowShuffled:
                    EasyAudioManager.Instance.StartCoroutine(EasyAudioManager.Instance.PlayAll(this, source, true));
                    if (easyAudio.playState == PlayState.FadeInOut)
                        EasyAudioManager.Instance.RegisterNewFade(source, easyAudio.fadeCurve, easyAudio.GetCombinedClipLenght(), easyAudio.isLooping);
                    break;
                case SelectState.AllOverlapped:
                    if (easyAudio.playState != PlayState.AsOneshot)
                        source.Stop();
                    float timer = easyAudio.playState == PlayState.Delayed ? easyAudio.delay : 0;
                    EasyAudioManager.Instance.StartCoroutine(EasyAudioManager.Instance.PlayAllTogether(this, source, easyAudio.GetLongestClipLength(), timer));
                    if (easyAudio.playState == PlayState.FadeInOut)
                        EasyAudioManager.Instance.RegisterNewFade(source, easyAudio.fadeCurve, easyAudio.GetLongestClipLength(), easyAudio.isLooping);
                    break;
            }
        }
        public void Play(AudioSource source, int specificId)
        {
            if (!StateNullCheck(source)) return;
            Stop(source);
            PlayState state = easyAudio.playState;
            if (state == PlayState.Delayed || state == PlayState.FadeInOut)
                state = PlayState.Normal;

            EasyAudioPlay(source, specificId, state);

        }
        public void PlayOneShot(AudioSource source, int id)
        {
            if (!StateNullCheck(source)) return;
            EasyAudioPlay(source, id, PlayState.AsOneshot);
        }
        #endregion

        #region Public Stop Functions
        /// <summary>
        /// Stop the audio from the source that was set in the inspector. Use this instead of AudioSource.Stop()!
        /// </summary>
        public void Stop()
        {
            if (audioSource == null || !audioSource.enabled)
                return;

            audioSource.Stop();
            EasyAudioManager.Instance.StopBySource(audioSource);
        }
        /// <summary>
        /// Stop the audio currently being played by the target AudioSource. Use this instead of AudioSource.Stop()!
        /// </summary>
        /// <param name="source">The AudioSource to stop</param>
        public void Stop(AudioSource source)
        {
            if (source == null || !source.enabled)
                return;

            source.Stop();
            EasyAudioManager.Instance.StopBySource(source);
        }
        #endregion

        /// <summary>
        /// Apply Settings to source and play by index and Playstate.
        /// </summary>
        /// <param name="source">Target AudioSource</param>
        /// <param name="id">AudioClip from audioClips at Index</param>
        /// <param name="ps">The state the clip is played</param>
        private void EasyAudioPlay(AudioSource source, int id, PlayState ps)
        {
            if (!StateNullCheck(source)) return;
            if (!easyAudio.ClipCheck(id)) return;

            AssignMixerGroup(source);
            source.volume = easyAudio.GetVolume * easyAudio.audioClips[id].volume;
            source.pitch = easyAudio.GetPitch * easyAudio.audioClips[id].pitch;
            source.loop = easyAudio.isLooping;
            source.clip = easyAudio.audioClips[id].clip;

            switch (ps)
            {
                case PlayState.Normal:
                    source.Play();
                    break;
                case PlayState.AsOneshot:
                    source.PlayOneShot(easyAudio.audioClips[id].clip);
                    break;
                case PlayState.Delayed:
                    source.PlayDelayed(Mathf.Abs(easyAudio.delay));
                    break;
                case PlayState.FadeInOut:
                    source.Play();
                    EasyAudioManager.Instance.RegisterNewFade(source, easyAudio.fadeCurve, easyAudio.audioClips[id].clip.length, easyAudio.isLooping);
                    break;
            }
        }
        private void AssignMixerGroup(AudioSource source)
        {
            if (easyAudio.mixerGroup == null) return;
            if (easyAudio.mixerGroup == source.outputAudioMixerGroup) return;
            source.outputAudioMixerGroup = easyAudio.mixerGroup;
        }

        private void IncreaseCounter() => counter = counter == easyAudio.audioClips.Count - 1 ? 0 : ++counter;

        /// <summary>
        /// Clips played with PlayState.Next will start at Index 0 again.
        /// </summary>
        public void ResetCounter() => counter = 0;

        private bool StateNullCheck(AudioSource source)
        {
            if (easyAudio == null)
            {
                if (source == null)
                {
                    Debug.LogWarning("Play was called on EasyAudio but no Source and Data are plugged in!");
                    return false;
                }
                else
                {
                    Debug.LogWarning($"EasyAudio.Play way called on the AudioSource: {source} but no EasyAudioObj was plugged in.");
                    return false;
                }
            }
            if (source == null)
            {
                Debug.LogWarning($"No audiosource passed in for {easyAudio.name}! Audioclip wasn't played!");
                return false;
            }
            else if (!source.enabled)
            {
                if (easyAudio.enableSource)
                {
                    source.enabled = true;
                    Debug.Log($"Audiosource of {source.gameObject.name} was disabled. {easyAudio.name} enabled it.");
                    return true;
                }
                else
                {
                    Debug.LogWarning($"Target AudioSource in {source.gameObject.name} is not enabled and {easyAudio.name} does not have permission to enable it.");
                    return false;
                }
            }
            return true;
        }
    }
}

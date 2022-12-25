/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CoreTools;

namespace CoreTools.DialogueSystem
{
    public class TextAnimator : MonoBehaviour
    {
        public TextAnimationStyle animationStyle = TextAnimationStyle.Instant;

        [SerializeField]
        [Range(0f, 1f)]
        float waitAmount = 0.05f;

        [SerializeField]
        TextMeshProUGUI textField;

        [SerializeField]
        AudioSource audioSource;

        [SerializeField]
        AudioClip textSound;

        //[SerializeField]
        //EasyAudioSystem.EasyAudio textAudio;

        [Header("Change last drawn:")]
        public bool bold = false;
        public bool italic = false;
        public bool resize = false;
        [Range(0, 200)]
        public int size = 100;
        public bool recolor = false;
        public Color color = Color.white;

        public void PlayText(string t)
        {
            if (textField == null)
            {
                Debug.LogError($"Textfield for TextAnimator not set of GameObj {gameObject.name}");
                return;
            }
            if (string.IsNullOrEmpty(t))
            {
                Debug.LogError($"Empty text passed to TextAnimator!");
                return;
            }
            StopAllCoroutines();
            textField.text = "";

            switch (animationStyle)
            {
                case TextAnimationStyle.Instant:
                    textField.text = t;
                    break;
                case TextAnimationStyle.PerLetter:
                    StartCoroutine(nameof(PlayByLetter), t);
                    break;
                case TextAnimationStyle.PerWord:
                    StartCoroutine(nameof(PlayByWord), t);
                    break;
                case TextAnimationStyle.Typewriter:
                    throw new Exception("Typewriter animation not implemented!");
            }
        }
        IEnumerator PlayByLetter(string t)
        {
            textField.text = t;
            textField.maxVisibleCharacters = 0;

            string cleanedText = StringUtility.StripHTMLLazy(t);
            WaitForSeconds wait = new WaitForSeconds(waitAmount);
            (string frontTags, string endTags) = GetHighligthTags();

            int clipCounter = 0;

            for (int i = 0; i < cleanedText.Length; i++)
            {
                // Reset the text to clear last char animation
                textField.text = t;
                // increases visible char count. Rich text will be auto skipped
                textField.maxVisibleCharacters = i + 1;

                // Get the last visible characters index wihtin the original uncleaned string
                int index = textField.GetTextInfo(t).characterInfo[i].index;
                // Insert new animation tags

                textField.text = textField.text.Insert(index, frontTags);
                textField.text += endTags;

                clipCounter++;
                //if (textAudio != null && clipCounter % 2 == 0)
                //    textAudio.Play();
                PlayTextSound();


                yield return wait;
            }
            // clear animation of last word
            textField.text = t;
        }        
        IEnumerator PlayByWord(string t)
        {
            textField.text = t;
            textField.maxVisibleCharacters = 0;

            string originalText = t;
            string[] unclearedWords = GetWordsFromDialogue(t, false);
            string[] clearedWords = GetWordsFromDialogue(t, true);
            
            WaitForSeconds wait = new WaitForSeconds(waitAmount);

            (string frontTags, string endTags) = GetHighligthTags();
            
            int visible = 0;  // Tags are not counted by maxVisibleCharacters in TMPro
            for (int i = 0; i < clearedWords.Length; i++)
            {
                if (string.IsNullOrEmpty(clearedWords[i])) continue;
                // Make another word + one whitespace visible
                visible += clearedWords[i].Length + 1;
                textField.maxVisibleCharacters = visible;

                // Reset the text and reinsert the tags for the newest word animation
                textField.text = originalText;
                textField.text = textField.text.Insert(GetCombinedWordLength(unclearedWords.Take(i)),
                                                       frontTags);
                textField.text = textField.text.Insert(GetCombinedWordLength(unclearedWords.Take(i+1)) + frontTags.Length -1,
                                                       endTags);
                textField.SetAllDirty();

                //if (textAudio != null)
                //    textAudio.Play();
                PlayTextSound();

                yield return wait;
            }
            // clear animation of last word
            textField.text = originalText;
        }
        (string, string) GetHighligthTags()
        {
            string frontTags = "";
            string endTags = "";
            if (bold)
            {
                frontTags += "<bold>";
                endTags += "</bold>";
            }
            if (italic)
            {
                frontTags += "<i>";
                endTags += "</i>";
            }
            if (resize)
            {
                frontTags += $"<size={size}%>";
                endTags += "</size>";
            }
            if (recolor)
            {
                frontTags += $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
                endTags += "</color>";
            }
            return (frontTags, endTags);
        }
        private string[] GetWordsFromDialogue(string d, bool cleaned)
        {
            string[] words = d.Split(' ');
            if (cleaned)
                for (int i = 0; i < words.Length; i++)
                {
                    // The array is used to calculate the visible characters per loop so
                    // tags have to be stripped to get the correct number.
                    // However the final TMPro field will ignore tags for visible count.
                    words[i] = StringUtility.StripHTMLLazy(words[i]);
                }
            return words;
        }
        private int GetCombinedWordLength(IEnumerable<string> words)
        {
            int counter = 0;
            foreach (string w in words)
            {
                counter += w.Length + 1; // +1 for whitespace used to split
            }
            return counter;
        }
        private void PlayTextSound()
        {
            if (audioSource != null && textSound != null)
            {
                audioSource.clip = textSound;
                audioSource.Play();
            }
        }
    }
}
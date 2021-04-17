using System;
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
        [SerializeField]
        TextAnimationStyle animationStyle;
        public TextAnimationStyle AnimationStyle { get => animationStyle; set => animationStyle = value; }

        [SerializeField]
        [Range(0f, .3f)]
        float waitAmount = 0.05f;

        [SerializeField]
        TextMeshProUGUI textField;

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
                    break;
            }
        }
        IEnumerator PlayByLetter(string t)
        {
            WaitForSeconds wait = new WaitForSeconds(waitAmount);
            for (int i = 0; i < t.Length; i++)
            {
                string newText = t.Substring(0, i).PadRight(t.Length - 1 - i);
                textField.text = newText;
                yield return wait;
            }
        }
        IEnumerator PlayByWord(string t)
        {
            string[] words = t.Split(' ');
            WaitForSeconds wait = new WaitForSeconds(waitAmount);
            System.Text.StringBuilder builder = new System.Text.StringBuilder(words[0], t.Length);
            for (int i = 1; i < words.Length; i++)
            {
                builder.Append(words[i] + ' ');
                textField.text = builder.ToString();
                yield return wait;
            }
        }
    }
}
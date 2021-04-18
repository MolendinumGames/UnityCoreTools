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
        [SerializeField]
        TextAnimationStyle animationStyle;
        public TextAnimationStyle AnimationStyle { get => animationStyle; set => animationStyle = value; }

        [SerializeField]
        [Range(0f, 1f)]
        float waitAmount = 0.05f;

        [SerializeField]
        TextMeshProUGUI textField;

        [SerializeField]
        EasyAudioSystem.EasyAudio textAudio;

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
                    StartCoroutine(nameof(PlayByWordHighlighted), t);
                    break;
                case TextAnimationStyle.Typewriter:
                    throw new Exception("Typewriter animation not implemented!");
            }
        }
        IEnumerator PlayByLetter(string t)
        {
            textField.text = t;
            textField.maxVisibleCharacters = 0;
            WaitForSeconds wait = new WaitForSeconds(waitAmount);

            for (int i = 0; i < t.Length; i++)
            {
                textField.maxVisibleCharacters = i;
                if (textAudio != null)
                    textAudio.Play();
                yield return wait;
            }
        }
        IEnumerator PlayByWord(string t)
        {
            textField.text = t;
            textField.maxVisibleCharacters = 0;
            string[] words = t.Split(' ');
            WaitForSeconds wait = new WaitForSeconds(waitAmount);

            int visible = 0;
            for (int i = 0; i < words.Length; i++)
            {
                visible += words[i].Length + 1;
                textField.maxVisibleCharacters = visible;
                if (textAudio != null)
                    textAudio.Play();
                yield return wait;
            }
        }
        
        IEnumerator PlayByWordHighlighted(string t)
        {
            textField.text = t;
            string originalText = t;
            textField.maxVisibleCharacters = 0;
            string[] words = t.Split(' ');
            WaitForSeconds wait = new WaitForSeconds(waitAmount);

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
            

            int visible = 0;
            for (int i = 0; i < words.Length; i++)
            {
                visible += words[i].Length + 1;
                textField.maxVisibleCharacters = visible;

                // reset the text and reinsert the tags for the newest word
                textField.text = originalText;
                textField.text = textField.text.Insert(visible - words[i].Length -1, frontTags);
                textField.text = textField.text + endTags;
                textField.SetAllDirty();

                if (textAudio != null)
                    textAudio.Play();

                yield return wait;
            }
            textField.text = originalText;
        }
        //int rolloverCharacterSpread = 1;
        //Color colorTint;
        //float fadeSpeed = 1f;
        //IEnumerator AnimateVertexColors()
        //{
        //    // Need to force the text object to be generated so we have valid data to work with right from the start.
        //    textField.ForceMeshUpdate();
        //    TMP_TextInfo textInfo = textField.textInfo;
        //    Color32[] newVertexColors;
        //    int currentCharacter = 0;
        //    int startingCharacterRange = currentCharacter;
        //    bool isRangeMax = false;

        //    while (!isRangeMax)
        //    {
        //        int characterCount = textInfo.characterCount;
        //        // Spread should not exceed the number of characters.
        //        byte fadeSteps = (byte)Mathf.Max(1, 255 / RolloverCharacterSpread);
        //        for (int i = startingCharacterRange; i < currentCharacter + 1; i++)
        //        {
        //            // Skip characters that are not visible
        //            if (!textInfo.characterInfo[i].isVisible)
        //                continue;
        //            // Get the index of the material used by the current character.
        //            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
        //            // Get the vertex colors of the mesh used by this text element (character or sprite).
        //            newVertexColors = textInfo.meshInfo[materialIndex].colors32;
        //            // Get the index of the first vertex used by this text element.
        //            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
        //            // Get the current character's alpha value.
        //            byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a - fadeSteps, 0, 255);
        //            // Set new alpha values.
        //            newVertexColors[vertexIndex + 0].a = alpha;
        //            newVertexColors[vertexIndex + 1].a = alpha;
        //            newVertexColors[vertexIndex + 2].a = alpha;
        //            newVertexColors[vertexIndex + 3].a = alpha;
        //            // Tint vertex colors
        //            // Note: Vertex colors are Color32 so we need to cast to Color to multiply with tint which is Color.
        //            newVertexColors[vertexIndex + 0] = (Color)newVertexColors[vertexIndex + 0] * colorTint;
        //            newVertexColors[vertexIndex + 1] = (Color)newVertexColors[vertexIndex + 1] * colorTint;
        //            newVertexColors[vertexIndex + 2] = (Color)newVertexColors[vertexIndex + 2] * colorTint;
        //            newVertexColors[vertexIndex + 3] = (Color)newVertexColors[vertexIndex + 3] * colorTint;
        //            if (alpha == 0)
        //            {
        //                startingCharacterRange += 1;
        //                if (startingCharacterRange == characterCount)
        //                {
        //                    // Update mesh vertex data one last time.
        //                    textField.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        //                    yield return new WaitForSeconds(1.0f);
        //                    // Reset the text object back to original state.
        //                    textField.ForceMeshUpdate();
        //                    yield return new WaitForSeconds(1.0f);
        //                    // Reset our counters.
        //                    currentCharacter = 0;
        //                    startingCharacterRange = 0;
        //                    //isRangeMax = true; // Would end the coroutine.
        //                }
        //            }
        //        }
        //        // Upload the changed vertex colors to the Mesh.
        //        textField.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        //        if (currentCharacter + 1 < characterCount) currentCharacter += 1;
        //        yield return new WaitForSeconds(0.25f - fadeSpeed * 0.01f);
        //    }
        //}
    }
}
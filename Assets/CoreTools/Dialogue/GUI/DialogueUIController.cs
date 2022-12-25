/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoreTools;
using System.Linq;
using TMPro;

namespace CoreTools.DialogueSystem
{
    public class DialogueUIController : MonoBehaviour
    {
        private Dialogue viewedDialogue;
        private string viewedNode;
        private List<Button> options = new List<Button>();

        [SerializeField]
        private GameObject dialogueParent;

        [Header("Layout Elements")]
        [SerializeField]
        private TextMeshProUGUI textArea;
        [SerializeField]
        private TextMeshProUGUI speakerField;
        [SerializeField]
        TextAnimator textAnim;

        [SerializeField]
        private bool orientateSpeaker = false;

        [SerializeField]
        private Image lefticon;
        [SerializeField]
        private Image rightIcon;
        [SerializeField]
        private GameObject optionArea;
        [SerializeField]
        private GameObject optionButtonPrefab;
        [SerializeField]
        private Button nextButton;

        private void Awake()
        {
            ClearUI();
        }

        public void AddOption(int index, string text)
        {
            Button button = Instantiate(optionButtonPrefab, optionArea.transform).GetComponent<Button>();
            options.Add(button);
            button.GetComponentInChildren<TextMeshProUGUI>().text = text;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => Next(index));
        }

        public void ProcessDialogue(Dialogue dialogue)
        {
            ClearUI();
            if (dialogue == null)
            {
                Debug.LogWarning("Null Dialogue has been pushed!");
                return;
            }
            viewedDialogue = dialogue;
            var node = dialogue.GetFirstNode();
            if (node != null)
            {
                ProcessNode(node);
            }
            else
            {
                Debug.LogWarning($"Empty but non null Dialogue has been pushed! Dialogue: {dialogue.name}");
            }
        }
        private void ProcessNode(DialogueNode node)
        {
            ClearUI();
            viewedNode = node.UniqueID;
            nextButton.gameObject.SetActive(true);

            if (!string.IsNullOrWhiteSpace(node.Text))
            {
                textArea.gameObject.SetActive(true);
                textAnim.PlayText(node.Text);
            }
            else
            {
                if (!(node is ChoiceNode))
                {
                    Next();
                    return;
                }
            }

            if (node.Orientation == DialogueOrientation.Left)
            {
                if (!string.IsNullOrWhiteSpace(node.Speaker))
                {
                    speakerField.gameObject.SetActive(true);
                    speakerField.text = node.Speaker;
                    if(orientateSpeaker)
                        speakerField.alignment = TMPro.TextAlignmentOptions.MidlineLeft;
                    
                }

                if (node.Portrait != null)
                {
                    lefticon.transform.parent.gameObject.SetActive(true);
                    lefticon.sprite = node.Portrait;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(node.Speaker))
                {
                    speakerField.gameObject.SetActive(true);
                    speakerField.text = node.Speaker;
                    if (orientateSpeaker)
                        speakerField.alignment = TMPro.TextAlignmentOptions.MidlineRight;
                }

                if (node.Portrait != null)
                {
                    rightIcon.transform.parent.gameObject.SetActive(true);
                    rightIcon.sprite = node.Portrait;
                }
            }


            if (node is ChoiceNode choiceNode)
            {
                if (choiceNode.ChoiceAmount > 0)
                {
                    nextButton.gameObject.SetActive(false);
                    optionArea.SetActive(true);
                    string[] choices = choiceNode.GetAllChoiceTexts();
                    for (int i = 0; i < choiceNode.ChoiceAmount; i++)
                    {
                        AddOption(i, choices[i]);
                    }
                }
                else
                {
                    Debug.LogWarning($"Choice Node pushed without choices!");
                    // ChoiceNode.Text will still be displayed
                    // but Next-button will remain active to proceed
                }
            }
        }
        public void Next()
        {
            var node = viewedDialogue.Next(viewedNode);
            if (node == null)
            {
                CloseDialogue();
            }
            else
            {
                viewedNode = node.UniqueID;
                ProcessNode(node);
            }
        }
        private void Next(int choice)
        {
            var node = viewedDialogue.Next(viewedNode, choice);
            if (node == null)
                CloseDialogue();
            else
            {
                viewedNode = node.UniqueID;
                ProcessNode(node);
            }
        }
        private void CloseDialogue()
        {
            ClearUI();
            dialogueParent.SetActive(false);
            // Reenable player controls
        }
        private void ClearUI()
        {
            foreach (GameObject option in options.Select( o => o.gameObject))
            {
                Destroy(option);
            }
            options.Clear();

            textArea.text = "";
            textArea.gameObject.SetActive(false);
            speakerField.gameObject.SetActive(false);
            lefticon.transform.parent.gameObject.SetActive(false);
            rightIcon.transform.parent.gameObject.SetActive(false);
        }
    }
}
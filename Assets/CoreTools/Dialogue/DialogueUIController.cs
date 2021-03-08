using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoreTools;
using TMPro;

namespace CoreTools.Dialogue
{
    public class DialogueUIController : MonoBehaviour
    {
        [Header("DownLeft Layout")]
        [SerializeField]
        private TextMeshProUGUI textArea;
        private TextMeshProUGUI speakerField;
        private Image speakerIcon;
        private TextMeshPro optionArea;
        private GameObject optionButtonPrefab;
        private List<Button> options = new List<Button>();
        public void AddOption(int index, string text)
        {
            Button button = Instantiate(optionButtonPrefab, optionArea.transform).GetComponent<Button>();
            options.Add(button);
            button.GetComponentInChildren<TextMeshProUGUI>().text = text;
            // add listener to button

        }


        public void ProcessDialogue(DialogueSO dialogue)
        {

        }
    }
}
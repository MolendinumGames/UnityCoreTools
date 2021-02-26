using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        private string uniqueID;

        private string text;

        private Sprite speakerImage;

        private string speaker;

        string child;
    }
}
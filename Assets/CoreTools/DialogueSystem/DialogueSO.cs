using CoreTools.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        [SerializeField]
        DialogueNode[] nodes;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTools.NodeSystem;

namespace CoreTools.DialogueSystem
{
    [System.Serializable]
    public class ChoiceField : ISingleChild
    {
        public string text;

        // UNDO IN CHOICE NODE
        [SerializeField]
        private string childId;
        public virtual string ChildID
        {
            get => childId;
#if UNITY_EDITOR
            set
            {
                if (childId != value)
                {
                    childId = value;
                }
            }
        }

        public void ClearChild()
        {
            ChildID = null;
        }

        public void ClearChild(string id)
        {
            if (ChildID == id)
                ClearChild();
        }
#endif

        public bool HasChild() => !string.IsNullOrWhiteSpace(childId);
    }
}
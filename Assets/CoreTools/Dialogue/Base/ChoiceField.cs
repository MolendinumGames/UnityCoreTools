using UnityEngine;
using CoreTools.NodeSystem;

namespace CoreTools.DialogueSystem
{
    [System.Serializable]
    public class ChoiceField : ISingleChild
    {
        // (!) Undo is handled in choice node

        public string text;

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
#endif
        }

#if UNITY_EDITOR
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

        public bool HasChild() =>
            !string.IsNullOrWhiteSpace(childId);
    }
}
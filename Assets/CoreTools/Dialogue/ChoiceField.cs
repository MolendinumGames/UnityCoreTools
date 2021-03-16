using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTools.NodeSystem;

namespace CoreTools.Dialogue
{
    [System.Serializable]
    public class ChoiceField : ISingleChild
    {
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
        }
#endif

        public bool HasChild() => !string.IsNullOrWhiteSpace(childId);
    }
}
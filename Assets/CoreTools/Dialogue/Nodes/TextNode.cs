using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools.NodeSystem;

namespace CoreTools.DialogueSystem
{
    public class TextNode : DialogueNode, ISingleChild
    {

        [SerializeField]
        private string childID;
        public virtual string ChildID
        {
            get => childID;
#if UNITY_EDITOR
            set
            {
                if (childID != value)
                {
                    Undo.RecordObject(this, "Changed Dialogue Node childID");
                    childID = value;
                    EditorUtility.SetDirty(this);
                }
            }
#endif
        }
        public virtual bool HasChild() => !string.IsNullOrWhiteSpace(childID);
        public virtual void ClearChild()
        {
            ChildID = null;
        }
        public virtual void ClearChild(string id)
        {
            if (ChildID == id)
                ClearChild();
        }
        protected override void OnReset()
        {
            childID = null;
        }
    }
}
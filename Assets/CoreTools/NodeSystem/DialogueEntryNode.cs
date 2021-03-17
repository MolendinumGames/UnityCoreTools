using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CoreTools.NodeSystem
{
    public class DialogueEntryNode : GraphNode, ISingleChild
    {
        Rect rect = new Rect(10f, 10f, 100f, 80f);
        public override Rect NodeRect { get => rect; set => rect = value; }
        public override Rect GetBaseRect() => rect;

        [SerializeField]
        string childID;
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

        public bool HasChild() => !string.IsNullOrWhiteSpace(childID);

#if UNITY_EDITOR
        public override void Reset()
        {
            ChildID = null;
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
    }
}
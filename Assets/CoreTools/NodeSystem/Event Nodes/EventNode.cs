using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CoreTools.NodeSystem
{
    public abstract class EventNode : GraphNode, ISingleChild, IMultiChild
    {
        public abstract void Raise();

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

        [SerializeField]
        private List<string> children = new List<string>();
        public IEnumerable<string> GetChildren() => children;
        public int ChildAmount => children.Count;

        public bool HasChild() => !string.IsNullOrWhiteSpace(childID) || ChildAmount > 0;

#if UNITY_EDITOR
        [SerializeField]
        Rect rect = new Rect(10f, 10f, 250f, 95f);
        public override Rect NodeRect { get => rect; set => rect = value; }
        public override Rect GetBaseRect() => rect;
#endif
    }
}
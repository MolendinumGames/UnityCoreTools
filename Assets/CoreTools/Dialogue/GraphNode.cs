using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CoreTools.Dialogue
{
    public abstract class GraphNode : ScriptableObject
    {
        [SerializeField]
        private string uniqueID = "newId";
        public string UniqueID
        {
            get => uniqueID;
            set
            {
                Undo.RecordObject(this, "Changed Node ID");
                uniqueID = value;
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField]
        string childID;
        public virtual string ChildID
        {
            get => childID;
            set
            {
                Undo.RecordObject(this, "Changed Dialogue Node childID");
                childID = value;
                EditorUtility.SetDirty(this);
            }
        }

#if UNITY_EDITOR
        public abstract Rect NodeRect { get; set; }

        public void SetPosition(Vector2 newPos) => NodeRect = new Rect(newPos, NodeRect.size);
#endif
    }
}
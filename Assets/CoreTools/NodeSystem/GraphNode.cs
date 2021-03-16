using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CoreTools.NodeSystem
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
                if (uniqueID != value)
                {
                    Undo.RecordObject(this, "Changed Node ID");
                    uniqueID = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        //[SerializeField]
        //string childID;
        //public virtual string ChildID
        //{
        //    get => childID;
        //    set
        //    {
        //        if (childID != value)
        //        {
        //            Undo.RecordObject(this, "Changed Dialogue Node childID");
        //            childID = value;
        //            EditorUtility.SetDirty(this);
        //        }
        //    }
        //}

#if UNITY_EDITOR
        public abstract Rect NodeRect { get; set; }

        public virtual void SetPosition(Vector2 newPos)
        {
            Undo.RecordObject(this, "Set Node Position");
            newPos.x = Mathf.Clamp(newPos.x, 0f, 4000f);
            newPos.y = Mathf.Clamp(newPos.y, 0f, 4000f);
            NodeRect = new Rect(newPos, NodeRect.size);
            EditorUtility.SetDirty(this);
        }
        public abstract Rect GetBaseRect();
        public abstract void Reset();
#endif
    }
}
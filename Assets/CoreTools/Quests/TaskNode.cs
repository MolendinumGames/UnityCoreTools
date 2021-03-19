using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools.NodeSystem;

namespace CoreTools.QuestSystem
{
    public class TaskNode : GraphNode, IMultiChild
    {
        public override bool IsEntry => false;

        [SerializeField]
        private List<string> children = new List<string>();

        public IEnumerable<string> GetChildren() => children;

        public bool HasChild(string id) => children.Contains(id);


        public bool HasChild() => children.Count > 0;
        public int ChildAmount => children.Count;


#if UNITY_EDITOR
        public void AddChild(string id)
        {
            if (!children.Contains(id))
            {
                Undo.RecordObject(this, "Added child to TaskNode");
                children.Add(id);
                EditorUtility.SetDirty(this);
            }
        }

        public void ClearAllChildren()
        {
            Undo.RecordObject(this, "Cleared TaskNode children");
            children.Clear();
            EditorUtility.SetDirty(this);
        }

        public void ClearChild(string id)
        {
            if (children.Contains(id))
            {
                Undo.RecordObject(this, "Cleared child from Quest Task");
                children.Remove(id);
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField]
        protected Rect rect = new Rect(10, 10, 300, 180);
        public override Rect NodeRect { get => rect; set => rect = value; }
        public override Rect GetBaseRect() => rect;
        public override void Reset()
        {
            Undo.RecordObject(this, "Reset QuestTaskNode");
            children.Clear();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
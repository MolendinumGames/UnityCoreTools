using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools.NodeSystem;

namespace CoreTools.QuestSystem
{
    public class QuestEntryNode : GraphNode, IMultiChild
    {
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
                Undo.RecordObject(this, "Added child to quest entry node");
                children.Add(id);
                EditorUtility.SetDirty(this);
            }
        }

        public void ClearAllChildren()
        {
            Undo.RecordObject(this, "Cleared quest entry node children");
            children.Clear();
            EditorUtility.SetDirty(this);
        }

        public void ClearChild(string id)
        {
            if (children.Contains(id))
            {
                Undo.RecordObject(this, "Cleared child from Quest entry");
                children.Remove(id);
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField]
        protected Rect rect = new Rect(10f, 10f, 100f, 80f);
        public override Rect NodeRect { get => rect; set => rect = value; }
        public override Rect GetBaseRect() => rect;
        public override void Reset()
        {
            Undo.RecordObject(this, "Reset QuestNode");
            children.Clear();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
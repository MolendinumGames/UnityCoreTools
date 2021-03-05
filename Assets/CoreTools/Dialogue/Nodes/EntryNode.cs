using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CoreTools.Dialogue
{
    public class EntryNode : GraphNode
    {
        Rect rect = new Rect(10f, 10f, 100f, 80f);
        public override Rect NodeRect { get => rect; set => rect = value; }

#if UNITY_EDITOR
        public override void Reset()
        {
            ChildID = null;
        }
#endif
    }
}
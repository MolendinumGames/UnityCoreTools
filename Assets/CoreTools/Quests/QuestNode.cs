using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTools;
using CoreTools.NodeSystem;

namespace CoreTools.QuestSystem
{
    public class QuestNode : GraphNode
    {
#if UNITY_EDITOR
        private Rect rect = new Rect(10f, 10f, 250f, 180f);
        public override Rect NodeRect { get => rect; set => rect = value; }
        public override Rect GetBaseRect() => rect;

        public override void Reset()
        {
            throw new System.NotImplementedException();
        }
#endif
        // conditions
        // description

    }
}
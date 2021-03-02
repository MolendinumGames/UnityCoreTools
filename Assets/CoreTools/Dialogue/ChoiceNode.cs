using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.Dialogue
{
    public class ChoiceNode : GraphNode
    {
        [SerializeField]
        int selectedPath = 0;
        public override string ChildID 
        { 
            get => base.ChildID;
            set => base.ChildID = value;
        }

        [SerializeField]
        Rect rect = new Rect(10f, 10f, 330f, 260f);
        public override Rect NodeRect { get => rect; set => rect = value; }
    }
}
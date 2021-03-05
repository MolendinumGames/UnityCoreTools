using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CoreTools.Dialogue
{
    public abstract class EventNode : GraphNode
    {
        public abstract void Raise();

        [SerializeField]
        Rect rect = new Rect(10f, 10f, 250f, 95f);
        public override Rect NodeRect { get => rect; set => rect = value; }
    }
}
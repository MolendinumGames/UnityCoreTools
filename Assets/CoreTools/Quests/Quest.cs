using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;
using CoreTools.NodeSystem;


namespace CoreTools.QuestSystem
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
    public class Quest : NodeHolder
    {
        protected override void SetupRootNode()
        {
            if (entryNode == null)
            {
                entryNode = CreateInstance<QuestEntryNode>();
                SetupNewNode(entryNode);
                entryNode.name = "Entry Node";
            }
        }

        public TaskNode CreateTaskNode()
        {
            var newNode = CreateInstance<TaskNode>();
            Undo.RegisterCreatedObjectUndo(newNode, "Created new task node");
            SetupNewNode(newNode);
            return newNode;
        }
    }
}
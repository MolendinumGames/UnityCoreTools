using CoreTools.NodeSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CoreTools.QuestSystem.Editor
{
    public class QuestDrawer : NodeDrawer
    {
        private QuestEditorWindow questEditor;
        private float oldLabelWidth;
        public QuestDrawer(QuestEditorWindow questEditor, NodeEditorWindow nodeEditor) : base(nodeEditor)
        {
            this.questEditor = questEditor;
            this.nodeEditor = nodeEditor;

            oldLabelWidth = EditorGUIUtility.labelWidth;
        }
        public override void DrawGraphNode(GraphNode node)
        {
            switch (node)
            {
                case QuestEntryNode n:
                    DrawEntryNode(n);
                    break;
                case TaskNode n:
                    DrawTaskNode(n);
                    break;
                default:
                    break;
            }
            if (node is IMultiChild multiParent)
            {
                // DrawAllConnections ? 
            }
        }
        private void DrawEntryNode(QuestEntryNode node)
        {

        }
        private void DrawTaskNode(TaskNode node)
        {
            var targetStyle = nodeEditor.focusedNode == node ? selectedStyle : nodeStyle;
            GUILayout.BeginArea(node.NodeRect, targetStyle);
            DrawHeader(node);
            GUILayout.EndArea();
        }
    }
}
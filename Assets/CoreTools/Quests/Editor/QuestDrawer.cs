using CoreTools.NodeSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CoreTools.QuestSystem.Editor
{
    public class QuestDrawer : GraphDrawer
    {
        protected QuestEditorWindow questEditor;
        public QuestDrawer(QuestEditorWindow questEditor, NodeEditorWindow nodeEditor) : base(nodeEditor)
        {
            this.questEditor = questEditor;
            this.nodeEditor = nodeEditor;
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
            DrawConnections(node);
        }
        private void DrawEntryNode(QuestEntryNode node)
        {
            GUILayout.BeginArea(node.NodeRect, entryStyle);
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = Mathf.CeilToInt(headerStyle.fontSize * 1.5f);
            EditorGUILayout.LabelField("Entry", headerStyle, GUILayout.Height(headerStyle.lineHeight * 1.8f));
            GUILayout.EndArea();
            DrawOutConnector(node);
        }
        private void DrawTaskNode(TaskNode node)
        {
            var targetStyle = nodeEditor.focusedNode == node ? selectedStyle : nodeStyle;
            GUILayout.BeginArea(node.NodeRect, targetStyle);
            DrawHeader(node);
            GUILayout.EndArea();
            DrawInConnector(node);
            DrawOutConnector(node);
        }
    }
}
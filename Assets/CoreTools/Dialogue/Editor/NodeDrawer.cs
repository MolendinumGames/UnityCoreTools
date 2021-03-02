using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;
using CoreTools.Dialogue;

namespace CoreTools.Dialogue.Editor
{
    public class NodeDrawer
    {
        protected readonly DialogueEditor dialogueEditor;

        public readonly Vector2 radioButtonSize = new Vector2(15f, 15f);
        public GUIStyle radioActiveStyle;
        public GUIStyle nodeStyle;
        public GUIStyle entryStyle;
        public GUIStyle headerStyle;
        private float oldLabelWidth;
        public NodeDrawer(DialogueEditor dialogueEditor)
        {
            this.dialogueEditor = dialogueEditor;
            oldLabelWidth = EditorGUIUtility.labelWidth;
            GenerateStyles();
        }

        public void DrawGraphNode(GraphNode node)
        {
            switch (node)
            {
                case EntryNode n:
                    DrawEntryNode(n);
                    break;
                case DialogueNode n:
                    DrawStandardNode(n);
                    break;
                case VoidEventNode n:
                    DrawVoidEventNode(n);
                    break;
                case BoolEventNode n:
                    DrawBoolEventNode(n);
                    break;
                case IntEventNode n:
                    DrawIntEventNode(n);
                    break;
                case FloatEventNode n:
                    DrawFloatEventNode(n);
                    break;
                case StringEventNode n:
                    DrawStringEventNode(n);
                    break;
            }
        }

        private void DrawFloatEventNode(FloatEventNode node)
        {
            GUILayout.BeginArea(node.NodeRect, nodeStyle);
            DrawHeader("Float Event Node");
            DrawEventNodeButtons(node);
            EditorGUI.BeginChangeCheck();
            EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
            FloatChannelSO channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(FloatChannelSO), false) as FloatChannelSO;
            float newValue = EditorGUILayout.FloatField("Value: ", node.Value);
            EditorGUIUtility.labelWidth = oldLabelWidth;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(dialogueEditor.selectedDialogue, "Changed Event Node");
                node.Channel = channel;
                node.Value = newValue;
                dialogueEditor.Repaint();
            }
            GUILayout.EndArea();
            DrawInConnector(node);
            DrawOutConnector(node);
        }

        private void DrawStringEventNode(StringEventNode node)
        {
            GUILayout.BeginArea(node.NodeRect, nodeStyle);
            DrawHeader("String Event Node");
            DrawEventNodeButtons(node);
            EditorGUI.BeginChangeCheck();
            EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
            StringChannelSO newChannel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(StringChannelSO), false) as StringChannelSO;
            string newValue = EditorGUILayout.TextField("Value: ", node.Value);
            EditorGUIUtility.labelWidth = oldLabelWidth;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(dialogueEditor.selectedDialogue, "Changed Dialogue");
                node.Channel = newChannel;
                node.Value = newValue;
                dialogueEditor.Repaint();
            }
            GUILayout.EndArea();
            DrawInConnector(node);
            DrawOutConnector(node);
        }

        private void DrawIntEventNode(IntEventNode node)
        {
            GUILayout.BeginArea(node.NodeRect, nodeStyle);
            DrawHeader("Int Event Node");
            DrawEventNodeButtons(node);
            EditorGUI.BeginChangeCheck();
            EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
            IntChannelSO channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(IntChannelSO), false) as IntChannelSO;
            int newValue = EditorGUILayout.IntField("Value: ", node.Value);
            EditorGUIUtility.labelWidth = oldLabelWidth;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(dialogueEditor.selectedDialogue, "Changed Event Node");
                node.Channel = channel;
                node.Value = newValue;
                dialogueEditor.Repaint();
            }
            GUILayout.EndArea();
            DrawInConnector(node);
            DrawOutConnector(node);
        }

        private void DrawBoolEventNode(BoolEventNode node)
        {
            GUILayout.BeginArea(node.NodeRect, nodeStyle);
            DrawHeader("Bool Event Node");
            DrawEventNodeButtons(node);
            EditorGUI.BeginChangeCheck();
            EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
            BoolChannelSO channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(BoolChannelSO), false) as BoolChannelSO;
            bool newValue = EditorGUILayout.Toggle("Value: ", node.Value);
            EditorGUIUtility.labelWidth = oldLabelWidth;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(dialogueEditor.selectedDialogue, "Changed Event Node");
                node.Channel = channel;
                node.Value = newValue;
                dialogueEditor.Repaint();
            }
            GUILayout.EndArea();
            DrawInConnector(node);
            DrawOutConnector(node);
        }

        private void DrawVoidEventNode(VoidEventNode node)
        {
            GUILayout.BeginArea(node.NodeRect, nodeStyle);
            DrawHeader("Void Event Node");
            DrawEventNodeButtons(node);
            EditorGUI.BeginChangeCheck();
            EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
            VoidChannelSO channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(VoidChannelSO), false) as VoidChannelSO;
            if (EditorGUI.EndChangeCheck())
                EditorGUIUtility.labelWidth = oldLabelWidth;
            {
                Undo.RecordObject(dialogueEditor.selectedDialogue, "Changed Event Node");
                node.Channel = channel;
                dialogueEditor.Repaint();
            }
            GUILayout.EndArea();
            DrawInConnector(node);
            DrawOutConnector(node);
        }
        private void DrawEventNodeButtons(EventNode node)
        {
            EditorGUILayout.BeginHorizontal();
            int buttonWidth = Mathf.FloorToInt(node.NodeRect.width / 3) - 13;

            if (GUILayout.Button("Reset",EditorStyles.miniButtonLeft, GUILayout.Width(buttonWidth)))
            {
                
            }
            if (GUILayout.Button("Delete", EditorStyles.miniButtonMid,GUILayout.Width(buttonWidth)))
            {
                dialogueEditor.MarkNodeToRemove(node);
            }
            if (GUILayout.Button("Add Child", EditorStyles.miniButtonRight,GUILayout.Width(buttonWidth)))
            {
                dialogueEditor.MarkAsCreationNode(node);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void GenerateStyles()
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);

            entryStyle = new GUIStyle(nodeStyle);
            entryStyle.border = new RectOffset(4, 4, 4, 4);
            entryStyle.alignment = TextAnchor.MiddleCenter;
            entryStyle.fontSize *= 2;

            headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.alignment = TextAnchor.UpperLeft;
        }
        private void DrawEntryNode(EntryNode node)
        {
            GUILayout.BeginArea(node.NodeRect, nodeStyle);
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = Mathf.CeilToInt(headerStyle.fontSize * 1.5f);
            EditorGUILayout.LabelField("Entry", headerStyle, GUILayout.Height(headerStyle.lineHeight * 1.8f));
            GUILayout.EndArea();
            DrawOutConnector(node);
        }

        private void DrawStandardNode(DialogueNode node)
        {
            // Setup
            GUILayout.BeginArea(node.NodeRect, nodeStyle);
            EditorGUI.BeginChangeCheck();
            EditorGUIUtility.labelWidth = oldLabelWidth * .5f;
            //

            DrawHeader("Standard Node");
            int buttonWidth = Mathf.FloorToInt(node.NodeRect.width / 3) - 13;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset", EditorStyles.miniButtonLeft, GUILayout.Width(buttonWidth)))
            {
                node.ResetValues();
                dialogueEditor.Repaint();
            }
            if (GUILayout.Button("Delete", EditorStyles.miniButtonMid, GUILayout.Width(buttonWidth)))
            {
                dialogueEditor.MarkNodeToRemove(node);
            }
            if (GUILayout.Button("Create Child", EditorStyles.miniButtonRight, GUILayout.Width(buttonWidth)))
            {
                dialogueEditor.MarkAsCreationNode(node);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(3f);

            EditorGUILayout.BeginHorizontal();
            Sprite newIcon = DrawIconField(node);
            string newSpeaker = DrawSpeakerField(node);
            EditorGUILayout.EndHorizontal();

            string newText = DrawDialogueBox(node);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(dialogueEditor.selectedDialogue, "Changed Dialogue");
                node.Text = newText;
                node.Speaker = newSpeaker;
                node.Portrait = newIcon;
            }
            GUILayout.EndArea();
            EditorGUIUtility.labelWidth = oldLabelWidth;

            // Connectors
            DrawInConnector(node);
            DrawOutConnector(node);
        }
        private void DrawHeader(string title)
        {
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel, GUILayout.Height(10f));
        }
        private Sprite DrawIconField(DialogueNode node)
        {
            EditorGUIUtility.labelWidth = .001f;
            var newSprite = (Sprite)EditorGUILayout.ObjectField(GUIContent.none, node.Portrait, typeof(Sprite), allowSceneObjects: false);
            EditorGUIUtility.labelWidth = oldLabelWidth * .5f;
            return newSprite;
        }
        private string DrawSpeakerField(DialogueNode node)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Speaker: ");
            string newSpeaker = EditorGUILayout.TextField(GUIContent.none, node.Speaker);
            EditorGUILayout.EndVertical();
            return newSpeaker;
        }
        private string DrawDialogueBox(DialogueNode node)
        {
            EditorGUILayout.LabelField("Dialogue: ");

            node.boxScroll = GUILayout.BeginScrollView(node.boxScroll, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            EditorStyles.textField.wordWrap = true;
            string newText = EditorGUILayout.TextArea(node.Text, GUILayout.Height(node.NodeRect.height - 30f));

            EditorGUILayout.EndScrollView();

            return newText;
        }
        private void DrawInConnector(GraphNode node)
        {
            if (node is EntryNode)
                return;

            Vector2 pos = GetInConnectorPos(node);
            Rect buttonRect = new Rect(pos, radioButtonSize);
            GUIStyle style = new GUIStyle(EditorStyles.radioButton);
            if (dialogueEditor.selectedDialogue.HasValidParent(node))
            {
                style.normal = style.onActive;
            }

            if (GUI.Button(buttonRect, GUIContent.none, style))
            {
                if (dialogueEditor.findingParentNode != null)
                {
                    dialogueEditor.ClearConnectingNodes();
                    dialogueEditor.findingParentNode = node as DialogueNode;
                    dialogueEditor.Repaint();
                }
                else if (dialogueEditor.findingChildNode != null)
                {
                    dialogueEditor.findingChildNode.ChildID = node.UniqueID;
                    dialogueEditor.ClearConnectingNodes();
                    dialogueEditor.Repaint();
                }
                else
                {
                    dialogueEditor.findingParentNode = node as DialogueNode;
                }
            }

        }
        private void DrawOutConnector(GraphNode node)
        {
            Vector2 pos = GetOutConnectorPos(node);
            Rect buttonRect = new Rect(pos, radioButtonSize);
            GUIStyle style = new GUIStyle(EditorStyles.radioButton);
            if (dialogueEditor.selectedDialogue.GetChildNode(node) != null)
            {
                style.normal = style.onActive;
            }

            if (GUI.Button(buttonRect, GUIContent.none, style))
            {
                if (dialogueEditor.findingChildNode != null)
                {
                    dialogueEditor.ClearConnectingNodes();
                    dialogueEditor.findingChildNode = node;
                    dialogueEditor.Repaint();
                }
                else if (dialogueEditor.findingParentNode != null)
                {
                    node.ChildID = dialogueEditor.findingParentNode.UniqueID;
                    dialogueEditor.ClearConnectingNodes();
                    dialogueEditor.Repaint();
                }
                else
                {
                    dialogueEditor.findingChildNode = node;
                }
            }
        }

        public Vector2 GetInConnectorPos(GraphNode node)
        {
            var nodePos = node.NodeRect.position;
            var yOffset = node.NodeRect.height * 0.5f - 5f;
            Vector2 target = nodePos + new Vector2(0, yOffset);
            return target;
        }
        public Vector2 GetOutConnectorPos(GraphNode node)
        {
            Vector2 nodePos = node.NodeRect.position;
            float xOffset = node.NodeRect.width - 15f;
            float yOffset = node.NodeRect.height * .5f - 5f;
            Vector2 target = nodePos + new Vector2(xOffset, yOffset);
            return target;
        }
        public void DrawConnection(GraphNode parentNode, GraphNode childNode)
        {
            float offsetValue = radioButtonSize.x * .5f;
            Vector3 offsetVector = new Vector3(offsetValue, offsetValue, 0);
            Vector3 startPoint = (Vector3)GetOutConnectorPos(parentNode) + offsetVector;
            Vector3 endPoint = (Vector3)GetInConnectorPos(childNode) + offsetVector;
            Vector3 startTangent = startPoint + (Vector3.right * 50f);
            Vector3 endTangent = endPoint + (Vector3.left * 50f);
            Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.white, null, 3f);
        }
    }
}
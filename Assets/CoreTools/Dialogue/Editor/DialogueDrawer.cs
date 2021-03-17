using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;
using CoreTools.DialogueSystem;
using CoreTools.NodeSystem;

namespace CoreTools.DialogueSystem.Editor
{
    public class DialogueDrawer : NodeDrawer
    {
        protected readonly DialogueEditorWindow dialogueEditor;

        public GUIStyle radioActiveStyle;
        public GUIStyle nodeStyle;
        private GUIStyle selectedStyle;
        public GUIStyle entryStyle;
        public GUIStyle headerStyle;
        private float oldLabelWidth;
        public DialogueDrawer(DialogueEditorWindow dialogueEditor, NodeEditorWindow nodeEditor) : base(nodeEditor)
        {
            this.nodeEditor = nodeEditor;
            this.dialogueEditor = dialogueEditor;

            oldLabelWidth = EditorGUIUtility.labelWidth;
            GenerateStyles();
        }

        public override void DrawGraphNode(GraphNode node)
        {
            switch (node)
            {
                case DialogueEntryNode n:
                    DrawEntryNode(n);
                    break;
                case ChoiceNode n:
                    DrawChoiceNode(n);
                    break;
                case TextNode n:
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

        #region DrawNode Methods
        private void DrawFloatEventNode(FloatEventNode node)
        {
            GUIStyle useStyle = dialogueEditor.focusedNode == node ? selectedStyle : nodeStyle;
            GUILayout.BeginArea(node.NodeRect, useStyle);

            DrawHeader(node);
            EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
            node.Channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(FloatChannelSO), false) as FloatChannelSO;
            node.Value = EditorGUILayout.FloatField("Value: ", node.Value);
            EditorGUIUtility.labelWidth = oldLabelWidth;

            GUILayout.EndArea();

            DrawInConnector(node);
            DrawOutConnector(node);
        }

        private void DrawStringEventNode(StringEventNode node)
        {
            GUIStyle useStyle = dialogueEditor.focusedNode == node ? selectedStyle : nodeStyle;
            GUILayout.BeginArea(node.NodeRect, useStyle);

            DrawHeader(node);
            EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
            node.Channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(StringChannelSO), false) as StringChannelSO;
            node.Value = EditorGUILayout.TextField("Value: ", node.Value);
            EditorGUIUtility.labelWidth = oldLabelWidth;

            GUILayout.EndArea();

            DrawInConnector(node);
            DrawOutConnector(node);
        }

        private void DrawIntEventNode(IntEventNode node)
        {
            GUIStyle useStyle = dialogueEditor.focusedNode == node ? selectedStyle : nodeStyle;
            GUILayout.BeginArea(node.NodeRect, useStyle);

            DrawHeader(node);

            EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
            node.Channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(IntChannelSO), false) as IntChannelSO;
            node.Value = EditorGUILayout.IntField("Value: ", node.Value);
            EditorGUIUtility.labelWidth = oldLabelWidth;

            GUILayout.EndArea();

            DrawInConnector(node);
            DrawOutConnector(node);
        }

        private void DrawBoolEventNode(BoolEventNode node)
        {
            GUIStyle useStyle = dialogueEditor.focusedNode == node ? selectedStyle : nodeStyle;
            GUILayout.BeginArea(node.NodeRect, useStyle);

            DrawHeader(node);

            EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
            node.Channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(BoolChannelSO), false) as BoolChannelSO;
            node.Value = EditorGUILayout.Toggle("Value: ", node.Value);
            EditorGUIUtility.labelWidth = oldLabelWidth;

            GUILayout.EndArea();

            DrawInConnector(node);
            DrawOutConnector(node);
        }

        private void DrawVoidEventNode(VoidEventNode node)
        {
            GUIStyle useStyle = dialogueEditor.focusedNode == node ? selectedStyle : nodeStyle;
            GUILayout.BeginArea(node.NodeRect, useStyle);

            DrawHeader(node);

            EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
            node.Channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(VoidChannelSO), false) as VoidChannelSO;

            GUILayout.EndArea();

            DrawInConnector(node);
            DrawOutConnector(node);
        }
        private void DrawEntryNode(DialogueEntryNode node)
        {
            GUILayout.BeginArea(node.NodeRect, entryStyle);
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = Mathf.CeilToInt(headerStyle.fontSize * 1.5f);
            EditorGUILayout.LabelField("Entry", headerStyle, GUILayout.Height(headerStyle.lineHeight * 1.8f));
            GUILayout.EndArea();
            DrawOutConnector(node);
        }
        private void DrawChoiceNode(ChoiceNode node)
        {
            GUIStyle useStyle = dialogueEditor.focusedNode == node ? selectedStyle : nodeStyle;
            GUILayout.BeginArea(node.NodeRect, useStyle);
            DrawHeader(node);
            GUILayout.Space(1f);

            GUILayout.BeginHorizontal();
            node.Portrait = DrawIconField(node);

            GUILayout.BeginVertical();
            node.Speaker = DrawSpeakerField(node);
            node.Orientation = (DialogueOrientation)EditorGUILayout.EnumPopup(node.Orientation);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.Space(1f);

            node.Text = DrawDialogueBox(node);

            int toRemoveChoice = -1;
            for (int i = 0; i < node.ChoiceAmount; i++)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox,
                                          GUILayout.Height(EditorGUIUtility.singleLineHeight),
                                          GUILayout.Width(node.NodeRect.width - 32f));

                if (GUILayout.Button("X", GUILayout.Width(EditorGUIUtility.singleLineHeight)))
                {
                    toRemoveChoice = i;
                }

                GUILayout.Label(i.ToString(), GUILayout.Width(18f));
                string newText = EditorGUILayout.TextField(GUIContent.none, node.GetTextOfChoice(i));
                node.SetTextOfChoice(i, newText);

                GUILayout.EndHorizontal();
                GUILayout.Space(2f);
            }

            if (toRemoveChoice >= 0)
            {
                node.RemoveChoice(toRemoveChoice);
                dialogueEditor.Repaint();
            }

            // Draw Add Choice Button
            if (GUILayout.Button("Add Choice"))
            {
                node.AddChoice();
                dialogueEditor.Repaint();
            }

            GUILayout.EndArea();

            DrawInConnector(node);
            DrawChoiceOutConnectors(node);
        }
        private void DrawStandardNode(TextNode node)
        {
            GUIStyle useStyle = dialogueEditor.focusedNode == node ? selectedStyle : nodeStyle;
            GUILayout.BeginArea(node.NodeRect, useStyle);
            EditorGUIUtility.labelWidth = oldLabelWidth * .5f;

            DrawHeader(node);
            GUILayout.Space(1f);

            GUILayout.BeginHorizontal();
            node.Portrait = DrawIconField(node);

            GUILayout.BeginVertical();
            node.Speaker = DrawSpeakerField(node);
            node.Orientation = (DialogueOrientation)EditorGUILayout.EnumPopup(node.Orientation);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(1f);

            node.Text = DrawDialogueBox(node);

            GUILayout.EndArea();

            EditorGUIUtility.labelWidth = oldLabelWidth;

            DrawInConnector(node);
            DrawOutConnector(node);
        }
        #endregion

        private void GenerateStyles()
        {
            nodeStyle = new GUIStyle()
            {
                padding = new RectOffset(20, 20, 15, 15),
                border = new RectOffset(33, 33, 33, 33),
            };
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.normal.textColor = Color.white;

            entryStyle = new GUIStyle()
            {
                border = new RectOffset(22, 22, 22, 22),
                padding = new RectOffset(20, 20, 15, 15),
                alignment = TextAnchor.MiddleCenter,
            };
            entryStyle.normal.textColor = Color.white;
            entryStyle.normal.background = EditorGUIUtility.Load("node3") as Texture2D;
            entryStyle.fontSize *= 2;

            selectedStyle = new GUIStyle(nodeStyle);
            selectedStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;

            // setting ehader style here based on EditorStyles.boldLabel will cause NullRef
            // create new headerstyle inside DrawHeader instead
        }

        private void DrawHeader(GraphNode node)
        {
            headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.UpperLeft
            };
            string headerName = node.GetType().GetLastTypeAsString();

            GUILayout.BeginHorizontal();

            GUILayout.Label(headerName, headerStyle);
            if (GUILayout.Button("Delete", GUILayout.Width(80f)))
            {
                dialogueEditor.MarkNodeToRemove(node);
            }

            GUILayout.EndHorizontal();
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
            //EditorGUILayout.LabelField("Dialogue: ");
            GUIStyle newStyle = new GUIStyle
            {
                fixedHeight = EditorGUIUtility.singleLineHeight * 3 + 5f
            };

            GUILayout.BeginVertical(newStyle);
            node.boxScroll = GUILayout.BeginScrollView(node.boxScroll, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);

            EditorStyles.textField.wordWrap = true;
            string newText = GUILayout.TextArea(node.Text, GUILayout.ExpandHeight(true));

            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            return newText;
        }
        private void DrawInConnector(GraphNode node)
        {
            if (node is DialogueEntryNode)
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
                if (dialogueEditor.findingChildNode != null &&
                    dialogueEditor.findingChildNode != node)
                {
                    if (dialogueEditor.findingChildNode is ChoiceNode choiceNode)
                    {
                        int id = dialogueEditor.findingChildChoiceId;
                        ((ISingleChild)choiceNode.GetAllChoices()[id]).ChildID = node.UniqueID;
                    }
                    else
                    {
                        ((ISingleChild)dialogueEditor.findingChildNode).ChildID = node.UniqueID;
                    }
                    dialogueEditor.ClearConnectingNodes();
                }
                else
                {
                    dialogueEditor.findingParentNode = node;
                    dialogueEditor.focusedNode = node;
                }
                dialogueEditor.Repaint();
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
                if (dialogueEditor.findingParentNode != null &&
                    dialogueEditor.findingParentNode != node)
                {
                    ((ISingleChild)node).ChildID = dialogueEditor.findingParentNode.UniqueID;
                    dialogueEditor.ClearConnectingNodes();
                }
                else
                {
                    dialogueEditor.findingChildNode = node;
                    dialogueEditor.focusedNode = node;
                }
                dialogueEditor.Repaint();
            }
        }
        private void DrawChoiceOutConnectors(ChoiceNode node)
        {

            List<ChoiceField> choices = node.GetAllChoices();
            for (int i = 0; i < choices.Count; i++)
            {
                Vector2 pos = GetChoiceConnectorPosition(node, i);
                Rect buttonRect = new Rect(pos, radioButtonSize);

                GraphNode child = dialogueEditor.selectedDialogue.GetAnyGraphNode(((ISingleChild)choices[i]).ChildID);
                GUIStyle style = new GUIStyle(EditorStyles.radioButton);
                if (child != null)
                    style.normal = style.onActive;

                if (GUI.Button(buttonRect, GUIContent.none, style))
                {
                    if (dialogueEditor.findingParentNode != null &&
                        dialogueEditor.findingParentNode != node)
                    {
                        ((ISingleChild)node.GetAllChoices()[i]).ChildID = dialogueEditor.findingParentNode.UniqueID;
                    }
                    else
                    {
                        dialogueEditor.findingChildChoiceId = i;
                        dialogueEditor.focusedNode = node;
                        dialogueEditor.findingChildNode = node;
                    }
                    dialogueEditor.ClearConnectingNodes();
                    dialogueEditor.Repaint();
                }
            }
        }
        public Vector2 GetChoiceConnectorPosition(ChoiceNode node, int id)
        {
            Vector2 nodePos = node.NodeRect.position;
            float xOffset = node.GetBaseRect().width - 15f;
            float yOffset = node.GetBaseRect().height - 16f;
            Vector2 baselineOutPos = nodePos + new Vector2(xOffset, yOffset);

            float offSet = id * (node.GetChoiceHeight()) - (0.5f * node.GetChoiceHeight());
            Vector2 pos = baselineOutPos + new Vector2(0, offSet);
            return pos;
        }

        public Vector2 GetInConnectorPos(GraphNode node)
        {
            var nodePos = node.NodeRect.position;
            var xOffset = 0f;
            var yOffset = node.NodeRect.height * 0.5f - 5f;
            if (node is ChoiceNode choiceNode)
            {
                nodePos = choiceNode.GetBaseRect().position;
                yOffset = choiceNode.GetBaseRect().height * 0.5f - 5f;
            }
            Vector2 target = nodePos + new Vector2(xOffset, yOffset);
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

            float yDistance = Mathf.Abs(parentNode.NodeRect.position.y - childNode.NodeRect.position.y);
            Vector3 startTangent = startPoint + (Vector3.right * tangentOffset);
            Vector3 endTangent = endPoint + (Vector3.left * tangentOffset);
            Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.white, null, 3f);
        }
        public void DrawChoiceNodeConnections(ChoiceNode node)
        {
            var choices = node.GetAllChoices();
            for (int i = 0; i < choices.Count; i++)
            {
                ISingleChild choiceAsParent = (ISingleChild)choices[i];
                if (!string.IsNullOrEmpty(choiceAsParent.ChildID))
                {
                    GraphNode child = dialogueEditor.selectedDialogue.GetAnyGraphNode(choiceAsParent.ChildID);
                    if (child == null)
                    {
                        choiceAsParent.ChildID = null;
                        EditorUtility.SetDirty(dialogueEditor.selectedDialogue);
                        continue;
                    }
                    else
                    {
                        float offsetValue = radioButtonSize.x * .5f;
                        Vector3 offsetVector = new Vector3(offsetValue, offsetValue, 0); // to draw into the middle of the button
                        Vector3 endPos = (Vector3)GetInConnectorPos(child) + offsetVector;
                        Vector3 startPos = (Vector3)GetChoiceConnectorPosition(node, i) + offsetVector;

                        float yDistance = Mathf.Abs(node.NodeRect.position.y - child.NodeRect.position.y);
                        Vector3 startTangent = startPos + (Vector3.right * tangentOffset);
                        Vector3 endTangent = endPos + (Vector3.left * tangentOffset);
                        Handles.DrawBezier(startPos, endPos, startTangent, endTangent, Color.white, null, 3f);
                    }
                }
            }
        }
    }
}
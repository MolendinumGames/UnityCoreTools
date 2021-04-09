using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;
using CoreTools.DialogueSystem;
using CoreTools.NodeSystem;

namespace CoreTools.DialogueSystem.Editor
{
    public class DialogueDrawer : GraphDrawer
    {
        protected readonly DialogueEditorWindow dialogueEditor;

        public GUIStyle radioActiveStyle;
        public DialogueDrawer(DialogueEditorWindow dialogueEditor, NodeEditorWindow nodeEditor) 
            : base(nodeEditor)
        {
            this.nodeEditor = nodeEditor;
            this.dialogueEditor = dialogueEditor;
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
                case DialogueEventNode n:
                    DrawDialogueEventNode(n);
                    break;
            }
            DrawConnections(node);
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
        //private void DrawInConnector(GraphNode node)
        //{
        //    if (node is DialogueEntryNode)
        //        return;

        //    Vector2 pos = GetSingleInConnectorPos(node);
        //    Rect buttonRect = new Rect(pos, radioButtonSize);
        //    GUIStyle style = new GUIStyle(EditorStyles.radioButton);
        //    if (dialogueEditor.selectedDialogue.HasValidParent(node))
        //    {
        //        style.normal = style.onActive;
        //    }

        //    if (GUI.Button(buttonRect, GUIContent.none, style))
        //    {
        //        if (dialogueEditor.findingChildNode != null &&
        //            dialogueEditor.findingChildNode != node)
        //        {
        //            if (dialogueEditor.findingChildNode is ChoiceNode choiceNode)
        //            {
        //                int id = dialogueEditor.findingChildChoiceId;
        //                ((ISingleChild)choiceNode.GetAllChoices()[id]).ChildID = node.UniqueID;
        //            }
        //            else
        //            {
        //                ((ISingleChild)dialogueEditor.findingChildNode).ChildID = node.UniqueID;
        //            }
        //            dialogueEditor.ClearConnectingNodes();
        //        }
        //        else
        //        {
        //            dialogueEditor.findingParentNode = node;
        //            dialogueEditor.focusedNode = node;
        //        }
        //        dialogueEditor.Repaint();
        //    }

        //}
        //private void DrawOutConnector(GraphNode node)
        //{
        //    Vector2 pos = GetOutConnectorPos(node);
        //    Rect buttonRect = new Rect(pos, radioButtonSize);
        //    GUIStyle style = new GUIStyle(EditorStyles.radioButton);
        //    if ((node as ISingleChild).HasChild())
        //    {
        //        style.normal = style.onActive;
        //    }

        //    if (GUI.Button(buttonRect, GUIContent.none, style))
        //    {
        //        if (dialogueEditor.findingParentNode != null &&
        //            dialogueEditor.findingParentNode != node)
        //        {
        //            ((ISingleChild)node).ChildID = dialogueEditor.findingParentNode.UniqueID;
        //            dialogueEditor.ClearConnectingNodes();
        //        }
        //        else
        //        {
        //            dialogueEditor.findingChildNode = node;
        //            dialogueEditor.focusedNode = node;
        //        }
        //        dialogueEditor.Repaint();
        //    }
        //}
        //protected void DrawChoiceOutConnectors(GraphNode node)
        //{
        //    IChoiceContainer choiceParent = (IChoiceContainer)node;
        //    for (int i = 0; i < choiceParent.ChoiceAmount; i++)
        //    {
        //        Vector2 pos = GetChoiceOutConnectorPos(node, i);
        //        Rect buttonRect = new Rect(pos, radioButtonSize);

        //        bool hasChild = !string.IsNullOrEmpty(choiceParent.GetChildOfChoice(i));
        //        GUIStyle style = new GUIStyle(EditorStyles.radioButton);
        //        if (hasChild)
        //            style.normal = style.onActive;

        //        if (GUI.Button(buttonRect, GUIContent.none, style))
        //        {
        //            if (nodeEditor.findingParentNode != null &&
        //                nodeEditor.findingParentNode != node)
        //            {
        //                nodeEditor.SetParentOfFindingParentNode(node, i);
        //            }
        //            else
        //            {
        //                nodeEditor.SetFindingChildNode(node, i);
        //            }
        //        }
        //    }
        //}
        //public Vector2 GetChoiceConnectorPosition(ChoiceNode node, int id)
        //{
        //    Vector2 nodePos = node.NodeRect.position;
        //    float xOffset = node.GetBaseRect().width - 15f;
        //    float yOffset = node.GetBaseRect().height - 16f;
        //    Vector2 baselineOutPos = nodePos + new Vector2(xOffset, yOffset);

        //    float offSet = id * (node.GetChoiceHeight()) - (0.5f * node.GetChoiceHeight());
        //    Vector2 pos = baselineOutPos + new Vector2(0, offSet);
        //    return pos;
        //}

        //public Vector2 GetInConnectorPos(GraphNode node)
        //{
        //    var nodePos = node.NodeRect.position;
        //    var xOffset = 0f;
        //    var yOffset = node.NodeRect.height * 0.5f - 5f;
        //    if (node is ChoiceNode choiceNode)
        //    {
        //        nodePos = choiceNode.GetBaseRect().position;
        //        yOffset = choiceNode.GetBaseRect().height * 0.5f - 5f;
        //    }
        //    Vector2 target = nodePos + new Vector2(xOffset, yOffset);
        //    return target;
        //}
        //public Vector2 GetOutConnectorPos(GraphNode node)
        //{
        //    Vector2 nodePos = node.NodeRect.position;
        //    float xOffset = node.NodeRect.width - 15f;
        //    float yOffset = node.NodeRect.height * .5f - 5f;
        //    Vector2 target = nodePos + new Vector2(xOffset, yOffset);
        //    return target;
        //}
        //public void DrawConnection(GraphNode parentNode, GraphNode childNode)
        //{
        //    float offsetValue = radioButtonSize.x * .5f;
        //    Vector3 offsetVector = new Vector3(offsetValue, offsetValue, 0);
        //    Vector3 startPoint = (Vector3)GetSingleOutConnectorPos(parentNode) + offsetVector;
        //    Vector3 endPoint = (Vector3)GetSingleInConnectorPos(childNode) + offsetVector;

        //    float yDistance = Mathf.Abs(parentNode.NodeRect.position.y - childNode.NodeRect.position.y);
        //    Vector3 startTangent = startPoint + (Vector3.right * tangentOffset);
        //    Vector3 endTangent = endPoint + (Vector3.left * tangentOffset);
        //    Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.white, null, 3f);
        //}
        //public void DrawChoiceNodeConnections(ChoiceNode node)
        //{
        //    var choices = node.GetAllChoices();
        //    for (int i = 0; i < choices.Count; i++)
        //    {
        //        ISingleChild choiceAsParent = (ISingleChild)choices[i];
        //        if (!string.IsNullOrEmpty(choiceAsParent.ChildID))
        //        {
        //            GraphNode child = dialogueEditor.selectedDialogue.GetAnyGraphNode(choiceAsParent.ChildID);
        //            if (child == null)
        //            {
        //                choiceAsParent.ChildID = null;
        //                EditorUtility.SetDirty(dialogueEditor.selectedDialogue);
        //                continue;
        //            }
        //            else
        //            {
        //                float offsetValue = radioButtonSize.x * .5f;
        //                Vector3 offsetVector = new Vector3(offsetValue, offsetValue, 0); // to draw into the middle of the button
        //                Vector3 endPos = (Vector3)GetSingleInConnectorPos(child) + offsetVector;
        //                Vector3 startPos = (Vector3)GetChoiceOutConnectorPos(node, i) + offsetVector;

        //                float yDistance = Mathf.Abs(node.NodeRect.position.y - child.NodeRect.position.y);
        //                Vector3 startTangent = startPos + (Vector3.right * tangentOffset);
        //                Vector3 endTangent = endPos + (Vector3.left * tangentOffset);
        //                Handles.DrawBezier(startPos, endPos, startTangent, endTangent, Color.white, null, 3f);
        //            }
        //        }
        //    }
        //}
    }
}
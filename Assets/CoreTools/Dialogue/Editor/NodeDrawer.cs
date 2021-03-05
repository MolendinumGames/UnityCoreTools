using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;
using CoreTools.Dialogue;

namespace CoreTools.Dialogue
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
                case ChoiceNode n:
                    DrawChoiceNode(n);
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

        #region DrawNode Methods
        private void DrawFloatEventNode(FloatEventNode node)
        {
            GUILayout.BeginArea(node.NodeRect, nodeStyle);
            DrawHeader(node);
            //DrawButtonBar(node);
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
            DrawHeader(node);
            //DrawButtonBar(node);
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
            DrawHeader(node);
            //DrawButtonBar(node);
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
            DrawHeader(node);
            //DrawButtonBar(node);
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
            DrawHeader(node);
            //DrawButtonBar(node);
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
        private void DrawEntryNode(EntryNode node)
        {
            GUILayout.BeginArea(node.NodeRect, nodeStyle);
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = Mathf.CeilToInt(headerStyle.fontSize * 1.5f);
            EditorGUILayout.LabelField("Entry", headerStyle, GUILayout.Height(headerStyle.lineHeight * 1.8f));
            GUILayout.EndArea();
            DrawOutConnector(node);
        }
        private void DrawChoiceNode(ChoiceNode node)
        {
            GUILayout.BeginArea(node.NodeRect, nodeStyle);

            DrawHeader(node);
            GUILayout.Space(1f);

            EditorGUI.BeginChangeCheck();

            GUILayout.BeginHorizontal();
            Sprite newIcon = DrawIconField(node);
            string newSpeaker = DrawSpeakerField(node);
            GUILayout.EndHorizontal();

            GUILayout.Space(1f);

            string newText = DrawDialogueBox(node);

            ChoiceField[] choices = node.GetAllChoices().ToArray();
            string[] newChoiceTexts = new string[node.ChoiceAmount];
            int toRemoveChoice = -1;
            for (int i = 0; i < choices.Length; i++)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.Width(node.NodeRect.width - 32f));
                if (GUILayout.Button("X", GUILayout.Width(EditorGUIUtility.singleLineHeight)))
                {
                    toRemoveChoice = i;
                }
                GUILayout.Label(i.ToString(), GUILayout.Width(16f));
                newChoiceTexts[i] = EditorGUILayout.TextField(GUIContent.none, choices[i].text);
                GUILayout.EndHorizontal();
                GUILayout.Space(2f);
            }


            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(dialogueEditor.selectedDialogue, "Changed Choice Node");
                node.Portrait = newIcon;
                node.Speaker = newSpeaker;
                node.Text = newText;
                for (int i = 0; i < choices.Length; i++)
                {
                    choices[i].text = newChoiceTexts[i];
                }
                if (toRemoveChoice >= 0)
                {
                    node.RemoveChoice(toRemoveChoice);
                    EditorUtility.SetDirty(dialogueEditor.selectedDialogue);
                    dialogueEditor.Repaint();
                }

                EditorUtility.SetDirty(dialogueEditor.selectedDialogue);
            }

            // Draw Add Choice Button
            if (GUILayout.Button("Add Choice"))
            {
                Undo.RecordObject(node, "Added Choice to node");
                node.AddChoice();
                EditorUtility.SetDirty(node);
                dialogueEditor.Repaint();
            }

            GUILayout.EndArea();

            DrawInConnector(node);
            DrawChoiceOutConnectors(node);
        }
        private void DrawStandardNode(DialogueNode node)
        {
            // Setup
            GUILayout.BeginArea(node.NodeRect, nodeStyle);
            EditorGUI.BeginChangeCheck();
            EditorGUIUtility.labelWidth = oldLabelWidth * .5f;
            //

            DrawHeader(node);

            GUILayout.Space(1f);

            GUILayout.BeginHorizontal();

            Sprite newIcon = DrawIconField(node);
            string newSpeaker = DrawSpeakerField(node);

            GUILayout.EndHorizontal();

            GUILayout.Space(1f);

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
        #endregion




        private void DrawButtonBar(GraphNode node)
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
            nodeStyle.padding = new RectOffset(20, 20, 15,15);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);

            entryStyle = new GUIStyle(nodeStyle);
            entryStyle.border = new RectOffset(4, 4, 4, 4);
            entryStyle.alignment = TextAnchor.MiddleCenter;
            entryStyle.fontSize *= 2;

            // setting ehader style here based on EditorStyles.boldLabel will cause NullRef
            // create new headerstyle inside DraHeader instead
        }

        private void DrawHeader(GraphNode node)
        {
            headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.alignment = TextAnchor.UpperLeft;
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
            GUIStyle newStyle = new GUIStyle();
            newStyle.fixedHeight = EditorGUIUtility.singleLineHeight * 3 + 5f;

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
                if (dialogueEditor.findingChildNode != null)
                {
                    if (dialogueEditor.findingChildNode is ChoiceNode choiceNode)
                    {
                        int id = dialogueEditor.findingChildChoiceId;
                        choiceNode.GetAllChoices()[id].childId = node.UniqueID;
                    }
                    else
                    {
                        dialogueEditor.findingChildNode.ChildID = node.UniqueID;
                    }
                    EditorUtility.SetDirty(dialogueEditor.selectedDialogue);
                    dialogueEditor.ClearConnectingNodes();
                    dialogueEditor.Repaint();
                }
                else
                {
                    dialogueEditor.ClearConnectingNodes();
                    dialogueEditor.findingParentNode = node as DialogueNode;
                    dialogueEditor.Repaint();
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
                if (dialogueEditor.findingParentNode != null)
                {
                    node.ChildID = dialogueEditor.findingParentNode.UniqueID;
                    EditorUtility.SetDirty(dialogueEditor.selectedDialogue);
                    dialogueEditor.ClearConnectingNodes();
                    dialogueEditor.Repaint();
                }
                else
                {
                    dialogueEditor.ClearConnectingNodes();
                    dialogueEditor.findingChildNode = node;
                    dialogueEditor.Repaint();
                }
            }
        }
        private void DrawChoiceOutConnectors(ChoiceNode node)
        {

            List<ChoiceField> choices = node.GetAllChoices();
            for (int i = 0; i < choices.Count; i++)
            {
                Vector2 pos = GetChoiceConnectorPosition(node, i);
                Rect buttonRect = new Rect(pos, radioButtonSize);

                GraphNode child = dialogueEditor.selectedDialogue.GetAnyGraphNode(choices[i].childId);
                GUIStyle style = new GUIStyle(EditorStyles.radioButton);
                if (child != null)
                    style.normal = style.onActive;

                if (GUI.Button(buttonRect, GUIContent.none, style))
                {
                    if (dialogueEditor.findingParentNode != null)
                    {
                        node.GetAllChoices()[i].childId = dialogueEditor.findingParentNode.UniqueID;
                        dialogueEditor.ClearConnectingNodes();
                    }
                    else
                    {
                        dialogueEditor.ClearConnectingNodes();
                        dialogueEditor.findingChildChoiceId = i;
                        dialogueEditor.findingChildNode = node;
                    }
                }
            }
        }
        public Vector2 GetChoiceConnectorPosition(ChoiceNode node, int id)
        {
            Vector2 nodePos = node.NodeRect.position;
            float xOffset = node.GetBasicRect().width - 15f;
            float yOffset = node.GetBasicRect().height - 16f;
            Vector2 baselineOutPos = nodePos + new Vector2(xOffset, yOffset);

            float offSet = id * (node.GetChoiceHeight()) - (0.5f * node.GetChoiceHeight());
            Vector2 pos = baselineOutPos + new Vector2(0, offSet);
            return pos;
        }

        public Vector2 GetInConnectorPos(GraphNode node)
        {
            var nodePos = node.NodeRect.position;
            var yOffset = node.NodeRect.height * 0.5f - 5f;
            if (node is ChoiceNode choiceNode)
            {
                nodePos = choiceNode.GetBasicRect().position;
                yOffset = choiceNode.GetBasicRect().height * 0.5f - 5f;
            }
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
        public void DrawChoiceNodeConnections(ChoiceNode node)
        {
            var choices = node.GetAllChoices();
            for (int i = 0; i < choices.Count; i++)
            {
                if (!string.IsNullOrEmpty(choices[i].childId))
                {
                    GraphNode child = dialogueEditor.selectedDialogue.GetAnyGraphNode(choices[i].childId);
                    if (child == null)
                    {
                        choices[i].childId = null;
                        EditorUtility.SetDirty(dialogueEditor.selectedDialogue);
                        continue;
                    }
                    else
                    {
                        float offsetValue = radioButtonSize.x * .5f;
                        Vector3 offsetVector = new Vector3(offsetValue, offsetValue, 0); // to draw into the middle of the button
                        Vector3 endPos = (Vector3)GetInConnectorPos(child) + offsetVector;
                        Vector3 startPos = (Vector3)GetChoiceConnectorPosition(node, i) + offsetVector;
                        Vector3 startTangent = startPos + (Vector3.right * 50f);
                        Vector3 endTangent = endPos + (Vector3.left * 50f);
                        Handles.DrawBezier(startPos, endPos, startTangent, endTangent, Color.white, null, 3f);
                    }
                }
            }
        }
    }
}
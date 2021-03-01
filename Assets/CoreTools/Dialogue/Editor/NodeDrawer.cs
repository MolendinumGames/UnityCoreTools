using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CoreTools.Dialogue.Editor
{
    public class NodeDrawer
    {
        protected readonly DialogueEditor dialogueEditor;

        public readonly Vector2 radioButtonSize = new Vector2(15f, 15f);
        public GUIStyle radioActiveStyle;
        public GUIStyle nodeStyle;
        private float oldLabelWidth;
        public NodeDrawer(DialogueEditor dialogueEditor)
        {
            this.dialogueEditor = dialogueEditor;
            oldLabelWidth = EditorGUIUtility.labelWidth;
            GenerateStyles();
        }
        public void DrawNode(DialogueNode node)
        {
            DrawStandardNode(node);
        }
        private void GenerateStyles()
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void DrawStandardNode(DialogueNode node)
        {
            // Setup
            GUILayout.BeginArea(node.rect, nodeStyle);
            EditorGUI.BeginChangeCheck();
            EditorGUIUtility.labelWidth = oldLabelWidth * .5f;
            //

            DrawHeader(node);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset", EditorStyles.miniButtonLeft))
            {
                node.ResetValues();
                dialogueEditor.Repaint();
            }
            if (GUILayout.Button("Delete", EditorStyles.miniButtonMid))
            {
                dialogueEditor.MarkNodeToRemove(node);
            }
            if (GUILayout.Button("Create Child", EditorStyles.miniButtonRight))
            {
                dialogueEditor.MarkAsCreationNode(node);
            }
            EditorGUILayout.EndHorizontal();
            Debug.Log("");


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
        private void DrawHeader(DialogueNode node)
        {
            EditorGUILayout.LabelField("Standard Node", EditorStyles.boldLabel);
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

            node.scroll = GUILayout.BeginScrollView(node.scroll, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            EditorStyles.textField.wordWrap = true;
            string newText = EditorGUILayout.TextArea(node.Text, GUILayout.Height(node.rect.height - 30f));

            EditorGUILayout.EndScrollView();

            return newText;
        }
        private void DrawInConnector(DialogueNode node)
        {
            Vector2 pos = GetInConnectorPos(node);
            Rect buttonRect = new Rect(pos, radioButtonSize);
            GUIStyle style = new GUIStyle(EditorStyles.radioButton);
            if (dialogueEditor.selectedDialogue.NodeHasParent(node))
            {
                style.normal = style.onActive;
            }

            if (GUI.Button(buttonRect, GUIContent.none, style))
            {
                if (dialogueEditor.findingParentNode != null)
                {
                    dialogueEditor.ClearConnectingNodes();
                    dialogueEditor.findingParentNode = node;
                    dialogueEditor.Repaint();
                }
                else if (dialogueEditor.findingChildNode != null)
                {
                    dialogueEditor.findingChildNode.ChildID = node.uniqueID;
                    dialogueEditor.ClearConnectingNodes();
                    dialogueEditor.Repaint();
                }
                else
                {
                    dialogueEditor.findingParentNode = node;
                }
            }

        }
        private void DrawOutConnector(DialogueNode node)
        {
            Vector2 pos = GetOutConnectorPos(node);
            Rect buttonRect = new Rect(pos, radioButtonSize);
            GUIStyle style = new GUIStyle(EditorStyles.radioButton);
            if (dialogueEditor.selectedDialogue.GetChildOfNode(node) != null)
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
                    node.ChildID = dialogueEditor.findingParentNode.uniqueID;
                    dialogueEditor.ClearConnectingNodes();
                    dialogueEditor.Repaint();
                }
                else
                {
                    dialogueEditor.findingChildNode = node;
                }
            }
        }

        public Vector2 GetInConnectorPos(DialogueNode node)
        {
            var nodePos = node.rect.position;
            var yOffset = node.rect.height * 0.5f - 5f;
            Vector2 target = nodePos + new Vector2(0, yOffset);
            return target;
        }
        public Vector2 GetOutConnectorPos(DialogueNode node)
        {
            Vector2 nodePos = node.rect.position;
            float xOffset = node.rect.width - 15f;
            float yOffset = node.rect.height * .5f - 5f;
            Vector2 target = nodePos + new Vector2(xOffset, yOffset);
            return target;
        }
        public void DrawConnection(DialogueNode parentNode, DialogueNode childNode)
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
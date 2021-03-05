using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CoreTools.Dialogue
{
    public class DialogueEditor : EditorWindow
    {
        private static readonly string windowTitle = "Dialogue Window";

        NodeDrawer nodeDrawer;

        public DialogueSO selectedDialogue;

        [NonSerialized]
        GraphNode draggedNode = null;
        [NonSerialized]
        Vector2 nodeDragOffset = Vector2.zero;
        [NonSerialized]
        bool dragginViewPort = false;
        [NonSerialized]
        Vector2 viewDragOffset = Vector2.zero;

        [NonSerialized]
        public DialogueNode findingParentNode = null;
        [NonSerialized]
        public GraphNode findingChildNode = null;
        [NonSerialized]
        public int findingChildChoiceId = -1;

        [NonSerialized]
        private GraphNode creatingNode;
        [NonSerialized]
        private GraphNode removeNode;

        Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Tools/DialogueEditor")]
        public static void OpenWindow()
        {
            var window = GetWindow<DialogueEditor>(windowTitle);
            window.Show();
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            DialogueSO targetObj = EditorUtility.InstanceIDToObject(instanceID) as DialogueSO;
            if (targetObj != null)
            {
                OpenWindow();
                return true;
            }
            return false;
        }
        private void OnEnable()
        {
            Debug.Log("OnEnable");
            nodeDrawer = new NodeDrawer(this);
            OnSelectionChange();
            ClearConnectingNodes();
            Repaint();
        }
        private void OnSelectionChange()
        {
            if (Selection.activeObject is DialogueSO)
            {
                selectedDialogue = Selection.activeObject as DialogueSO;
                GUI.changed = true;
                Repaint();
            }
        }
        private void OnFocus()
        {
            Debug.Log("OnFocus");
            if (nodeDrawer == null)
                nodeDrawer = new NodeDrawer(this);
            ClearConnectingNodes();
            Repaint();
        }
        private void OnProjectChange()
        {
            Debug.Log("project change");
        }
        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue selected!");
                return;
            }
            DrawToolBar();
            DrawEventToolbar();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true);
            Rect canvas = GUILayoutUtility.GetRect(4000, 4000);
            Texture2D background = Resources.Load("background2") as Texture2D;
            Rect texCoords = new Rect(0, 0, canvas.width / background.width, canvas.height / background.height);
            GUI.DrawTextureWithTexCoords(canvas, background, texCoords);

            DrawGraph();
            ProcessEvents();
            EditorGUILayout.EndScrollView();

            if (creatingNode != null)
                CreateNewDialogueNode();
            if (removeNode != null)
                RemoveNode(removeNode);
        }
        private void DrawGraph()
        {
            foreach (GraphNode node in selectedDialogue.GetAllGraphNodes())
            {
                nodeDrawer.DrawGraphNode(node);

                if (node is ChoiceNode choiceNode)
                {
                    nodeDrawer.DrawChoiceNodeConnections(choiceNode);
                }
                else
                {
                    var child = selectedDialogue.GetChildNode(node);
                    if (child != null)
                        nodeDrawer.DrawConnection(node, child);
                }
            }

            DrawSearchingNodeConnection();
        }
        private void DrawSearchingNodeConnection()
        {
            if (findingParentNode != null)
            {
                float offsetValue = nodeDrawer.radioButtonSize.x * .5f;
                Vector3 offsetVector = new Vector3(offsetValue, offsetValue, 0);
                Vector3 startPoint = (Vector3)nodeDrawer.GetInConnectorPos(findingParentNode) + offsetVector;
                Vector3 endPoint = Event.current.mousePosition;
                Vector3 startTangent = startPoint + (Vector3.left * 50f);
                Vector3 endTangent = endPoint + (Vector3.right * 50f);

                Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.red, null, 4f);
                Repaint();
            }
            else if (findingChildNode != null)
            {
                if (findingChildNode is ChoiceNode choiceNode)
                {
                    if (findingChildChoiceId >= 0 && findingChildChoiceId < choiceNode.ChoiceAmount)
                    {
                        float offsetValue = nodeDrawer.radioButtonSize.x * .5f;
                        Vector3 offsetVector = new Vector3(offsetValue, offsetValue, 0);

                        Vector3 startPoint = (Vector3)nodeDrawer.GetChoiceConnectorPosition(choiceNode, findingChildChoiceId) + offsetVector;
                        Vector3 endPoint = Event.current.mousePosition;

                        Vector3 startTangent = startPoint + (Vector3.right * 50f);
                        Vector3 endTangent = endPoint + (Vector3.left * 50f);

                        Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.red, null, 4f);
                        Repaint();
                    }
                }
                else
                {
                    float offsetValue = nodeDrawer.radioButtonSize.x * .5f;
                    Vector3 offsetVector = new Vector3(offsetValue, offsetValue, 0);
                    Vector3 startPoint = (Vector3)nodeDrawer.GetOutConnectorPos(findingChildNode) + offsetVector;
                    Vector3 endPoint = Event.current.mousePosition;
                    Vector3 startTangent = startPoint + (Vector3.right * 50f);
                    Vector3 endTangent = endPoint + (Vector3.left * 50f);

                    Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.red, null, 4f);
                    Repaint();
                }
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 0)
                {
                    OnLeftClick();
                }
                else if (Event.current.button == 1)
                {
                    OnRightClick();
                }
            }
            else if (Event.current.type == EventType.MouseDrag)
            {
                OnMouseDrag();
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                OnMouseUp();
            }
        }
        private GraphNode GetNodeAtPoint(Vector2 mousePos)
        {
            // returns EntryNode OR the LAST node from GetNodes() that matches the position
            GraphNode targetNode = null;
            foreach (GraphNode node in selectedDialogue.GetAllGraphNodes())
            {
                if (node.NodeRect.Contains(mousePos))
                {
                    targetNode = node;
                }
            }

            if (selectedDialogue.GetEntryNode().NodeRect.Contains(mousePos))
                targetNode = selectedDialogue.GetEntryNode();

            return targetNode;
        }
        public void ClearConnectingNodes()
        {
            findingChildNode = null;
            findingParentNode = null;
            findingChildChoiceId = -1;
        }

        private void OnLeftClick()
        {
            draggedNode = GetNodeAtPoint(Event.current.mousePosition);
            if (draggedNode != null)
            {
                nodeDragOffset = draggedNode.NodeRect.position - (Event.current.mousePosition + scrollPosition);
            }
            else
            {
                dragginViewPort = true;
                viewDragOffset = Event.current.mousePosition + scrollPosition;
            }

            if (findingChildNode != null)
            {
                if (findingChildNode is DialogueNode)
                    (findingChildNode as DialogueNode).ChildID = null;
                else if (findingChildNode is EntryNode)
                    (findingChildNode as EntryNode).ChildID = null;
            }
            if (findingParentNode != null)
            {
                //foreach (DialogueNode parent in selectedDialogue.GetParentNodes(findingParentNode))
                //{
                //    parent.ChildID = null;
                //}
                findingParentNode = null;
            }

            ClearConnectingNodes();
        }
        private void OnRightClick()
        {
            ClearConnectingNodes();
        }
        private void OnMouseUp()
        {
            draggedNode = null;
            nodeDragOffset = Vector2.zero;
            dragginViewPort = false;
        }
        private void OnMouseDrag()
        {
            if (draggedNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Moved GraphNode");
                Vector2 newPosition = Event.current.mousePosition + scrollPosition + nodeDragOffset;
                draggedNode.SetPosition(newPosition);
                GUI.changed = true;
                Repaint();
            }
            else if (dragginViewPort)
            {
                scrollPosition = viewDragOffset - Event.current.mousePosition;
                Repaint();
            }
        }
        private void DrawToolBar()
        {
            string[] tools = { "New Dialogue Node", "New Choice Node", "Save" };
            int newSelection = GUILayout.Toolbar(-1, tools);
            switch (newSelection)
            {
                case 0:
                    CreateNewDialogueNode();
                    break;
                case 1:
                    CreateNewChoiceNode();
                    break;
                case 2:
                    AssetDatabase.SaveAssets();
                    break;
                default:
                    break;
            }
        }
        private void DrawEventToolbar()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Create Event Channel: ", EditorStyles.toolbarButton, GUILayout.Width(200f));
            string[] tools = { "Void", "Bool", "Float", "Int", "String" };
            int newSelection = GUILayout.Toolbar(-1, tools);
            switch (newSelection)
            {
                case 0:
                    selectedDialogue.CreateVoidEventNode();
                    Repaint();
                    break;
                case 1:
                    selectedDialogue.CreateBoolEventNode();
                    Repaint();
                    break;
                case 2:
                    selectedDialogue.CreateFloatEventNode();
                    Repaint();
                    break;
                case 3:
                    selectedDialogue.CreateIntEventNode();
                    Repaint();
                    break;
                case 4:
                    selectedDialogue.CreateStringEventNode();
                    Repaint();
                    break;
                default:
                    break;
            }
            EditorGUILayout.EndHorizontal();
        }

        public void MarkAsCreationNode(GraphNode node)
        {
            creatingNode = node;
        }
        private void CreateNewDialogueNode()
        {
            DialogueNode newNode = selectedDialogue.CreateDialogueNode(creatingNode);
            if (creatingNode != null)
            {
                float xOffset = creatingNode.NodeRect.width + 50f;
                float yOffset = 20f;
                Vector2 newPosition = creatingNode.NodeRect.position + new Vector2(xOffset, yOffset);
                newNode.SetPosition(newPosition);
            }
            creatingNode = null;
            Repaint();
        }
        private void CreateNewChoiceNode()
        {
            ChoiceNode newNode = selectedDialogue.CreateChoiceNode(creatingNode);
            if (creatingNode != null)
            {
                float xOffset = creatingNode.NodeRect.width + 50f;
                float yOffset = 20f;
                Vector2 newPosition = creatingNode.NodeRect.position + new Vector2(xOffset, yOffset);
                newNode.SetPosition(newPosition);
            }
            creatingNode = null;
            Repaint();
        }
        public void MarkNodeToRemove(GraphNode node)
        {
            removeNode = node;
        }
        private void RemoveNode(GraphNode node)
        {
            selectedDialogue.RemoveNode(node);
            removeNode = null;
            Repaint();
        }
    }
}
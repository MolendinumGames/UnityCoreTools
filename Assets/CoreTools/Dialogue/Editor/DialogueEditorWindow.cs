using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using CoreTools.NodeSystem;

namespace CoreTools.DialogueSystem.Editor
{
    public class DialogueEditorWindow : EditorWindow
    {
        private static readonly string windowTitle = "Dialogue Window";

        DialogueNodeDrawer nodeDrawer;

        public Dialogue selectedDialogue;

        private readonly Vector2 canvasSize = new Vector2(4000, 4000);
        private readonly Vector2 standardNodePosition = new Vector2(20f, 20f);
        private readonly float connectionWidth = 4f;
        private readonly float tangentOffset = 50f;

        // Dragging
        private GraphNode draggedNode = null;
        private Vector2 nodeDragOffset = Vector2.zero;
        private bool dragginViewPort = false;
        private Vector2 viewPortDragStartPoint = Vector2.zero;

        private Vector2 scrollPosition = Vector2.zero;

        // Connecting
        [NonSerialized]
        public GraphNode findingParentNode = null;
        [NonSerialized]
        public GraphNode findingChildNode = null;
        [NonSerialized]
        public int findingChildChoiceId = -1;

        private GraphNode removeNode;

        // Popup
        private bool popupOpen = false;
        private Vector2 creationPopupPosition = Vector2.zero;
        private readonly Vector2 popupSize = new Vector2(160f, 210f);

        // Drawn last (on top)
        [NonSerialized]
        public GraphNode focusedNode = null;


        [MenuItem("Tools/DialogueEditor")]
        public static void OpenWindow()
        {
            var window = GetWindow<DialogueEditorWindow>(windowTitle);
            window.Show();
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue targetObj = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (targetObj != null)
            {
                OpenWindow();
                return true;
            }
            return false;
        }
        private void OnEnable()
        {
            nodeDrawer = new DialogueNodeDrawer(this);
            OnSelectionChange();
            Undo.undoRedoPerformed += Repaint;
        }
        private void OnDisable()
        {
            Undo.undoRedoPerformed -= Repaint;
        }
        private void OnSelectionChange()
        {
            if (Selection.activeObject is Dialogue)
            {
                selectedDialogue = Selection.activeObject as Dialogue;
                Repaint();
            }
        }
        private void OnFocus()
        {
            if (nodeDrawer == null)
                nodeDrawer = new DialogueNodeDrawer(this);

            Repaint();
        }
        private void OnLostFocus()
        {
            ClearPopup();
            focusedNode = null;
        }
        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue selected!");
                return;
            }


            DrawHeaderToolbar();

            DrawEventToolbar();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true);

            DrawBackGround();

            DrawGraph();

            DrawSearchingNodeConnection();

            HandlePopup();

            EditorGUILayout.EndScrollView();

            ProcessEvents();

            HandleRemoveNode();

            if (dragginViewPort)
                Repaint();
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
            //else if (Event.current.keyCode == KeyCode.Space)
            //{
            //    OpenCreationPopup();
            //}
        }
        private void DrawHeaderToolbar()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Editing: " + selectedDialogue.name, EditorStyles.toolbarButton, GUILayout.Width(200f));
            string[] tools = { "New Dialogue Node", "New Choice Node", "Save" };
            int newSelection = GUILayout.Toolbar(-1, tools);
            GUILayout.EndHorizontal();

            // Process Button Press:
            switch (newSelection)
            {
                case 0:
                    GraphNode node = selectedDialogue.CreateDialogueNode();
                    node.SetPosition(standardNodePosition);
                    break;
                case 1:
                    GraphNode newNode = selectedDialogue.CreateChoiceNode();
                    newNode.SetPosition(standardNodePosition);
                    break;
                case 2:
                    AssetDatabase.SaveAssets();
                    break;
                default:
                    break;
            }
            if (newSelection >= 0)
            {
                EditorUtility.SetDirty(selectedDialogue);
                ClearPopup();
                ClearConnectingNodes();
                Repaint();
            }
        }
        private void DrawEventToolbar()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Create Event Channel: ", EditorStyles.toolbarButton, GUILayout.Width(200f));
            string[] tools = { "Void", "Bool", "Float", "Int", "String" };
            int newSelection = GUILayout.Toolbar(-1, tools);

            // Process Button press
            switch (newSelection)
            {
                case 0:
                    selectedDialogue.CreateVoidEventNode();
                    break;
                case 1:
                    selectedDialogue.CreateBoolEventNode();
                    break;
                case 2:
                    selectedDialogue.CreateFloatEventNode();
                    break;
                case 3:
                    selectedDialogue.CreateIntEventNode();
                    break;
                case 4:
                    selectedDialogue.CreateStringEventNode();
                    break;
                default:
                    break;
            }
            if (newSelection >= 0)
            {
                EditorUtility.SetDirty(selectedDialogue);
                ClearConnectingNodes();
                ClearPopup();
                Repaint();
            }
            EditorGUILayout.EndHorizontal();
        }
        private void DrawBackGround()
        {
            Rect canvas = GUILayoutUtility.GetRect(canvasSize.x, canvasSize.y);
            Texture2D background = Resources.Load("background2") as Texture2D;
            Rect texCoords = new Rect(0, 0, canvas.width / background.width, canvas.height / background.height);

            GUI.DrawTextureWithTexCoords(canvas, background, texCoords);
        }
        private void DrawGraph()
        {
            foreach (GraphNode node in selectedDialogue.GetAllGraphNodes())
            {
                if (node != focusedNode)
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
            // Draw selected Node last / on top
            DrawSelectedNode();
        }
        private void DrawSelectedNode()
        {
            if (focusedNode != null)
                nodeDrawer.DrawGraphNode(focusedNode);
        }
        private void DrawSearchingNodeConnection()
        {
            float offsetValue = nodeDrawer.radioButtonSize.x * .5f;
            Vector3 offsetVector = new Vector3(offsetValue, offsetValue, 0);
            Vector3 endPoint = Event.current.mousePosition;

            if (findingParentNode != null)
            {
                Vector3 startPoint = (Vector3)nodeDrawer.GetInConnectorPos(findingParentNode) + offsetVector;
                Vector3 startTangent = startPoint + (Vector3.left * tangentOffset);
                Vector3 endTangent = endPoint + (Vector3.right * tangentOffset);

                Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.red, null, connectionWidth);
                Repaint();
            }
            else if (findingChildNode != null)
            {
                if (findingChildNode is ChoiceNode choiceNode)
                {
                    if (findingChildChoiceId >= 0 && findingChildChoiceId < choiceNode.ChoiceAmount)
                    {
                        Vector3 startPoint = (Vector3)nodeDrawer.GetChoiceConnectorPosition(choiceNode, findingChildChoiceId) + offsetVector;
                        Vector3 startTangent = startPoint + (Vector3.right * tangentOffset);
                        Vector3 endTangent = endPoint + (Vector3.left * tangentOffset);

                        Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.red, null, connectionWidth);
                    }
                }
                else
                {
                    Vector3 startPoint = (Vector3)nodeDrawer.GetOutConnectorPos(findingChildNode) + offsetVector;
                    Vector3 startTangent = startPoint + (Vector3.right * 50f);
                    Vector3 endTangent = endPoint + (Vector3.left * 50f);

                    Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.red, null, connectionWidth);
                }
                Repaint();
            }
        }
        private void HandlePopup()
        {
            if (popupOpen)
                DrawCreationPopup(creationPopupPosition);
        }
        private void HandleRemoveNode()
        {
            if (removeNode != null)
                RemoveNode(removeNode);
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

        #region Mouse Events
        private void OnLeftClick()
        {
            GraphNode targetedNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition - new Vector2(0, EditorGUIUtility.singleLineHeight * 2 + 5));
            draggedNode = targetedNode;
            focusedNode = targetedNode;

            if (targetedNode != null)
            {
                nodeDragOffset = draggedNode.NodeRect.position - (Event.current.mousePosition + scrollPosition);

                if (findingParentNode != null)
                {
                    if (findingParentNode != targetedNode)
                        ((ISingleChild)targetedNode).ChildID = findingParentNode.UniqueID;
                    ClearConnectingNodes();
                    Repaint();
                }
                else if (findingChildNode != null)
                {
                    if (findingChildNode == targetedNode)
                    {
                        // do nothing
                    }
                    else if (findingChildNode is ChoiceNode choiceNode)
                    {
                        if (findingChildChoiceId >= 0 && findingChildChoiceId < choiceNode.GetAllChoices().Count)
                        {
                            choiceNode.SetChildOfChoice(findingChildChoiceId, targetedNode.UniqueID);
                            ClearConnectingNodes();
                            Repaint();
                        }
                    }
                    else
                    {
                        ((ISingleChild)findingChildNode).ChildID = targetedNode.UniqueID;
                        ClearConnectingNodes();
                        Repaint();
                    }
                }
            }
            else
            {
                focusedNode = null;
                dragginViewPort = true;
                viewPortDragStartPoint = Event.current.mousePosition + scrollPosition;
                if (findingChildNode != null)
                {
                    ((ISingleChild)findingChildNode).ChildID = null;
                    if (findingChildNode is ChoiceNode choiceNode)
                    {
                        if (findingChildChoiceId >= 0 && findingChildChoiceId < choiceNode.ChoiceAmount)
                        {
                            choiceNode.SetChildOfChoice(findingChildChoiceId, null);
                        }
                    }
                }
                ClearConnectingNodes();
                Repaint();
            }

            if (focusedNode != null)
                Repaint();
            if (popupOpen)
            {
                Rect popupRect = new Rect(creationPopupPosition, popupSize);
                if (!popupRect.Contains(Event.current.mousePosition + scrollPosition))
                {
                    ClearPopup();
                    Repaint();
                }
            }
        }
        private void OnRightClick()
        {
            ClearConnectingNodes();
            //focusedNode = null;
            //ClearPopup();
            OpenCreationPopup();
            Repaint();
        }
        private void OnMouseUp()
        {
            draggedNode = null;
            nodeDragOffset = Vector2.zero;
            dragginViewPort = false;
            viewPortDragStartPoint = Vector2.zero;
        }
        private void OnMouseDrag()
        {
            if (draggedNode != null)
            {
                Vector2 newPosition = Event.current.mousePosition + scrollPosition + nodeDragOffset;
                draggedNode.SetPosition(newPosition);
                GUI.changed = true;
                Repaint();
            }
            else if (dragginViewPort)
            {
                scrollPosition = viewPortDragStartPoint - Event.current.mousePosition;
                Repaint();
            }
        }
        #endregion

        #region Node Removing
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
        #endregion

        #region Popup
        private void OpenCreationPopup()
        {
            creationPopupPosition = Event.current.mousePosition + scrollPosition - new Vector2(0, EditorGUIUtility.singleLineHeight * 2);
            popupOpen = true;
            ClearConnectingNodes();
            focusedNode = null;
            Repaint();
        }
        private void DrawCreationPopup(Vector2 position)
        {
            GUIStyle popupStyle = new GUIStyle();
            popupStyle.normal.background = EditorGUIUtility.Load("node5") as Texture2D;
            popupStyle.normal.textColor = Color.white;
            popupStyle.padding = new RectOffset(20, 20, 15, 15);
            popupStyle.border = new RectOffset(33, 33, 33, 33);


            Rect popupRect = new Rect(position, popupSize);
            GUILayout.BeginArea(popupRect, popupStyle);

            bool buttonPressed = false;
            GUILayout.Label("Create New Node: ", EditorStyles.boldLabel);
            if (GUILayout.Button("Dialogue Node"))
            {
                GraphNode newNode = selectedDialogue.CreateDialogueNode();
                newNode.SetPosition(creationPopupPosition);
                buttonPressed = true;
            }
            if (GUILayout.Button("Choice Node"))
            {
                GraphNode newNode = selectedDialogue.CreateChoiceNode();
                newNode.SetPosition(creationPopupPosition);
                buttonPressed = true;
            }
            if (GUILayout.Button("Void Event Node"))
            {
                GraphNode newNode = selectedDialogue.CreateVoidEventNode();
                newNode.SetPosition(creationPopupPosition);
                buttonPressed = true;
            }
            if (GUILayout.Button("Bool Event Node"))
            {
                GraphNode newNode = selectedDialogue.CreateBoolEventNode();
                newNode.SetPosition(creationPopupPosition);
                buttonPressed = true;
            }
            if (GUILayout.Button("String Event Node"))
            {
                GraphNode newNode = selectedDialogue.CreateStringEventNode();
                newNode.SetPosition(creationPopupPosition);
                buttonPressed = true;
            }
            if (GUILayout.Button("Int Event Node"))
            {
                GraphNode newNode = selectedDialogue.CreateIntEventNode();
                newNode.SetPosition(creationPopupPosition);
                buttonPressed = true;
            }
            if (GUILayout.Button("Float Event Node"))
            {
                GraphNode newNode = selectedDialogue.CreateFloatEventNode();
                newNode.SetPosition(creationPopupPosition);
                buttonPressed = true;
            }
            if (buttonPressed)
            {
                EditorUtility.SetDirty(selectedDialogue);
                ClearPopup();
                Repaint();
            }

            GUILayout.EndArea();
        }
        private void ClearPopup()
        {
            popupOpen = false;
            creationPopupPosition = Vector2.zero;
        }
        #endregion

        // UNUSED FOR NOW
        //public void MarkAsCreationNode(GraphNode node)
        //{
        //    creatingNode = node;
        //}
        //public void ResetNode(GraphNode node)
        //{
        //    node.Reset();
        //    Repaint();
        //}

    }
}
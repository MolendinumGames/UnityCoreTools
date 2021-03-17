using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using CoreTools.NodeSystem;

namespace CoreTools.NodeSystem
{
    public abstract class NodeEditorWindow : EditorWindow
    {
        protected abstract string NoGraphMessage { get; }

        protected NodeDrawer nodeDrawer;

        public NodeHolder selectedGraph;

        private readonly Vector2 canvasSize = new Vector2(4000, 4000);
        private readonly Vector2 standardNodePosition = new Vector2(20f, 20f);
        private readonly string backgroundAssetpath = "background2";

        // Node Dragging
        private GraphNode draggedNode = null;
        private Vector2 nodeDragOffset = Vector2.zero;

        // Viewport Dragging
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

        // Connection Drawing
        public readonly float connectionWidth = 4f;
        public readonly float tangentOffset = 50f;

        private GraphNode removeNode = null;
        // private GraphNode createNode = null;

        // Popup
        private bool popupOpen = false;
        private Vector2 creationPopupPosition = Vector2.zero;
        private readonly Vector2 popupSize = new Vector2(160f, 210f);

        // Drawn last (on top)
        [NonSerialized]
        public GraphNode focusedNode = null;

        protected virtual void OnEnable()
        {
            OnSelectionChange();
            Undo.undoRedoPerformed += Repaint;
        }
        protected virtual void OnDisable()
        {
            Undo.undoRedoPerformed -= Repaint;
        }
        protected void OnSelectionChange()
        {
            if (ProcessSelection())
                Repaint();
        }
        protected abstract bool ProcessSelection();
        protected virtual void OnFocus()
        {
            Repaint();
        }
        protected virtual void OnLostFocus()
        {
            ClearPopup();
            focusedNode = null;
        }
        protected void OnGUI()
        {
            if (selectedGraph == null)
            {
                GUILayout.Label(NoGraphMessage);
                return;
            }

            // Header toolbar here?
            // Name - UsageInfo - Save

            // toolbar not handled here

            // Draw usage tips?

            //handled here
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true);

            // handled here
            DrawBackGround();

            DrawGraph();

            DrawSearchingNodeConnection();

            // Handled here but gives OnPopupGUI
            // maybe bool if implemented?
            DrawPopup();

            // handled here
            EditorGUILayout.EndScrollView();

            // handled here. Gives virtual OnEvent functions
            ProcessEvents();

            // handled here
            HandleRemoveNode();

            if (dragginViewPort)
                Repaint();
        }
        private void ProcessEvents() // handled here
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

        private void DrawBackGround()
        {
            Rect canvas = GUILayoutUtility.GetRect(canvasSize.x, canvasSize.y);
            Texture2D background = Resources.Load(backgroundAssetpath) as Texture2D;
            Rect texCoords = new Rect(0, 0, canvas.width / background.width, canvas.height / background.height);

            GUI.DrawTextureWithTexCoords(canvas, background, texCoords);
        }
        private void DrawGraph()
        {
            foreach (GraphNode node in selectedGraph.GetAllGraphNodes())
            {
                if (node != focusedNode)
                    nodeDrawer.DrawGraphNode(node);

                //if (node is ChoiceNode choiceNode)
                //{
                //    nodeDrawer.DrawChoiceNodeConnections(choiceNode);
                //}
                //else
                //{
                //    var child = selectedDialogue.GetChildNode(node);
                //    if (child != null)
                //        nodeDrawer.DrawConnection(node, child);
                //}
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
            Vector3 mousePos = Event.current.mousePosition;

            if (findingParentNode != null)
            {
                nodeDrawer.DrawParentSearchConnection(findingParentNode, mousePos);
                Repaint();
            }
            else if (findingChildNode != null)
            {
                if (findingChildNode is IChoiceContainer)
                {
                    nodeDrawer.DrawChildSearchConnection(findingChildNode, findingChildChoiceId, mousePos);
                }
                else
                {
                    nodeDrawer.DrawChildSearchConnection(findingChildNode, mousePos);
                }
                Repaint();
            }
        }
        private void DrawPopup()
        {
            if (!popupOpen)
                return;

            GUIStyle popupStyle = new GUIStyle();
            popupStyle.normal.background = EditorGUIUtility.Load("node5") as Texture2D;
            popupStyle.normal.textColor = Color.white;
            popupStyle.padding = new RectOffset(20, 20, 15, 15);
            popupStyle.border = new RectOffset(33, 33, 33, 33);


            Rect popupRect = new Rect(creationPopupPosition, popupSize);
            GUILayout.BeginArea(popupRect, popupStyle);

            bool buttonPressed = false;
            GUILayout.Label("Create New Node: ", EditorStyles.boldLabel);
            //if (GUILayout.Button("Dialogue Node"))
            //{
            //    GraphNode newNode = selectedGraph.CreateDialogueNode();
            //    newNode.SetPosition(creationPopupPosition);
            //    buttonPressed = true;
            //}
            //if (GUILayout.Button("Choice Node"))
            //{
            //    GraphNode newNode = selectedGraph.CreateChoiceNode();
            //    newNode.SetPosition(creationPopupPosition);
            //    buttonPressed = true;
            //}
            //if (GUILayout.Button("Void Event Node"))
            //{
            //    GraphNode newNode = selectedGraph.CreateVoidEventNode();
            //    newNode.SetPosition(creationPopupPosition);
            //    buttonPressed = true;
            //}
            //if (GUILayout.Button("Bool Event Node"))
            //{
            //    GraphNode newNode = selectedGraph.CreateBoolEventNode();
            //    newNode.SetPosition(creationPopupPosition);
            //    buttonPressed = true;
            //}
            //if (GUILayout.Button("String Event Node"))
            //{
            //    GraphNode newNode = selectedGraph.CreateStringEventNode();
            //    newNode.SetPosition(creationPopupPosition);
            //    buttonPressed = true;
            //}
            //if (GUILayout.Button("Int Event Node"))
            //{
            //    GraphNode newNode = selectedGraph.CreateIntEventNode();
            //    newNode.SetPosition(creationPopupPosition);
            //    buttonPressed = true;
            //}
            //if (GUILayout.Button("Float Event Node"))
            //{
            //    GraphNode newNode = selectedGraph.CreateFloatEventNode();
            //    newNode.SetPosition(creationPopupPosition);
            //    buttonPressed = true;
            //}
            //if (buttonPressed)
            //{
            //    EditorUtility.SetDirty(selectedGraph);
            //    ClearPopup();
            //    Repaint();
            //}

            GUILayout.EndArea();
        }
        private void HandleRemoveNode()
        {
            if (removeNode != null)
                RemoveNode(removeNode);
        }
        private GraphNode GetNodeAtPoint(Vector2 mousePos)
        {
            // returns selected Node or the LAST node from GetNodes() that matches the position
            GraphNode targetNode = null;
            foreach (GraphNode node in selectedGraph.GetAllGraphNodes())
            {
                if (node.NodeRect.Contains(mousePos))
                {
                    targetNode = node;
                }
            }

            if (selectedGraph.GetEntryNode().NodeRect.Contains(mousePos))
                targetNode = selectedGraph.GetEntryNode();

            return targetNode;
        }
        public void ClearConnectingNodes()
        {
            findingChildNode = null;
            findingParentNode = null;
            findingChildChoiceId = -1;
        }

        #region Mouse Events
        protected virtual void OnLeftClick()
        {
            focusedNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition - new Vector2(0, EditorGUIUtility.singleLineHeight * 2 + 5));
            draggedNode = focusedNode;

            if (focusedNode != null)
            {
                // start drag
                nodeDragOffset = draggedNode.NodeRect.position - (Event.current.mousePosition + scrollPosition);

                // check if making connection
                if (findingParentNode != null && findingParentNode != focusedNode)
                {
                    if (focusedNode is ISingleChild singleParent)
                        singleParent.ChildID = findingParentNode.UniqueID;

                    if (focusedNode is IMultiChild multiParent)
                        multiParent.AddChild(findingParentNode.UniqueID);

                    // Just Reset if clicked on ChoiceNode
                }
                else if (findingChildNode != null && findingChildNode != focusedNode)
                {
                    if (findingChildNode is ISingleChild singleParent)
                        singleParent.ChildID = focusedNode.UniqueID;

                    if (findingChildNode is IMultiChild multiParent)
                        multiParent.AddChild(focusedNode.UniqueID);

                    if (findingChildNode is IChoiceContainer choiceParent)
                        choiceParent.SetChildOfChoice(findingChildChoiceId, focusedNode.UniqueID);
                }
            }
            else
            {
                focusedNode = null;

                // start dragging viewport
                dragginViewPort = true;
                viewPortDragStartPoint = Event.current.mousePosition + scrollPosition;

                // clear connection only if a node was searching a Child
                if (findingChildNode != null)
                {
                    if (findingChildNode is ISingleChild singleParent)
                        singleParent.ChildID = null;
                    if (findingChildNode is IMultiChild multiParent)
                        multiParent.ClearAllChildren();
                    if (findingChildNode is IChoiceContainer choiceParent)
                        choiceParent.SetChildOfChoice(findingChildChoiceId, null);
                }
            }

            if (popupOpen)
            {
                Rect popupRect = new Rect(creationPopupPosition, popupSize);
                if (!popupRect.Contains(Event.current.mousePosition + scrollPosition))
                {
                    ClearPopup();
                }
            }

            ClearConnectingNodes();
            Repaint();
        }
        protected virtual void OnRightClick()
        {
            ClearConnectingNodes();
            //focusedNode = null;
            //ClearPopup();
            OpenCreationPopup();
            Repaint();
        }
        protected virtual void OnMouseUp()
        {
            draggedNode = null;
            nodeDragOffset = Vector2.zero;
            dragginViewPort = false;
            viewPortDragStartPoint = Vector2.zero;
        }
        protected virtual void OnMouseDrag()
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
        protected void RemoveNode(GraphNode node)
        {
            selectedGraph.RemoveNode(node);
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
        protected abstract void OnDrawPopupContent(Vector2 position);
        private void ClearPopup()
        {
            popupOpen = false;
            creationPopupPosition = Vector2.zero;
        }
        #endregion
    }
}
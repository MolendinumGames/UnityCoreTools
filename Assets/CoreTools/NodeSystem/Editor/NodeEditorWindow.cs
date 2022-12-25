/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

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
        protected abstract int TopToolbarCount { get; }

        protected GraphDrawer nodeDrawer;

        public NodeHolder selectedGraph;

        private readonly Vector2 canvasBaseSize = new Vector2(2500, 2500);
        private readonly Vector2 extraSize = new Vector2(450f, 450f);
        private readonly Vector2 maxAreaSize = new Vector2(1000000f, 1000000f);
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
        protected Vector2 creationPopupPosition = Vector2.zero;
        private readonly Vector2 popupSize = new Vector2(160f, 40f);

        // Drawn last (on top)
        [NonSerialized]
        public GraphNode focusedNode = null;

        protected abstract int PopupButtonCount { get; }

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

            DrawHeaderToolbar();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true);

            DrawBackGround();

            DrawGraph();

            DrawSearchingNodeConnection();

            DrawPopup();

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
        }

        private void DrawBackGround()
        {
            Vector2 size = GetAreaSize();
            Rect canvas = GUILayoutUtility.GetRect(size.x, size.y);
            Texture2D background = Resources.Load(backgroundAssetpath) as Texture2D;
            Rect texCoords = new Rect(0, 0, canvas.width / background.width, canvas.height / background.height);

            GUI.DrawTextureWithTexCoords(canvas, background, texCoords);
        }
        private void DrawHeaderToolbar()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Editing: " + selectedGraph.name, EditorStyles.toolbarButton, GUILayout.Width(200f));
            GUILayout.Label("(!) Right click to open creation menu");
            if (GUILayout.Button("Save"))
            {
                AssetDatabase.SaveAssets();
            }
            GUILayout.EndHorizontal();
        }
        private void DrawGraph()
        {
            foreach (GraphNode node in selectedGraph.GetAllGraphNodes())
            {
                if (node != focusedNode)
                    nodeDrawer.DrawGraphNode(node);
            }

            // Draw selected Node last / on top:
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

            float popupHeight = popupSize.y + (EditorGUIUtility.singleLineHeight + 1f) * PopupButtonCount;
            Vector2 newPopupSize = new Vector2(popupSize.x, popupSize.y + popupHeight);
            Rect popupRect = new Rect(creationPopupPosition, newPopupSize);
            GUILayout.BeginArea(popupRect, popupStyle);

            GUILayout.Label("Create New Node: ", EditorStyles.boldLabel);
            OnDrawPopupContent(Event.current.mousePosition);

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
            float yOffset = TopToolbarCount * (EditorGUIUtility.singleLineHeight + 2);
            focusedNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition - new Vector2(0, yOffset));
            draggedNode = focusedNode;

            if (focusedNode != null)
            {
                // start drag
                nodeDragOffset = draggedNode.NodeRect.position - (Event.current.mousePosition + scrollPosition);

                // Don't let EntryNode eb set as child
                if (focusedNode.IsEntry)
                {
                    ClearConnectingNodes();
                    Repaint();
                    return;
                }

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
        public virtual void SetChildOfFindingChildNode(GraphNode node)
        {
            if (node == null)
                return;

            if (findingChildNode is ISingleChild singleParent)
                singleParent.ChildID = node.UniqueID;

            if (findingChildNode is IMultiChild multiParent)
                multiParent.AddChild(node.UniqueID);

            if (findingChildNode is IChoiceContainer choiceParent)
                choiceParent.SetChildOfChoice(findingChildChoiceId, node.UniqueID);

            ClearConnectingNodes();
            Repaint();
        }
        public virtual void SetParentOfFindingParentNode(GraphNode newParent, int choiceId)
        {
            if (newParent == null)
                return;

            if (newParent is ISingleChild singleParent)
                singleParent.ChildID = findingParentNode.UniqueID;

            if (newParent is IMultiChild multiParent)
                multiParent.AddChild(findingParentNode.UniqueID);

            if (newParent is IChoiceContainer choiceParent
                && choiceId >= 0)
                choiceParent.SetChildOfChoice(choiceId, findingParentNode.UniqueID);

            ClearConnectingNodes();
            Repaint();
        }
        public void SetFindingParentNode(GraphNode node)
        {
            ClearConnectingNodes();
            findingParentNode = node;
            focusedNode = node;
            Repaint();
        }
        public void SetFindingChildNode(GraphNode node)
        {
            ClearConnectingNodes();
            findingChildNode = node;
            Repaint();
        }
        public void SetFindingChildNode(GraphNode node, int choiceId)
        {
            ClearConnectingNodes();
            findingChildChoiceId = choiceId;
            findingChildNode = node;
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
        protected void ClearPopup()
        {
            popupOpen = false;
            creationPopupPosition = Vector2.zero;
        }
        #endregion

        private Vector2 GetAreaSize()
        {
            float width = canvasBaseSize.x;
            float height = canvasBaseSize.y;

            foreach (var node in selectedGraph.GetAllGraphNodes())
            {
                if (node.NodeRect.x > width)
                    width = node.NodeRect.x;
                if (node.NodeRect.y > height)
                    height = node.NodeRect.y;
            }

            Vector2 newSize = new Vector2(width, height) + extraSize;
            newSize.x = Mathf.Clamp(newSize.x, canvasBaseSize.x, maxAreaSize.x);
            newSize.y = Mathf.Clamp(newSize.y, canvasBaseSize.y, maxAreaSize.y);
            return newSize;
        }
    }
}
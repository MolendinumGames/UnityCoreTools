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

namespace CoreTools.DialogueSystem.Editor
{
    public class DialogueEditorWindow : NodeEditorWindow
    {
        public Dialogue selectedDialogue;
        private DialogueDrawer dialogueDrawer;

        protected override int PopupButtonCount => 7;
        protected override int TopToolbarCount => 1;
        protected override string NoGraphMessage => "No Dialogue Selected!";

        private static readonly string windowTitle = "Dialogue Editor";

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
        protected override void OnEnable()
        {
            base.OnEnable();
            dialogueDrawer = new DialogueDrawer(this, this);
            nodeDrawer = dialogueDrawer;
        }
        public override void SetChildOfFindingChildNode(GraphNode node)
        {
            if (node == null)
                return;

            if (findingChildNode is ISingleChild singleParent)
                singleParent.ChildID = node.UniqueID;

            //if (findingChildNode is IMultiChild multiParent)
            //    multiParent.AddChild(node.UniqueID);

            if (findingChildNode is IChoiceContainer choiceParent)
                choiceParent.SetChildOfChoice(findingChildChoiceId, node.UniqueID);

            ClearConnectingNodes();
            Repaint();
        }
        protected override void OnDrawPopupContent(Vector2 position)
        {
            bool buttonPressed = false;
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
                GraphNode newNode = selectedDialogue.CreateEventNode<VoidEventNode>();
                newNode.SetPosition(creationPopupPosition);
                buttonPressed = true;
            }
            if (GUILayout.Button("Dialogue Event Node"))
            {
                GraphNode newNode = selectedDialogue.CreateEventNode<DialogueEventNode>();
                newNode.SetPosition(creationPopupPosition);
                buttonPressed = true;
            }
            if (GUILayout.Button("Bool Event Node"))
            {
                GraphNode newNode = selectedDialogue.CreateEventNode<BoolEventNode>();
                newNode.SetPosition(creationPopupPosition);
                buttonPressed = true;
            }
            if (GUILayout.Button("String Event Node"))
            {
                GraphNode newNode = selectedDialogue.CreateEventNode<StringEventNode>();
                newNode.SetPosition(creationPopupPosition);
                buttonPressed = true;
            }
            if (GUILayout.Button("Int Event Node"))
            {
                GraphNode newNode = selectedDialogue.CreateEventNode<IntEventNode>();
                newNode.SetPosition(creationPopupPosition);
                buttonPressed = true;
            }
            if (GUILayout.Button("Float Event Node"))
            {
                GraphNode newNode = selectedDialogue.CreateEventNode<FloatEventNode>();
                newNode.SetPosition(creationPopupPosition);
                buttonPressed = true;
            }
            if (buttonPressed)
            {
                EditorUtility.SetDirty(selectedDialogue);
                ClearPopup();
                Repaint();
            }
        }

        protected override bool ProcessSelection()
        {
            if (Selection.activeObject is Dialogue newDialogue)
            {
                selectedGraph = newDialogue;
                selectedDialogue = newDialogue;
                return true;
            }

            return false;
        }
    }
}
/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using CoreTools.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using CoreTools.NodeSystem;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
    public class Dialogue : NodeHolder
    {
        public DialogueNode GetFirstNode()
        {
            GraphNode currentNode = GetEntryNode();
            return GetNextDialogue(currentNode);
        }
        public DialogueNode Next(string currentId)
        {
            DialogueNode currentNode = GetAnyGraphNode(currentId) as DialogueNode;
            if (currentNode != null)
            {
                if (currentNode is ChoiceNode)
                {
                    Debug.LogWarning($"Next called on choice node without index being passed in!" +
                        $" Dialogue: {this.name}. NodeID{currentId}.");
                    return Next(currentId, 0);
                }
                else return GetNextDialogue(currentNode);
            }
            else
            {
                Debug.LogWarning($"Cannot find current node. Dialogue: {this.name}. " +
                    $"CurrentID: {currentId}. Returning null.");
                return null;
            }
        }
        public DialogueNode Next(string currentId, int choice)
        {
            ChoiceNode currentNode = GetAnyGraphNode(currentId) as ChoiceNode;
            if (currentNode != null)
            {
                if (choice >= 0 && choice < currentNode.ChoiceAmount)
                {
                    string childId = currentNode.GetChildOfChoice(choice);
                    var childNode = GetAnyGraphNode(childId);
                    if (childNode == null)
                    {
                        return null;
                    }
                    else if (childNode is EventNode e)
                    {
                        e.Raise();
                        return GetNextDialogue(e);
                    }
                    else return childNode as DialogueNode;
                }
                else
                {
                    Debug.LogError($"Tried to access choice Index out of range. " +
                        $"Dialogue: {name}, NodeID: {currentId}, Index: {choice}.");
                    return null;
                }

            }
            else
            {
                Debug.LogError($"Trying to Next() choide node with ID but node is null. " +
                    $"Processing as non-choicenode next.");
                return Next(currentId);
            }
        }
        private DialogueNode GetNextDialogue(GraphNode fromNode)
        {
            GraphNode currentNode = fromNode;
            DialogueNode nextNode = null;
            while (nextNode == null)
            {
                currentNode = GetChildNode(currentNode);

                if (currentNode == null)
                {
                    return null;
                }
                else if (currentNode is DialogueNode dNode)
                {
                    nextNode = dNode;
                }
                else if (currentNode is EventNode eventNode)
                {
                    eventNode.Raise();
                }
            }
            return nextNode;
        }

        #region GetNodes Methodes
        public DialogueNode GetDialogueNode(string id)
        {
            if (nodeLookup.ContainsKey(id))
                return nodeLookup[id] as TextNode;
            else return null;
        }
        public EventNode GetEventNode(string id)
        {
            if (nodeLookup.ContainsKey(id))
                return nodeLookup[id] as EventNode;
            else return null;
        }
        public GraphNode GetChildNode(GraphNode node)
        {
            string childId = (node as ISingleChild).ChildID;
            if (string.IsNullOrEmpty(childId))
                return null;
            if (nodeLookup.ContainsKey(childId))
                return nodeLookup[childId];
            else return null;
        }
        public IEnumerable<GraphNode> GetAllParents(GraphNode node)
        {
            string id = node.UniqueID;
            foreach (GraphNode checkNode in GetAllGraphNodes())
            {
                if (checkNode is IChoiceContainer choiceParent)
                {
                    if (choiceParent.HasChild(id))
                        yield return checkNode;
                }
                else if (checkNode is IMultiChild multiParent)
                {
                    if (multiParent.HasChild(id))
                        yield return checkNode;
                }
                else if (checkNode is ISingleChild singleParent)
                {
                    if (singleParent.ChildID == id)
                        yield return checkNode;
                }
            }
        }
        #endregion

        #region Node Validation
        public bool HasValidChild(GraphNode node)
        {
            return node switch
            {
                ISingleChild n => n.HasChild(),
                IMultiChild n => n.HasChild(),
                IChoiceContainer n => n.HasChild(),
                _ => false
            };
        }
        public bool HasValidParent(GraphNode node)
        {
            if (node is DialogueEntryNode)
                return false;

            string id = node.UniqueID;
            if (string.IsNullOrEmpty(id))
                return false;

            foreach (GraphNode parent in GetAllGraphNodes())
            {
                if (node == parent)
                    continue;

                if (parent is IChoiceContainer choiceNode)
                {
                    if (choiceNode.GetAllChildren().Contains(id))
                        return true;
                }
                else if (parent is IMultiChild multiParent)
                {
                    foreach ( var multiChildNode in multiParent.GetChildren())
                    {
                        if (multiChildNode == id)
                            return true;
                    }
                }
                else if (parent is ISingleChild singleParent)
                {
                    if (singleParent.ChildID == id)
                        return true;
                }
            }
            return false;
        }
        #endregion

#if UNITY_EDITOR
        protected override void SetupRootNode()
        {
            if (entryNode == null)
            {
                entryNode = CreateInstance<DialogueEntryNode>();
                SetupNewNode(entryNode);
                entryNode.name = "EntryNode";
            }
        }

        #region Node Creation
        public TextNode CreateDialogueNode()
        {
            TextNode newNode = CreateInstance<TextNode>();
            Undo.RegisterCreatedObjectUndo(newNode, "New Node Created");
            SetupNewNode(newNode);
            return newNode;
        }
        public TextNode CreateDialogueNode(GraphNode parent)
        {
            TextNode newNode = CreateDialogueNode();
            AddChildToParent(parent, newNode.UniqueID);
            return newNode;
        }

        public ChoiceNode CreateChoiceNode()
        {
            ChoiceNode newNode = CreateInstance<ChoiceNode>();
            Undo.RegisterCreatedObjectUndo(newNode, "Created Choice node");
            SetupNewNode(newNode);
            return newNode;
        }
        public ChoiceNode CreateChoiceNode(GraphNode parent)
        {
            ChoiceNode newNode = CreateChoiceNode();
            AddChildToParent(parent, newNode.UniqueID);
            return newNode;
        }
        #endregion
#endif
    }
}
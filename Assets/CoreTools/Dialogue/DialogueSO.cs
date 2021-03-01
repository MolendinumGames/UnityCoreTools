using CoreTools.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
    public class DialogueSO : ScriptableObject, ISerializationCallbackReceiver
    {
        List<DialogueNode> nodes = new List<DialogueNode>();

        private Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        private void OnValidate()
        {
            // will not get called in final build!
            PopulateNodeLookup();
        }

        private void PopulateNodeLookup()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in GetNodes())
            {
                nodeLookup[node.uniqueID] = node;
            }
        }

        public IEnumerable<DialogueNode> GetNodes() => nodes;

        private void TryGenerateRootNode()
        {
            if (nodes.Count == 0)
                CreateNode();
        }
        public DialogueNode GetRootNode() => nodes[0];
        public DialogueNode GetNodeById(string id)
        {
            if (nodeLookup.ContainsKey(id))
                return nodeLookup[id];
            else return null;
        }
        public DialogueNode GetChildOfNode(DialogueNode parent)
        {
            if (!string.IsNullOrEmpty(parent.ChildID))
                return GetNodeById(parent.ChildID);
            else return null;
        }
        public bool NodeHasLegalChild(DialogueNode node)
        {
            if (!string.IsNullOrEmpty(node.ChildID))
            {
                DialogueNode child = GetNodeById(node.ChildID);
                return child != null;
            }
            else return false;
        }
        public IEnumerable<DialogueNode> GetParentNodes(DialogueNode node)
        {
            foreach (DialogueNode dialogueNode in GetNodes())
            {
                if (dialogueNode.ChildID == node.uniqueID)
                    yield return dialogueNode;
            }
        }
        public bool NodeHasParent(DialogueNode node)
        {
            foreach (DialogueNode dialogueNode in GetParentNodes(node))
            {
                if (dialogueNode != null)
                    return true;
            }
            return false;
        }
        public DialogueNode CreateNode()
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.uniqueID = Guid.NewGuid().ToString();
            newNode.name = newNode.uniqueID;
            nodes.Add(newNode);
            OnValidate();
            return newNode;
        }
        public DialogueNode CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateNode();
            if (parent != null)
                parent.ChildID = newNode.uniqueID;
            return newNode;
        }
        public void RemoveNode(DialogueNode node)
        {
            string removeId = node.uniqueID;
            foreach (DialogueNode checkNode in GetNodes())
            {
                if (checkNode.ChildID == removeId)
                    checkNode.ChildID = null;
            }
            nodes.Remove(node);
            OnValidate();
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            TryGenerateRootNode();
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {

        }
    }
}
using CoreTools.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        [SerializeField]
        List<DialogueNode> nodes = new List<DialogueNode>();

        private Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        private void Awake()
        {
#if UNITY_EDITOR
            TryGenerateRootNode();
#endif
            PopulateNodeLookup();
        }

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
#if UNITY_EDITOR
            if (nodes.Count == 0)
                nodes.Add(new DialogueNode());
#endif
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
    }
}
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
        [SerializeField]
        List<GraphNode> allNodes = new List<GraphNode>();

        //[SerializeField]
        //List<DialogueNode> dialogueNodes = new List<DialogueNode>();

        //[SerializeField]
        //List<EventNode> eventNodes = new List<EventNode>();

        [SerializeField]
        Dictionary<string, GraphNode> nodeLookup = new Dictionary<string, GraphNode>();

        [SerializeField]
        EntryNode entryNode;

        private void OnValidate()
        {
            // will not get called in final build!
            PopulateNodeLookup();
        }

        private void PopulateNodeLookup()
        {
            nodeLookup.Clear();
            foreach (GraphNode node in GetAllGraphNodes())
            {
                nodeLookup[node.UniqueID] = node;
            }
        }

        #region GetNodes Methodes
        public IEnumerable<GraphNode> GetAllGraphNodes() => allNodes;
        public GraphNode GetAnyGraphNode(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            if (nodeLookup.ContainsKey(id))
                return nodeLookup[id];
            else return null;
        }
        public EntryNode GetEntryNode() => entryNode;
        public DialogueNode GetDialogueNode(string id)
        {
            if (nodeLookup.ContainsKey(id))
                return nodeLookup[id] as DialogueNode;
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
            if (string.IsNullOrEmpty(node.ChildID))
                return null;
            if (nodeLookup.ContainsKey(node.ChildID))
                return nodeLookup[node.ChildID];
            else return null;
        }
        public IEnumerable<GraphNode> GetAllParent(GraphNode node)
        {
            string id = node.UniqueID;
            foreach (GraphNode parent in GetAllGraphNodes())
            {
                if (parent.ChildID.Equals(id))
                    yield return parent;
            }
        }
        #endregion

        #region Node Validation
        public bool HasValidChild(GraphNode node)
        {
            GraphNode child = GetChildNode(node);
            return child != null;
        }
        public bool HasValidParent(GraphNode node)
        {
            if (node is EntryNode)
                return false;

            string id = node.UniqueID;
            if (string.IsNullOrEmpty(id))
                return false;

            foreach (GraphNode parent in GetAllGraphNodes())
            {
                if (parent.ChildID == id)
                    return true;
            }
            return false;
        }
        #endregion

        #region Editor functionality
#if UNITY_EDITOR
        private void SetupRootNode()
        {
            if (entryNode == null)
            {
                entryNode = CreateInstance<EntryNode>();
                entryNode.name = "EntryNode";
                entryNode.UniqueID = Guid.NewGuid().ToString();
                allNodes.Add(entryNode);
                PopulateNodeLookup();
            }
        }
        public DialogueNode CreateDialogueNode()
        {
            Undo.RecordObject(this, "Added Dialogue node");
            DialogueNode newNode = CreateInstance<DialogueNode>();
            SetupNewNode(newNode);
            return newNode;
        }
        public DialogueNode CreateDialogueNode(GraphNode parent)
        {
            DialogueNode newNode = CreateDialogueNode();
            if (parent != null)
                parent.ChildID = newNode.UniqueID;
            return newNode;
        }

        public VoidEventNode CreateVoidEventNode()
        {
            Undo.RecordObject(this, "Created Void Event Node");
            VoidEventNode newNode = CreateInstance<VoidEventNode>();
            SetupNewNode(newNode);
            return newNode;
            
        }
        public VoidEventNode CreateVoidEventNode(GraphNode parent)
        {
            VoidEventNode newNode = CreateVoidEventNode();
            if (parent != null)
                parent.ChildID = newNode.UniqueID;
            return newNode;
        }

        public BoolEventNode CreateBoolEventNode()
        {
            Undo.RecordObject(this, "Created Bool Event Node");
            BoolEventNode newNode = CreateInstance<BoolEventNode>();
            SetupNewNode(newNode);
            return newNode;

        }
        public BoolEventNode CreateBoolEventNode(GraphNode parent)
        {
            BoolEventNode newNode = CreateBoolEventNode();
            if (parent != null)
                parent.ChildID = newNode.UniqueID;
            return newNode;
        }

        public IntEventNode CreateIntEventNode()
        {
            Undo.RecordObject(this, "Created Int Event Node");
            IntEventNode newNode = CreateInstance<IntEventNode>();
            SetupNewNode(newNode);
            return newNode;

        }
        public IntEventNode CreateIntEventNode(GraphNode parent)
        {
            IntEventNode newNode = CreateIntEventNode();
            if (parent != null)
                parent.ChildID = newNode.UniqueID;
            return newNode;
        }

        public FloatEventNode CreateFloatEventNode()
        {
            Undo.RecordObject(this, "Created Float Event Node");
            FloatEventNode newNode = CreateInstance<FloatEventNode>();
            SetupNewNode(newNode);
            return newNode;

        }
        public FloatEventNode CreateFloatEventNode(GraphNode parent)
        {
            FloatEventNode newNode = CreateFloatEventNode();
            if (parent != null)
                parent.ChildID = newNode.UniqueID;
            return newNode;
        }

        public StringEventNode CreateStringEventNode()
        {
            Undo.RecordObject(this, "Created String Event Node");
            StringEventNode newNode = CreateInstance<StringEventNode>();
            SetupNewNode(newNode);
            return newNode;

        }
        public StringEventNode CreateStringEventNode(GraphNode parent)
        {
            StringEventNode newNode = CreateStringEventNode();
            if (parent != null)
                parent.ChildID = newNode.UniqueID;
            return newNode;
        }


        public void RemoveNode(GraphNode node)
        {
            string removeId = node.UniqueID;
            allNodes.Remove(node);
            nodeLookup.Remove(removeId);
            Undo.DestroyObjectImmediate(node);
            ClearFromAllChildren(removeId);
            PopulateNodeLookup();
        }
        private void ClearFromAllChildren(string id)
        {
            foreach (GraphNode node in GetAllGraphNodes())
            {
                if (node.ChildID == id)
                    node.ChildID = null;
            }
        }
        private void SetupNewNode(GraphNode node)
        {
            node.UniqueID = Guid.NewGuid().ToString();
            node.name = node.UniqueID;
            allNodes.Add(node);
            PopulateNodeLookup();
        }
#endif
        #endregion

        #region Serialization
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            SetupRootNode();
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (GraphNode node in GetAllGraphNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
            //EditorUtility.SetDirty(this);
#endif
        }

        public void OnAfterDeserialize()
        {

        }
        #endregion
    }
}
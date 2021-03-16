using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;
using System.IO;
using System;

namespace CoreTools.NodeSystem
{
    public abstract class NodeHolder : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        protected List<GraphNode> allNodes = new();

        [SerializeField]
        protected Dictionary<string, GraphNode> nodeLookup = new();

        [SerializeField]
        protected GraphNode entryNode;

        protected void OnValidate()
        {
            // will not get called in final build!
            this.name = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this));
            PopulateNodeLookup();
        }
        protected void PopulateNodeLookup()
        {
            nodeLookup.Clear();
            foreach (GraphNode node in GetAllGraphNodes())
            {
                nodeLookup[node.UniqueID] = node;
            }
        }
        public GraphNode GetEntryNode() => entryNode;
        public IEnumerable<GraphNode> GetAllGraphNodes() => allNodes;
        public GraphNode GetAnyGraphNode(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            if (nodeLookup.ContainsKey(id))
                return nodeLookup[id];
            else return null;
        }
        public void RemoveNode(GraphNode node)
        {
            Undo.RecordObject(this, "Removed Node");
            string removeId = node.UniqueID;
            allNodes.Remove(node);
            nodeLookup.Remove(removeId);
            ClearFromAllChildren(removeId);
            Undo.DestroyObjectImmediate(node);
            PopulateNodeLookup();
            EditorUtility.SetDirty(this);
        }
        protected void ClearFromAllChildren(string id)
        {
            foreach (GraphNode node in GetAllGraphNodes())
            {
                if (node is ISingleChild singleParent)
                {
                    if (singleParent.ChildID == id)
                        singleParent.ChildID = null;
                }
                else if (node is IMultiChild multiParent)
                {
                    if (multiParent.HasChild(id))
                        multiParent.ClearChild(id);
                }


                //ISingleChild parent = (ISingleChild)node;
                //if (node is ChoiceNode choiceNode)
                //    choiceNode.ClearIdFromChoices(id);
                //else if (parent.ChildID == id)
                //    parent.ChildID = null;
            }
        }
        protected abstract void SetupRootNode();
        protected void SetupNewNode(GraphNode node)
        {
            node.UniqueID = Guid.NewGuid().ToString();
            node.name = node.UniqueID;

            Undo.RecordObject(this, "Created Node");

            allNodes.Add(node);
            PopulateNodeLookup();
            EditorUtility.SetDirty(this);
        }
        protected void AddChildToParent(GraphNode parent, string child)
        {
            if (parent != null && !string.IsNullOrWhiteSpace(child))
            {
                if (parent is ISingleChild singleParent)
                {
                    singleParent.ChildID = parent.UniqueID;
                }

                if (parent is IMultiChild multiParent)
                {
                    multiParent.AddChild(parent.UniqueID);
                }
            }
        }

        #region Create Event Nodes

        public VoidEventNode CreateVoidEventNode()
        {
            VoidEventNode newNode = CreateInstance<VoidEventNode>();
            Undo.RegisterCreatedObjectUndo(newNode, "Created New Void Node");
            SetupNewNode(newNode);
            return newNode;

        }
        public VoidEventNode CreateVoidEventNode(GraphNode parent)
        {
            VoidEventNode newNode = CreateVoidEventNode();
            AddChildToParent(parent, newNode.UniqueID);
            return newNode;
        }

        public BoolEventNode CreateBoolEventNode()
        {
            BoolEventNode newNode = CreateInstance<BoolEventNode>();
            Undo.RegisterCreatedObjectUndo(newNode, "Created New Int Node");
            SetupNewNode(newNode);
            return newNode;

        }
        public BoolEventNode CreateBoolEventNode(GraphNode parent)
        {
            BoolEventNode newNode = CreateBoolEventNode();
            AddChildToParent(parent, newNode.UniqueID);
            return newNode;
        }

        public IntEventNode CreateIntEventNode()
        {
            IntEventNode newNode = CreateInstance<IntEventNode>();
            Undo.RegisterCreatedObjectUndo(newNode, "Created New Int Node");
            SetupNewNode(newNode);
            return newNode;

        }
        public IntEventNode CreateIntEventNode(GraphNode parent)
        {
            IntEventNode newNode = CreateIntEventNode();
            AddChildToParent(parent, newNode.UniqueID);
            return newNode;
        }

        public FloatEventNode CreateFloatEventNode()
        {
            FloatEventNode newNode = CreateInstance<FloatEventNode>();
            Undo.RegisterCreatedObjectUndo(newNode, "Created New Float Node");
            SetupNewNode(newNode);
            return newNode;

        }
        public FloatEventNode CreateFloatEventNode(GraphNode parent)
        {
            FloatEventNode newNode = CreateFloatEventNode();
            AddChildToParent(parent, newNode.UniqueID);
            return newNode;
        }

        public StringEventNode CreateStringEventNode()
        {
            StringEventNode newNode = CreateInstance<StringEventNode>();
            Undo.RegisterCreatedObjectUndo(newNode, "Created New String Node");
            SetupNewNode(newNode);
            return newNode;

        }
        public StringEventNode CreateStringEventNode(GraphNode parent)
        {
            StringEventNode newNode = CreateStringEventNode();
            AddChildToParent(parent, newNode.UniqueID);
            return newNode;
        }

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
#endif
        }

        public void OnAfterDeserialize()
        {
            // Do nothing
        }
        #endregion
    }
}
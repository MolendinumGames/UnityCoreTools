using CoreTools.Dialogue;
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
    public class DialogueSO : NodeHolder/*, ISerializationCallbackReceiver*/
    {
        //[SerializeField]
        //List<GraphNode> allNodes = new List<GraphNode>();

        //[SerializeField]
        //Dictionary<string, GraphNode> nodeLookup = new Dictionary<string, GraphNode>();

        //[SerializeField]
        //DialogueEntryNode entryNode;

        //private void OnValidate()
        //{
        //    // will not get called in final build!
        //    this.name = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this));
        //    PopulateNodeLookup();
        //}

        //private void PopulateNodeLookup()
        //{
        //    nodeLookup.Clear();
        //    foreach (GraphNode node in GetAllGraphNodes())
        //    {
        //        nodeLookup[node.UniqueID] = node;
        //    }
        //}

        public DialogueNode GetStartNode()
        {
            GraphNode currentNode = GetEntryNode();
            return GetNextDialogue(currentNode);
        }
        public DialogueNode Next(string currentId)
        {
            GraphNode currentNode = GetAnyGraphNode(currentId);
            if (currentNode != null)
            {
                if (currentNode is ChoiceNode)
                {
                    Debug.LogWarning($"Next called on choice node without index being passed in! Dialogue: {this.name}. NodeID{currentId}.");
                    return Next(currentId, 0);
                }
                else return GetNextDialogue(currentNode);
            }
            else
            {
                Debug.LogWarning($"Cannot current node. Dialogue: {this.name}. CurrentID: {currentId}. Returning null.");
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
                else return null;
            }
            else
            {
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
                    return null;
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
        //public IEnumerable<GraphNode> GetAllGraphNodes() => allNodes;
        //public GraphNode GetAnyGraphNode(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return null;
        //    if (nodeLookup.ContainsKey(id))
        //        return nodeLookup[id];
        //    else return null;
        //}
        //public DialogueEntryNode GetEntryNode() => entryNode;
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
                ISingleChild parent = (ISingleChild)checkNode;
                if (checkNode is ChoiceNode choiceNode)
                {
                    if (choiceNode.GetAllChildren().Contains(node.UniqueID))
                        yield return choiceNode;
                }
                if (parent.ChildID.Equals(id))
                    yield return checkNode;
            }
        }
        #endregion

        #region Node Validation
        public bool HasValidChild(GraphNode node)
        {
            if (node is ChoiceNode)
                return false; //

            GraphNode child = GetChildNode(node);
            return child != null;
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
                if (parent is ChoiceNode choiceNode)
                {
                    if (choiceNode.GetAllChildren().Contains(id))
                        return true;
                }
                else if (((ISingleChild)parent).ChildID == id)
                    return true;
            }
            return false;
        }
        #endregion

        #region Editor functionality
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

        public DialogueNode CreateDialogueNode()
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            Undo.RegisterCreatedObjectUndo(newNode, "New Node Created");
            SetupNewNode(newNode);
            return newNode;
        }
        public DialogueNode CreateDialogueNode(GraphNode parent)
        {
            DialogueNode newNode = CreateDialogueNode();
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

        //public VoidEventNode CreateVoidEventNode()
        //{
        //    VoidEventNode newNode = CreateInstance<VoidEventNode>();
        //    Undo.RegisterCreatedObjectUndo(newNode, "Created New Void Node");
        //    SetupNewNode(newNode);
        //    return newNode;
            
        //}
        //public VoidEventNode CreateVoidEventNode(GraphNode parent)
        //{
        //    VoidEventNode newNode = CreateVoidEventNode();
        //    if (parent != null)
        //        ((ISingleChild)parent).ChildID = newNode.UniqueID;
        //    return newNode;
        //}

        //public BoolEventNode CreateBoolEventNode()
        //{
        //    BoolEventNode newNode = CreateInstance<BoolEventNode>();
        //    Undo.RegisterCreatedObjectUndo(newNode, "Created New Int Node");
        //    SetupNewNode(newNode);
        //    return newNode;

        //}
        //public BoolEventNode CreateBoolEventNode(GraphNode parent)
        //{
        //    BoolEventNode newNode = CreateBoolEventNode();
        //    if (parent != null)
        //        ((ISingleChild)parent).ChildID = newNode.UniqueID;
        //    return newNode;
        //}

        //public IntEventNode CreateIntEventNode()
        //{
        //    IntEventNode newNode = CreateInstance<IntEventNode>();
        //    Undo.RegisterCreatedObjectUndo(newNode, "Created New Int Node");
        //    SetupNewNode(newNode);
        //    return newNode;

        //}
        //public IntEventNode CreateIntEventNode(GraphNode parent)
        //{
        //    IntEventNode newNode = CreateIntEventNode();
        //    if (parent != null)
        //        ((ISingleChild)parent).ChildID = newNode.UniqueID;
        //    return newNode;
        //}

        //public FloatEventNode CreateFloatEventNode()
        //{
        //    FloatEventNode newNode = CreateInstance<FloatEventNode>();
        //    Undo.RegisterCreatedObjectUndo(newNode, "Created New Float Node");
        //    SetupNewNode(newNode);
        //    return newNode;

        //}
        //public FloatEventNode CreateFloatEventNode(GraphNode parent)
        //{
        //    FloatEventNode newNode = CreateFloatEventNode();
        //    if (parent != null)
        //        ((ISingleChild)parent).ChildID = newNode.UniqueID;
        //    return newNode;
        //}

        //public StringEventNode CreateStringEventNode()
        //{
        //    StringEventNode newNode = CreateInstance<StringEventNode>();
        //    Undo.RegisterCreatedObjectUndo(newNode, "Created New String Node");
        //    SetupNewNode(newNode);
        //    return newNode;

        //}
        //public StringEventNode CreateStringEventNode(GraphNode parent)
        //{
        //    StringEventNode newNode = CreateStringEventNode();
        //    if (parent != null)
        //        ((ISingleChild)parent).ChildID = newNode.UniqueID;
        //    return newNode;
        //}


        //public void RemoveNode(GraphNode node)
        //{
           
        //    Undo.RecordObject(this, "Removed Node");
        //    string removeId = node.UniqueID;
        //    allNodes.Remove(node);
        //    nodeLookup.Remove(removeId);
        //    ClearFromAllChildren(removeId);
        //    Undo.DestroyObjectImmediate(node);
        //    PopulateNodeLookup();
        //    EditorUtility.SetDirty(this);
        //}
        //private void ClearFromAllChildren(string id)
        //{
        //    foreach (GraphNode node in GetAllGraphNodes())
        //    {
        //        ISingleChild parent = (ISingleChild)node;
        //        if (node is ChoiceNode choiceNode)
        //            choiceNode.ClearIdFromChoices(id);
        //        else if (parent.ChildID == id)
        //            parent.ChildID = null;
        //    }
        //}
        //private void SetupNewNode(GraphNode node)
        //{
        //    node.UniqueID = Guid.NewGuid().ToString();
        //    node.name = node.UniqueID;

        //    Undo.RecordObject(this, "Created Node");

        //    allNodes.Add(node);
        //    PopulateNodeLookup();
        //    EditorUtility.SetDirty(this);
        //}
#endif
        #endregion

//        #region Serialization
//        public void OnBeforeSerialize()
//        {
//#if UNITY_EDITOR
//            SetupRootNode();
//            if (AssetDatabase.GetAssetPath(this) != "")
//            {
//                foreach (GraphNode node in GetAllGraphNodes())
//                {
//                    if (AssetDatabase.GetAssetPath(node) == "")
//                    {
//                        AssetDatabase.AddObjectToAsset(node, this);
//                    }
//                }
//            }
//#endif
//        }

//        public void OnAfterDeserialize()
//        {

//        }
//        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;

namespace CoreTools.Dialogue
{
    public class VoidEventNode : EventNode
    {
        [SerializeField]
        VoidChannelSO channel;
        public VoidChannelSO Channel
        {
            get => channel;
#if UNITY_EDITOR
            set
            {
                Undo.RecordObject(this, "Changed VoidChannel EventNode");
                channel = value;
                EditorUtility.SetDirty(this);
            }
#endif
        }

        public override void Raise()
        {
            if (channel != null)
                channel.Raise();
            else
                Debug.LogError($"Empty VoidChannel for node: {this.name}");
        }
#if UNITY_EDITOR
        public override void Reset()
        {
            Undo.RecordObject(this, "Reset Void Node");
            ChildID = null;
            channel = null;
        }
#endif
    }
}
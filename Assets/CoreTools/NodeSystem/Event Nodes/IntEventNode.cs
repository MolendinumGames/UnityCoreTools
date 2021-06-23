using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTools;
using UnityEditor;

namespace CoreTools.NodeSystem
{
    public class IntEventNode : EventNode
    {
        [SerializeField]
        IntChannel channel;
        public IntChannel Channel
        {
            get => channel;
#if UNITY_EDITOR
            set
            {
                if (channel != value)
                {
                    Undo.RecordObject(this, "Changed IntChannel EventNode");
                    channel = value;
                    EditorUtility.SetDirty(this);
                }
            }
#endif
        }

        [SerializeField]
        int value = 0;
        public int Value
        {
            get => value;
#if UNITY_EDITOR
            set
            {
                if (this.value != value)
                {
                    Undo.RecordObject(this, "Changed IntChannel EventNode Value");
                    this.value = value;
                }
            }
#endif
        }

        public override void Raise()
        {
            if (channel != null)
                channel.Raise(Value);
            else
                Debug.LogError($"Empty IntChannel for node: {this.name}");
        }
#if UNITY_EDITOR
        public override void Reset()
        {
            Undo.RecordObject(this, "Reset Int Event Node");
            ChildID = null;
            channel = null;
            value = 0;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
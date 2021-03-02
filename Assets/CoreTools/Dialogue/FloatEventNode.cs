using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;

namespace CoreTools.Dialogue
{
    public class FloatEventNode : EventNode
    {
        [SerializeField]
        FloatChannelSO channel;
        public FloatChannelSO Channel
        {
            get => channel;
#if UNITY_EDITOR
            set
            {
                Undo.RecordObject(this, "Changed FloatChannel EventNode");
                channel = value;
                EditorUtility.SetDirty(this);
            }
#endif
        }

        [SerializeField]
        float value = 0f;
        public float Value
        {
            get => value;
#if UNITY_EDITOR
            set
            {
                Undo.RecordObject(this, "Changed FloatChannel EventNode Value");
                this.value = value;
            }
#endif
        }

        public override void Raise()
        {
            if (channel != null)
                channel.Raise(Value);
            else
                Debug.LogError($"Empty FloatChannel for node: {this.name}");
        }
    }
}
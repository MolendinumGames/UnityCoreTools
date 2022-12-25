/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools.NodeSystem;

namespace CoreTools.DialogueSystem
{
    public class DialogueEventNode : EventNode
    {
        [SerializeField]
        DialogueChannel channel;
        public DialogueChannel Channel
        {
            get => channel;
#if UNITY_EDITOR
            set
            {
                if (channel != value)
                {
                    Undo.RecordObject(this, "Changed DialogueChannel EventNode");
                    channel = value;
                    EditorUtility.SetDirty(this);
                }
            }
#endif
        }

        [SerializeField]
        Dialogue value = null;
        public Dialogue Value
        {
            get => value;
#if UNITY_EDITOR
            set
            {
                if (this.value != value)
                {
                    Undo.RecordObject(this, "Changed DialogueChannel EventNode Value");
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
                Debug.LogError($"Empty FloatChannel for node: {this.name}");
        }
#if UNITY_EDITOR
        public override void Reset()
        {
            Undo.RecordObject(this, "Reset Float Event Node");
            ChildID = null;
            channel = null;
            value = null;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
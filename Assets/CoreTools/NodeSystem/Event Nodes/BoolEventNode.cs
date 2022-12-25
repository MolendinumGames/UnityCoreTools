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
using CoreTools;

namespace CoreTools.NodeSystem
{
    public class BoolEventNode : EventNode
    {
        [SerializeField]
        BoolChannel channel;
        public BoolChannel Channel
        {
            get => channel;
#if UNITY_EDITOR
            set
            {
                if (channel != value)
                {
                    Undo.RecordObject(this, "Changed BoolChannel EventNode");
                    channel = value;
                    EditorUtility.SetDirty(this);
                }

            }
#endif
        }

        [SerializeField]
        bool value = true;
        public bool Value
        {
            get => value;
#if UNITY_EDITOR
            set
            {
                if (this.value != value)
                {
                    Undo.RecordObject(this, "Changed BoolChannel EventNode Value");
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
                Debug.LogError($"Empty BoolChannel for node: {name}");
        }
#if UNITY_EDITOR
        public override void Reset()
        {
            Undo.RecordObject(this, "Reset Bool Event Node");
            ChildID = null;
            channel = null;
            value = true;
#endif
        }
    }
}
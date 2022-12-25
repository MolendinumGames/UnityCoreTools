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
    public class VoidEventNode : EventNode
    {
        [SerializeField]
        VoidChannel channel;
        public VoidChannel Channel
        {
            get => channel;
#if UNITY_EDITOR
            set
            {
                if (channel != value)
                {
                    Undo.RecordObject(this, "Changed VoidChannel EventNode");
                    channel = value;
                    EditorUtility.SetDirty(this);
                }
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
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
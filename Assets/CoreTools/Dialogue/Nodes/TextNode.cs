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
    public class TextNode : DialogueNode, ISingleChild
    {

        [SerializeField]
        private string childID;
        public virtual string ChildID
        {
            get => childID;
#if UNITY_EDITOR
            set
            {
                if (childID != value)
                {
                    Undo.RecordObject(this, "Changed Dialogue Node childID");
                    childID = value;
                    EditorUtility.SetDirty(this);
                }
            }
#endif
        }
        public virtual bool HasChild() => !string.IsNullOrWhiteSpace(childID);
        public virtual void ClearChild()
        {
            ChildID = null;
        }
        public virtual void ClearChild(string id)
        {
            if (ChildID == id)
                ClearChild();
        }
        protected override void OnReset()
        {
            childID = null;
        }
    }
}
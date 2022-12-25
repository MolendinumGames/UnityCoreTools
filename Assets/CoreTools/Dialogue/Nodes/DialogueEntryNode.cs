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

namespace CoreTools.NodeSystem
{
    public class DialogueEntryNode : GraphNode, ISingleChild
    {
        public override bool IsEntry => true;

        Rect rect = new Rect(10f, 10f, 100f, 80f);
        public override Rect NodeRect { get => rect; set => rect = value; }
        public override Rect GetBaseRect() => rect;

        [SerializeField]
        string childID;
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

        public bool HasChild() => !string.IsNullOrWhiteSpace(childID);

#if UNITY_EDITOR
        public override void Reset()
        {
            ChildID = null;
        }

        public void ClearChild()
        {
            ChildID = null;
        }

        public void ClearChild(string id)
        {
            if (ChildID == id)
                ClearChild();
        }
#endif
    }
}
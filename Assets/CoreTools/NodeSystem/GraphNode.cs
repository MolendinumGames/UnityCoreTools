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
    public abstract class GraphNode : ScriptableObject
    {
        [SerializeField]
        private string uniqueID = "newId";
        public string UniqueID
        {
            get => uniqueID;
            set
            {
                if (uniqueID != value)
                {
                    Undo.RecordObject(this, "Changed Node ID");
                    uniqueID = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        public abstract bool IsEntry { get; }

#if UNITY_EDITOR
        public abstract Rect NodeRect { get; set; }

        public virtual void SetPosition(Vector2 newPos)
        {
            Undo.RecordObject(this, "Set Node Position");
            newPos.x = Mathf.Clamp(newPos.x, 0f, Mathf.Infinity);
            newPos.y = Mathf.Clamp(newPos.y, 0f, Mathf.Infinity);
            NodeRect = new Rect(newPos, NodeRect.size);
            EditorUtility.SetDirty(this);
        }
        public abstract Rect GetBaseRect();
        public abstract void Reset();
#endif
    }
}
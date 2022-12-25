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
    public abstract class EventNode : GraphNode, ISingleChild, IMultiChild
    {
        public abstract void Raise();

        public override bool IsEntry => false;

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

        [SerializeField]
        private List<string> children = new List<string>();
        public IEnumerable<string> GetChildren() => children;
        public int ChildAmount => children.Count;

        public bool HasChild() => !string.IsNullOrWhiteSpace(childID) || ChildAmount > 0;
        public bool HasChild(string id)
        {
            return children.Contains(id) || childID == id;
        }
        public void ClearChild()
        {
            Undo.RecordObject(this, "Cleared Children from Event Node");
            children.Clear();
            childID = null;
        }
        public void ClearChild(string id)
        {
            if (HasChild(id))
            {
                Undo.RecordObject(this, "Cleaer Child from Event Node");
                if (id == ChildID)
                    ChildID = null;
                if (children.Contains(id))
                    children.Remove(id);
            }
        }
        public void ClearAllChildren()
        {
            ClearChild();
        }

        public void AddChild(string id)
        {
            if (ChildID != id)
                ChildID = id;

            if (!children.Contains(id))
            {
                Undo.RecordObject(this, "Added Child to Event Node");
                children.Add(id);
            }
        }

#if UNITY_EDITOR
        [SerializeField]
        Rect rect = new Rect(10f, 10f, 250f, 95f);
        public override Rect NodeRect { get => rect; set => rect = value; }
        public override Rect GetBaseRect() => rect;
#endif
    }
}
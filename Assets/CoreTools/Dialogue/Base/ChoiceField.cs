/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using UnityEngine;
using CoreTools.NodeSystem;

namespace CoreTools.DialogueSystem
{
    [System.Serializable]
    public class ChoiceField : ISingleChild
    {
        // (!) Undo is handled in choice node

        public string text;

        [SerializeField]
        private string childId;
        public virtual string ChildID
        {
            get => childId;
#if UNITY_EDITOR
            set
            {
                if (childId != value)
                {
                    childId = value;
                }
            }
#endif
        }

#if UNITY_EDITOR
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

        public bool HasChild() =>
            !string.IsNullOrWhiteSpace(childId);
    }
}
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

namespace CoreTools.NodeSystem
{
    public interface IChoiceContainer
    {
        public void SetChildOfChoice(int id, string child);
        public string GetChildOfChoice(int id);
        public void ClearChild(string child);
        public List<string> GetAllChildren();
        public bool HasChild();
        public bool HasChild(string child);
        
        public abstract int ChoiceAmount { get; }

#if UNITY_EDITOR
        public float GetChoiceHeight();
#endif

    }
}
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
    public interface ISingleChild
    {
        public string ChildID
        { 
            get;
            #if UNITY_EDITOR 
            set; 
            #endif 
        }

        public bool HasChild();
        public void ClearChild();
        public void ClearChild(string id);
    }
}
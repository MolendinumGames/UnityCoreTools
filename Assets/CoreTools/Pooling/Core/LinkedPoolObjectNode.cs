/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CoreTools.Pooling
{
    [System.Serializable]
    public class LinkedPoolObjectNode
    {
        public LinkedPoolObjectNode(GameObject pooledObject, LinkedPoolObjectNode previous, LinkedPoolObjectNode next)
        {
            this.PooledObject = pooledObject;
            this.Next = next;
            this.Previous = previous;
        }

        public LinkedPoolObjectNode(GameObject pooledObject) : this(pooledObject, null, null)
        { }

        public LinkedPoolObjectNode() : this(null, null, null)
        { }


        public GameObject PooledObject { get; set; }

        public LinkedPoolObjectNode Previous { get; set; }

        public LinkedPoolObjectNode Next { get; set; }
    }
}

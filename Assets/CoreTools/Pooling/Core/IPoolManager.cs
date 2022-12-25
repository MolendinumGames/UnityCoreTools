/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.Pooling
{
    public interface IPoolManager
    {
        public Dictionary<string, GameObjectPool> PoolLookup { get; set; }

        /// <summary>
        /// Get a pooled GameObject. Can return null if pool settings limits reached. 
        /// </summary>
        /// <param name="key">Unique pool key as specified in the GlobalPoolManager</param>
        public GameObject RequestObject(string key);

        /// <summary>
        /// Get a reference to a GameObject pool.
        /// </summary>
        /// <param name="key">Unique pool key as specified in the GlobalPoolManager</param>
        public GameObjectPool GetPool(string key);

#if UNITY_EDITOR
        public void AddPool();
#endif
    }
}
/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using CoreTools.Pooling;
using UnityEngine;

namespace CoreTools.Pooling
{
    [System.Serializable]
    public class PoolDataSet
    {
        [SerializeField] string key;
        public string Key { get => key; }

        [SerializeField] bool populateOnAwake;
        public bool PopulateOnAwake { get => populateOnAwake; }

        public GameObjectPool Pool;

        public PoolDataSet()
        {
            key = "";
            populateOnAwake = true;
            Pool = new GameObjectPool(null, 10, 999, false);
        }
    }
}
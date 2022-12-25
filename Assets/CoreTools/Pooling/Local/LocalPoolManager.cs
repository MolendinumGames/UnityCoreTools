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

namespace CoreTools.Pooling
{
    public class LocalPoolManager : MonoBehaviour, IPoolManager
    {
        [SerializeField]
        List<PoolDataSet> localPools = new List<PoolDataSet>();

        public Dictionary<string, GameObjectPool> PoolLookup { get; set; } = new();

        private void Awake()
        {
            foreach (var poolData in localPools)
            {
                // TODO check pool validation, only init if pool is checked for create on start
                if (poolData.PopulateOnAwake)
                {
                    poolData.Pool.SetParentTransform(gameObject.transform);
                    poolData.Pool.Initialize();
                }
                PoolLookup.Add(poolData.Key, poolData.Pool);
            }
        }

        public GameObject RequestObject(string key)
        {
            if (PoolLookup.ContainsKey(key))
            {
                return PoolLookup[key].RequestObject();
            }
            else
            {
                Debug.LogWarning($"No pool with key {key} has been found.");
                return null;
            }
        }

        public GameObjectPool GetPool(string key)
        {
            if (PoolLookup.ContainsKey(key))
            {
                return PoolLookup[key];
            }
            else
            {
                Debug.LogWarning($"No pool with key {key} has been found.");
                return null;
            }
        }

#if UNITY_EDITOR
        public void AddPool()
        {
            localPools.Add(new PoolDataSet()
            {
                Pool = new GameObjectPool(null, 0, 999, false),
            });
        }
#endif

    }
}
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
using UnityEngine;
using CoreTools.Pooling;

namespace CoreTools
{
    public class LocalPoolSimple : MonoBehaviour
    {
        [SerializeField]
        bool createOnAwake = true;

        public bool CreateOnAwake
        {
            get => createOnAwake;
            set => createOnAwake = value;
        }

        [SerializeField]
        GameObjectPool pool;

        void Start()
        {
            if (CreateOnAwake)
            InitializePool();
        }

        public void InitializePool()
        {
            pool.SetParentTransform(gameObject.transform);
            pool.Initialize();
        }

        public GameObject GetPooledGameObject() =>
            pool.RequestObject();

        public GameObjectPool GetPool() => pool;
    }
}

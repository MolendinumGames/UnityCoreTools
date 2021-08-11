/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
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
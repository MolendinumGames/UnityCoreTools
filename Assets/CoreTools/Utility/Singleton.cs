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

namespace CoreTools
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool quitting = false;
        private static readonly object Lock = new object();
        protected abstract bool Persistent { get; }
        private static T instance;
        public static T Instance
        {
            get
            {
                if (quitting)
                    return null;

                lock (Lock)
                {
                    if (instance == null)
                    {
                        var instances = FindObjectsOfType(typeof(T));
                        if (instances.Length > 0)
                        {
                            instance = instances[0] as T;
                            for (int i = 1; i < instances.Length; i++)
                            {
                                Destroy(instances[i]);
                            }
                        }
                        if (instance == null)
                        {
                            instance = new GameObject($"{typeof(T)}Singleton").AddComponent<T>();
                        }
                    }
                    return instance;
                }
            }
        }
        protected virtual void Awake()
        {
            if (Persistent)
                DontDestroyOnLoad(gameObject);
        }
        private void OnApplicationQuit() => quitting = true;
    }
}


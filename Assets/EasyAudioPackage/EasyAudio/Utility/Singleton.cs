using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyAudioSystem.Core
{

    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool quitting = false;
        private static T instance;
        public static T Instance
        {
            get
            {
                if (quitting)
                    return null;

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
        private void OnApplicationQuit()
        {
            quitting = true;
        #if UNITY_EDITOR
            quitting = false;
        #endif
        }
    }
}

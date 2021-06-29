using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools.Pooling;

namespace CoreTools
{
    public class GlobalPoolManager : Singleton<GlobalPoolManager>
    {
        protected override bool Persistent => false;

#if UNITY_EDITOR
        [MenuItem("Tools/Global Pool Manager")]
        public static void GetOrCreatePoolManager()
        {
            GlobalPoolManager pool = FindObjectOfType<GlobalPoolManager>();
            if (pool == null)
            {
                pool = new GameObject("GlobalPoolManager").AddComponent(typeof(GlobalPoolManager)) as GlobalPoolManager;
            }
            Selection.activeGameObject = pool.gameObject;
        }
#endif

        Dictionary<string, GameObjectPool> poolLookup = new();

    }
}
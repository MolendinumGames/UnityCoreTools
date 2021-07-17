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

        [SerializeField]
        GlobalPoolDataSet[] globalPools = new GlobalPoolDataSet[0];

        Dictionary<string, GameObjectPool> poolLookup = new();

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/Global Pool Manager")]
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

        protected override void Awake()
        {
            base.Awake();

            foreach (var poolData in globalPools)
            {
                // TODO check pool validation, only init if pool is checked for create on start
                poolData.Pool.Initialize();
                poolLookup.Add(poolData.Key, poolData.Pool);
            }
        }

        /// <summary>
        /// Get a pooled GameObject. Can return null if pool settings limits reached. 
        /// </summary>
        /// <param name="key">Unique pool key as specified in the GlobalPoolManager</param>
        public GameObject RequestObject(string key)
        {
            if (poolLookup.ContainsKey(key))
            {
                return poolLookup[key].RequestObject();
            }
            else
            {
                Debug.LogWarning($"No pool with key {key} has been found.");
                return null;
            }
        }

        /// <summary>
        /// Get a reference to a GameObject pool.
        /// </summary>
        /// <param name="key">Unique pool key as specified in the GlobalPoolManager</param>
        public GameObjectPool GetPool(string key)
        {
            if (poolLookup.ContainsKey(key))
            {
                return poolLookup[key];
            }
            else
            {
                Debug.LogWarning($"No pool with key {key} has been found.");
                return null;
            }
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.Pooling
{
    public class LocalPoolManager : MonoBehaviour, IPoolManager
    {

        [SerializeField]
        List<GlobalPoolDataSet> globalPools = new(1);

        public Dictionary<string, GameObjectPool> PoolLookup { get; set; } = new();

        private void Awake()
        {
            foreach (var poolData in globalPools)
            {
                // TODO check pool validation, only init if pool is checked for create on start
                if (poolData.PopulateOnAwake)
                    poolData.Pool.Initialize();
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
            globalPools.Add(new GlobalPoolDataSet()
            {
                Pool = new GameObjectPool(null, 0, 999, false),
            });
        }
#endif

    }
}
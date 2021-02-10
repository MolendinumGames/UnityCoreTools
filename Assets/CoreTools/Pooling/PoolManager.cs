using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CoreTools.Pooling;

namespace CoreTools
{
    public class PoolManager : MonoBehaviour
    {
        #region Singleton
        private static PoolManager instance;
        public static PoolManager Instance
        {
            get
            {
                if (!instance)
                    instance = FindObjectOfType(typeof(PoolManager)) as PoolManager;
                if (!instance)
                    Debug.LogError("No PoolManager found!");
                return instance;
            }
        }
        #endregion

        private Transform transf;
        [SerializeField]
        private PoolItem[] poolItems;
        private Dictionary<string, List<GameObject>> allPools = new Dictionary<string, List<GameObject>>();
        private Dictionary<string, PoolItem> allPoolItems = new Dictionary<string, PoolItem>();

        [SerializeField] bool logOnCreate = true;
        [SerializeField] bool logOnNull = true;

        private void Awake()
        {
            instance = this;
            transf = transform;
            GeneratePoolItemDict();
            GeneratePoolDict();

        }

        #region Public Methods
        public GameObject RequestItem(string key)
        {
            if (!allPools.ContainsKey(key))
            {
                Debug.LogError($"The key <{key}> was not found in the pool. Returning null.");
                return null;
            }

            // Try find existing obj in list
            for (int i = 0; i < allPools[key].Count; i++)
            {
                GameObject target = allPools[key][i];
                if (target == null && logOnNull)
                {
                    Debug.LogWarning($"PoolItem with key: <{key}> at index {i} is null!");
                }
                else if (!target.activeInHierarchy)
                {
                    target.SetActive(true);
                    allPools[key].MoveToIndex(i, allPools[key].Count - 1);
                    return target;
                }
            }

            // Create a new obj, add it and return it
            if (allPools[key].Count < allPoolItems[key].maxAmount)
            {
                if (logOnCreate)
                    Debug.LogWarning($"Creating new GO with key <{key}>.");

                return CreateAndAdd(allPoolItems[key].prefab, allPools[key]);
            }

            if (allPoolItems[key].reuseOnFull)
            {
                var item = allPools[key][0];
                item.SetActive(false);
                item.SetActive(true); // trigger OnEnable
                allPools[key].MoveToIndex(0, allPools[key].Count - 1);
                return item;
            }

            if (logOnNull)
                Debug.LogWarning($"Pool with key <{key}> reached the limit and is returning null!");
            return null;
        }
        public void EmptyPool(string key)
        {
            Debug.Log($"Pool with key <{key}> was cleared. {allPools[key].Count} GameObjects destroyed.");
            for (int i = 0; i < allPools[key].Count; i++)
                if (allPools[key][i] != null)
                    Destroy(allPools[key][i]);
            allPools[key].Clear();
        }
        public void CullPoolOverhead(string key)
        {
            if (!allPools.ContainsKey(key))
            {
                Debug.LogWarning($"Pool with key <{key}> wasn't found and cannot be culled.");
                return;
            }

            int n = allPoolItems[key].startingAmount;
            var pool = allPools[key];
            while (pool.Count > n)
            {
                int index = -1;
                for (int i = 0; i < pool.Count; i++)
                {
                    if (pool[i] == null || !pool[i].activeInHierarchy)
                    {
                        index = i;
                        break;
                    }
                }
                if (index < 0)
                    break;
                Destroy(pool[index]);
                pool.RemoveAt(index);
            }

        }
        #endregion

        #region Private Methods
        private void GeneratePoolDict()
        {
            for (int i = 0; i < poolItems.Length; i++)
            {
                if (!CheckPoolItem(poolItems[i]))
                    continue;

                if (!allPools.ContainsKey(poolItems[i].key))
                    allPools.Add(poolItems[i].key, CreatePool(poolItems[i].prefab, poolItems[i].startingAmount));
                else
                    Debug.LogWarning($"Duplicate key: {poolItems[i].key}!");
            }
        }
        private void GeneratePoolItemDict()
        {
            for (int i = 0; i < poolItems.Length; i++)
            {
                if (!CheckPoolItem(poolItems[i]))
                {
                    Debug.LogWarning($"Poolitem number {i} didn't pass the check. Pool will be skipped.");
                    continue;
                }

                if (!allPoolItems.ContainsKey(poolItems[i].key))
                    allPoolItems.Add(poolItems[i].key, poolItems[i]);
                else
                    Debug.LogWarning($"Duplicate key: {poolItems[i].key}!");
            }
        }
        private List<GameObject> CreatePool(GameObject prefab, int amount)
        {
            List<GameObject> newPool = new List<GameObject>();
            for (int i = 0; i < amount; i++)
            {
                GameObject go = CreateAndAdd(prefab, newPool);
                go.SetActive(false);
            }
            return newPool;
        }
        private GameObject CreateAndAdd(GameObject prefab, List<GameObject> targetList)
        {
            GameObject go = Instantiate(prefab, transf.position, Quaternion.identity);
            go.transform.parent = transform;
            targetList.Add(go);
            return go;
        }
        private bool CheckPoolItem(PoolItem item)
        {
            bool check = string.IsNullOrWhiteSpace(item.key)
                      && item.maxAmount > 0
                      && item.prefab != null;
            if (!check)
                Debug.LogWarning($"A Poolitem <{item.key}> failed a check.");
            return check;
        }
        #endregion
    }
}


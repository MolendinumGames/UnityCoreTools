using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.Pooling
{
    public class PoolManager : Singleton<PoolManager>
    {
        protected override bool Persistent => true;

        [SerializeField]
        PoolItem[] poolData;

        Dictionary<string, List<GameObject>> activePools = new Dictionary<string, List<GameObject>>();
        Dictionary<string, PoolItem> allPoolItems = new Dictionary<string, PoolItem>();

        [Header("Debugging")]
        [SerializeField]
        bool onNewMemberCreated = true;
        [SerializeField] 
        bool onNullMember = true;
        [SerializeField]
        bool onPoolLoaded = false;
        [SerializeField]
        bool onPoolUnloaded = true;


        protected override void Awake()
        {
            base.Awake();
            GeneratePoolItemDict();
            GeneratePoolLookup();
        }

        #region Public Methods
        public GameObject RequestItem(string key)
        {
            if (!activePools.ContainsKey(key))
            {
                Debug.LogError($"The key <{key}> was not found in the pool. Returning null.");
                return null;
            }

            // Try find existing obj in list
            for (int i = 0; i < activePools[key].Count; i++)
            {
                GameObject target = activePools[key][i];
                if (target == null && onNullMember)
                {
                    Debug.LogWarning($"PoolItem with key: <{key}> at index {i} is null!");
                }
                else if (!target.activeInHierarchy)
                {
                    target.SetActive(true);
                    activePools[key].MoveToIndex(i, activePools[key].Count - 1);
                    return target;
                }
            }

            // Create a new obj, add it and return it
            if (activePools[key].Count < allPoolItems[key].MaxAmount)
            {
                if (onNewMemberCreated)
                    Debug.LogWarning($"Creating new GO with key <{key}>.");

                return CreateAndAdd(allPoolItems[key].Prefab, activePools[key]);
            }

            if (allPoolItems[key].ReuseOnFull)
            {
                var item = activePools[key][0];
                item.SetActive(false);
                item.SetActive(true); // trigger OnEnable
                activePools[key].MoveToIndex(0, activePools[key].Count - 1);
                return item;
            }

            if (onNullMember)
                Debug.LogWarning($"Pool with key <{key}> reached the limit and is returning null!");
            return null;
        }
        public void LoadPool(string key)
        {
            if (activePools.ContainsKey(key))
            {
                Debug.Log($"Pool with key {key} already loaded!");
                return;
            }
            else if (allPoolItems.ContainsKey(key))
            {
                activePools.Add(key, CreatePool(allPoolItems[key].Prefab, 
                                                allPoolItems[key].StartingAmount));
                if (onPoolLoaded)
                    Debug.Log($"Pool with key {key} has been loaded.");
            }
            else
            {
                Debug.LogWarning($"Pool with key {key} cannot be loaded. Data is missing.");
            }
        }
        public void UnloadPool(string key)
        {
            if (activePools.ContainsKey(key))
            {
                foreach (var go in activePools[key])
                {
                    Destroy(go);
                }
                activePools.Remove(key);
                if (onPoolUnloaded)
                    Debug.Log($"Pool with Key {key} has been unloaded.");
            }
            else
            {
                Debug.LogWarning($"Cannot unload pool with key: {key}. " +
                    $"Pool is either not loaded or key doesn't exist.");
            }
        }
        public void EmptyPool(string key)
        {
            Debug.Log($"Pool with key <{key}> was cleared. {activePools[key].Count} GameObjects destroyed.");
            for (int i = 0; i < activePools[key].Count; i++)
            {
                if (activePools[key][i] != null)
                    Destroy(activePools[key][i]);
            }
            activePools[key].Clear();
        }
        public void CullPoolOverhead(string key)
        {
            if (!activePools.ContainsKey(key))
            {
                Debug.LogWarning($"Pool with key <{key}> wasn't found and cannot be culled.");
                return;
            }

            int n = allPoolItems[key].StartingAmount;
            var pool = activePools[key];
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
        private void GeneratePoolLookup()
        {
            for (int i = 0; i < poolData.Length; i++)
            {
                if (!CheckPoolItem(poolData[i]) || !poolData[i].CreateOnStart)
                {
                    continue;
                }
                else if (!activePools.ContainsKey(poolData[i].Key))
                {
                    LoadPool(poolData[i].Key);
                }
                else
                    Debug.LogWarning($"Duplicate key: {poolData[i].Key}!");
            }
        }
        private void GeneratePoolItemDict()
        {
            for (int i = 0; i < poolData.Length; i++)
            {
                if (!CheckPoolItem(poolData[i]))
                {
                    Debug.LogWarning($"Poolitem number {i} didn't pass the check. Pool will be skipped.");
                    continue;
                }

                if (!allPoolItems.ContainsKey(poolData[i].Key))
                    allPoolItems.Add(poolData[i].Key, poolData[i]);
                else
                    Debug.LogWarning($"Duplicate key: {poolData[i].Key}!");
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
            GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);
            go.transform.parent = transform;
            targetList.Add(go);
            return go;
        }
        protected bool CheckPoolItem(PoolItem item)
        {
            bool check = string.IsNullOrWhiteSpace(item.Key)
                         && item.MaxAmount > 0
                         && item.Prefab != null;
            if (!check)
                Debug.LogWarning($"A Poolitem <{item.Key}> failed a check.");
            return check;
        }
        #endregion
    }
}


using UnityEngine;

namespace CoreTools.Pooling
{
    [System.Serializable]
    public class PoolItem
    {
        public string key;
        public GameObject prefab;
        public int startingAmount;
        public int maxAmount;
        public bool reuseOnFull = false;
        public PoolItem(string key, GameObject prefab, int startingAmount, int maxAmount)
        {
            this.key = key;
            this.prefab = prefab;
            this.startingAmount = startingAmount;
            this.maxAmount = maxAmount;
        }
    }
}

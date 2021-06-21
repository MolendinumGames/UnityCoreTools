using UnityEngine;

namespace CoreTools.Pooling
{
    [System.Serializable]
    public class PoolItem
    {
        /* The serialized backing fields are neccessary since
         * Unity doesn't serialize properties and thus they 
         * cannot be accessed by the PoolItem PropertyDrawer
         */

        [SerializeField]
        string key;
        public string Key { get => key; }

        [SerializeField]
        GameObject prefab;
        public GameObject Prefab { get => prefab; }

        [SerializeField]
        int startingAmount;
        public int StartingAmount { get => startingAmount; }

        [SerializeField]
        int maxAmount;
        public int MaxAmount { get => maxAmount; }

        [SerializeField]
        bool reuseOnFull = false;
        public bool ReuseOnFull { get => reuseOnFull; }

        [SerializeField]
        bool createOnStart = true;
        public bool CreateOnStart { get => createOnStart; }

        public PoolItem(string key, GameObject prefab, int startingAmount, int maxAmount)
        {
            this.key = key;
            this.prefab = prefab;
            this.startingAmount = startingAmount;
            this.maxAmount = maxAmount;
            this.reuseOnFull = false;
            this.createOnStart = true;
        }
    }
}

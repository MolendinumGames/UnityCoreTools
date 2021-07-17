using UnityEngine;
using CoreTools.Pooling;

namespace CoreTools.Pooling
{
    [System.Serializable]
    public class GlobalPoolDataSet
    {
        [SerializeField] string key;
        public string Key { get => key; }

        [SerializeField] bool populateOnAwake;
        public bool PopulateOnAwake { get => populateOnAwake; }

        public GameObjectPool Pool;

        public GlobalPoolDataSet()
        {
            key = "";
            populateOnAwake = true;
            Pool = new GameObjectPool(null, 10, 999, false);
        }
    }
}
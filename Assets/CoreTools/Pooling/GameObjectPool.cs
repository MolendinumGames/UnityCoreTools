using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTools;

namespace CoreTools.Pooling
{
    public class GameObjectPool
    {
        LinkedPoolObjectList pool = new();

        // Serialized backing fields will let us edit the property in the Inspector

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

        private bool HasRoom { get => pool.Count < maxAmount; }


        public GameObjectPool(GameObject prefab, int startingAmount, int maxAmount, bool reuseOnFull)
        {
            this.prefab = prefab;
            this.startingAmount = startingAmount;
            this.maxAmount = maxAmount;
            this.reuseOnFull = reuseOnFull;
        }

        public GameObjectPool(GameObject prefab, int startingAmount, int maxAmount) :
            this(prefab, startingAmount, maxAmount, false)
        { }

        public GameObjectPool() : this(null, 0, 0, false)
        { }


        public void Initialize()
        {
            // TODO:
            // Verify pool settings

            pool.Clear();

            for (int i = 0; i < startingAmount; i++)
            {
                CreateAndAdd();
            }
        }

        public GameObject RequestObject()
        {
            var go = GetFirstInactive();

            if (go == null && HasRoom)
                go = CreateAndAdd();
            
            if (go == null && reuseOnFull)
                go = GetReusedObject();

            if (go != null)
                go.SetActive(true);

            return go;
        }

        /// <summary>
        /// Destroy all pooled GameObjects and clear the pool. Use with caution!
        /// </summary>
        public void UnloadPool()
        {
            // This method should be slightly cheaper than using Remove(node) foreach node in the pool
            // because we are not going to relink any nodes

            int unloadedAmount = pool.Count;

            // Remove all GameObjects from the pool
            foreach (var go in pool.GetGameObjects())
                UnityEngine.Object.Destroy(go);
            
            // Clear all nodes
            pool.Clear();

            Debug.Log($"Pool {ToString()} has been cleared. {unloadedAmount} Objects destroyed.");
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryCullPoolOverhead()
        {
            int objectsCulled = 0;
            foreach (var node in pool.GetNodes())
            {
                if (pool.Count <= StartingAmount)
                    break;

                if (NodeIsUnused(node))
                {
                    DestroyNode(node);
                    objectsCulled++;
                }
            }

            Debug.Log($"{ToString()} was culled: {objectsCulled} objects destroyed.");
        }

        public void ForceCullPoolOverhead()
        {
            TryCullPoolOverhead();

            int objectsCulled = 0;
            while (pool.Count > StartingAmount)
            {
                DestroyNode(pool.First);
                objectsCulled++;
            }

            Debug.Log($"{ToString()} was force culled: {objectsCulled} objects destroyed.");
        }

        private GameObject GetFirstInactive()
        {
            foreach (var node in pool.GetNodes())
            {
                if (NodeIsUnused(node))
                    return TakeGameObjectFromNode(node);
            }

            // No unused GameObject found:
            return null;
        }

        private GameObject CreateAndAdd()
        {
            var go = UnityEngine.Object.Instantiate(prefab);
            pool.AppendNew(go);
            go.SetActive(false);
            return go;
        }

        private GameObject TakeGameObjectFromNode(LinkedPoolObjectNode node)
        {
            pool.MoveToEnd(node);
            var go = node.PooledObject;
            return go;
        }

        private bool NodeIsUnused(LinkedPoolObjectNode node) => 
            !node.PooledObject.activeInHierarchy;

        private void DestroyNode(LinkedPoolObjectNode node)
        {
            var go = pool.Remove(node);
            UnityEngine.Object.Destroy(go);
        }

        private GameObject GetReusedObject() =>
            TakeGameObjectFromNode(pool.First);


    }
}
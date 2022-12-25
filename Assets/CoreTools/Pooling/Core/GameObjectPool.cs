/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using UnityEngine;
using CoreTools;

namespace CoreTools.Pooling
{
    [System.Serializable]
    public class GameObjectPool
    {
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



        LinkedPoolObjectList pool = new LinkedPoolObjectList();

        // Serialized backing fields will let us edit the property in the Inspector

        [SerializeField]
        [Tooltip("The GameObject that will be pooled.")]
        GameObject prefab;
        public GameObject Prefab { get => prefab; }

        [SerializeField]
        [Tooltip("To avoid instantiating at runtime set it to a high enough value so the pool doesn't need to grow.")]
        int startingAmount;
        public int StartingAmount { get => startingAmount; }

        [SerializeField]
        [Tooltip("When limit is reached either returns null or reuses first used Object. Use to limit highest possible memory use.")]
        int maxAmount;
        public int MaxAmount { get => maxAmount; }

        [SerializeField]
        [Tooltip("If true, the pool will reuse the first object given out when the pool limit is reached.")]
        bool reuseOnFull = false;
        public bool ReuseOnFull { get => reuseOnFull; }

        /// <summary>
        /// Returns true if the current amount of pooled objects is lower than the limit of the pool.
        /// </summary>
        public bool HasRoom { get => pool.Count < maxAmount; }

        private Transform parentTransform;


        public void Initialize()
        {
            // Verify pool data
            if (prefab == null)
            {
                Debug.LogError($"A gameobject pool has no prefab attached!");
                return;
            }
            if (startingAmount > maxAmount)
            {
                Debug.LogError($"A gameobject pool cannot be initialized because its limit is lower than the " +
                    $"starting amount! Corresponding prefab: {prefab}");
                return;
            }
            if (startingAmount < 0)
            {
                Debug.LogError($"The starting amount of a gameobject pool cannot be less than 0! Pool has not been" +
                    $"initialized. Corresponding prefab: {prefab}");
                return;
            }
            //////////
            
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
        /// Destroy all pooled GameObjects and clear the pool.
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
        /// Destroys currently inactive pooled objects until the pool is back to the starting amount
        /// or ran out of inactive objects. Use with caution and only if memory used is an issue.
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

        /// <summary>
        /// Destroys currently inactive pooled objects until the pool is back to the starting amount
        /// or ran out of inactive objects. Use with caution and only if memory used is an issue.
        /// </summary>
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

        public void SetParentTransform(Transform t)
        {
            parentTransform = t;
        }

        private GameObject GetFirstInactive()
        {
            foreach (var node in pool.GetNodes())
            {
                if (NodeIsUnused(node))
                    return TakeGameObjectFromNode(node);
            }

            // No unused GameObject found
            return null;
        }

        private GameObject CreateAndAdd()
        {
            var go = UnityEngine.Object.Instantiate(prefab);
            pool.AppendNew(go);
            if (parentTransform != null)
                go.transform.parent = parentTransform;
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
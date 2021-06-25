using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTools;

public class GameObjectPool
{
    List<GameObject> pool = new();


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

    [SerializeField]
    bool createOnStart = true;
    public bool CreateOnStart { get => createOnStart; }

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
        // verify pool settings

        for (int i = 0; i < startingAmount; i++)
        {
            CreateAndAdd();
        }
    }

    public GameObject RequestObject()
    {
        var go = GetFirstInactive();

        if (go == null)
        {
            if (HasRoom)
                return CreateAndAdd();

            if (reuseOnFull) // TODO
                throw new NotImplementedException("Reuse on full not implemented!"); 
        }

        return go;
    }

    public void UnloadPool()
    {
        int lastAmount = pool.Count;
        foreach (GameObject go in pool)
            GameObject.Destroy(go);
        pool.Clear();

        Debug.Log($"Pool {ToString()} has been cleared. {lastAmount} Objects destroyed.");
    }

    public void TryCullPoolOverhead()
    {
        int objectsCulled = 0;
        while (pool.Count > StartingAmount)
        {
            int index = -1;
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i] == null || !pool[i].activeInHierarchy)
                {
                    index = i;
                    GameObject.Destroy(pool[index]);
                    objectsCulled++;
                    break;
                }
            }
            if (index < 0)
                break;

            pool.RemoveAt(index);
        }

        Debug.Log($"{ToString()} pool force culled: {objectsCulled} objects destroyed.");
    }

    public void ForceCullPoolOverhead()
    {
        TryCullPoolOverhead();

        int objectsCulled = 0;
        while (pool.Count > StartingAmount)
        {
            int lastIndex = pool.Count - 1;
            GameObject.Destroy(pool[lastIndex]);
            objectsCulled++;
            pool.RemoveAt(lastIndex);
        }

        Debug.Log($"{ToString()} pool force culled: {objectsCulled} objects destroyed.");
    }

    private GameObject GetFirstInactive()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
                return pool[i];
        }
        return null;
    }

    private GameObject CreateAndAdd()
    {
        var go = GameObject.Instantiate(prefab);
        pool.Add(go);
        return go;
    }

}

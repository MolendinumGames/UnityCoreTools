using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTools;

public class GameObjectPool
{
    // Serialized backing fields will let us edit the property in the Inspector

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


    public GameObjectPool(string key, GameObject prefab, int startingAmount, int maxAmount, bool reuseOnFull, bool createOnStart)
    {
        this.key = key;
        this.prefab = prefab;
        this.startingAmount = startingAmount;
        this.maxAmount = maxAmount;
        this.reuseOnFull = reuseOnFull;
        this.createOnStart = createOnStart;
    }

    public GameObjectPool(string key, GameObject prefab, int startingAmount, int maxAmount) :
        this(key, prefab, startingAmount, maxAmount, false, true)
    { }

    public GameObjectPool(string key) : this(key, null, 0, 0, false, true)
    { }

    public GameObjectPool() : this("", null, 0, 0, false, true)
    { }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskData : ScriptableObject
{
    private string uniqueId;
    public string UniqueID
    {
        get
        {
            if (string.IsNullOrWhiteSpace(""))
                uniqueId = name;

            return uniqueId;
        }
    }

    private int chachedValue = 0;
    public int Value 
    { 
        get => Value;
        set => this.chachedValue = value;
    }
}

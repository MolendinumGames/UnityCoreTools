using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.UI
{
    public interface IDragSource<T> where T : class
    {
        T GetItem();
        int GetAmount();
        void RemoveItems(int amount);
    }
}


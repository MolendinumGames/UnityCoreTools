using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.UI
{
    public interface IDragDestination<T> where T : class
    {
        int MaxAcceptable(T item);
        void SetItem(T item, int amount);
    }
}

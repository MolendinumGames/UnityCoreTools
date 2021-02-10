using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.UI
{
    public interface IDragContainer<T> : IDragSource<T>, IDragDestination<T> where T : class
    {

    }
}

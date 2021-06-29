using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CoreTools.Pooling
{
    class LinkedPoolObjectNode
    {
        public LinkedPoolObjectNode(GameObject pooledObject, LinkedPoolObjectNode previous, LinkedPoolObjectNode next)
        {
            this.PooledObject = pooledObject;
            this.Next = next;
            this.Previous = previous;
        }

        public LinkedPoolObjectNode(GameObject pooledObject) : this(pooledObject, null, null)
        { }

        public LinkedPoolObjectNode() : this(null, null, null)
        { }


        public GameObject PooledObject { get; set; }

        public LinkedPoolObjectNode Previous { get; set; }

        public LinkedPoolObjectNode Next { get; set; }
    }
}

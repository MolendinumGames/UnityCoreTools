using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools
{
    public abstract class BaseChannel : ScriptableObject
    {
        [TextArea]
        [SerializeField]
        private string description = "";

        [SerializeField]
        private bool logOnNoListener = true;

        protected virtual void LogNoListener()
        {
            if (logOnNoListener)
                Debug.Log($"{name} event has been raised but there are no listeners for this channel. Notes: {description}");
        }
    }
}
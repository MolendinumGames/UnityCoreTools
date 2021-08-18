/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

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
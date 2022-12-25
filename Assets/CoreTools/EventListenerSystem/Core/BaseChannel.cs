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
                Debug.Log($"{name} event has been raised but there are no listeners for this channel. EventNotes: {description}");
        }
    }
}
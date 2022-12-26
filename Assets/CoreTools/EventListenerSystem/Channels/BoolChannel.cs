/* Copyright (c) 2022 - Christoph R�mer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "NewBoolChannel", menuName = "Channel/Bool")]
    public class BoolChannel : BaseChannel
    {
        public UnityAction<bool> OnEventRaised;

        public void Raise(bool value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
            else
                LogNoListener();
        }
    }
}
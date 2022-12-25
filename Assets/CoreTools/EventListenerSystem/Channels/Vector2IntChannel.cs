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
using UnityEngine.Events;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "NewVector2IntChannel", menuName = "Channel/Vector2Int")]
    public class Vector2IntChannel : BaseChannel
    {
        public UnityAction<Vector2Int> OnEventRaised;

        public void Raise(Vector2Int value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
            else
                LogNoListener();
        }
    }
}
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
    public class DialogueChannelListener : MonoBehaviour
    {
        [SerializeField]
        private DialogueChannel channel = null;

        public UnityEvent<Dialogue> OnEventRaised;

        private void OnEnable()
        {
            if (channel)
                channel.OnEventRaised += Respond;
        }
        private void OnDisable()
        {
            if (channel)
                channel.OnEventRaised -= Respond;
        }
        private void Respond(Dialogue value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
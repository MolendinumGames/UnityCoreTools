/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    public class BoolChannelListener : MonoBehaviour
    {
        [SerializeField] private BoolChannel channel = default;

        public UnityEvent<bool> OnEventRaised;

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
        private void Respond(bool value) => OnEventRaised?.Invoke(value);
    }
}
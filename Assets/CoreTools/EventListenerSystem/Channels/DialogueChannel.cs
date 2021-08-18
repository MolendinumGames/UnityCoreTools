/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "New Dialogue Channel", menuName = "Channel/Dialogue)", order = 1)]
    public class DialogueChannel : BaseChannel
    {
        public UnityAction<Dialogue> OnEventRaised;

        public void Raise(Dialogue value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
            else
                LogNoListener();
        }
    }
}
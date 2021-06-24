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
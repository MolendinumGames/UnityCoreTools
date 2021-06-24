using System.Collections;
using System.Collections.Generic;
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
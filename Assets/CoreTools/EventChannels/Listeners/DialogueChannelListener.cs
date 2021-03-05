using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    public class DialogueChannelListener : MonoBehaviour
    {
        [SerializeField]
        private DialogueChannelSO channel = null;

        public UnityEvent<DialogueSO> OnEventRaised;

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
        private void Respond(DialogueSO value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
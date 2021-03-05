using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "New Dialogue Channel", menuName = "Channel/Dialogue)", order = 1)]
    public class DialogueChannelSO : BaseChannelSO
    {
        public UnityAction<DialogueSO> OnEventRaised;

        public void Raise(DialogueSO value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
            else
                LogNoListener();
        }
    }
}
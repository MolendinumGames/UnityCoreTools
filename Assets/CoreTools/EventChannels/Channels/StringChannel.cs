using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "NewStringChannel", menuName = "Channel/String")]
    public class StringChannel : BaseChannel
    {
        public UnityAction<string> OnEventRaised;

        public void Raise(string value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
            else
                LogNoListener();
        }
    }
}
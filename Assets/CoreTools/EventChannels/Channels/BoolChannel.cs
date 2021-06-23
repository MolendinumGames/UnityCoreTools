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
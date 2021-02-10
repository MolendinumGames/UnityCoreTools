using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "NewIntChannel", menuName = "Channel/Int")]
    public class IntChannelSO : BaseChannelSO
    {
        public UnityAction<int> OnEventRaised;

        public void Raise(int value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
            else
                LogNoListener();
        }
    }
}
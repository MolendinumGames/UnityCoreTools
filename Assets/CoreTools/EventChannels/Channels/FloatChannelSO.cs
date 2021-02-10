using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "NewFloatChannel", menuName = "Channel/Float")]
    public class FloatChannelSO : BaseChannelSO
    {
        public UnityAction<float> OnEventRaised;

        public void Raise(float value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
            else
                LogNoListener();
        }
    }
}
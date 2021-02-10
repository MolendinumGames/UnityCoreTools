using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "NewVector3Channel", menuName = "Channel/Vector3")]
    public class Vector3ChannelSO : BaseChannelSO
    {
        public UnityAction<Vector3> OnEventRaised;

        public void Raise(Vector3 value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
            else
                LogNoListener();
        }
    }
}
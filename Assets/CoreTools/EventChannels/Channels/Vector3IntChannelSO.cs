using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "NewVector3IntChannel", menuName = "Channel/Vector3Int")]
    public class Vector3IntChannelSO : BaseChannelSO
    {
        public UnityAction<Vector3Int> OnEventRaised;

        public void Raise(Vector3Int value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
            else
                LogNoListener();
        }
    }
}

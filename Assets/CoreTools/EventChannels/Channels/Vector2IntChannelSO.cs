using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "NewVector2IntChannel", menuName = "Channel/Vector2Int")]
    public class Vector2IntChannelSO : BaseChannelSO
    {
        public UnityAction<Vector2Int> OnEventRaised;

        public void Raise(Vector2Int value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
            else
                LogNoListener();
        }
    }
}
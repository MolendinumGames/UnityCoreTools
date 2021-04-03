using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    [CreateAssetMenu(fileName = "NewVector2Channel", menuName = "Channel/Vector2")]
    public class Vector2Channel : BaseChannel
    {
        public UnityAction<Vector2> OnEventRaised;

        public void Raise(Vector2 value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
            else
                LogNoListener();
        }
    }
}
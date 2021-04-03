using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    public class Vector2ChannelListener : MonoBehaviour
    {
        [SerializeField] private Vector2Channel channel = default;

        public UnityEvent<Vector2> OnEventRaised;

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
        private void Respond(Vector2 value) => OnEventRaised?.Invoke(value);
    }
}
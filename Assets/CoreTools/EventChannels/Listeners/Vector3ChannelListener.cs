using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    public class Vector3ChannelListener : MonoBehaviour
    {
        [SerializeField] private Vector3Channel channel = default;

        public UnityEvent<Vector3> OnEventRaised;

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
        private void Respond(Vector3 value) => OnEventRaised?.Invoke(value);
    }
}


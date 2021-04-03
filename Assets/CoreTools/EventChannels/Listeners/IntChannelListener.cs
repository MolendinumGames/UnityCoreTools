using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    public class IntChannelListener : MonoBehaviour
    {
        [SerializeField] private IntChannel channel = default;

        public UnityEvent<int> OnEventRaised;

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
        private void Respond(int value) => OnEventRaised?.Invoke(value);
    }
}
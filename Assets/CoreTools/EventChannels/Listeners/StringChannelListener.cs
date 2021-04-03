using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    public class StringChannelListener : MonoBehaviour
    {
        [SerializeField] private StringChannel channel = default;

        public UnityEvent<string> OnEventRaised;

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
        private void Respond(string value) => OnEventRaised?.Invoke(value);
    }
}
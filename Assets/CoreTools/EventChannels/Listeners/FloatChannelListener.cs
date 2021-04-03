using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    public class FloatChannelListener : MonoBehaviour
    {
        [SerializeField] private FloatChannel channel = default;

        public UnityEvent<float> OnEventRaised;

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
        private void Respond(float value) => OnEventRaised?.Invoke(value);
    }
}
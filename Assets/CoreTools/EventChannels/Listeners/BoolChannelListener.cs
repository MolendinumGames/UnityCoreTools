using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    public class BoolChannelListener : MonoBehaviour
    {
        [SerializeField] private BoolChannel channel = default;

        public UnityEvent<bool> OnEventRaised;

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
        private void Respond(bool value) => OnEventRaised?.Invoke(value);
    }
}
using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    public class Vector2IntChannelListener : MonoBehaviour
    {
        [SerializeField] private Vector2IntChannel channel = default;

        public UnityEvent<Vector2Int> OnEventRaised;

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
        private void Respond(Vector2Int value) => OnEventRaised?.Invoke(value);
    }
}

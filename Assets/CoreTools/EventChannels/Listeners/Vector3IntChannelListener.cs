using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
    public class Vector3IntChannelListener : MonoBehaviour
    {
        [SerializeField] private Vector3IntChannel channel = default;

        public UnityEvent<Vector3Int> OnEventRaised;

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
        private void Respond(Vector3Int value) => OnEventRaised?.Invoke(value);
    }
}

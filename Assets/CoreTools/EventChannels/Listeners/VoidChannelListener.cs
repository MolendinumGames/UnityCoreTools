using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
	public class VoidChannelListener : MonoBehaviour
	{
		[SerializeField] private VoidChannel channel = default;

		public UnityEvent OnEventRaised;

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
        private void Respond() => OnEventRaised?.Invoke();
    }	
}

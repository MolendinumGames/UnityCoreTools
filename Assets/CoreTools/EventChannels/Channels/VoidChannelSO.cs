using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
	[CreateAssetMenu(fileName = "NewVoidChannel", menuName = "Channel/Void")]
	public class VoidChannelSO : BaseChannelSO
	{
		public UnityAction OnEventRaised;

		public void Raise()
        {
			if (OnEventRaised != null)
				OnEventRaised.Invoke();
			else
				LogNoListener();
        }
	}	
}

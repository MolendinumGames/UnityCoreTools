/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

using UnityEngine;
using UnityEngine.Events;

namespace CoreTools
{
	[CreateAssetMenu(fileName = "NewVoidChannel", menuName = "Channel/Void")]
	public class VoidChannel : BaseChannel
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

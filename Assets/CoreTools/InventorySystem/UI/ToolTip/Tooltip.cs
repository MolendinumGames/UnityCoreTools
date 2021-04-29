using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace CoreTools.InventorySystem.UI
{
	public class Tooltip : MonoBehaviour
	{
		[SerializeField]
		TextMeshProUGUI headerText;
		[SerializeField]
		TextMeshProUGUI bodyText;

		public void Initialitze(string header, string body)
        {
			headerText.text = header;
			bodyText.text = body;
			StartAnimation();
        }

        private void StartAnimation()
        {

        }
    }	
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace InventorySystem.UI
{
	public class ItemTooltip : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI headerText;
		[SerializeField] TextMeshProUGUI bodyText;

		public void SetUp(InventoryItem item)
        {
			if (item == null)
            {
				headerText.text = "Default Header";
				bodyText.text = "Default Description";
				return;
            }

			headerText.text = item.GetName();
			bodyText.text = item.GetToolTip();
        }
	}	
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
	[System.Serializable]
	public class EquipmentSlot
	{
		public  EquipmentItem item;
		[SerializeField]
		private EquipmentType type;
		[SerializeField]
		private string uniqueId;

		public EquipmentItem GetItem() => item;
		public EquipmentType GetEquipmentType() => type;
		public string GetID()
        {
			if (string.IsNullOrWhiteSpace(uniqueId))
            {
				uniqueId = System.Guid.NewGuid().ToString();
            }
			return uniqueId;
        }

		public void SetItem(EquipmentItem item)
        {
			if (item != null)
				this.item = item;
        }
		public void ClearSlot() => item = null;
	}	
}

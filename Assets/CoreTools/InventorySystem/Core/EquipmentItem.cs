using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
	[CreateAssetMenu(fileName =	"NewEquipment", menuName = "Inventory/Item/Equipemnt", order = 1)]
	public class EquipmentItem : InventoryItem
	{
		[SerializeField]EquipmentType type;

		public EquipmentType GetEquipmentType() => type;
	}	
}

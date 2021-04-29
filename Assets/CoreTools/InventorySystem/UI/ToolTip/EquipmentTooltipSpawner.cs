using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.InventorySystem.UI
{
    public class EquipmentTooltipSpawner : TooltipSpawner
    {
        public override bool CanSpawnToolTip() => GetComponent<EquipmentSlotUI>().GetItem() != null;

        public override void UpdateTooltip(GameObject tipObject)
        {
            InventoryItem item = GetComponent<EquipmentSlotUI>().GetItem();
            Tooltip itemTooltip = tipObject.GetComponent<Tooltip>();
            if (item != null && itemTooltip != null)
            {
                itemTooltip.Initialitze(item.ItemName, item.Description);
            }
        }
    }
}

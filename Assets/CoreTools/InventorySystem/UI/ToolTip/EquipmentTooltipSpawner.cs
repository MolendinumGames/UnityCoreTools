using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTools.UI;

namespace CoreTools.InventorySystem.UI
{
    public class EquipmentTooltipSpawner : TooltipSpawner
    {
        public override bool CanSpawnToolTip() => GetComponent<EquipmentSlotUI>().GetItem() != null;

        public override void UpdateTooltip(GameObject tipObject)
        {
            InventoryItem item = GetComponent<EquipmentSlotUI>().GetItem();
            TooltipController itemTooltip = tipObject.GetComponent<TooltipController>();
            if (item != null && itemTooltip != null)
            {
                itemTooltip.Initialitze(item.ItemName, item.Description);
            }
        }
    }
}

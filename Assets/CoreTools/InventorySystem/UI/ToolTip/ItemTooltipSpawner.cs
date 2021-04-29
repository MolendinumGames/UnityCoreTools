using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.InventorySystem.UI
{
    public class ItemTooltipSpawner : TooltipSpawner
    {
        public override bool CanSpawnToolTip() => GetComponent<InventorySlotUI>().GetItem() != null;

        public override void UpdateTooltip(GameObject tipObject)
        {
            Tooltip itemTooltip = tipObject.GetComponent<Tooltip>();
            InventoryItem item = GetComponent<InventorySlotUI>().GetItem();
            if (itemTooltip != null && item != null)
            {
                itemTooltip.Initialitze(item.ItemName, item.Description);
            }
        }
    }
}

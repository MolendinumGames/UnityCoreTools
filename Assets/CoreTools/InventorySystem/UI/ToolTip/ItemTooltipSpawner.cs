using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.UI
{
    public class ItemTooltipSpawner : ToolTipSpawner
    {
        public override bool CanSpawnToolTip()
        {
            return GetComponent<InventorySlotUI>().GetItem() != null;
        }

        public override void UpdateTooltip(GameObject tip)
        {
            ItemTooltip toolTip = tip.GetComponent<ItemTooltip>();
            InventoryItem item = GetComponent<InventorySlotUI>().GetItem();

            if (toolTip)
            {
                toolTip.SetUp(item);
            }
        }
    }
}

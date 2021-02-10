using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.UI
{
    public class EquipmentTooltipSpawner : ToolTipSpawner
    {
        public override bool CanSpawnToolTip()
        {
            return GetComponent<EquipmentSlotUI>()?.GetItem() != null;
        }

        public override void UpdateTooltip(GameObject tip)
        {
            if (tip == null) return;
            InventoryItem item = GetComponent<EquipmentSlotUI>().GetItem();
            tip.GetComponent<ItemTooltip>().SetUp(item);
        }
    }
}

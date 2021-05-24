using UnityEngine;
using CoreTools.UI;

namespace CoreTools.InventorySystem.UI
{
    public class ItemTooltipSpawner : TooltipSpawner
    {
        public override bool CanSpawnToolTip() => GetComponent<InventorySlotUI<InventoryItem>>().GetItem() != null;

        public override void UpdateTooltip(GameObject tipObject)
        {
            TooltipController itemTooltip = tipObject.GetComponent<TooltipController>();
            InventoryItem item = GetComponent<InventorySlotUI<InventoryItem>>().GetItem();
            if (itemTooltip != null && item != null)
            {
                itemTooltip.Initialitze(item.ItemName, item.Description);
            }
        }
    }
}

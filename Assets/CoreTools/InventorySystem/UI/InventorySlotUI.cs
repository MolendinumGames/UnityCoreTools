using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoreTools.InventorySystem;

namespace CoreTools.InventorySystem.UI
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<InventoryItem>
    {
        [SerializeField] 
        Image iconImage;

        Inventory inventory;
        int slotID;

        public int GetAmount() => inventory.GetAmountIntSlot(slotID);

        public InventoryItem GetItem() => inventory.GetItemInSlot(slotID);

        public int MaxAcceptable(InventoryItem item)
        {
            if (inventory.GetItemInSlot(slotID) == null)
                return item.MaxStack;
            else
                return inventory.GetAmountIntSlot(slotID);
        }

        public void RemoveItems(int amount) => inventory.RemoveFromSlot(slotID, amount);

        public void SetItem(InventoryItem item, int amount) => inventory.TryAddItemToSlot(item, slotID, amount);

        public void SetUp(Inventory inventory, int slotID)
        {
            this.inventory = inventory;
            this.slotID = slotID;
            if (inventory != null && inventory.GetItemInSlot(slotID) != null)
                SetIcon(inventory.GetItemInSlot(slotID));
            else ResetIcon();
        }

        private void SetIcon(InventoryItem item)
        {
            Sprite icon = item.Icon;
            iconImage.color = Color.white;
            if (icon != null)
                iconImage.sprite = icon;
        }
        private void ResetIcon()
        {
            iconImage.color = Color.clear;
            iconImage.sprite = null;
        }
    }
}

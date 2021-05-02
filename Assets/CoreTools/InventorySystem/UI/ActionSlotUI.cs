using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoreTools.UI;

namespace CoreTools.InventorySystem
{
    public class ActionSlotUI : MonoBehaviour, IDragContainer<ActionItem>
    {
        [SerializeField]
        Image iconImage;

        [SerializeField]
        Inventory inventory;
        int slotID;

        public void SetUp(Inventory inventory, int slotID)
        {
            this.inventory = inventory;
            this.slotID = slotID;
            if (inventory != null && inventory.GetItemInSlot(slotID) != null)
                SetIcon(inventory.GetItemInSlot(slotID));
            else ResetIcon();
        }

        private void OnEnable() => inventory.inventoryUpdated += RefreshUI;
        private void OnDisable() => inventory.inventoryUpdated -= RefreshUI;

        public int GetAmount() => inventory.GetAmountIntSlot(slotID);

        public ActionItem GetItem() => inventory.GetItemInSlot(slotID) as ActionItem;

        public int MaxAcceptable(ActionItem item)
        {
            if (GetAmount() == 0)
                return item.MaxStack;
            else if (item.GetType() == GetItem().GetType())
                return Mathf.Clamp(item.MaxStack - GetAmount(), 0, item.MaxStack);
            else return 0;
        }

        public void RemoveItems(int amount) => inventory.RemoveFromSlot(slotID, amount);

        public void SetItem(ActionItem item, int amount) => inventory.TryAddItemToSlot(item, slotID, amount);

        private void RefreshUI()
        {
            InventoryItem i = GetItem();
            if (i != null) 
                SetIcon(i);
            else 
                ResetIcon();
        }
        private void SetIcon(InventoryItem item)
        {
            Sprite icon = item.Icon;
            iconImage.color = Color.white;
            if (icon != null)
                iconImage.sprite = icon;
            else
                iconImage.sprite = null;
        }
        private void ResetIcon()
        {
            iconImage.color = Color.clear;
            iconImage.sprite = null;
        }
    }
}

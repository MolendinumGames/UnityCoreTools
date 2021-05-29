using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoreTools.UI;
using CoreTools.InventorySystem;

namespace CoreTools.InventorySystem.UI
{
    public abstract class InventorySlotUI<T> : MonoBehaviour, IDragContainer<T> where T : InventoryItem
    {
        [SerializeField] Image iconImage;

        [SerializeField] Inventory inventory;
        [SerializeField] int slotID;

        public void SetUp(Inventory inventory, int slotID)
        {
            this.inventory = inventory;
            this.slotID = slotID;
            if (inventory != null && inventory.GetItemInSlot(slotID) != null)
                SetIcon(inventory.GetItemInSlot(slotID));
            else
                ResetIcon();
        }

        private void OnEnable() =>
            inventory.inventoryUpdated += RefreshUI;

        private void OnDisable() => 
            inventory.inventoryUpdated -= RefreshUI;

        public int GetAmount() =>
            inventory.GetAmountIntSlot(slotID);

        public T GetItem() =>
            inventory.GetItemInSlot(slotID) as T;

        public int MaxAcceptable(T item)
        {
            if (item == null) 
                return 0;
            else if (GetItem() == null)
                return item.MaxStack;
            else
                return GetAvailableSpace();
        }

        public void RemoveAmount(int amount) =>
            inventory.RemoveFromSlot(slotID, amount);

        public void SetItem(T item, int amount) =>
            inventory.TryAddItemToSlot(item, slotID, amount); // WHAT??!?!?!?

        public int TryAddAmount(T item, int amount)
        {
            throw new NotImplementedException();
        }

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
            iconImage.color = Color.white;
            if (item.Icon != null)
                iconImage.sprite = item.Icon;
        }
        private void ResetIcon()
        {
            iconImage.color = Color.clear;
            iconImage.sprite = null;
        }

        private int GetAvailableSpace()
        {
            int current = GetAmount();
            int max = GetItem().MaxStack;
            int available = max - current;
            return Mathf.Clamp(available, 0, max);
        }
    }
}

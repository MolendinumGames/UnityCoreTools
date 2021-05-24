using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.InventorySystem.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        GameObject slotPrefab;

        public Inventory inventory;

        List<InventorySlotUI<InventoryItem>> slots = new List<InventorySlotUI<InventoryItem>>();

        private void OnEnable()
        {
            Subscribe();
            RefreshInventoryUI();
        }
        private void OnDisable()
        {
            Unsubscribe();
        }

        private void RefreshInventoryUI()
        {
            if (inventory == null)
                return;

            Resize();
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].SetUp(inventory, i);
            }
        }
        private void Resize()
        {
            ClearDead();
            if (slots.Count == inventory.Size)
                return;

            while (slots.Count > 0 && slots.Count > inventory.Size)
            {
                Destroy(slots[slots.Count - 1].gameObject);
                slots.RemoveAt(slots.Count - 1);
            }
            while (slots.Count < inventory.Size)
            {
                GameObject gO = Instantiate(slotPrefab, transform);
                slots.Add(gO.GetComponent<InventorySlotUI<InventoryItem>>());
            }
        }
        private void ClearDead()
        {
            for (int i = slots.Count - 1; i >= 0; i--)
            {
                if (slots[i] == null)
                    slots.RemoveAt(i);
            }
        }
        private void Subscribe()
        {
            if (inventory != null)
                inventory.inventoryUpdated += RefreshInventoryUI;
        }
        private void Unsubscribe()
        {
            if (inventory != null)
                inventory.inventoryUpdated -= RefreshInventoryUI;
        }
    }
}

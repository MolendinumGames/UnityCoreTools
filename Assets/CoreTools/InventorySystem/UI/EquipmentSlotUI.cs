using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;
using UnityEngine.UI;

namespace CoreTools.InventorySystem.UI
{
	public class EquipmentSlotUI : MonoBehaviour, IDragContainer<InventoryItem>
	{
		public EquipmentHolder holder;
        public string connectedId;

        [SerializeField] Image icon;
        [SerializeField] Sprite fallbackIcon;

        private void OnEnable() => holder.equipmentUpdated += RefreshUI;
        private void OnDisable() => holder.equipmentUpdated -= RefreshUI;

        public int GetAmount()
        {
            if (holder && holder.GetItem(connectedId) != null)
                return 1;
            else return 0;
        }

        public InventoryItem GetItem()
        {
            if (holder)
                return holder.GetItem(connectedId);
            else return null;
        }

        public int MaxAcceptable(InventoryItem item)
        {
            if (!holder
                || holder.GetSlotType(connectedId) == EquipmentType.Default // This means the connectedID doesn't exist in the Equipment holder
                || item as EquipmentItem == null
                || ((EquipmentItem)item).EquipmentType != holder.GetSlotType(connectedId))
                return 0;
            else return 1;
        }

        public void RemoveItems(int amount)
        {
            if (holder)
                holder.RemoveItemByID(connectedId);
        }

        public void SetItem(InventoryItem item, int amount)
        {
            if (holder)
                holder.EquipItemIntoSlot(item as EquipmentItem, connectedId);

        }
        private void RefreshUI()
        {
            // Get Component
            if (icon == null)
                icon = GetComponentInChildren<Image>();
            if (icon == null) return;

            InventoryItem i = GetItem() as InventoryItem;
            if (i != null)
            {
                Sprite s = i.Icon;
                if (s != null) // target icon
                {
                    icon.sprite = s;
                }
                else // fallback Icon
                {
                    icon.sprite = fallbackIcon;
                }
                icon.color = Color.white;
            }
            else // empty slot
            {
                icon.sprite = null;
                icon.color = Color.clear;
            }


        }
    }	
}

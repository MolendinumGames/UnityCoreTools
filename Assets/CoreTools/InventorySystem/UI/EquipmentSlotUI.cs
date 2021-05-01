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

        [SerializeField]
        Image icon;
        [SerializeField] Sprite fallbackIcon;

        private void Awake()
        {
            icon = GetComponentInChildren<Image>();
            ClearIcon();
        }
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
            InventoryItem i = GetItem();
            if (i != null)
            {
                if (i.Icon != null)
                    icon.sprite = i.Icon;
                else // fallback
                    icon.sprite = fallbackIcon;

                icon.color = Color.white;
            }
            else ClearIcon();
        }
        private void ClearIcon()
        {
            icon.sprite = null;
            icon.color = Color.clear;
        }
    }	
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Max allowed size")]
        private int inventorySize;

        private InventorySlot[] slots;

        [SerializeField]
        private InventoryItem restrictedType;

        public event Action inventoryUpdated;

        protected void Awake() => SetUpInventory();

        private void SetUpInventory()
        {
            slots = new InventorySlot[inventorySize];
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = new InventorySlot();
            }
        }
        public InventoryItem GetItemInSlot(int index)
        {
            try
            {
               return slots[index].item;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public int GetAmountIntSlot(int index)
        {
            try
            {
                return slots[index].GetAmount();
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int GetSpaceInSlot(int index) => slots[index].GetSpace();

        public void RemoveFromSlot(int index, int amount)
        {
            slots[index].RemoveAmountFromSlot(amount);
            RaiseUpdateEvent();
        }
        public int Size => inventorySize;


        private bool FindEmptySlot(out int index)
        {
            index = -1;
            for (int i = 0; i < inventorySize; i++)
            {
                if (slots[i].IsEmpty())
                {
                    index = i;
                    return true;
                }
            }
            return false;
        }
        public bool TryAddItem(InventoryItem item, int amount, out int remainder)
        {
            remainder = amount;
            if (!ItemIsCorrectType(item))
                return false;

            for (int i = 0; i < slots.Length; i++)
            {
                if (GetItemInSlot(i) == item && slots[i].HasSpace())
                {
                    amount = slots[i].AddAmountToSlot(amount);
                    remainder = amount;
                    if (amount <= 0)
                    {
                        RaiseUpdateEvent();
                        return true;
                    }
                }
            }
            while (amount > 0)
            {
                int newIndex = -1;
                if (FindEmptySlot(out newIndex))
                {
                    slots[newIndex].item = item;
                    amount = slots[newIndex].AddAmountToSlot(amount);
                    remainder = amount;
                    if (amount <= 0)
                    {
                        RaiseUpdateEvent();
                        return true;
                    }
                }
                else
                {
                    RaiseUpdateEvent();
                    return false;
                }
            }
            return false;
        }
        public bool TryAddItemToSlot(InventoryItem item, int id, int amount)
        {
            if (!ItemIsCorrectType(item))
                return false;

            int remainder = SetSlot(id, item, amount);
            if (remainder > 0)
            {
                return TryAddItem(item, remainder, out _);
            }
            RaiseUpdateEvent();
            return true;
        }

        protected void RaiseUpdateEvent() => inventoryUpdated?.Invoke();

        public bool HasRestriction() => restrictedType != null; // needed?
        public bool ItemIsCorrectType(InventoryItem item) => 
            restrictedType == null || item.GetType() == restrictedType.GetType();


        public int SetSlot(int slot, InventoryItem item, int amount)
        {
            slots[slot].Reset();
            slots[slot].item = item;
            return slots[slot].AddAmountToSlot(amount);
        }
    }
}

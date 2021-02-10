using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class InventorySlot
    {
        public InventoryItem item;
        private int amount;

        public void Reset()
        {
            item = null;
            amount = 0;
        }
        public bool HasSpace() => item != null ? amount < item.GetMaxStack() : true;

        public int GetSpace() => item != null ? item.GetMaxStack() - amount : 0;
        public int AddAmountToSlot(int count)
        {
            if (count > GetSpace())
            {
                int extra = count - GetSpace();
                amount = item.GetMaxStack();
                return extra;
            }
            else
            {
                amount += count;
                return 0;
            }
        }
        public void RemoveAmountFromSlot(int count)
        {
            amount -= count;
            if (amount < 1)
                Reset();
        }
        public int GetAmount() => amount;

        public bool IsEmpty() => item == null || amount < 1;

        public void SetSlot(InventoryItem item, int count)
        {
            this.item = item;
            this.amount = Mathf.Clamp(count, 0, item.GetMaxStack());
        }
    }
}
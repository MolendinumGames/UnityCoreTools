using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTools.SaveSystem;

namespace CoreTools.InventorySystem
{
    public class StandardInventory : Inventory, ISaveable
    {
        [System.Serializable]
        public struct SlotSaveData
        {
            public string id;
            public int amount;
        }
        public object CaptureState()
        {
            SlotSaveData[] saveData = new SlotSaveData[Size];
            for (int i = 0; i < Size; i++)
            {
                saveData[i].id = GetItemInSlot(i).UniqueID;
                saveData[i].amount = GetAmountIntSlot(i);
            }
            return saveData;
        }

        public void RestoreState(object state)
        {
            var saveData = state as SlotSaveData[];
            if (saveData == null)
            {
                Debug.LogWarning($"Couldn't read Inventory save data for GameObject {gameObject.name}");
                return;
            }
            if (saveData.Length != Size)
            {
                Debug.LogWarning("Corrupted save data. Saved Inventory slot amount doesn't match Size");
            }

            for (int i = 0; i < Size; i++)
            {
                InventoryItem item = ItemLookup.GetItemByID(saveData[i].id);
                int amount = saveData[i].amount;
                TryAddItemToSlot(item, i, amount);
            }

            RaiseUpdateEvent();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystem;

namespace InventorySystem
{
    public class EquipmentHolder : MonoBehaviour, ISaveable
    {
        [SerializeField] EquipmentSlot[] slots;

        public event Action equipmentUpdated;

        public EquipmentItem GetItem(string id)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (id.Equals(slots[i].GetID()))
                    return slots[i].GetItem();
            }
            Debug.Log($"{gameObject.name} cannot find Equipmentitem with ID: {id}");
            return null;
        }
        public void RemoveItemByID(string id)
        {
            EquipmentSlot slot = GetSlotByID(id);
            if (slot == null) return;
            slot.ClearSlot();
            RaiseUpdate();
        }
        public EquipmentType GetSlotType(string id)
        {
            EquipmentSlot slot = GetSlotByID(id);
            if (slot == null) return EquipmentType.Default;
            else return slot.GetEquipmentType();
        }
        public bool EquipItemIntoSlot(EquipmentItem item, string id)
        {
            EquipmentSlot slot = GetSlotByID(id);
            if (slot != null && item.GetEquipmentType() == slot.GetEquipmentType())
            {
                slot.SetItem(item);
                RaiseUpdate();
                return true;
            }
            RaiseUpdate();
            return false;
        }

        private void Start()
        {
            RaiseUpdate();
        }
        private EquipmentSlot GetSlotByID(string id)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (id.Equals(slots[i].GetID()))
                    return slots[i];
            }
            return null;
        }


        [System.Serializable]
        public struct EquipmentSlotData
        {
            public string itemId;
            public string id;
        }
        public object CaptureState()
        {
            EquipmentSlotData[] data = new EquipmentSlotData[slots.Length];
            for (int i = 0; i < data.Length; i++)
            {
                data[i].itemId = slots[i].GetItem().GetID();
                data[i].id = slots[i].GetID();
            }
            return data;
        }

        public void RestoreState(object state)
        {
            EquipmentSlotData[] data = state as EquipmentSlotData[];
            if (data.Length == 0)
            {
                Debug.LogWarning($"{gameObject.name} had no equipment save data.");
                return;
            }
            for (int i = 0; i < slots.Length; i++)
            {
                for (int j = 0; j < data.Length; j++)
                {
                    if (data[j].id == slots[i].GetID())
                    {
                        string itemID = data[j].itemId;
                        InventoryItem newitem = InventoryItem.GetItemByID(itemID);

                        slots[i].SetItem(newitem as EquipmentItem);
                        break;
                    }
                }
            }
        }
        private void RaiseUpdate() => equipmentUpdated?.Invoke();
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.InventorySystem
{
    public static class ItemLookup
    {
        // Table: ID - Scriptableobject
        private static Dictionary<string, InventoryItem> data = new Dictionary<string, InventoryItem>();

        /// <summary> 
        ///  Get InventoryItem ScriptableObject by ID;
        /// </summary>
        /// <param name="id">Unique ID (GUID)</param>
        public static InventoryItem GetItemByID(string id)
        {
            if (data == null)
                PopulateItemLookup();

            if (string.IsNullOrEmpty(id) || !data.ContainsKey(id))
            {
                Debug.LogError($"No Item with ID: '{id}' found. Returning null.");
                return null;
            }
            else return data[id];
        }
        private static void PopulateItemLookup()
        {
            data = new Dictionary<string, InventoryItem>();
            InventoryItem[] items = Resources.LoadAll<InventoryItem>("");
            for (int i = 0; i < items.Length; i++)
            {
                string id = items[i].UniqueID;
                if (!data.ContainsKey(id))
                {
                    data[id] = items[i];
                }
                else
                {
                    Debug.LogError($"Dublicate Item ID! Item names: {items[i].ItemName}, {data[id].ItemName}. ID: {id}");
                    continue;
                }
            }
        }
    }
}

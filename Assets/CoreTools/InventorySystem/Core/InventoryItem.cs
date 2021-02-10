using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] new string name;
        [SerializeField] string uniqueID;
        [SerializeField] string description;
        [SerializeField] string tooltip;
        [SerializeField] Sprite icon;
        [SerializeField] int maxStack;
        [SerializeField] Mesh mesh;
        [SerializeField] Material material;

        #region Public Properties
        public string GetName() => name;
        public string GetID() => uniqueID;
        public string GetDescription() => description;
        public string GetToolTip() => tooltip;
        public Sprite GetIcon() => icon;
        public int GetMaxStack() => maxStack;
        public Mesh GetMesh() => mesh;
        public Material GetMaterial() => material;
        #endregion

        private static Dictionary<string, InventoryItem> itemLookup;

        #region Static Methods
        public static InventoryItem GetItemByID(string id)
        {
            if (itemLookup == null)
                PopulateItemLookup();

            if (id == null || !itemLookup.ContainsKey(id))
            {
                Debug.LogWarning($"No Item with ID: '{id}' found. Returning null.");
                return null;
            }
            else return itemLookup[id];
        }
        private static void PopulateItemLookup()
        {
            itemLookup = new Dictionary<string, InventoryItem>();
            InventoryItem[] items = Resources.LoadAll<InventoryItem>("");
            for (int i = 0; i < items.Length; i++)
            {
                string id = items[i].uniqueID;
                if (!itemLookup.ContainsKey(id))
                    itemLookup[id] = items[i];
                else
                    Debug.LogWarning($"Dublicate Item ID. Names: {items[i].GetName()}, {itemLookup[id].GetName()}. ID: {id}");
            }
        }
        #endregion

        #region Public Methods
        public void OnAfterDeserialize()
        {
            HandleID();
        }

        public void OnBeforeSerialize()
        {
            // Not needed
        }
        #endregion

        #region Private Methods
        private void HandleID()
        {
            if (string.IsNullOrWhiteSpace(uniqueID))
            {
                uniqueID = System.Guid.NewGuid().ToString();
            }
        }
        #endregion
    }
}

using UnityEngine;

namespace CoreTools.InventorySystem
{
    public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        public string UniqueID { get; private set; } = "";

        [SerializeField]
        string itemName;
        public string ItemName { get => itemName; }

        [SerializeField] 
        string description;
        public string Description { get => description; }

        [SerializeField] 
        string tooltip;
        public string Tooltip { get => tooltip; }

        [SerializeField] 
        Sprite icon;
        public Sprite Icon { get => icon; }

        [SerializeField] 
        int maxStack;
        public int MaxStack { get => maxStack; }

        #region Serialization
        public void OnAfterDeserialize() => HandleID();

        public void OnBeforeSerialize() { } // not needed

        private void HandleID()
        {
            if (string.IsNullOrWhiteSpace(UniqueID))
            {
                UniqueID = System.Guid.NewGuid().ToString();
            }
        }
        #endregion
    }
}

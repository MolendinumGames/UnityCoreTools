namespace CoreTools.InventorySystem
{
    //[CreateAssetMenu(fileName = "NewActionItem", menuName ="Inventory/Item/Action")]
    public abstract class ActionItem : InventoryItem, IConsumable
    {
        public abstract void Consume();
    }
}

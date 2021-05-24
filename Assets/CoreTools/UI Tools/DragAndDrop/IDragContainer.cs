namespace CoreTools.UI
{
    public interface IDragContainer<T> where T : class
    {
        T GetItem();
        void SetItem(T item, int amount);

        int GetAmount();
        void RemoveAmount(int amount);
        int TryAddAmount(T item, int amount);

        int MaxAcceptable(T item);
    }
}

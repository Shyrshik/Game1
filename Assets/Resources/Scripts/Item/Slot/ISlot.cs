namespace Items
{
    public interface ISlot
    {
        public Item Item { get; }
        public bool IsEmpty { get; }
        public bool TryAddItem(Item item);
        public bool CanAdd(Item item);
        public void RemoveItem();
    }
}
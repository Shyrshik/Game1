public interface ISlot
{
    Item Item { get; }

    bool AddItem(Item item);
    bool IsEmpty();
    void RemoveItem();
}
﻿public interface ISlot
{
    public Item Item { get; }
    public bool IsEmpty { get; }
    public bool AddItem(Item item);
    public void RemoveItem();
}
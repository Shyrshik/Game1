using System;
using Unity.VisualScripting;

namespace Items
{
    public abstract class Slot : ISlot
    {
        public Item Item { get => IsEmpty ? null : _item; }
        private Item _item;

        public bool IsEmpty { get => _isEmpty; }
        protected bool _isEmpty = true;
        public event Action ItemAdded;
        public Action ItemRemoved;
        public bool AddItem(Item item)
        {
            if (IsEmpty && !item.IsUnityNull() && CanAdd(item))
            {
                _isEmpty = false;
                _item = item;
                ItemAdded();
                return true;
            }
            return false;
        }

        public abstract bool CanAdd(Item item);

        public void RemoveItem()
        {
            _isEmpty = true;
            _item = null;
            ItemRemoved();
        }
    }
}
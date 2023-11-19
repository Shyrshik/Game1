using System;

namespace Items
{
    public abstract class Slot : ISlot, IAddedEvent, IRemovedEvent
    {
        public Item Item { get => IsEmpty ? null : _item; }
        private Item _item;

        public bool IsEmpty { get => _isEmpty; }
        protected bool _isEmpty = true;
        public event Action Added;
        public event Action Removed;
        public bool TryAddItem(Item item)
        {
            if (IsEmpty && (item is not null) && CanAdd(item))
            {
                _isEmpty = false;
                _item = item;
                if (Added is not null) Added();
                return true;
            }
            return false;
        }

        public abstract bool CanAdd(Item item);

        public void RemoveItem()
        {
            _isEmpty = true;
            _item = null;
            if (Removed is not null) Removed();
        }
    }
}
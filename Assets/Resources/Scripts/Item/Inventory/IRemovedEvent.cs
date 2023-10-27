using System;

namespace Items
{
    public interface IRemovedEvent
    {
        public event Action Removed;
    }
}
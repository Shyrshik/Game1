using System;

namespace Items
{
    public interface IAddedEvent
    {
        public event Action Added;
    }
}
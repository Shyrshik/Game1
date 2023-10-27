namespace Items
{
    public class SlotSimple : Slot
    {
        public override bool CanAdd(Item item) => item is not null;
    }
}